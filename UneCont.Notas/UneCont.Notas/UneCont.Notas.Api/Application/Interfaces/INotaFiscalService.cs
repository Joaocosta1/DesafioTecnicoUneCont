using UneCont.Notas.Api.Application.DTOs;

namespace UneCont.Notas.Api.Application.Interfaces;

public interface INotaFiscalService
{
    NotaFiscalResponseDto Cadastrar(NotaFiscalCreateRequestDto dto);
    IReadOnlyCollection<NotaFiscalResponseDto> ListarTodas();
}
