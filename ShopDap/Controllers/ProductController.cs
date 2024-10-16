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
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(ILogger<ProductController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllProductsAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Product>> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetProductByIdAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult> CreateProductAsync([FromBody] Product newProduct)
        {
            try
            {
                if (newProduct == null)
                {
                    return BadRequest("Product object is null.");
                }
                var createdId = await _unitOfWork.ProductRepository.AddAsync(newProduct);
                var createdProduct = await _unitOfWork.ProductRepository.GetAsync(createdId);
                return CreatedAtAction(nameof(GetProductByIdAsync), new { id = createdId }, createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreateProductAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<ActionResult> UpdateProductAsync(int id, [FromBody] Product updatedProduct)
        {
            try
            {
                if (updatedProduct == null)
                {
                    return BadRequest("Product object is null.");
                }
                var existingProduct = await _unitOfWork.ProductRepository.GetAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }
                await _unitOfWork.ProductRepository.UpdateAsync(updatedProduct);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in UpdateProductAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<ActionResult> DeleteProductAsync(int id)
        {
            try
            {
                var existingProduct = await _unitOfWork.ProductRepository.GetAsync(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }
                await _unitOfWork.ProductRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in DeleteProductAsync: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
