using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Autenticacao/CriarTokenUtilizacaoServico")]
    public async Task<IActionResult> Authenticate([FromBody] AuthRequestDto request)
    {
        var result = await _authService.AuthenticateAsync(request);
        if (!string.IsNullOrEmpty(result.Error))
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}