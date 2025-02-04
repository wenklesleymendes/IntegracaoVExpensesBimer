public interface IAuthService
{
    Task<AuthResponseDto> AuthenticateAsync(AuthRequestDto requestDto);
}