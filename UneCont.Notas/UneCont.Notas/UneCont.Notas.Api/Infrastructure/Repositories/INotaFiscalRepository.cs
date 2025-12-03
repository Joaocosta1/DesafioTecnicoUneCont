using UneCont.Notas.Api.Domain.Entities;

namespace UneCont.Notas.Api.Infrastructure.Repositories;

public interface INotaFiscalRepository
{
    void Adicionar(NotaFiscal n);
    IReadOnlyCollection<NotaFiscal> ObterTodas();
}
