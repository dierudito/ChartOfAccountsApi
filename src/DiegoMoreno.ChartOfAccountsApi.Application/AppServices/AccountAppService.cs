using AutoMapper;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
using DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;
using DiegoMoreno.ChartOfAccountsApi.Domain.Entities;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Repositories;
using DiegoMoreno.ChartOfAccountsApi.Domain.Interfaces.Services;
using DiegoMoreno.ChartOfAccountsApi.Domain.ValueObjects;
using System.Net;

namespace DiegoMoreno.ChartOfAccountsApi.Application.AppServices;
public class AccountAppService(
    IAccountService accountService,
    IAccountRepository accountRepository,
    IAccountTypeRepository accountTypeRepository,
    IMapper mapper) : IAccountAppService
{
    public async Task<Response<AddAccountResponseDto>> AddAsync(AddAccountRequestDto requestDto)
    {
        if (!CodeGroupVo.IsValid(requestDto.CodeGroup))
            return new Response<AddAccountResponseDto>(null, HttpStatusCode.BadRequest,
                $"The code {requestDto.CodeGroup} is invalid");

        if ((await accountTypeRepository.GetByIdAsync(requestDto.IdAccountType)) == null)
            return new Response<AddAccountResponseDto>(null, HttpStatusCode.BadRequest,
                "Account type does not exist");

        var account = mapper.Map<Account>(requestDto);

        if ((await accountRepository.GetAccountByParentAndCodeAsync(account.IdParentAccount, account.Code)) != null)
            return new Response<AddAccountResponseDto>(null, HttpStatusCode.BadRequest,
                $"The code {requestDto.CodeGroup} already exists");

        if (account.IdParentAccount.HasValue)
        {
            var parentAccount = await accountRepository.GetByIdAsync(requestDto.IdParentAccount!.Value);

            if (parentAccount == null || parentAccount.AcceptEntries)
                return new Response<AddAccountResponseDto>(null, HttpStatusCode.BadRequest,
                    "The parent is invalid");

            var codeGroup = await accountService.GetCodeGroupAsync(account);
            if (codeGroup != requestDto.CodeGroup) 
                return new Response<AddAccountResponseDto>(null, HttpStatusCode.BadRequest,
                    $"The code {requestDto.CodeGroup} is invalid");
        }

        try
        {
            account = await accountRepository.AddAsync(account);
            if (await accountRepository.CommitAsync() < 1)
                return new Response<AddAccountResponseDto>(null, HttpStatusCode.InternalServerError,
                    "Unable to save account");
        }
        catch
        {
            return new Response<AddAccountResponseDto>(null, HttpStatusCode.InternalServerError,
                "Unable to save account");
        }

        var accountResponse = mapper.Map<AddAccountResponseDto>(account);

        return new(accountResponse, HttpStatusCode.Created, message: "Account registered!");
    }

    public async Task<Response<bool>> DeleteAsync(DeleteAccountRequestDto requestDto)
    {
        var account = await accountRepository.GetByIdAsync(requestDto.Id);

        if (account == null)
            return new(false, HttpStatusCode.NoContent, "Account not found");

        if ((await accountRepository.GetAccountChildrenAsync(account.Id)).Any())
            return new(false, HttpStatusCode.NotAcceptable, "This account has children");

        try
        {
            await accountRepository.DeleteAsync(account.Id);
            if (await accountRepository.CommitAsync() < 1)
                return new(false, HttpStatusCode.InternalServerError, "Unable to delete account");
        }
        catch
        {
            return new(false, HttpStatusCode.InternalServerError, "Unable to delete account");
        }

        return new(true, message: "Account deleted");
    }

    public async Task<Response<NextAccountCodeResponseDto>> GetSuggestedCodeAsync(NextAccountCodeRequestDto requestDto)
    {
        var suggestedCode = await accountService.CalculateNextCodeAsync(requestDto.IdParentAccount);
        var message = "Suggested code generated!";

        if (!string.IsNullOrWhiteSpace(suggestedCode))
            return new Response<NextAccountCodeResponseDto>(new(suggestedCode), message: message);

        var newSuggestedCode = 
            await accountService.CalculateNextNewParentCodeAsync(requestDto.IdParentAccount!.Value);

        var statusCode = string.IsNullOrWhiteSpace(newSuggestedCode.suggestedCode)
            ? HttpStatusCode.NoContent
            : HttpStatusCode.OK;

        message = string.IsNullOrWhiteSpace(newSuggestedCode.suggestedCode) 
            ? "There aren't new codes to suggest" 
            : message;

        return new Response<NextAccountCodeResponseDto>(
            new(newSuggestedCode.suggestedCode!, newSuggestedCode.account), statusCode, message);
    }

    public async Task<Response<GetListAccountWithCodeGroupResponseDto>> 
        GetListAccountWithCodeGroupAsync(GetListAccountWithCodeGroupRequestDto requestDto)
    {
        var accounts =
            await accountRepository.GetAsync(account => (requestDto.OnlyParent && !account.AcceptEntries) ||
                                                        !requestDto.OnlyParent,
                                             new()
                                             {
                                                 Page = requestDto.Pagination.Page,
                                                 Size = requestDto.Pagination.Size
                                             },
                                             include => include.AccountType);

        if (accounts.qtd == 0)
            return new Response<GetListAccountWithCodeGroupResponseDto>(
                null, HttpStatusCode.NoContent, "Account not found");

        var accountsResponse = await GetListWithCodeGroupAndNameAsync(accounts.items.ToList());
        var responseDto = new GetListAccountWithCodeGroupResponseDto(
            accountsResponse, new(accounts.qtd, requestDto.Pagination));

        return new(responseDto, message: "Ok");
    }

    public async Task<Response<GetListAccountWithCodeGroupResponseDto>> 
        SearchAccountAsync(SearchAccountRequestDto requestDto)
    {
        var accountsWanted = await accountRepository.SearchAccountAsync(requestDto.searchTerm,
                                             new()
                                             {
                                                 Page = requestDto.Pagination.Page,
                                                 Size = requestDto.Pagination.Size
                                             });

        if (accountsWanted.qtd > 0)
        {
            var accountsResponse = await GetListWithCodeGroupAndNameAsync(accountsWanted.items);
            var responseDto = new GetListAccountWithCodeGroupResponseDto(
                accountsResponse, new(accountsWanted.qtd, requestDto.Pagination));

            return new(responseDto, message: "Ok");
        }

        var accounts =
            await accountRepository.GetAsync(null, 
                                             new()
                                             {
                                                 Page = requestDto.Pagination.Page,
                                                 Size = requestDto.Pagination.Size
                                             },
                                             include => include.AccountType);

        if (accounts.qtd == 0)
            return new Response<GetListAccountWithCodeGroupResponseDto>(
                null, HttpStatusCode.NoContent, "Account not found");

        var accountsWithCodeGroup = await GetListWithCodeGroupAndNameAsync(accounts.items.ToList());
        var accountsWantedWithCodeGroup = 
            accountsWithCodeGroup.Where(a => a.CodeGroup.Contains(requestDto.searchTerm)).ToList();

        if (accountsWantedWithCodeGroup.Count == 0)
            return new Response<GetListAccountWithCodeGroupResponseDto>(
                null, HttpStatusCode.NoContent, "Account not found");

        var responseDtoWithCodeGroup = 
            new GetListAccountWithCodeGroupResponseDto(accountsWantedWithCodeGroup, 
            new(accountsWantedWithCodeGroup.Count, requestDto.Pagination));

        return new(responseDtoWithCodeGroup, message: "Ok");
    }

    private async Task<List<AccountWithCodeGroupDto>> 
        GetListWithCodeGroupAndNameAsync(List<Account> accounts)
    {
        List<AccountWithCodeGroupDto> accountWithCodeGroupDtos = [];
        foreach (var account in accounts)
        {
            var codeGroup = await accountService.GetCodeGroupAsync(account);
            accountWithCodeGroupDtos.Add(new(account.Id, codeGroup, account.Name,
                account.IdAccountType, account.AccountType.Name, account.AcceptEntries));
        }

        return accountWithCodeGroupDtos.OrderBy(account => account.CodeGroup).ToList();
    }
}
