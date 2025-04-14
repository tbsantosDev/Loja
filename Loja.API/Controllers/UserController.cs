using Loja.Application.DTOs.UserDTOs;
using Loja.Application.Interfaces;
using Loja.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WireMock.Admin.Mappings;

namespace Loja.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInterface _userInterface;

        public UserController(IUserInterface userInterface)
        {
            _userInterface = userInterface;
        }

        [HttpPut("BlockUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> BlockUser(BlockUserDto blockUserDto)
        {
            try
            {
                var blockUser = await _userInterface.BlockUser(blockUserDto);
                return Ok(blockUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [Authorize(Roles = "admin")]
        [HttpPut("ApproveBusinessUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> ApproveBusinessUser(ApproveUserDto approveUserDto)
        {
            try
            {
                var approveBusinessUser = await _userInterface.ApproveBusinessUser(approveUserDto);
                return Ok(approveBusinessUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost("CreateAdminUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> CreateAdminUser(CreateUserDto createUserDto)
        {
            try
            {
                var createAdminUser = await _userInterface.CreateAdminUser(createUserDto);
                return Ok(createAdminUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteUser/{userId}")]
        public async Task<ActionResult<ResponseModel<UserModel>>> DeleteUser(int userId)
        {
            try
            {
                var deleteUser = await _userInterface.DeleteUser(userId);
                return Ok(deleteUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> GetCurrentUser()
        {
            try
            {
                var getCurrentUser = await _userInterface.GetCurrentUser();
                return Ok(getCurrentUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetUsers")]
        public async Task<ActionResult<ResponseModel<List<UserModel>>>> GetUsers(string? name, string? email)
        {
            try
            {
                var getUsers = await _userInterface.GetUsers(name, email);
                return Ok(getUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("RequestPasswordReset")]
        public async Task<ActionResult<ResponseModel<string>>> RequestPasswordReset([FromBody] string email)
        {
            try
            {
                var requestPasswordReset = await _userInterface.RequestPasswordReset(email);
                return Ok(requestPasswordReset);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ResetPassword")]
        public async Task<ActionResult<ResponseModel<string>>> ResetPassword([FromBody] ResetUserPasswordDto resetUserPasswordDto)
        {
            try
            {
                var ResetPassword = await _userInterface.ResetPassword(
                    resetUserPasswordDto.Email,
                    resetUserPasswordDto.Token,
                    resetUserPasswordDto.NewPassword
                );
                return Ok(ResetPassword);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        [HttpPut("UpdateCurrentUser")]
        public async Task<ActionResult<ResponseModel<UserModel>>> UpdateCurrentUser(UpdateUserDto updateUserDto)
        {
            try
            {
                var updateCurrentUser = await _userInterface.UpdateCurrentUser(updateUserDto);
                return Ok(updateCurrentUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetUserById/{userId}")]
        public async Task<ActionResult<ResponseModel<UserModel>>> GetUserById(int userId)
        {
            try
            {
                var getUserById = await _userInterface.GetUserById(userId);
                return Ok(getUserById);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetBusinessUsersPendingApproval")]
        public async Task<ActionResult<List<ResponseModel<UserModel>>>> GetBusinessUsersPendingApproval()
        {
            try
            {
                var getBusinessUsersPendingApproval = await _userInterface.GetBusinessUsersPendingApproval();
                return Ok(getBusinessUsersPendingApproval);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = "admin")]
        [HttpGet("GetBlockedUsers")]
        public async Task<ActionResult<List<ResponseModel<UserModel>>>> GetBlockedUsers()
        {
            try
            {
                var getBlockedUsers = await _userInterface.GetBlockedUsers();
                return Ok(getBlockedUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
