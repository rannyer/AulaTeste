using AulaTeste;

namespace TesteAulaTest;

public class TestCalculadora
{
    [Fact]
    public void Somar_DoisPositivos_RetornaSoma()
    {
        // Arrange
        var calc = new Calculadora();
        var n1 = 2;
        var n2 = 3;
       
        // Act
        var resultado = calc.Somar(n1, n2);
        
        // Assert
        Assert.Equal(5, resultado);
    }
    
}