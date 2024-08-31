using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SmartShopAPI.Authorization;
using SmartShopAPI.Data;
using SmartShopAPI.Entities;
using SmartShopAPI.Exceptions;
using SmartShopAPI.Interfaces;
using SmartShopAPI.Models.Dtos.CartItem;

namespace SmartShopAPI.Services
{
    public class CartService : ICartService
    {
        private readonly SmartShopDbContext _context;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;

        public CartService(SmartShopDbContext context, IUserContextService userContextService, IMapper mapper, IAuthorizationService authorizationService)
        {
            _context = context;
            _userContextService = userContextService;
            _mapper = mapper;
            _authorizationService = authorizationService;
        }

        public IEnumerable<CartItem> GetById(int userId)
        {
            var currentUserId = _userContextService.GetUserId();
            if(userId != currentUserId)
            {
                throw new ForbidException("Authorization failed");  
            }
            var cartItems = _context.CartItems
                .Include(p => p.Product)
                .Where(x => x.UserId == userId)
                .ToList();
            return cartItems;
        }

        public int AddItemToCart(CreateCartItemDto dto)
        {
            var existingCartItem = _context.CartItems
                .Include(c=>c.Product)
                .FirstOrDefault(ci => ci.ProductId == dto.ProductId && ci.UserId == _userContextService.GetUserId());
            CartItem cartItem;
            if (existingCartItem != null)
            {
                existingCartItem.Quantity += dto.Quantity;
                cartItem = existingCartItem;
            }
            else
            {
                cartItem = _mapper.Map<CartItem>(dto);
                cartItem.UserId = _userContextService.GetUserId();
                _context.CartItems.Add(cartItem);
            }
            _context.SaveChanges();
            return cartItem.Id;
        }

        public void DeleteItemFromCart(int cartItemId)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(x => x.Id == cartItemId) ?? throw new NotFoundException("Cart item not found"); 
            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, cartItem,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Authorization failed");    
            }
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }

        public void UpdateCartItem(int cartItemId, UpdateCartItemDto dto)
        {
            var cartItem = _context.CartItems
                .FirstOrDefault(x => x.Id == cartItemId) ?? throw new NotFoundException("Cart item not found");         
            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, cartItem,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;
            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Authorization failed");
            }
            cartItem.Quantity = dto.Quantity;
            _context.SaveChanges();
        }

        public void ClearCartItems(int userId)
        {
            var cartItems = _context.CartItems.Where(c => c.UserId == userId);
            _context.RemoveRange(cartItems);
            _context.SaveChanges();
        }
    }
}
