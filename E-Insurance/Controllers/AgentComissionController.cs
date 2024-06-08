/*using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AgentCommissionController : ControllerBase
{
    private readonly IAgentCommission _agentCommissionService;

    public AgentCommissionController(IAgentCommission agentCommissionService)
    {
        _agentCommissionService = agentCommissionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAgentComission()
    {
        try
        {
            var agentCommissions = await _agentCommissionService.GetAllAgentComission();
            return Ok(agentCommissions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while retrieving agent commissions: {ex.Message}");
        }
    }
}
*/