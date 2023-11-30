using AdvanceEshop.Models;
using Newtonsoft.Json;

namespace AdvanceEshop.Models
{
    public class CartItemModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? Discount { get; set; }

        public decimal? Total
        {
            get { return (Price * (1 - Discount) * Quantity); }
        }

        public string Image { get; set; }
        public CartItemModel()
        {

        }
        public CartItemModel(Product product)
        {
            ProductId = product.ProductId;
            ProductName = product.ProductName;
            Price = product.ProductPrice;
            Discount = product.ProductDiscount;
            Quantity = 1;
            Image = product.ProductPhoto;
        }
    }
}


