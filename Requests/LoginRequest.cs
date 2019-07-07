using Chair80CP.Libs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chair80CP.Requests
{
    public class LoginRequest : IRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username Is Required !")]
        public string Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Is Required !")]
        public string Password { get; set; }

        public string firebase_token { get; set; }
        public bool RememberMe { get; set; }
    }
}