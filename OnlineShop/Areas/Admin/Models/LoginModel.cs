using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessageResourceName = "Username_Required", ErrorMessageResourceType = typeof(StaticResources.Resources))]
        [Display(Name ="Username", ResourceType = typeof(StaticResources.Resources))]
        public string UserName { set; get; }

        [Required(ErrorMessageResourceName = "Password_Required", ErrorMessageResourceType = typeof(StaticResources.Resources))]
        [Display(Name ="Password", ResourceType = typeof(StaticResources.Resources))]
        public string PassWord { set; get; }

        [Display(Name = "RememberMe", ResourceType = typeof(StaticResources.Resources))]
        public bool RememberMe { set; get; }
    }
}