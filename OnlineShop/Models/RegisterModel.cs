using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class RegisterModel
    {
        [Key]
        public long ID { set; get; }

        [Display(Name ="Tên đăng nhập")]
        [Required(ErrorMessage ="Yêu cầu nhập tên đăng nhập")]
        public string userName { set; get; }

        [Display(Name ="Mật khẩu")]
        [StringLength(20, MinimumLength =6, ErrorMessage ="Mật khẩu phải nhiều hơn 6 ký tự")]
        [Required(ErrorMessage = "Yêu cầu nhập password")]
        public string password { set; get; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("password", ErrorMessage ="Xác nhận mật khẩu không đúng")]
        [Required(ErrorMessage = "Yêu cầu nhập xác nhận mật khẩu")]
        public string confirmPassword { set; get; }

        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Yêu cầu nhập họ tên")]
        public string name { set; get; }

        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Yêu cầu nhập địa chỉ")]
        public string address { set; get; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Yêu cầu nhập email")]
        public string email { set; get; }

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Yêu cầu nhập số điện thoại")]
        public string phone { set; get; }

        [Display(Name ="Tỉnh/thành")]
        public string ProvinceID { get; set; }

        [Display(Name ="Quận/Huyện")]
        public string DistrictID { get; set; }

        [Display(Name ="Phường/Xã")]
        public string PrecinctID { get; set; }
    }
}