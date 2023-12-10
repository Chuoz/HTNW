using AdvanceEshop.Data;
using AdvanceEshop.Infrastructure;
using AdvanceEshop.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdvanceEshop.Components
{
    public class CartWidget:ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View(HttpContext.Session.GetJson<CartItemModel>("cart"));
        }
    }
}
