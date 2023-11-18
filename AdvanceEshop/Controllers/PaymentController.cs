using AdvanceEshop.Data;
using AdvanceEshop.Infrastructure;
using AdvanceEshop.Models;
using Microsoft.AspNetCore.Mvc;
using static AdvanceEshop.Models.PaymentClient;

namespace AdvanceEshop.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaypalClient _paypalClient;
        private readonly ApplicationDbContext _context;

        public PaymentController(PaypalClient paypalClient, ApplicationDbContext context)
        {
            _paypalClient = paypalClient;
            _context = context;
        }
        public IActionResult Index()
        {
            return RedirectToAction();
        }


        public IActionResult PaypalDemo()
        {
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PaypalOrder(CancellationToken cancellationToken)
        {
            // Tạo đơn hàng (thông tin lấy từ Session???)
            Cart cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
            var tongTien = cart.ComputeTotalValues().ToString("F2");
            var donViTienTe = "USD";
            // OrderId - mã tham chiếu (duy nhất)
            var orderIdref = "DH" + DateTime.Now.Ticks.ToString();

            try
            {
                // a. Create paypal order
                var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, orderIdref);

                return Ok(response);
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

        public async Task<IActionResult> PaypalCapture(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);

                var reference = response.purchase_units[0].reference_id;

                // Put your logic to save the transaction here
                // You can use the "reference" variable as a transaction key
                // Lưu đơn hàng vô database

                return Ok(response);
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
    }
}
