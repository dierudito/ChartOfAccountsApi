using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Request;
using DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;

namespace DiegoMoreno.ChartOfAccountsApi.Application.Interfaces;
public interface IAccountAppService
{
    Task<Response<AddAccountResponseDto>> AddAsync(AddAccountRequestDto requestDto);
    Task<Response<bool>> DeleteAsync(DeleteAccountRequestDto requestDto);
    Task<Response<NextAccountCodeResponseDto>> GetSuggestedCodeAsync(NextAccountCodeRequestDto requestDto);
    Task<Response<GetListAccountWithCodeGroupResponseDto>> GetListAccountWithCodeGroupAsync(GetListAccountWithCodeGroupRequestDto requestDto);
    Task<Response<GetListAccountWithCodeGroupResponseDto>> SearchAccountAsync(SearchAccountRequestDto requestDto);
}
