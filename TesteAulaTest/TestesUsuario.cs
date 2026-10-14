using System.Security.AccessControl;
using AulaTeste;
using NSubstitute;

namespace TesteAulaTest;


public class RepositorioDummy : IRepositorioDeUsuarios
{
    public bool EmailJaExiste(string email)
    {
        throw new NotImplementedException();
    }

    public void Adicionar(Usuario usuario)
    {
        throw new NotImplementedException();
    }
}

public class RepositorioStubEmailExistente : IRepositorioDeUsuarios
{
    public bool EmailJaExiste(string email) => true;

    public void Adicionar(Usuario usuario)
    {
       
    }
}
public class RepositorioStubEmailLivre : IRepositorioDeUsuarios
{
    public bool EmailJaExiste(string email) => false;

    public void Adicionar(Usuario usuario)
    {
       
    }
}

public class EmailDummy : IEnviadorDeEmail{
    public void Enviar(string para, string assunto)
    {
        
    }
}
public class EnviadorDeEmailSpy : IEnviadorDeEmail
{
    public List<(String para, String assunto)> Enviados { get; } = new();
    public void Enviar(string para, string assunto)
    {
        Enviados.Add((para, assunto));
    }
}

public class LogDummy : ILogDeAuditoria
{
    public void Registrar(string mensagem =  "teste")
    {
    }
}

public class TestesUsuario
{

    [Fact]
    public void Cadsatrar_NomeVazio_LancaExcecao()
    {
       
    }

    [Fact]
    public void Cadastar_EmailJaExiste_LancaExcecao()
    {
        var servico = new ServicoDeCadastro(new RepositorioStubEmailExistente(), new EmailDummy(), new LogDummy());

        Assert.Throws<InvalidOperationException>(() => servico.Cadastrar("Joana Java", "jojo@jojo.com"));
    }

    [Fact]
    public void Cadastra_Dados_Envia_EmailDeBoasVindas()
    {
        var spy = new EnviadorDeEmailSpy();
        var repo =  new RepositorioStubEmailLivre();
        var service = new ServicoDeCadastro(repo, spy, new LogDummy());
        
        service.Cadastrar("Maria MariaDb", "mama@db.com");

        Assert.Single(spy.Enviados);
        Assert.Equal("mama@db.com", spy.Enviados[0].para);
    }

    [Fact]
    public void Cadsatrar_DadosValidos_EnviaEmailUmaVez()
    {
        var repo = Substitute.For<IRepositorioDeUsuarios>();
        var email = Substitute.For<IEnviadorDeEmail>();
        var log = Substitute.For<ILogDeAuditoria>();
        repo.EmailJaExiste("mama@db.com").Returns(false);
        
        var servico = new ServicoDeCadastro(repo, email, log);
        servico.Cadastrar("Maria Db", "mama@db.com");
        email.Received(1).Enviar("mama@db.com", "Bem vindo!");
        
    }
    
    
    
    
    
    
    
    
    
}