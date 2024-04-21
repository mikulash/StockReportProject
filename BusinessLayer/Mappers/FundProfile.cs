using AutoMapper;
using BusinessLayer.DTOs.FundDTO;
using DataAccessLayer.Models;

namespace BusinessLayer.Mappers;

public class FundProfile : Profile
{
    public FundProfile()
    {
        CreateMap<CreateFundDto, Fund>();
        CreateMap<UpdateFundDto, Fund>();
        CreateMap<Fund, ViewFundDto>();
    }
}