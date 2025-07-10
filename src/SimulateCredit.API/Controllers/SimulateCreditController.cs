using Microsoft.AspNetCore.Mvc;
using SimulateCredit.Application.DTOs;
using SimulateCredit.Application.Exceptions;
using SimulateCredit.Application.Ports.Incoming;

namespace SimulateCredit.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SimulateCreditController : ControllerBase
{
    private readonly ISimulateCreditUseCase _useCase;
    private readonly ILogger<SimulateCreditController> _logger;

    public SimulateCreditController(ISimulateCreditUseCase useCase, ILogger<SimulateCreditController> logger)
    {
        _useCase = useCase;
        _logger = logger;
    }

    /// <summary>
    /// Simulate a single loan.
    /// </summary>
    [HttpPost("simulate")]
    public async Task<IActionResult> SimulateAsync([FromBody] SimulateCreditRequest request)
    {
        _logger.LogInformation("Received single simulation request");
        try
        {
            var result = await _useCase.ExecuteAsync(request);
            return Ok(result);
        }
        catch (SimulationException ex)
        {
            _logger.LogError(ex, "Simulation failed");
            return StatusCode(500, ex.Message);
        }
    }

    /// <summary>
    /// Simulates multiple loans in batch.
    /// </summary>
    [HttpPost("simulate/bulk")]
    public async Task<IActionResult> SimulateBulkAsync([FromBody] List<SimulateCreditRequest> requests)
    {
        _logger.LogInformation("Received bulk simulation request with {Count} items", requests.Count);
        try
        {
            var tasks = requests.Select(_useCase.ExecuteAsync);
            var results = await Task.WhenAll(tasks);
            return Ok(results);
        }
        catch (SimulationException ex)
        {
            _logger.LogError(ex, "Bulk simulation failed");
            return StatusCode(500, ex.Message);
        }
    }
}
