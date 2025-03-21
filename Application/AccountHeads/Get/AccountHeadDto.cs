using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.AccountHeads.Get;

public class AccountHeadDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public AccountType Type { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<AccountHead, AccountHeadDto>();
        }
    }
}