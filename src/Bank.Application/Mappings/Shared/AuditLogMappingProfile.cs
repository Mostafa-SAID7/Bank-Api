using AutoMapper;
using Bank.Application.DTOs.Shared.Notification;
using Bank.Application.DTOs.Shared.Audit;
using Bank.Application.DTOs.Shared.RateLimit;
using Bank.Domain.Entities.Shared;

namespace Bank.Application.Mappings.Shared;

/// <summary>
/// AutoMapper profile for AuditLog entity mappings
/// </summary>
public class AuditLogMappingProfile : Profile
{
    public AuditLogMappingProfile()
    {
        CreateMap<AuditLog, AuditLogDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.Action, opt => opt.MapFrom(src => src.Action))
            .ForMember(dest => dest.EntityType, opt => opt.MapFrom(src => src.EntityType))
            .ForMember(dest => dest.EntityId, opt => opt.MapFrom(src => src.EntityId))
            .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp))
            .ReverseMap();
    }
}

