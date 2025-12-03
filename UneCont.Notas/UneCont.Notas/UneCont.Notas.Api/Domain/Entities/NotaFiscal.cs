namespace UneCont.Notas.Api.Domain.Entities;

public class NotaFiscal
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string NumeroNota { get; private set; }
    public string Cliente { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataEmissao { get; private set; }
    public DateTime DataCadastro { get; private set; }

    public NotaFiscal(string numeroNota, string cliente, decimal valor, DateTime dataEmissao)
    {
        if (string.IsNullOrWhiteSpace(numeroNota))
            throw new ArgumentException("Número da nota é obrigatório.", nameof(numeroNota));
        if (string.IsNullOrWhiteSpace(cliente))
            throw new ArgumentException("Cliente é obrigatório.", nameof(cliente));
        if (valor <= 0)
            throw new ArgumentException("Valor deve ser maior que zero.", nameof(valor));

        NumeroNota = numeroNota;
        Cliente = cliente;
        Valor = valor;
        DataEmissao = dataEmissao;
        DataCadastro = DateTime.UtcNow;
    }
}
