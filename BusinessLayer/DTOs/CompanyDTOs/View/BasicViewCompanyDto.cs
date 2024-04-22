namespace BusinessLayer.DTOs.CompanyDTOs.View;

public class BasicViewCompanyDto
{
    public long Id { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string Cusip { get; set; } = string.Empty;
}
