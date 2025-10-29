using AutoMapper;
using GraphQLServer.GraphQL.Types;
using GraphQLServer.Models;

namespace GraphQLServer.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile() 
        {
            //This is intended to omit the inner items
            CreateMap<Inventory, InventoryInputType>()
                .ReverseMap()
                .ForMember(u => u.Item, invInput => invInput.Ignore());

            CreateMap<Inventory, InventoryPayload>()
                .ReverseMap()
                .ForMember(u => u.Item, invInput => invInput.Ignore());
        }
    }
}
