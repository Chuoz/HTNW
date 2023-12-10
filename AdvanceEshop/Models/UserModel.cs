using System.ComponentModel.DataAnnotations;

namespace AdvanceEshop.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Yêu cầu nhập UserName")]
        public string UserName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Yêu cầu nhập Email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yêu cầu nhập Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
