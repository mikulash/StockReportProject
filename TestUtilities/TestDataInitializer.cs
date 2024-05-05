using DataAccessLayer.Models;

namespace TestUtilities;

public static class TestDataInitializer
{
    public static Fund GetTestFund() => GetTestFunds().ElementAt(0);
    
    public static List<Fund> GetTestFunds()
    {
        return new List<Fund>
        {
            new Fund
            {
                Id = 1,
                FundName = "STARK"
            },
            new Fund
            {
                Id = 2,
                FundName = "CLARK"
            },
            new Fund
            {
                Id = 3,
                FundName = "BEST"
            },
            new Fund
            {
                Id = 4,
                FundName = "UTF-8"
            }
        };
    }
}
