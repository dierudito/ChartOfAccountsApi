using AutoMapper;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;

namespace DiegoMoreno.ChartOfAccountsApi.Application.AutoMapper;
public class AccountTypeMapper : Profile
{
    public AccountTypeMapper()
    {
        CreateMap<AccountType, AccountTypeResponseDto>();
    }
}
