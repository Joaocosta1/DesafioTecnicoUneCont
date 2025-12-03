using System.ComponentModel.DataAnnotations;

namespace UneCont.Notas.Api.Application.DTOs;

public class NotaFiscalCreateRequestDto
{
    [Required]
    public string NumeroNota { get; set; } = string.Empty;
    [Required]
    public string Cliente { get; set; } = string.Empty;
    [Range(0.01, double.MaxValue)]
    public decimal Valor { get; set; }
    [Required]
    public DateTime DataEmissao { get; set; }
}
