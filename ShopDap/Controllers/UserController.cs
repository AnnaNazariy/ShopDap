using Microsoft.AspNetCore.Mvc;
using ShopDap.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using ShopDap.Entities;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ShopDap.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(ILogger<UserController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _unitOfWork.UserRepository.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllUsersAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<User>> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetUserByIdAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult> CreateUserAsync([FromBody] User newUser)
        {
            try
            {
                if (newUser == null)
                {
                    return BadRequest("User object is null.");
                }
                var createdId = await _unitOfWork.UserRepository.AddAsync(newUser);
                var createdUser = await _unitOfWork.UserRepository.GetAsync(createdId);
                return CreatedAtAction(nameof(GetUserByIdAsync), new { id = createdId }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateUserAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<ActionResult> UpdateUserAsync(int id, [FromBody] User updatedUser)
        {
            try
            {
                if (updatedUser == null)
                {
                    return BadRequest("User object is null.");
                }
                var existingUser = await _unitOfWork.UserRepository.GetAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }
                await _unitOfWork.UserRepository.UpdateAsync(updatedUser);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateUserAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult> DeleteUserAsync(int id)
        {
            try
            {
                var existingUser = await _unitOfWork.UserRepository.GetAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }
                await _unitOfWork.UserRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteUserAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
