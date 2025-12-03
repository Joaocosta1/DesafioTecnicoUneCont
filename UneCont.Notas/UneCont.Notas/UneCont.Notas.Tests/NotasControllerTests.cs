using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UneCont.Notas.Api.Application.DTOs;
using UneCont.Notas.Api.Application.Interfaces;
using UneCont.Notas.Api.Controllers;
using Xunit;

namespace UneCont.Notas.Tests;

public class NotasControllerTests
{
    private NotasController CriarControllerComMock(out Mock<INotaFiscalService> serviceMock)
    {
        serviceMock = new Mock<INotaFiscalService>();
        return new NotasController(serviceMock.Object);
    }

    [Fact]
    public void Get_DeveRetornarOkComListaDeNotas()
    {
       
        var controller = CriarControllerComMock(out var serviceMock);

        var listaSimulada = new List<NotaFiscalResponseDto>
    {
        new()
        {
            NumeroNota = "NF-001",
            Cliente = "Cliente Teste",
            ValorFormatado = "R$ 100,00",
            DataEmissao = new DateTime(2025, 1, 1),
            DataCadastro = DateTime.UtcNow
        }
    };

        serviceMock
            .Setup(s => s.ListarTodas())
            .Returns(listaSimulada.AsReadOnly());

       
        var result = controller.Get(); 

        
        var okResult = Assert.IsType<OkObjectResult>(result);
        var retorno = Assert.IsAssignableFrom<IEnumerable<NotaFiscalResponseDto>>(okResult.Value);

        Assert.Single(retorno);
    }

    [Fact]
    public void Post_ComModelValido_DeveRetornarCreated()
    {
       
        var controller = CriarControllerComMock(out var serviceMock);

        var dtoEntrada = new NotaFiscalCreateRequestDto
        {
            NumeroNota = "NF-002",
            Cliente = "Cliente 2",
            Valor = 50.0m,
            DataEmissao = new DateTime(2025, 2, 1)
        };

        var respostaService = new NotaFiscalResponseDto
        {
            NumeroNota = dtoEntrada.NumeroNota,
            Cliente = dtoEntrada.Cliente,
            ValorFormatado = "R$ 50,00",
            DataEmissao = dtoEntrada.DataEmissao,
            DataCadastro = DateTime.UtcNow
        };

        serviceMock
            .Setup(s => s.Cadastrar(It.IsAny<NotaFiscalCreateRequestDto>()))
            .Returns(respostaService);
        
        var result = controller.Post(dtoEntrada);
       
        var createdResult = Assert.IsType<CreatedResult>(result);
        var retorno = Assert.IsType<NotaFiscalResponseDto>(createdResult.Value);

        Assert.Equal("NF-002", retorno.NumeroNota);
        Assert.Equal("Cliente 2", retorno.Cliente);
    
        serviceMock.Verify(s => s.Cadastrar(It.IsAny<NotaFiscalCreateRequestDto>()), Times.Once);
    }

    [Fact]
    public void Post_ComModelInvalido_DeveRetornarBadRequest()
    {

        var controller = CriarControllerComMock(out var _);

        var dtoEntrada = new NotaFiscalCreateRequestDto();

        controller.ModelState.AddModelError("NumeroNota", "Obrigatório");

        var result = controller.Post(dtoEntrada);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(badRequest.Value);
    }
}
