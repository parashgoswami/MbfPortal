using Application.Locations.Get;
using AutoMapper;
using Domain.Entities;

namespace Application.Members.Get;

public class MemberDto
{
    public int Id { get; set; }
    public string EmpNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Nominee { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime DOJ { get; set; }
    public DateTime? DOS { get; set; }    
    public decimal Share { get; set; }
    public bool IsActive { get; set; } 
    public LocationDto? Location { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Member, MemberDto>();
        }
    }
}
