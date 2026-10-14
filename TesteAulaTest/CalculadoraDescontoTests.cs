using AulaTeste;

namespace TesteAulaTest;

public class CalculadoraDescontoTests
{
    private readonly CalculadoraDesconto cal = new ();

    public static TheoryData<decimal, bool, decimal> Cenarios => new()
    {
        { 0m, false, 0.00m },
        { 499.99m, false, 0.00m },
        { 500m, false, 0.10m },
        { 500.01m, false, 0.10m },
        { 100m, true, 0.05m },
        { 800m, true, 0.15m }
    };
    
    [Theory]
    [MemberData(nameof(Cenarios))]
    public void CalcularPercentual_Cenarios_RetornaPercentualDesconto(
        decimal valor,
        bool vip,
        decimal percentualEsperado
        )
    {
        var percentual = cal.CalcularPercentual(valor, vip);
        Assert.Equal(percentualEsperado, percentual);   
    }
    [Fact]
    public void CalcularValorFinal_ValorCompraNegativo_LancaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => cal.CalcularValorFinal(-100m, false));
    }
    [Fact]
    public void CalcularValorFinal_ValorCompraPositivo_RetornaValorFinalComDesconto()
    {
        var valorFinal = cal.CalcularValorFinal(1000m, true);
        Assert.Equal(850m, valorFinal);
    }
}