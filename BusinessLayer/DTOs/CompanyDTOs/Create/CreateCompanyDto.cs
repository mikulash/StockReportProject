namespace BusinessLayer.DTOs.CompanyDTOs.Create;

public class CreateCompanyDto
{
    public required string CompanyName { get; set; }
    public required string Ticker { get; set; }
    public required string CUSIP { get; set; }
}
