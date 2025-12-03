namespace UneCont.Notas.Api.Application.DTOs;

public class NotaFiscalResponseDto
{
    public string NumeroNota { get; set; } = string.Empty;
    public string Cliente { get; set; } = string.Empty;
    public string ValorFormatado { get; set; } = string.Empty;
    public DateTime DataEmissao { get; set; }
    public DateTime DataCadastro { get; set; }
}
