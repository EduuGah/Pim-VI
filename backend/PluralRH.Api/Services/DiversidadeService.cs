// -----------------------------------------------------------------------------
// [PESSOA 1] Regras do sistema: diversidade e inclusão
// cadastrar conteúdos, disponibilizar materiais, cadastrar ações
// (Os treinamentos de D&I usam o TreinamentoService com a categoria
//  DiversidadeInclusao, e a participação é acompanhada no Dashboard.)
// -----------------------------------------------------------------------------
using PluralRH.Api.DTOs;
using PluralRH.Api.Excecoes;
using PluralRH.Data.Models;
using PluralRH.Data.Repositories;

namespace PluralRH.Api.Services;

public class DiversidadeService
{
    private readonly MaterialRepositorio _materiais;
    private readonly IRepositorio<AcaoInclusao> _acoes;
    private readonly IRepositorio<Treinamento> _treinamentos;

    public DiversidadeService(MaterialRepositorio materiais, IRepositorio<AcaoInclusao> acoes,
                              IRepositorio<Treinamento> treinamentos)
    {
        _materiais = materiais;
        _acoes = acoes;
        _treinamentos = treinamentos;
    }

    // =============================== MATERIAIS ===============================

    public async Task<List<MaterialResposta>> ListarMateriaisAsync(bool somenteAtivos) =>
        (await _materiais.ListarComTreinamentoAsync(somenteAtivos)).Select(MaterialResposta.De).ToList();

    public async Task<MaterialResposta> CriarMaterialAsync(MaterialRequisicao requisicao)
    {
        await ValidarTreinamentoAsync(requisicao.TreinamentoId);

        var material = new MaterialEducativo { PublicadoEm = DateTime.Now };
        PreencherMaterial(material, requisicao);
        await _materiais.AdicionarAsync(material);
        return MaterialResposta.De(material);
    }

    public async Task<MaterialResposta> AtualizarMaterialAsync(int id, MaterialRequisicao requisicao)
    {
        await ValidarTreinamentoAsync(requisicao.TreinamentoId);

        var material = await _materiais.BuscarPorIdAsync(id)
            ?? throw new NaoEncontradoException("Material não encontrado.");
        PreencherMaterial(material, requisicao);
        await _materiais.AtualizarAsync(material);
        return MaterialResposta.De(material);
    }

    public async Task ExcluirMaterialAsync(int id)
    {
        var material = await _materiais.BuscarPorIdAsync(id)
            ?? throw new NaoEncontradoException("Material não encontrado.");
        await _materiais.RemoverAsync(material);
    }

    // ================================ AÇÕES ==================================

    public async Task<List<AcaoResposta>> ListarAcoesAsync() =>
        (await _acoes.ListarAsync()).OrderByDescending(a => a.Data).Select(AcaoResposta.De).ToList();

    public async Task<AcaoResposta> CriarAcaoAsync(AcaoRequisicao requisicao)
    {
        var acao = new AcaoInclusao();
        PreencherAcao(acao, requisicao);
        await _acoes.AdicionarAsync(acao);
        return AcaoResposta.De(acao);
    }

    public async Task<AcaoResposta> AtualizarAcaoAsync(int id, AcaoRequisicao requisicao)
    {
        var acao = await _acoes.BuscarPorIdAsync(id)
            ?? throw new NaoEncontradoException("Ação não encontrada.");
        PreencherAcao(acao, requisicao);
        await _acoes.AtualizarAsync(acao);
        return AcaoResposta.De(acao);
    }

    public async Task ExcluirAcaoAsync(int id)
    {
        var acao = await _acoes.BuscarPorIdAsync(id)
            ?? throw new NaoEncontradoException("Ação não encontrada.");
        await _acoes.RemoverAsync(acao);
    }

    // ------------------------------------------------------------------ regras

    private async Task ValidarTreinamentoAsync(int? treinamentoId)
    {
        if (treinamentoId is int id && await _treinamentos.BuscarPorIdAsync(id) is null)
            throw new RegraNegocioException("Treinamento vinculado não encontrado.");
    }

    private static void PreencherMaterial(MaterialEducativo material, MaterialRequisicao requisicao)
    {
        material.Titulo = requisicao.Titulo.Trim();
        material.Descricao = requisicao.Descricao.Trim();
        material.Tipo = requisicao.Tipo!.Value;
        material.Tema = requisicao.Tema.Trim();
        material.Link = string.IsNullOrWhiteSpace(requisicao.Link) ? null : requisicao.Link.Trim();
        material.TreinamentoId = requisicao.TreinamentoId;
        material.Ativo = requisicao.Ativo;
    }

    private static void PreencherAcao(AcaoInclusao acao, AcaoRequisicao requisicao)
    {
        // REGRA: uma ação só pode ter participantes depois de realizada
        if (requisicao.Status != StatusAcao.Realizada && requisicao.Participantes > 0)
            throw new RegraNegocioException("Só informe participantes quando a ação estiver como 'Realizada'.");

        acao.Titulo = requisicao.Titulo.Trim();
        acao.Descricao = requisicao.Descricao.Trim();
        acao.Tipo = requisicao.Tipo!.Value;
        acao.Data = requisicao.Data!.Value;
        acao.Local = requisicao.Local.Trim();
        acao.Responsavel = requisicao.Responsavel.Trim();
        acao.Participantes = requisicao.Participantes;
        acao.Status = requisicao.Status;
    }
}
