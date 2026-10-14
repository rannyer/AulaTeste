namespace AulaTeste;

public record Usuario(string Nome, string Email);

public interface IRepositorioDeUsuarios
{
    bool EmailJaExiste(string email);
    void Adicionar(Usuario usuario);
}

public interface IEnviadorDeEmail
{
    void Enviar(string para, string assunto);
}

public interface ILogDeAuditoria
{
    void Registrar(string mensagem);
}

public class ServicoDeCadastro
{
    private readonly IRepositorioDeUsuarios _repo;
    private readonly IEnviadorDeEmail _email;
    private readonly ILogDeAuditoria _log;

    public ServicoDeCadastro(IRepositorioDeUsuarios repo, IEnviadorDeEmail email, ILogDeAuditoria log)
    {
        _repo = repo;
        _email = email;
        _log = log;
    }

    public void Cadastrar(string nome, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.");

        if (_repo.EmailJaExiste(email))
            throw new InvalidOperationException("E-mail já cadastrado.");

        _repo.Adicionar(new Usuario(nome, email));
        _email.Enviar(email, "Bem-vindo!");
        _log.Registrar($"Usuário {email} cadastrado");
    }
}
