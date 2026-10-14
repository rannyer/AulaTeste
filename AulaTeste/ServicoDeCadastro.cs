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

    public Usuario Cadastrar(string nome, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("E-mail inválido.", nameof(email));

        var emailNormalizado = email.Trim().ToLowerInvariant();

        if (_repo.EmailJaExiste(emailNormalizado))
            throw new InvalidOperationException("E-mail já cadastrado.");

        var usuario = new Usuario(nome.Trim(), emailNormalizado);

        _repo.Adicionar(usuario);
        _log.Registrar($"Usuário {emailNormalizado} cadastrado");

        try
        {
            _email.Enviar(emailNormalizado, "Bem-vindo!");
        }
        catch (Exception ex)
        {
            // O cadastro já foi feito; falha no e-mail não pode desfazer isso
            _log.Registrar($"Falha ao enviar boas-vindas para {emailNormalizado}: {ex.Message}");
        }

        return usuario;
    }
}