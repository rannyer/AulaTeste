namespace AulaTeste;

public class CalculadoraDesconto
{
    public const decimal LimiteParaDesconto = 500m;

    public decimal CalcularPercentual(decimal valorCompra, bool clienteVip)
    {
        if(valorCompra <0)
        {
            throw new ArgumentException("O valor da compra deve ser um número positivo.");
        }
        decimal percentualDesconto = 0m;

        if (valorCompra >= LimiteParaDesconto) percentualDesconto += 0.10m;
        if (clienteVip) percentualDesconto += 0.05m;;

        return percentualDesconto;
    }
    public decimal CalcularValorFinal(decimal valorCompra, bool clienteVip)
    {
        var percentualDesconto = CalcularPercentual(valorCompra, clienteVip);
        return Math.Round(valorCompra * (1- percentualDesconto), 2);
    }
    
    
}