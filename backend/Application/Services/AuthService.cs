﻿using Application.Dtos;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordRecoveryService _passwordRecovery;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            IJwtProvider jwtProvider,
            IPasswordRecoveryService passwordRecovery)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _passwordRecovery = passwordRecovery;
        }

        public async Task<AuthResponseDto> RegisterAsync(SignUpUserDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmail(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Surname = dto.Surname,
                Patronymic = dto.Patronymic,
                Position = dto.Position,
                Email = dto.Email
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.AddUser(user);

            var token = _jwtProvider.GenerateToken(user);
            return new AuthResponseDto { Token = token };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
        {
            var user = await _userRepository.GetUserByEmail(dto.Email);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid email or password.");
            }

            var token = _jwtProvider.GenerateToken(user);
            return new AuthResponseDto { Token = token };
        }

        public async Task GenerateResetCode(Guid userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                throw new Exception("User not exists.");
            }
            string reserCode = await _passwordRecovery.SendCodeAsync(user.Email);
            user.ResetCode = reserCode;
            user.ResetCodeExpiration = DateTime.UtcNow.AddMinutes(15);
            await _userRepository.UpdateUser(user);
        }

        public async Task ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userRepository.GetUserById(dto.UserId);
            if (user == null)
            {
                throw new Exception("User not exists.");
            }
            if (user.ResetCode == null || user.ResetCodeExpiration < DateTime.UtcNow || user.ResetCode != dto.Code)
            {
                throw new Exception("False code.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepository.UpdateUser(user);
        }
    }
}
