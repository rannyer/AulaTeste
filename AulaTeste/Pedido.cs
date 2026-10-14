namespace AulaTeste;

public class Pedido
{
    public int Id { get; set; }

    public string EmailCliente { get; set; }

    public decimal Total { get; set; }
}

public interface IPedidoRepository
{
    void Salvar(Pedido pedido);
}

public interface INotificador
{
    void Enviar(string destinatario, string mensagem);
}

public class PedidoService
{
    private readonly IPedidoRepository _repositorio;
    private readonly INotificador _notificador;

    public PedidoService(IPedidoRepository repositorio, INotificador notificador)
    {
        _repositorio = repositorio;
        _notificador = notificador;
    }

    public void Finalizar(Pedido pedido)
    {
        if (pedido.Total <= 0)
            throw new InvalidOperationException("Pedido sem valor nao pode ser finalizado!");
        
        _repositorio.Salvar(pedido);
        _notificador.Enviar(pedido.EmailCliente, $"#{pedido.Id} - Pedido finalizado!");

    }
}