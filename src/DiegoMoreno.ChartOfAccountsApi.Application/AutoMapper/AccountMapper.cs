using AutoMapper;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using DiegoMoreno.ChartOfAccountsApi.Domain.ValueObjects;

namespace DiegoMoreno.ChartOfAccountsApi.Application.AutoMapper;

public class AccountMapper : Profile
{
    public AccountMapper()
    {
        CreateMap<Account, AddAccountResponseDto>();
        CreateMap<AddAccountRequestDto, Account>()
            .ForMember(x => x.Code, o => o.MapFrom(src => CodeGroupVo.GetCode(src.CodeGroup)));
    }
}
