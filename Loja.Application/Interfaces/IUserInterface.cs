﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loja.Application.DTOs.UserDTOs;
using Loja.Domain.Models;

namespace Loja.Application.Interfaces
{
    public interface IUserInterface
    {
        Task<ResponseModel<List<UserModel>>> GetUsers(string? name, string? email);
        Task<ResponseModel<UserModel>> GetCurrentUser();
        Task<ResponseModel<UserModel>> CreateAdminUser(CreateUserDto createUserDto);
        Task<ResponseModel<UserModel>> CreateCommonUser(CreateUserDto createUserDto);
        Task<ResponseModel<UserModel>> UpdateCurrentUser(UpdateUserDto updateUserDto);
        Task<ResponseModel<UserModel>> DeleteUser(int userId);
        Task<ResponseModel<UserModel>> BlockUser(int userId);
        Task<ResponseModel<string>> RequestPasswordReset(string email);
        Task<ResponseModel<string>> ResetPassword(string email, string token, string newPassword);

    }
}
