using AulaTeste;

namespace TesteAulaTest.pedido_test;

public class PedidoRepositoryFake : IPedidoRepository
{
    public List<Pedido> Salvos { get; } = new();
    public void Salvar(Pedido pedido) => Salvos.Add(pedido);
}

public class NotificadorSpy : INotificador
{
    public List<(string Para, string Msg)> Enviadas { get; } = new();

    public void Enviar(string destinatario, string mensagem)
    {
        Enviadas.Add((destinatario, mensagem));
    }
}

public class PedidoServiceTestWithFake
{
    [Fact]
    public void Finalizar_PedidoValido_SalvaENotificaCliente()
    {
        var repo = new PedidoRepositoryFake();
        var notificador = new NotificadorSpy();
        var serivco = new PedidoService(repo, notificador);
        var pedido = new Pedido { Id = 12, EmailCliente = "jana_java@gmail.com", Total = 150m };
        
        serivco.Finalizar(pedido);
        Assert.Single(repo.Salvos);
        Assert.Single(notificador.Enviadas);
        Assert.Equal("jana_java@gmail.com", notificador.Enviadas[0].Para);
        Assert.Contains("#12", notificador.Enviadas[0].Msg);
        
    }
    
    [Fact]
    public void Finalizar_PedidoValido_NaoSalvaNemNotificaCliente()
    {
        var repo = new PedidoRepositoryFake();
        var notificador = new NotificadorSpy();
        var serivco = new PedidoService(repo, notificador);

        Assert.Throws<InvalidOperationException>(() => serivco.Finalizar(new Pedido { Total = 0 }));
        
        Assert.Empty(repo.Salvos);
        Assert.Empty(notificador.Enviadas);
        
    }

}