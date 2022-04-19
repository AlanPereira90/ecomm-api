using Microsoft.AspNetCore.Mvc;

namespace src.application.controllers;

[ApiController]
[Route("/status")]
public class ReadinessController : ControllerBase
{
  [HttpGet]
  public IActionResult Ping()
  {
    return Ok(new { status = true });
  }
}
