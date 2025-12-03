using System.Globalization;
using UneCont.Notas.Api.Application.DTOs;
using UneCont.Notas.Api.Application.Interfaces;
using UneCont.Notas.Api.Domain.Entities;
using UneCont.Notas.Api.Infrastructure.Repositories;

namespace UneCont.Notas.Api.Application.Services;

public class NotaFiscalService : INotaFiscalService
{
    private readonly INotaFiscalRepository _repo;
    private readonly CultureInfo _culture = new("pt-BR");

    public NotaFiscalService(INotaFiscalRepository repo)
    {
        _repo = repo;
    }

    public NotaFiscalResponseDto Cadastrar(NotaFiscalCreateRequestDto dto)
    {
        var nf = new NotaFiscal(dto.NumeroNota, dto.Cliente, dto.Valor, dto.DataEmissao);
        _repo.Adicionar(nf);
        return Map(nf);
    }

    public IReadOnlyCollection<NotaFiscalResponseDto> ListarTodas()
        => _repo.ObterTodas().Select(Map).ToList().AsReadOnly();

    private NotaFiscalResponseDto Map(NotaFiscal n)
        => new()
        {
            NumeroNota = n.NumeroNota,
            Cliente = n.Cliente,
            ValorFormatado = n.Valor.ToString("C", _culture),
            DataEmissao = n.DataEmissao,
            DataCadastro = n.DataCadastro
        };
}
