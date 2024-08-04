using System.Net;

namespace DiegoMoreno.ChartOfAccountsApi.Infra.CrossCutting.Shared;

public static class ApiConfigurations
{
    public const HttpStatusCode DefaultStatusCode = HttpStatusCode.OK;
    public const int DefaultPageNumber = 1;
    public const int DefaultPageSize = 25;
    public const string RouteAccount = "accounts";
    public const string RouteAccountType = "accountTypes";
    public static string ConncetionString { get; set; } = string.Empty;
    public static string CorsPolicyName => "corsdodiegomoreno";
    public static string BackendUrl { get; set; } = string.Empty;
    public static string FrontendUrl { get; set; } = string.Empty;
}
