using Microsoft.AspNetCore.Mvc;
using SimulateCredit.Application.DTOs;
using SimulateCredit.Application.Ports.Incoming;

namespace SimulateCredit.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SimulateCreditController : ControllerBase
{
    private readonly ISimulateCreditUseCase _useCase;

    public SimulateCreditController(ISimulateCreditUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>
    /// Simula um único empréstimo.
    /// </summary>
    [HttpPost("simulate")]
    public async Task<IActionResult> SimulateAsync([FromBody] SimulateCreditRequest request)
    {
        var result = await _useCase.ExecuteAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Simula múltiplos empréstimos em lote.
    /// </summary>
    [HttpPost("simulate/bulk")]
    public async Task<IActionResult> SimulateBulkAsync([FromBody] List<SimulateCreditRequest> requests)
    {
        var tasks = requests.Select(_useCase.ExecuteAsync);
        var results = await Task.WhenAll(tasks);
        return Ok(results);
    }
}
