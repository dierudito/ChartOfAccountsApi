using System.Net;
using System.Text.Json.Serialization;

namespace DiegoMoreno.ChartOfAccountsApi.Application.Dtos.Responses;
public class Response<TData>
{
    [JsonConstructor]
    public Response() => Code = HttpStatusCode.OK;

    public Response(TData? data, HttpStatusCode code = HttpStatusCode.OK, string? message = null)
    {
        Data = data;
        Code = code;
        Message = message;
    }

    public TData? Data { get; set; }

    public string? Message { get; set; }

    [JsonIgnore]
    public HttpStatusCode Code { get; set; }
}