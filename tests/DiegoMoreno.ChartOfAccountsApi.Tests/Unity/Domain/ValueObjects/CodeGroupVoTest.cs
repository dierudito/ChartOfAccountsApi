using DiegoMoreno.ChartOfAccountsApi.Domain.ValueObjects;

namespace DiegoMoreno.ChartOfAccountsApi.Tests.Unity.Domain.ValueObjects;

public class CodeGroupVoTests
{
    [Theory]
    [InlineData("1", true)]
    [InlineData("999", true)]
    [InlineData("1.1", true)]
    [InlineData("10.50.100", true)]
    [InlineData("99.999.1", true)]
    [InlineData("", false)]
    [InlineData("0", false)]
    [InlineData("1000", false)]
    [InlineData("1.0", false)]
    [InlineData("1.1000", false)]
    [InlineData("1..2", false)]
    [InlineData("1.", false)]
    [InlineData("a.b.c", false)]
    [InlineData("1 2 3", false)]
    [Trait(nameof(CodeGroupVo), nameof(CodeGroupVo.IsValid))]
    public void IsValid_ShouldReturnExpectedResult(string codeGroup, bool expectedResult)
    {
        // Act
        var result = CodeGroupVo.IsValid(codeGroup);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData("", 0)]
    [InlineData("a", 0)]
    [InlineData("1", 1)]
    [InlineData("1.1", 1)]
    [Trait(nameof(CodeGroupVo), nameof(CodeGroupVo.GetCode))]
    public void GetCode_ShouldReturnExpectedCode(string codeGroup, int expectedCode)
    {
        // Act
        var result = CodeGroupVo.GetCode(codeGroup);

        // Assert
        Assert.Equal(expectedCode, result);
    }
}
