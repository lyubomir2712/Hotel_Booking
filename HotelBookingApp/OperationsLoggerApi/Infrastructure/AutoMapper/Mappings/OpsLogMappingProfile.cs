using AutoMapper;
using OperationsLoggerApi.Data.Models;
using OperationsLoggerApi.Infrastructure.AutoMapper.DTOs;

namespace OperationsLoggerApi.Infrastructure.AutoMapper.Mappings;

public class OpsLogMappingProfile : Profile
{
    public OpsLogMappingProfile()
    {
        CreateMap<OpsLogEntryModel, OpsLogEntryDto>().ReverseMap();
    }
}