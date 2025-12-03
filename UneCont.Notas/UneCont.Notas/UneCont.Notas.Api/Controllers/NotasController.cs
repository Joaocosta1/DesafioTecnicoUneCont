using Microsoft.AspNetCore.Mvc;
using UneCont.Notas.Api.Application.DTOs;
using UneCont.Notas.Api.Application.Interfaces;

namespace UneCont.Notas.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotasController : ControllerBase
{
    private readonly INotaFiscalService _svc;

    public NotasController(INotaFiscalService svc)
    {
        _svc = svc;
    }

    [HttpPost]
    public IActionResult Post([FromBody] NotaFiscalCreateRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = _svc.Cadastrar(dto);
        return Created("", result);
    }

    [HttpGet]
    public IActionResult Get() => Ok(_svc.ListarTodas());
}
