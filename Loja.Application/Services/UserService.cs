using BCrypt.Net;
using Loja.Application.DTOs.UserDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Loja.Domain.Models.Enums;
using Loja.Infra.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
        public async Task<ResponseModel<UserModel>> BlockUser(BlockUserDto blockUserDto)
        {
            ResponseModel<UserModel> response = new();

            try
            {
                var blockUser = await _context.Users.FirstOrDefaultAsync(bU => bU.Id == blockUserDto.Id);
                if (blockUser == null) 
                {
                    response.Message = "Usuário não encontrado!";
                    response.Status = false;
                    return response;
                }

                blockUser.IsBlocked = blockUserDto.IsBlocked;

                _context.Update(blockUser);
                await _context.SaveChangesAsync();


                if (blockUserDto.IsBlocked == true)
                {
                    response.Dados = blockUser;
                    response.Message = "Usuário bloqueado com sucesso!";
                    response.Status = true;
                    return response;
                }
                else
                {
                    response.Dados = blockUser;
                    response.Message = "Usuário desbloqueado com sucesso!";
                    response.Status = true;
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Status = false;
                return response;
            }

        }

        public async Task<ResponseModel<UserModel>> ApproveBusinessUser(ApproveUserDto approveUserDto)
        {
            ResponseModel<UserModel> response = new();

            try
            {
                var existingUser = await _context.Users.SingleOrDefaultAsync(eU => eU.Id == approveUserDto.Id);
                if (existingUser == null)
                {
                    response.Message = "Usuário não encontrado!";
                    response.Status = false;
                    return response;
                }

                existingUser.Approved = approveUserDto.Approved;

                _context.Update(existingUser);
                await _context.SaveChangesAsync();

                response.Dados = existingUser;
                response.Message = "Usuário aprovado com sucesso!";
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

        public async Task<ResponseModel<UserModel>> CreateAdminUser(CreateUserDto createUserDto)
        {
            ResponseModel<UserModel> response = new();

            try
            {
                var existingUser = await _context.Users.SingleOrDefaultAsync(eU => eU.Email == createUserDto.Email);
                if (existingUser != null)
                {
                    response.Message = "Desculpe, este E-mail já está em uso!";
                    response.Status = false;
                    return response;
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
                var createAdmin = new UserModel()
                {
                    Name = createUserDto.Name,
                    Email = createUserDto.Email,
                    Password = passwordHash,
                    Phone = createUserDto.Phone,
                    Address = createUserDto.Address,
                    UserType = Domain.Models.Enums.UserEnum.admin,
                    Approved = true,
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmationToken = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                };

                _context.Add(createAdmin);
                await _context.SaveChangesAsync();

                response.Dados = createAdmin;
                response.Message = "Usuário administrador criado com sucesso!";
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

        public async Task<ResponseModel<UserModel>> CreateUser(CreateUserDto createUserDto)
        {
            ResponseModel<UserModel> response = new();

            try
            {
                var existingUser = await _context.Users.SingleOrDefaultAsync(eU => eU.Email == createUserDto.Email);
                if (existingUser != null)
                {
                    response.Message = "Desculpe, este E-mail já está em uso!";
                    response.Status = false;
                    return response;
                }

                var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

                var createUser = new UserModel()
                {
                    Name = createUserDto.Name,
                    Email = createUserDto.Email,
                    Password = passwordHash,
                    Phone = createUserDto.Phone,
                    Address = createUserDto.Address,
                    UserType = createUserDto.UserType,
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmationToken = Guid.NewGuid().ToString(),
                    EmailConfirmed = false,
                };

                if (createUser.UserType == Domain.Models.Enums.UserEnum.commonClient) 
                { 
                    createUser.Approved = true;
                } else
                {
                    createUser.Approved = false;
                }

                _context.Add(createUser);
                await _context.SaveChangesAsync();
                await SendConfirmationEmail(createUser);

                response.Message = "Usuário criado com sucesso, por favor, verifique seu e-mail para confirmar o registro!";
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

                response.Message = "Usuário excluído com sucesso!";
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

        public async Task<ResponseModel<string>> RequestPasswordReset(string email)
        {
            var response = new ResponseModel<string>();

            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    response.Message = "E-mail não encontrado.";
                    response.Status = false;
                    return response;
                }

                user.PasswordResetToken = Guid.NewGuid().ToString();
                user.PasswordResetTokenExpires = DateTime.UtcNow.AddHours(1);

                _context.Update(user);
                await _context.SaveChangesAsync();

                var resetLink = $"http://localhost:3000/forgetPassword?token={user.PasswordResetToken}&email={user.Email}";

                var appPassword = Environment.GetEnvironmentVariable("APP_PASSWORD_GOOGLE");
                if (string.IsNullOrEmpty(appPassword))
                {
                    response.Message = "Erro interno: Senha do aplicativo não configurada.";
                    response.Status = false;
                    return response;
                }

                // Configurar o cliente SMTP
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587; // Porta para envio SMTP, geralmente 587 ou 465
                    smtpClient.Credentials = new NetworkCredential("seuemail@gmail.com", appPassword);
                    smtpClient.EnableSsl = true;

                    // Configurar o e-mail a ser enviado
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("seuemail@gmail.com", "Nome do seu negocio"),
                        Subject = "Redefinição de senha",
                        Body = $"Olá {user.Name},\n\nPor favor, redefina sua senha clicando no link abaixo:\n{resetLink}",
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(user.Email);

                    // Enviar o e-mail
                    await smtpClient.SendMailAsync(mailMessage);
                }

                response.Message = "E-mail de redefinição de senha enviado.";
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Erro interno: {ex.Message}";
                response.Status = false;
            }

            return response;
        }

        public async Task<ResponseModel<string>> ResetPassword(string email, string token, string newPassword)
        {
            var response = new ResponseModel<string>();

            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Email == email && u.PasswordResetToken == token);

                if (user == null || user.PasswordResetTokenExpires < DateTime.UtcNow)
                {
                    response.Message = "Token inválido ou expirado.";
                    response.Status = false;
                    return response;
                }

                // Atualizar a senha
                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpires = null;

                _context.Update(user);
                await _context.SaveChangesAsync();

                response.Message = "Senha redefinida com sucesso.";
                response.Status = true;
            }
            catch (Exception ex)
            {
                response.Message = $"Erro interno: {ex.Message}";
                response.Status = false;
            }

            return response;
        }

        public async Task<ResponseModel<UserModel>> UpdateCurrentUser(UpdateUserDto updateUserDto)
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
                    response.Message = "Nenhum usuário localizado!";
                    return response;
                }

                user.Name = updateUserDto.Name;
                user.Phone = updateUserDto.Phone;
                user.Address = updateUserDto.Address;

                _context.Update(user);
                await _context.SaveChangesAsync();

                response.Dados = user;
                response.Message = "Dados atualizados com sucesso!";
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

        public async Task<ResponseModel<UserModel>> GetUserById(int userId)
        {
            ResponseModel<UserModel> response = new();

            try
            {
                var getUserById = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (getUserById == null) {
                    response.Message = "Usuário não encontraado!";
                    return response;
                }

                response.Dados = getUserById;
                response.Message = "Usuário encontrado com sucesso!";
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

        public async Task<ResponseModel<List<UserModel>>> GetBusinessUsersPendingApproval()
        {
            ResponseModel<List<UserModel>> response = new();

            try
            {
                var getBusinessUsersPendingApproval = await _context.Users.Where(u => u.Approved == false).ToListAsync();

                if (getBusinessUsersPendingApproval == null) 
                {
                    response.Message = "Nenhum usuário pendente de aprovação!";
                    return response;
                }

                response.Dados = getBusinessUsersPendingApproval;
                response.Message = "Usuários pendentes de aprovação coletados com sucesso!";
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

        public async Task<ResponseModel<List<UserModel>>> GetBlockedUsers()
        {
            ResponseModel<List<UserModel>> response = new();

            try
            {
                var getBlockedUsers = await _context.Users.Where(u => u.IsBlocked == true).ToListAsync();

                if (getBlockedUsers == null)
                {
                    response.Message = "Nenhum usuário bloqueado!";
                    return response;
                }
                response.Dados = getBlockedUsers;
                response.Message = "Usuários bloqueados coletados com sucesso!";
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

        private async Task SendConfirmationEmail(UserModel user)
        {
            try
            {
                var confirmationLink = $"http://localhost:3000/confirmEmail?token={user.EmailConfirmationToken}";

                var appPassword = Environment.GetEnvironmentVariable("APP_PASSWORD_GOOGLE");
                if (string.IsNullOrEmpty(appPassword))
                {
                    Console.WriteLine("Senha APP não configurada");
                }

                // Configurar o cliente SMTP
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587; // Porta para envio SMTP, geralmente 587 ou 465
                    smtpClient.Credentials = new NetworkCredential("seuEmail@email.com", appPassword);
                    smtpClient.EnableSsl = true;

                    // Configurar o e-mail a ser enviado
                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("seuEmail@email.com", "Nome do seu negócio"),
                        Subject = "Confirmação de E-mail",
                        Body = $"Olá {user.Name},\n\nPor favor, confirme seu e-mail clicando no link abaixo:\n{confirmationLink}",
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(user.Email);

                    // Enviar o e-mail
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Tratamento de erro no envio de e-mail (registrar erro, etc)
                Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
            }
        }
    }
}
