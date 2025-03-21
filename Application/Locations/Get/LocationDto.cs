using AutoMapper;
using Domain.Entities;

namespace Application.Locations.Get;

public class LocationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Location, LocationDto>();
        }
    }
}


