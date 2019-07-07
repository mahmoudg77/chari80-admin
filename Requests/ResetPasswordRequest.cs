using Chair80CP.Libs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Chair80CP.Requests
{
    public class ResetPasswordRequest:IRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "New Password Is Required !")]
        public string NewPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password Is Required !")]
        public string ConfirmPassword { get; set; }


        public WEBResult<bool> isValid()
        {
            if (!base.isValid().isSuccess) return base.isValid();

            if (NewPassword != ConfirmPassword) return WEBResult<bool>.Error(ResponseCode.UserValidationField, "Confirm Password Not the same of New Password");

            return WEBResult<bool>.Success(true);
       
        }
    }
}