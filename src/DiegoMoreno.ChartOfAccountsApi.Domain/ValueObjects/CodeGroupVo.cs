using System.Text.RegularExpressions;

namespace DiegoMoreno.ChartOfAccountsApi.Domain.ValueObjects;
public static class CodeGroupVo
{
    public static bool IsValid(string codeGroup)
    {
        var regex = new Regex(@"^([1-9]\d{0,2}(?:\.[1-9]\d{0,2})*)$");
        return regex.IsMatch(codeGroup);
    }

    public static int GetCode(string codeGroup)
    {
        if (string.IsNullOrEmpty(codeGroup)) return 0;

        if (!IsValid(codeGroup)) return 0;
        if (!codeGroup.Contains('.')) return Convert.ToInt32(codeGroup);

        var splitedCode = codeGroup.Split('.');
        var lastCode = splitedCode.LastOrDefault();
        return Convert.ToInt32(lastCode);
    }
}
