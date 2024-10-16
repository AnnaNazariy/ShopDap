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
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(ILogger<OrderController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllOrders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetAllAsync();
                _logger.LogInformation("Returned all orders from database.");
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllOrdersAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Order>> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetAsync(id);
                if (order == null)
                {
                    _logger.LogError($"Order with id: {id} not found.");
                    return NotFound();
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetOrderByIdAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult> CreateOrderAsync([FromBody] Order newOrder)
        {
            try
            {
                if (newOrder == null)
                {
                    return BadRequest("Order object is null.");
                }
                var createdId = await _unitOfWork.OrderRepository.AddAsync(newOrder);
                var createdOrder = await _unitOfWork.OrderRepository.GetAsync(createdId);
                return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = createdId }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateOrderAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("UpdateOrder/{id}")]
        public async Task<ActionResult> UpdateOrderAsync(int id, [FromBody] Order updatedOrder)
        {
            try
            {
                if (updatedOrder == null)
                {
                    return BadRequest("Order object is null.");
                }
                var existingOrder = await _unitOfWork.OrderRepository.GetAsync(id);
                if (existingOrder == null)
                {
                    return NotFound();
                }
                await _unitOfWork.OrderRepository.UpdateAsync(updatedOrder);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateOrderAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("DeleteOrder/{id}")]
        public async Task<ActionResult> DeleteOrderAsync(int id)
        {
            try
            {
                var existingOrder = await _unitOfWork.OrderRepository.GetAsync(id);
                if (existingOrder == null)
                {
                    return NotFound();
                }
                await _unitOfWork.OrderRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteOrderAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
