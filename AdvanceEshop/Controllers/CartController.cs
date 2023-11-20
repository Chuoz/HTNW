using AdvanceEshop.Data;
using AdvanceEshop.Infrastructure;
using AdvanceEshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdvanceEshop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("Cart", HttpContext.Session.GetJson<Cart>("cart"));
        }

        public Cart? Cart { get; set; }
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, quantity);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }

        [HttpPost]
        public IActionResult AddToCartFromDetails(int productId, int quantity = 1)
        {
            Product? product = _context.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                if(Cart != null)
                {
                    Cart.AddItem(product, quantity);
                    HttpContext.Session.SetJson("cart", Cart);
                }
                else
                {
                    Console.WriteLine("Loi");
                }
            }
            return View("Cart", Cart);
        }




        public IActionResult UpdateCart(int productId)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
                Cart.AddItem(product, -1);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            Product? product = _context.Products
                .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart = HttpContext.Session.GetJson<Cart>("cart");
                Cart.RemoveLine(product);
                HttpContext.Session.SetJson("cart", Cart);
            }
            return View("Cart", Cart);
        }
    }
}
