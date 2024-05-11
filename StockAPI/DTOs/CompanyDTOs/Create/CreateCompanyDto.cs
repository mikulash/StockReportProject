namespace StockAPI.DTOs.CompanyDTOs.Create;

public class CreateCompanyDto
{
    public required string CompanyName { get; set; }
    public string Ticker { get; set; } = string.Empty;
    public required string CUSIP { get; set; }
}
