using DataAccessLayer.Models;

namespace TestUtilities;

public static class TestDataInitializer
{
    public static Fund GetTestFund() => GetTestFunds().ElementAt(0);
    
    public static List<Fund> GetTestFunds()
    {
        return
        [
            new() { Id = 1, FundName = "STARK" },
            new() { Id = 2, FundName = "CLARK" },
            new() { Id = 3, FundName = "BEST" },
            new() { Id = 4, FundName = "UTF-8" }
        ];
    }

    public static Fund GetUncommittedTestFund() => GetUncommittedTestFunds().ElementAt(0);
    
    public static List<Fund> GetUncommittedTestFunds() => 
        [
            new() {FundName = "SHARK"},
            new() {FundName = "LINUX"},
            new() {FundName = "WINDOWS"}
        ];
}
