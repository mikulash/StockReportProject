﻿using AutoMapper;
using StockBusinessLayer.Services;
using DataAccessLayer.Models;
using FileLoader.FileParserStrategy;
using FileLoader.Model;
using FileLoader.Reader;
using GenericBusinessLayer.Services;
using StockInfrastructure.Query.Filters.EntityFilters;
using StockAPI.DTOs.CompanyDTOs.Create;
using StockBusinessLayer.Exceptions;
using StockBusinessLayer.Services.CompanyService;
using StockBusinessLayer.Services.IndexRecordService;
using StockBusinessLayer.Services.NullableIndexRecordService;

namespace StockBusinessLayer.Facades.ProcessFileFacade;

public class ProcessFileFacade : IProcessFileFacade
{
    private const FileType DefaultFileType = FileType.Csv;
    
    private readonly IParserMiddleware _parserMiddleware;
    private readonly INullableIndexRecordService _nullableIndexRecordService;
    private readonly IGenericService<Fund, long> _fundService;
    private readonly ICompanyService _companyService;
    private readonly IIndexRecordService _indexRecordService;
    private readonly IMapper _mapper;

    public ProcessFileFacade(IParserMiddleware parserMiddleware, 
        INullableIndexRecordService nullableIndexRecordService, 
        IGenericService<Fund, long> fundService, 
        ICompanyService companyService, 
        IIndexRecordService indexRecordService,
        IMapper mapper)
    {
        _parserMiddleware = parserMiddleware;
        _nullableIndexRecordService = nullableIndexRecordService;
        _fundService = fundService;
        _companyService = companyService;
        _indexRecordService = indexRecordService;
        _mapper = mapper;
    }

    private List<NullableIndexRecordDto> ParseAndGetRecords(IReader reader, FileType fileType)
    {
        _parserMiddleware.SetNewParserStrategy(fileType);
        return _parserMiddleware.ParseFileToList(reader);
    }

    private static FileType ConvertContentType(string contentType) 
        => contentType switch
        {
            "text/csv" or "application/vnd.ms-excel" => FileType.Csv,
            "application/json" => FileType.Json,
            _ => DefaultFileType
        };

    private async Task<Fund> FetchByNameOrCreateAsync(string fundName)
    {
        var queryResult = 
            await _fundService.FetchFilteredAsync(new ExactFundFilter { EQ_FundName = fundName }, null);

        if (queryResult.Items.Any())
        {
            return queryResult.Items.Count() == 1
                ? queryResult.Items.First()
                : throw new InvalidRecordsException("More Funds with provided FundName found!");
        }

        var fund = new Fund() { FundName = fundName };
        return await _fundService.CreateAsync(fund, false);
    }

    private async Task<Dictionary<string, Company>> FetchByCusipOrCreateAsync(List<CreateCompanyDto> companies)
    {
        var queryResult
            = await _companyService.FetchFilteredAsync(
                new CompanyCusipRangeFilter { IN_CUSIP = companies.Select(comp => comp.CUSIP).ToList() }, 
                null);
        var existingCompanies = queryResult.Items.ToList();

        var newCompanies = 
            companies.Where(comp => existingCompanies.TrueForAll(x => x.CUSIP != comp.CUSIP));

        var newEntities = 
            await _companyService.CreateRangeAsync(_mapper.Map<IEnumerable<Company>>(newCompanies).ToArray(), false);
        
        existingCompanies.AddRange(newEntities);

        return existingCompanies.ToDictionary(comp => comp.CUSIP, comp => comp);
    }

    private async Task CreateIndexRecords(List<NullableIndexRecordDto> indexRecords, DateOnly issueDate,
        Fund fund, IReadOnlyDictionary<string, Company> companies)
    {
        List<IndexRecord> newIndexRecords = [];

        foreach (var item in indexRecords)
        {
            var indexRecord = _mapper.Map<IndexRecord>(item);
            indexRecord.IssueDate = issueDate;
            indexRecord.Fund = fund;
            indexRecord.Company = companies!.GetValueOrDefault(item.CUSIP);
            newIndexRecords.Add(indexRecord);
        }

        await _indexRecordService.CreateRangeAsync(newIndexRecords.ToArray());
    }

    private async Task CheckRecordExistence(string fundName, DateOnly date)
    {
        if (await _indexRecordService.ExistByDateAndFundNameAsync(fundName, date))
        {
            throw new RecordExistenceCheckException(fundName, date);
        }
    }
    
    public async Task ProcessAndSaveFileAsync(Stream file, string contentType)
    {
        using var reader = new BinaryStreamReader(file);
        var records = ParseAndGetRecords(reader, ConvertContentType(contentType));

        _nullableIndexRecordService.ApplyFilter(records);
        var fund = await FetchByNameOrCreateAsync(_nullableIndexRecordService.FundName);
        var companyDictionary = await FetchByCusipOrCreateAsync(_nullableIndexRecordService.CompanyList);

        await CheckRecordExistence(fund.FundName, _nullableIndexRecordService.Date);
        
        await CreateIndexRecords(_nullableIndexRecordService.IndexRecordList, 
            _nullableIndexRecordService.Date, fund, companyDictionary);
    }
}
