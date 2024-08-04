using DiegoMoreno.ChartOfAccountsApi.Domain.ValueObjects;

namespace DiegoMoreno.ChartOfAccountsApi.Tests.Unity.Domain.ValueObjects;

public class CodeGroupVoTests
{
    [Theory]
    [InlineData("1", true)]             // Número simples
    [InlineData("999", true)]           // Número de 3 dígitos (limite superior)
    [InlineData("1.1", true)]            // Dois números separados por ponto
    [InlineData("10.50.100", true)]      // Três números separados por ponto
    [InlineData("99.999.1", true)]       // Combinação de números de 2 e 3 dígitos
    [InlineData("", false)]             // String vazia (não permitida)
    [InlineData("0", false)]             // Zero não é permitido
    [InlineData("1000", false)]          // Número maior que 999
    [InlineData("1.0", false)]           // Zero após o ponto
    [InlineData("1.1000", false)]        // Número maior que 999 após o ponto
    [InlineData("1..2", false)]          // Pontos consecutivos
    [InlineData("1.", false)]            // Ponto no final
    [InlineData("a.b.c", false)]         // Letras não são permitidas
    [InlineData("1 2 3", false)]        // Espaços não são permitidos
    [Trait(nameof(CodeGroupVo), nameof(CodeGroupVo.IsValid))]
    public void IsValid_ShouldReturnExpectedResult(string codeGroup, bool expectedResult)
    {
        // Act
        var result = CodeGroupVo.IsValid(codeGroup);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(null, 0)]                // Null
    [InlineData("", 0)]                  // String vazia
    [InlineData("1", 1)]                 // Número simples
    [InlineData("999", 999)]             // Número de 3 dígitos
    [Trait(nameof(CodeGroupVo), nameof(CodeGroupVo.GetCode))]
    public void GetCode_ShouldReturnExpectedCode(string codeGroup, int expectedCode)
    {
        // Act
        var result = CodeGroupVo.GetCode(codeGroup);

        // Assert
        Assert.Equal(expectedCode, result);
    }
}
