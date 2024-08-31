using SmartShopAPI.Entities;
using SmartShopAPI.Models.Dtos.CartItem;

namespace SmartShopAPI.Interfaces
{
    public interface ICartService
    {
        IEnumerable<CartItem> GetById(int userId);
        int AddItemToCart(CreateCartItemDto dto);
        void DeleteItemFromCart(int cartItemId);
        void UpdateCartItem(int cartItemId, UpdateCartItemDto dto);
        void ClearCartItems(int userId);
    }
}