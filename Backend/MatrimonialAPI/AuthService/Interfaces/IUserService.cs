﻿using AuthService.Models;
using AuthService.Models.DTOs;

namespace AuthService.Interfaces
{
    public interface IUserService
    {
        public Task<ReturnDTO> Login(UserLoginDTO loginDTO);
        public Task<ReturnDTO> Register(UserRegisterDTO userDTO);
        public Task<string> UpdateUserStatus(UpdateUserStatusDTO updateuserstatusDTO);
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<string> UpdateUserRole(int userId, string role);
    }
}
