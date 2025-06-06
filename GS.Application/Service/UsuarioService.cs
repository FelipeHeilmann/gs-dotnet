namespace GS.Application.Service;

public class UsuarioService
{
    private const string UsuarioPadrao = "rm551026";
    private const string SenhaPadrao = "admin1234!";

    public bool Autenticar(string usuario, string senha)
    {
        return usuario == UsuarioPadrao && senha == SenhaPadrao;
    }
}
