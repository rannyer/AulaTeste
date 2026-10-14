using AulaTeste;
using NSubstitute;

namespace TesteAulaTest;

public class TestesMocksUsuario
{
    private readonly IRepositorioDeUsuarios _repo = Substitute.For<IRepositorioDeUsuarios>();
    private readonly IEnviadorDeEmail _email = Substitute.For<IEnviadorDeEmail>();
    private readonly ILogDeAuditoria _log = Substitute.For<ILogDeAuditoria>();
    private readonly ServicoDeCadastro _servico;

    public TestesMocksUsuario()
    {
        _servico = new ServicoDeCadastro(_repo, _email, _log);
    }
    
    // CAMINHO FELIZ

    [Fact]
    public void Cadastrar_DadosValidos_RetornaUsuarioCriado()
    {
        var usuario = _servico.Cadastrar("Carlos C#", "caca@email.com");
        Assert.Equal("Carlos C#", usuario.Nome);
        Assert.Equal("caca@email.com", usuario.Email);
    }

    [Fact]
    public void Cadastrar_DadosValidos_SalvaUsuarioNoRepositorio()
    {
        _servico.Cadastrar("Carlos C#", "caca@email.com");
        
        _repo.Received(1).Adicionar(Arg.Is<Usuario>(u => u.Nome == "Carlos C#" && u.Email == "caca@email.com"));
    }
    [Fact]
    public void Cadastrar_DadosValidos_EnviaEmailDeBoasVindas()
    {
        _servico.Cadastrar("Carlos C#", "caca@email.com");
        
       _email.Received(1).Enviar("caca@email.com", "Bem-vindo!");
    }

    [Fact]
    public void Cadastra_EmailComMaiusculasEEspacos_SalvaENviarNormalizado()
    {
        _servico.Cadastrar("        CALORS     C#     ", "     caca@email.com");
        
        _repo.Received(1).Adicionar(new Usuario("Carlos C#", "caca@email.com"));
        
        _email.Received(1).Enviar("caca@email.com", "Bem-vindo!");
    }
    
    // EMail duplicado

    [Fact]
    public void Cadstar_EMailJaExiste_LancaExcecaoENaoSalvaNemEnvia()
    {
        _repo.EmailJaExiste("caca@email.com").Returns(true);

        Assert.Throws< InvalidOperationException>(() => _servico.Cadastrar("Carlos C#", "caca@email.com"));
        
        _repo.DidNotReceive().Adicionar(Arg.Any<Usuario>());
        _email.DidNotReceiveWithAnyArgs().Enviar(default!, default);
    }

    [Theory]
    [InlineData("", "caca@email.com")]
    [InlineData("      ", "caca@email.com")]
    [InlineData(null, "caca@email.com")]
    [InlineData("Calors C#", "")]
    [InlineData("Calors C#", "cacaemail.com")]
    public void Cadastrar_DadosInvalidos_NaoTocaEmNenhumaDependencia(string? nome, string? email)
    {
        Assert.Throws<ArgumentException>(()=> _servico.Cadastrar(nome, email));
        
        Assert.Empty(_repo.ReceivedCalls());
        Assert.Empty(_email.ReceivedCalls());
        Assert.Empty((_log).ReceivedCalls());
    }

    [Fact]
    public void Cadastrar_FalhaNoEmail_UsuarioContinuaCadastradoEErroVaiPraLog()
    {
        _email
            .When(e => e.Enviar(Arg.Any<string>(), Arg.Any<string>()))
            .Do (_ => throw new Exception("SMTP fora do ar"));

        var usuario = _servico.Cadastrar("Calors C#", "caca@email.com");
        
        Assert.NotNull(usuario);
        _repo.Received(1).Adicionar(Arg.Any<Usuario>());
        _log.Received(1).Registrar(Arg.Is<string>(m=>m.Contains("Falha") && m.Contains("SMTP fora do ar")));
    }
    
    
    
    

}