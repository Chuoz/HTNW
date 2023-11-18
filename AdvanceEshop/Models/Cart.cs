using AdvanceEshop.Models;

namespace AdvanceEshop.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();
        public void AddItem(Product product, int quantity)
        {
            CartLine? line = Lines.Where(p => p.Product.ProductId == product.ProductId).FirstOrDefault();
            if (line != null)
            {
                line.Quantity += quantity;
            }
            else
            {
                Lines.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
        }

        public void RemoveLine(Product product) => Lines.RemoveAll(p => p.Product.ProductId == product.ProductId);

        public decimal ComputeTotalValues() => (decimal)Lines.Sum(e => e.Product?.ProductPrice * (1 - e.Product?.ProductDiscount) * e.Quantity);
    }
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product Product { get; set; } = new();
        public int Quantity { get; set; }
    }
}


