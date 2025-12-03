using System;
using UneCont.Notas.Api.Application.DTOs;
using UneCont.Notas.Api.Application.Services;
using UneCont.Notas.Api.Infrastructure.Repositories;
using Xunit;

public class NotaFiscalServiceTests
{
    [Fact]
    public void Deve_Cadastrar_Nota_Com_Sucesso()
    {
        var repo = new InMemoryNotaFiscalRepository();
        var svc = new NotaFiscalService(repo);

        var dto = new NotaFiscalCreateRequestDto
        {
            NumeroNota = "NF-1",
            Cliente = "Cliente Teste",
            Valor = 100,
            DataEmissao = new DateTime(2025,1,1)
        };

        var r = svc.Cadastrar(dto);
        var all = svc.ListarTodas();

        Assert.Single(all);
        Assert.Contains("100", r.ValorFormatado);
    }
}
