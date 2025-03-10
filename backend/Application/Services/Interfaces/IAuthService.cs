﻿using Application.Dtos;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(SignUpUserDto dto);
        Task<AuthResponseDto> LoginAsync(LoginUserDto dto);
        Task GenerateResetCode(Guid userId);
        Task ChangePasswordAsync(ChangePasswordDto dto);
    }
}
