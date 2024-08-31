namespace SmartShopAPI.Models.Dtos.Product
{
    public class ReadProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }
    }
}
