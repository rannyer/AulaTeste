using AulaTeste;

namespace TesteAulaTest;

public class ContaBancariaTests
{
    private readonly ContaBancaria _conta;
    
    public ContaBancariaTests()
    {
        _conta = new ContaBancaria("Vorcatrix", 100m);
    }
    
    //
    //
    //
    //
    [Fact]
    public void Construtor_DadosValidos_CriaContaComSaldoInicial()
    {
        // Arrange
        var titular = "Joana Java";
        var saldoInicial = 100m;
        
        // Act
        var conta = new ContaBancaria(titular, saldoInicial);
        
        // Assert
        Assert.Equal(titular, conta.Titular);
        Assert.Equal(saldoInicial, conta.Saldo);
    }
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("     ")]
    public void Construtor_TitularNuloOuVazio_LancarArgumentException(string? titular)
    {
        Assert.Throws<ArgumentException>(() => new ContaBancaria(titular!, 100m));
    }
    
    [Fact]
    public void Depositar_ValorPositivo_AumentaSaldo()
    {
        _conta.Depositar(100);
        Assert.Equal(200m, _conta.Saldo);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Depositar_ValorZeroOuNegativo_LancaExcecao(decimal valor)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _conta.Depositar(valor));
    }
    
    //Sacar_ValorMenorQueSaldo_DiminuiSaldo
    [Fact]
    public void Sacar_ValorMenorQueSaldo_DiminuiSaldo()
    {
        _conta.Sacar(50);
        Assert.Equal(50m, _conta.Saldo);
    }
    
    //Sacar_Valor_IgualSaldo_ZeraConta
    [Fact]
    public void Sacar_ValorIgualSaldo_ZeraConta()
    {
        _conta.Sacar(100);
        Assert.Equal(0m, _conta.Saldo);
    }
    
    //Sacar_ValorMaiorQueSaldo_LancaExcecaoENaoAlteraSaldo
    [Fact]
    public void Sacar_ValorMaiorQueSaldo_LancaExcecaoENaoAlteraSaldo()
    {
        Assert.Throws<InvalidOperationException>(() => _conta.Sacar(150));
        Assert.Equal(100m, _conta.Saldo);
    }
    
    //Transferir_ValorMenorQueSaldo_TransfereValorEntreContas
    [Fact]
    public void Transferir_SaldoSuficiente_TransfereValorEntreContas()
    {
        var contaDestino = new ContaBancaria("Celia C#", 50m);
        _conta.Transferir(50, contaDestino);
        Assert.Equal(50m, _conta.Saldo);
        Assert.Equal(100m, contaDestino.Saldo);
    }
    [Fact]
    public void Transferir_SaldoInsuficiente_LancaExcecaoENaoAlteraSaldos()
    {
        var contaDestino = new ContaBancaria("Celia C#", 50m);
        Assert.Throws<InvalidOperationException>(() => _conta.Transferir(150, contaDestino));
        Assert.Equal(100m, _conta.Saldo);
        Assert.Equal(50m, contaDestino.Saldo);
    }



}