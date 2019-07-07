using Chair80CP.Libs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chair80CP.Requests
{
    public class IRequest
    {
        
        public WEBResult<bool> isValid()
        {
            var props = this.GetType().GetProperties().Where(a => a.CustomAttributes.Where(at => at.AttributeType.Name == "RequiredAttribute").Count() > 0);
            foreach (var item in props)
            {
                if (item.GetValue(this) == null) return WEBResult<bool>.Error(ResponseCode.BackendInternalServer, item.CustomAttributes.Where(at => at.AttributeType.Name == "RequiredAttribute").FirstOrDefault().NamedArguments.First(a => a.MemberName == "ErrorMessage").TypedValue.Value.ToString());
            }
            return WEBResult<bool>.Success(true);
        }
    }
}