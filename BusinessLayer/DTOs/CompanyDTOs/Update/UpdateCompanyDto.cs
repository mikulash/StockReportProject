namespace BusinessLayer.DTOs.CompanyDTOs.Update;

public class UpdateCompanyDto
{
    public required string CompanyName { get; set; }
    public required string Ticker { get; set; }
    public required string CUSIP { get; set; }
}
