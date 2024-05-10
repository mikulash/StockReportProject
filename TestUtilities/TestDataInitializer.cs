using DataAccessLayer.Models;
using FileLoader.Model;

namespace TestUtilities;

public static class TestDataInitializer
{
    public static Fund GetTestFund() => GetTestFunds().ElementAt(0);

    public static List<Fund> GetTestFunds() =>
    [
        new() { Id = 1, FundName = "STARK" },
        new() { Id = 2, FundName = "CLARK" },
        new() { Id = 3, FundName = "BEST" },
        new() { Id = 4, FundName = "UTF-8" }
    ];

    public static Fund GetUncommittedTestFund() => GetUncommittedTestFunds().ElementAt(0);

    public static List<Fund> GetUncommittedTestFunds() =>
    [
        new() { FundName = "SHARK" },
        new() { FundName = "LINUX" },
        new() { FundName = "WINDOWS" }
    ];


    public static Company GetTestCompany() => GetTestCompanies().ElementAt(0);
    
    public static List<Company> GetTestCompanies() =>
    [
        new() {Id = 1, CompanyName = "TESLA INC", Ticker = "TSLA", CUSIP = "88160R101" },
        new() {Id = 2, CompanyName = "ROKU INC", Ticker = "ROKU", CUSIP = "77543R102" },
        new() {Id = 3, CompanyName = "COINBASE GLOBAL INC", Ticker = "COIN", CUSIP = "19260Q107" },
        new() {Id = 4, CompanyName = "BLOCK INC", Ticker = "SQ", CUSIP = "852234103" },
        new() {Id = 5, CompanyName = "ROBINHOOD MARKETS INC - A", Ticker = "HOOD", CUSIP = "770700102" }
    ];

    public static Company GetUncommittedTestCompany() => GetUncommittedTestCompanies().ElementAt(0);
    
    public static List<Company> GetUncommittedTestCompanies() =>
    [
        new() { CompanyName = "BEAM THERAPEUTICS INC", Ticker = "BEAM", CUSIP = "07373V105" },
        new() { CompanyName = "ROBLOX CORP -CLASS A", Ticker = "RBLX", CUSIP = "771049103" },
        new() { CompanyName = "PINTEREST INC- CLASS A", Ticker = "PINS", CUSIP = "72352L106" },
        new() { CompanyName = "TELADOC HEALTH INC", Ticker = "TDOC", CUSIP = "87918A105" },
        new() { CompanyName = "2U INC", Ticker = "TWOU", CUSIP = "90214J101" }
    ];


    public static IndexRecord GetTestIndexRecord() => GetTestIndexRecords().ElementAt(0);
    
    public static List<IndexRecord> GetTestIndexRecords() =>
    [
        new() { Id = 1, FundId = 1, CompanyId = 1, IssueDate = new DateOnly(2024, 1, 1), Shares = 1000, MarketValue = 1000, Weight = 20.50 },
        new() { Id = 2, FundId = 1, CompanyId = 2, IssueDate = new DateOnly(2024, 1, 1), Shares = 800, MarketValue = 50, Weight = 30.50 },
        new() { Id = 3, FundId = 1, CompanyId = 3, IssueDate = new DateOnly(2024, 1, 1), Shares = 300, MarketValue = 30, Weight = 48.05 },
        new() { Id = 4, FundId = 1, CompanyId = 4, IssueDate = new DateOnly(2024, 1, 1), Shares = 500, MarketValue = 120, Weight = 0.95 },
        new() { Id = 5, FundId = 1, CompanyId = 1, IssueDate = new DateOnly(2024, 1, 2), Shares = 1000, MarketValue = 1020, Weight = 20.50 },
        new() { Id = 6, FundId = 1, CompanyId = 2, IssueDate = new DateOnly(2024, 1, 2), Shares = 800, MarketValue = 70, Weight = 30.50 },
        new() { Id = 7, FundId = 1, CompanyId = 3, IssueDate = new DateOnly(2024, 1, 2), Shares = 300, MarketValue = 10, Weight = 47.30 },
        new() { Id = 8, FundId = 1, CompanyId = 4, IssueDate = new DateOnly(2024, 1, 2), Shares = 500, MarketValue = 80, Weight = 1.70 }
    ];

    public static IndexRecord GetUncommittedTestIndexRecord() => GetUncommittedTestIndexRecords().ElementAt(0);
    
    public static List<IndexRecord> GetUncommittedTestIndexRecords() =>
    [
        new () { FundId = 1, CompanyId = 1, IssueDate = new DateOnly(2024, 1, 5), Shares = 300, MarketValue = 120, Weight = 5.36 },
        new () { FundId = 1, CompanyId = 2, IssueDate = new DateOnly(2024, 1, 5), Shares = 4567, MarketValue = 4567, Weight = 65.97 },
        new () { FundId = 1, CompanyId = 3, IssueDate = new DateOnly(2024, 1, 5), Shares = 10321, MarketValue = 81723, Weight = 28.13 },
        new () { FundId = 1, CompanyId = 4, IssueDate = new DateOnly(2024, 1, 5), Shares = 111, MarketValue = 9, Weight = 0.54 },
    ];

    public static List<NullableIndexRecordDto> GetTestNullableIndexRecords() =>
    [
        new () { Fund = "STARK", Company = "Windows", CUSIP = "1323124PW", Ticker = "WIN", 
            Date = new DateOnly(2024, 1, 1), MarketValue = 10.25, Shares = 1032132, Weight = 82.13 },
        new () { Fund = "STARK", Company = "BetterWindows", CUSIP = "103LL0312M", Ticker = "BWIN", 
            Date = new DateOnly(2024, 1, 1), MarketValue = 3213124.10291, Shares = 32131, Weight = 12.17 },
        new () { Fund = "STARK", Company = "EventBetterWindows", CUSIP = "DAS3120LLS", Ticker = "EBWIN", 
            Date = new DateOnly(2024, 1, 1), MarketValue = 4.20, Shares = 4, Weight = 5.7 }
    ];
}
