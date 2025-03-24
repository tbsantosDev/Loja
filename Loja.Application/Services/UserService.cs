using Loja.Application.DTOs.UserDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Loja.Infra.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Loja.Application.Services
{
    public class UserService : IUserInterface
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(AppDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }
        public async Task<ResponseModel<UserModel>> BlockUser(int userId)
        {
            throw new NotImplementedException();

        }

        public Task<ResponseModel<UserModel>> CreateAdminUser(CreateUserDto createUserDto)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserModel>> CreateCommonUser(CreateUserDto createUserDto)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseModel<UserModel>> DeleteUser(int userId)
        {
            ResponseModel<UserModel> response = new();

            try
            {
                var deleteUser = await _context.Users.FirstOrDefaultAsync(d => d.Id == userId);
                if (deleteUser == null) 
                {
                    response.Message = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }

                _context.Remove(userId);
                await _context.SaveChangesAsync();

                response.Message = "Usuário excluido com sucesso!";
                response.Status = true;
                return response;


            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UserModel>> GetCurrentUser()
        {
            ResponseModel<UserModel> response = new();

            try
            {
                var userIdClaim = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    response.Message = "Usuário não encontrado.";
                    response.Status = false;
                    return response;
                }

                var userId = int.Parse(userIdClaim.Value);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    response.Message = "Usuário não localizado!";
                    return response;
                }
                response.Dados = user;
                response.Message = "Dados do Usuário coletado com sucesso!";
                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<List<UserModel>>> GetUsers(string? name, string? email)
        {
            ResponseModel<List<UserModel>> response = new();

            try
            {
                var getUsers = _context.Users.AsQueryable();

                if (!string.IsNullOrEmpty(name))
                {
                    getUsers = getUsers.Where(u => u.Name.Contains(name));
                }
                if (!string.IsNullOrEmpty(email))
                {
                    getUsers = getUsers.Where(u => u.Email.Contains(email));
                }

                response.Dados = await getUsers.ToListAsync();
                response.Status = true;
                response.Message = "Dados coletados com sucesso!";

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public Task<ResponseModel<string>> RequestPasswordReset(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<string>> ResetPassword(string email, string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<UserModel>> UpdateCurrentUser(UpdateUserDto updateUserDto)
        {
            throw new NotImplementedException();
        }
    }
}
