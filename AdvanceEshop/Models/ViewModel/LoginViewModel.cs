using System.ComponentModel.DataAnnotations;

namespace AdvanceEshop.Models.ViewModel
{
    public class LoginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập UserName")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yêu cầu nhập Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

    }
}
