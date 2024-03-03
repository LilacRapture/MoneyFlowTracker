namespace MoneyFlowTracker.Api.Util.Mapper;

using AutoMapper;
using MoneyFlowTracker.Api.Domain.NetItem;
using MoneyFlowTracker.Business.Domain.NetItem;

public class MoneyFlowTrackerMapperProfile : Profile
{
    public MoneyFlowTrackerMapperProfile() : base(nameof(MoneyFlowTrackerMapperProfile))
    {
        // CardModel
        CreateMap<NetItemModel, GetNetItemsByDateQueryApi.NetItemReponseDto>()
            .ForMember(i => i.IsNet, config => config.MapFrom(i => true))
        ;
    }
}