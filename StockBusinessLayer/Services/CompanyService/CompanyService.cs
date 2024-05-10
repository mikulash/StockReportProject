﻿using StockBusinessLayer.Exceptions;
using DataAccessLayer.Models;
using GenericBusinessLayer.Exceptions;
using GenericBusinessLayer.Services;
using GenericInfrastructure.Query;
using StockInfrastructure.Query;
using StockInfrastructure.UnitOfWork;

namespace StockBusinessLayer.Services.CompanyService;

public class CompanyService : GenericService<Company, long, IStockUnitOfWork>, ICompanyService
{
    public CompanyService(IStockUnitOfWork unitOfWork, IQuery<Company, long> query) : base(unitOfWork, query)
    {
    }

    public async Task<Company> FindByIdAllIndexRecordsAsync(long id)
    {
        var company = await Repository.GetByIdAsync(id, comp => comp.IndexRecords!);

        return company ?? throw new NoSuchEntityException<long>(typeof(Company), id);
    }
}
