using UneCont.Notas.Api.Domain.Entities;

namespace UneCont.Notas.Api.Infrastructure.Repositories;

public class InMemoryNotaFiscalRepository : INotaFiscalRepository
{
    private readonly List<NotaFiscal> _list = new();

    public void Adicionar(NotaFiscal n) => _list.Add(n);

    public IReadOnlyCollection<NotaFiscal> ObterTodas() => _list.AsReadOnly();
}
