namespace AulaTeste;

public class ContaBancaria
{
    public string Titular { get;  }
    public decimal Saldo { get; private set; }

    public ContaBancaria(string titular, decimal saldoInicial)
    {
        if(string.IsNullOrWhiteSpace(titular))
        {
            throw new ArgumentException("O titular não pode ser nulo ou vazio.", nameof(titular));
        }

        if (saldoInicial < 0)
        {
            throw new ArgumentException("O saldo inicial não pode ser negativo.", nameof(saldoInicial));
        }
        Titular = titular;
        Saldo = saldoInicial;
        

    }

    public void Depositar(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(valor),"O valor do depósito deve ser positivo.");
        }
        Saldo += valor;
    }

    public void Sacar(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(valor), "O valor do saque deve ser positivo.");
        }

        if (valor > Saldo)
        {
            throw new InvalidOperationException("Saldo insuficiente para o saque.");
        }

        Saldo -= valor;
    }
    public void Transferir(decimal valor, ContaBancaria contaDestino)
    {
        ArgumentNullException.ThrowIfNull(contaDestino);
        Sacar(valor);
        contaDestino.Depositar(valor);
    }
        
}