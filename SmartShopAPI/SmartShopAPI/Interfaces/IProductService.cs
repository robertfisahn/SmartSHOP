using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SmartShopAPI.Models.Dtos;
using SmartShopAPI.Models.Dtos.Product;

namespace SmartShopAPI.Interfaces
{
    public interface IProductService
    {
        Task<int> CreateAsync(int categoryId, CreateProductDto dto, IFormFile? file);
        Task DeleteAsync(int categoryId, int productId);
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<List<ProductDto>> GetProductsAsync(string searchPhrase);
        Task<PagedResult<ProductDto>> GetAsync(int categoryId, QueryParams query);
        Task<ProductDto> GetByIdAsync(int productId);
        Task UpdateAsync(int productId, UpdateProductDto dto);
    }
}