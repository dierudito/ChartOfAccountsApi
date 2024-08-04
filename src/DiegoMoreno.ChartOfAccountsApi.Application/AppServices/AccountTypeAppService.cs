using AutoMapper;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories;
using System.Net;

namespace DiegoMoreno.ChartOfAccountsApi.Application.AppServices;
public class AccountTypeAppService(IAccountTypeRepository accountTypeRepository, IMapper mapper) : IAccountTypeAppService
{
    public async Task<Response<List<AccountTypeResponseDto>>> GetAllAsync()
    {
        var accountTypes = (await accountTypeRepository.GetAllAsync()).ToList();

        if (accountTypes == null || accountTypes.Count == 0)
            return new Response<List<AccountTypeResponseDto>>(null, HttpStatusCode.NoContent, "Account types not found");

        var accountTypesDto = mapper.Map<List<AccountTypeResponseDto>>(accountTypes);
        return new Response<List<AccountTypeResponseDto>>(accountTypesDto, message: "Ok");
    }

    public async Task<Response<AccountTypeResponseDto>> GetByIdAsync(GetAccountTypeByIdRequestDto requestDto)
    {
        var accountType = await accountTypeRepository.GetByIdAsync(requestDto.Id);

        if (accountType == null) return new Response<AccountTypeResponseDto>(null, HttpStatusCode.NotFound, "This type of account doesn't exist");

        var accountTypeDto = mapper.Map<AccountTypeResponseDto>(accountType);
        return new(accountTypeDto, message: "Ok");
    }
}
