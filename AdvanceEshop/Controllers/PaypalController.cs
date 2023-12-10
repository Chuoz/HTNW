using AdvanceEshop.Data;
using AdvanceEshop.Infrastructure;
using AdvanceEshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using static AdvanceEshop.Models.PaymentClient;

namespace AdvanceEshop.Controllers
{
    [Authorize]
    public class PaypalController : Controller
    {
        private readonly PaypalClient _paypalClient;
        private readonly ApplicationDbContext _context;
        public PaypalController(ApplicationDbContext context, PaypalClient paypalClient)
        {
            _context = context;
            _paypalClient = paypalClient;
        }

        public IActionResult Index()
        {
            return RedirectToAction();
        }
        public IActionResult Paypal()
        {
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View();
        }

        public async Task<IActionResult> PaypalOrder(CancellationToken cancellationToken)
        {
            // Tạo đơn hàng, lấy thông tin từ Session
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            // Lấy grandTotalString từ Session
            var grandTotalString = HttpContext.Session.GetString("GrandTotal");

            // Kiểm tra xem grandTotalString có tồn tại không
            if (string.IsNullOrEmpty(grandTotalString))
            {
                TempData["error"] = "Lỗi khi lấy GrandTotal từ session";
                return RedirectToAction("Error");
            }

            // Chuyển đổi grandTotalString thành decimal
            if (decimal.TryParse(grandTotalString, out var grandTotal))
            {
                // Lấy tổng tiền
                decimal totalAmount = grandTotal;
                // lấy đơn vị tiền tệ
                string currencyCode = "USD";
                // OrderId - Mã tham chiếu (duy nhất)
                var orderIdref = "SP" + DateTime.Now.Ticks.ToString();
                try
                {
                    // Convert totalAmount từ decimal sang string để truyền vào phương thức CreateOrder
                    string totalAmountString = totalAmount.ToString("0.##");
                    // Create paypal order
                    var response = await _paypalClient.CreateOrder(totalAmountString, currencyCode, orderIdref);
                    
                        return Ok(response);
                        
                }
                catch (Exception ex)
                {
                    var error = new
                    {
                        ex.GetBaseException().Message
                    };
                    return BadRequest(error);
                }

            }

            return View();
        }
        public async Task<IActionResult> PaypalCapture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);
                var reference = response.purchase_units[0].reference_id;
                // put your logic to save the transaction here
                // you can use the "reference" variable as a transaction key
                // lưu đơn hàng vào database
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                if (userEmail == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    var ordercode = Guid.NewGuid().ToString();
                    var orderItem = new OrderModel();
                    orderItem.OrderCode = ordercode;
                    orderItem.UserName = userEmail;
                    orderItem.Status = 1;
                    orderItem.CreatedDate = DateTime.Now;
                    _context.Add(orderItem);
                    _context.SaveChanges();

                    List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                    foreach (var cart in cartItems)
                    {
                        var orderdetails = new OrderDetails();
                        orderdetails.UserName = userEmail;
                        orderdetails.OrderCode = ordercode;
                        orderdetails.ProductId = cart.ProductId;
                        orderdetails.Price = cart.Price;
                        orderdetails.Quantity = cart.Quantity;
                        _context.Add(orderdetails);
                        _context.SaveChanges();
                    }
                    HttpContext.Session.Remove("Cart");
                    TempData["success"] = " Checkout thành công, vui lòng chờ duyệt đơn hàng";
                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };
                return BadRequest(error);
            }
        }

        public IActionResult Success()
        {
            TempData["success"] = "Thanh toán thành công!";
            return View();
        }

        public IActionResult Fail()
        {
            TempData["success"] = "Thanh toán thất bại!";
            return View();
        }
    }
}
