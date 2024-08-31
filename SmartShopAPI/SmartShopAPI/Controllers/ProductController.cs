using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Models.Dtos.Product;

namespace SmartShopAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("product/all")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("product")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery]string searchPhrase)
        {
            var products = await _productService.GetProductsAsync(searchPhrase);
            return Ok(products);
        }

        [HttpGet("category/{categoryId}/product")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get([FromRoute]int categoryId, [FromQuery]QueryParams query)
        {
            var products = await _productService.GetAsync(categoryId, query);
            return Ok(products);
        }

        [HttpGet("product/{productId}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDto>> GetById([FromRoute]int productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            return Ok(product);
        }

        [HttpPost(Name="CreateProduct")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Create([FromRoute]int categoryId, [FromBody]CreateProductDto dto, IFormFile? file)
        {
            var productId = await _productService.CreateAsync(categoryId, dto, file);
            return Created($"category/{categoryId}/product/{productId}", null);
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete([FromRoute]int categoryId, [FromRoute]int productId) 
        {
            await _productService.DeleteAsync(categoryId, productId);
            return NoContent();
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Update([FromRoute]int productId, UpdateProductDto dto)
        {
            await _productService.UpdateAsync(productId, dto);
            return NoContent();
        }
    }
}
