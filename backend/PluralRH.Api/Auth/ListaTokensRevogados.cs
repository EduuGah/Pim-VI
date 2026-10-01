// -----------------------------------------------------------------------------
// [PESSOA 1] Autenticação: logout
// O JWT é "stateless": depois de emitido, vale até expirar. Para o logout
// funcionar de verdade, guardamos aqui o id (jti) dos tokens encerrados.
// Toda requisição consulta esta lista (ver AutenticacaoExtensions).
// Obs.: fica em memória; num sistema maior usaríamos um cache como o Redis.
// -----------------------------------------------------------------------------
using System.Collections.Concurrent;

namespace PluralRH.Api.Auth;

public class ListaTokensRevogados
{
    // ConcurrentDictionary: seguro para várias requisições ao mesmo tempo
    private readonly ConcurrentDictionary<string, DateTime> _revogados = new();

    public void Revogar(string jti, DateTime expiraEm)
    {
        _revogados[jti] = expiraEm;
        RemoverExpirados();
    }

    public bool EstaRevogado(string jti) => _revogados.ContainsKey(jti);

    // Token que já expirou não precisa mais ficar na lista
    private void RemoverExpirados()
    {
        foreach (var item in _revogados.Where(i => i.Value < DateTime.UtcNow))
            _revogados.TryRemove(item.Key, out _);
    }
}
