using Microsoft.AspNetCore.Mvc;

namespace src.application.controllers;

[ApiController]
[Route("/status")]
public class ReadinessController : ControllerBase
{

  private readonly ILogger<ReadinessController> _logger;

  public ReadinessController(ILogger<ReadinessController> logger)
  {
    _logger = logger;
  }

  [HttpGet]
  public IActionResult Ping()
  {
    return Ok(new { status = true });
  }
}
