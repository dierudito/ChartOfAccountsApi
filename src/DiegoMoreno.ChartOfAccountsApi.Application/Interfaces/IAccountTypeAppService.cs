using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;

namespace DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;
public interface IAccountTypeAppService
{
    Task<Response<AccountTypeResponseDto>> GetByIdAsync(GetAccountTypeByIdRequestDto requestDto);

    Task<Response<List<AccountTypeResponseDto>>> GetAllAsync();
}