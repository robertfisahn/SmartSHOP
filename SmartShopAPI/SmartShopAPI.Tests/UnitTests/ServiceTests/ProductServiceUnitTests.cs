using Moq;
using AutoMapper;
using SmartShopAPI.Models;
using SmartShopAPI.Models.Dtos.Product;
using SmartShopAPI.Services;
using SmartShopAPI.Tests.Helpers;
using SmartShopAPI.Data;
using SmartShopAPI.Exceptions;
using SmartShopAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using Moq.EntityFrameworkCore;
using FluentAssertions;

namespace SmartShopAPI.Tests
{
    public class ProductServiceUnitTests
    {
        private readonly Mock<SmartShopDbContext> _mockContext;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductService _service;
        private readonly List<Product> _products;

        public ProductServiceUnitTests()
        {
            _mockContext = MockDbContext.CreateMockDbContext();
            _mockMapper = new Mock<IMapper>();
            _service = new ProductService(_mockContext.Object, _mockMapper.Object);

            var categories = new List<Category> {
                new () { Id = 1, Name = "PS4" },
                new () { Id = 2, Name = "PS5" }
             };
            _products = new List<Product>
            {
                new () { Id = 1, Name = "The Last of Us Part II Remastered", Price = 199.99M, CategoryId = 2, ImagePath = "images/products/defaultt.jpg" },
                new () { Id = 2, Name = "God of War", Price = 149.99M, CategoryId = 1 },
                new () { Id = 3, Name = "Spider-Man 2", Price = 100.00M, CategoryId = 1 }
            };
            var mockProducts = MockDbContext.CreateMockDbSet(_products);
            var mockCategories = MockDbContext.CreateMockDbSet(categories);

            _mockContext.Setup(c => c.Products).ReturnsDbSet(mockProducts);
            _mockContext.Setup(c => c.Categories).ReturnsDbSet(mockCategories);
            _mockContext.Setup(c => c.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                .Callback((Product p, CancellationToken ct) =>
                {
                    p.Id = _products.Max(x => x.Id) + 1;
                    _products.Add(p);
                });
            _mockContext.Setup(c => c.Products.Remove(It.IsAny<Product>()))
                .Callback((Product p) =>
                {
                    var productToRemove = _products.FirstOrDefault(p => p.Id == p.Id);
                    if (productToRemove != null)
                    {
                        _products.Remove(productToRemove);
                    }
                });
            _mockMapper.Setup(m => m.Map<ProductDto>(It.IsAny<Product>()))
                .Returns((Product source) => new ProductDto { Id = source.Id, Name = source.Name });

            _mockMapper.Setup(m => m.Map<Product>(It.IsAny<UpsertProductDto>()))
                .Returns((UpsertProductDto dto) => new Product { Name = dto.Name, CategoryId = 1 });

            _mockMapper.Setup(m => m.Map(It.IsAny<UpsertProductDto>(), It.IsAny<Product>()))
                .Callback((UpsertProductDto dto, Product product) => {
                    product.Name = dto.Name;
                });
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(88, true)]
        public async Task CheckCategory_Validation(int categoryId, bool shouldThrow)
        {
            Func<Task> act = () => _service.CheckCategory(categoryId);

            if (shouldThrow)
            {
                await act.Should().ThrowAsync<NotFoundException>();
            }
            else
            {
                await act.Should().NotThrowAsync();
            }
        }

        [Fact]
        public async Task GetById_ExistingProduct() 
        {
            var existingProduct = await _service.GetByIdAsync(1);
            Assert.NotNull(existingProduct);
            Assert.Equal(1, existingProduct.Id);
        }

        [Fact]
        public async Task GetById_NonExistingProduct_ThrowsNotFoundException()
        {
            var nonExistingProductId = 88;
            await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(nonExistingProductId));
        }

        [Fact]
        public async Task Create_Product_Successfully()
        {
            var newProduct = new UpsertProductDto { Name = "Spider-man 3", CategoryId = 1 };
            var newProductId = await _service.CreateAsync(newProduct, null);
            var createdProduct = await _service.GetByIdAsync(newProductId);
            Assert.NotNull(createdProduct);
            _mockContext.Verify(c => c.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Product_Successfully()
        {
            var existingProduct = await _service.GetByIdAsync(1);
            await _service.DeleteAsync(existingProduct.Id);
            var productExists = _mockContext.Object.Products.Any(p => p.Id == existingProduct.Id);
            Assert.False(productExists);
            _mockContext.Verify(c => c.Products.Remove(It.Is<Product>(p => p.Id == existingProduct.Id)), Times.Once);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Delete_Product_NonExistingProduct_ThrowsNotFoundException()
        {
            var nonExistingProductId = 88;
            await Assert.ThrowsAsync<NotFoundException>(() =>  _service.DeleteAsync(nonExistingProductId));
        }

        [Fact]
        public async Task Update_Product_Successfully()
        {
            var dto = new UpsertProductDto { Name = "Update product" };
            await _service.UpdateAsync(1, dto, null);
            var updateProduct = await _mockContext.Object.Products.FirstOrDefaultAsync(p => p.Id == 1);
            Assert.Equal("Update product", updateProduct.Name);
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Update_Product_NonExistingProduct_ThrowsNotFoundException()
        {
            var nonExistingProductId = 99;
            var dto = new UpsertProductDto { Name = "Update Product" };
            await Assert.ThrowsAsync<NotFoundException>(() => _service.UpdateAsync(nonExistingProductId, dto, null));
            _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task FilterProducts_ReturnsFilteredProducts()
        {
            var result = await _service.FilterProducts(1, "Spider");
            Assert.Single(result);
            Assert.Equal("Spider-Man 2", result[0].Name);
        }

        [Fact]
        public void PaginateProducts_PaginatesCorrectly()
        {
            _products.Add(new Product { Id = 4, Name = "Grand Theft Auto V", Price = 100.00M, CategoryId = 1 });
            int pageNumber = 2;
            int pageSize = 2;
            var paginated = _service.PaginateProducts(_products, pageNumber, pageSize);
            Assert.Equal(2, paginated.Count);
            Assert.Equal("Grand Theft Auto V", paginated[1].Name);
        }

        [Fact]
        public void SortProducts_ByNameAscending()
        {
            var sortBy = "Name";
            var sortOrder = SortOrder.Ascending;
            var sort = _service.SortProducts(_products, sortOrder, sortBy).ToList();
            Assert.Equal("God of War", sort[0].Name);
            Assert.Equal("The Last of Us Part II Remastered", sort[2].Name);
        }

        [Fact]
        public void SortProducts_ByPriceAscending()
        {   
            var sortBy = "Price";
            var sortOrder = SortOrder.Ascending;
            var sort = _service.SortProducts(_products, sortOrder, sortBy).ToList();
            Assert.Equal("Spider-Man 2", sort[0].Name);
            Assert.Equal("The Last of Us Part II Remastered", sort[2].Name);
        }

        [Fact]
        public void SortProducts_ByPriceDescending()
        {
            var sortBy = "Price";
            var sortOrder = SortOrder.Descending;
            var sort = _service.SortProducts(_products, sortOrder, sortBy).ToList();
            Assert.Equal("The Last of Us Part II Remastered", sort[0].Name);
            Assert.Equal("Spider-Man 2", sort[2].Name);
        }

        [Fact]
        public void SortProducts_ByNameDescending()
        {
            var sortBy = "Name";
            var sortOrder = SortOrder.Descending;
            var sort = _service.SortProducts(_products, sortOrder, sortBy).ToList();
            Assert.Equal("The Last of Us Part II Remastered", sort[0].Name);
            Assert.Equal("God of War", sort[2].Name);
        }
    }
}