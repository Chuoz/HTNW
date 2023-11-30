using AdvanceEshop.Data;
using AdvanceEshop.Infrastructure;
using AdvanceEshop.Models;
using AdvanceEshop.Models.ViewModel;
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
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(x => x.Price * (1 - x.Discount) * x.Quantity)
            };
            return View(cartVM);
        }

        public async Task<IActionResult> AddToCart(int Id)
        {
            Product product = await _context.Products.FindAsync(Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItems == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItems.Quantity += 1;
            }
            HttpContext.Session.SetJson("Cart", cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> UpdateCart(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveCart(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            cart.RemoveAll(p => p.ProductId == Id);
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Clear()
        {
            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index");

        }
    }
}
