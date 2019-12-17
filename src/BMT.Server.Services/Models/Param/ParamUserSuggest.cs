using System;
using System.Collections.Generic;
using System.Text;

namespace BMT.Server.Services.Models.Param
{
    public class ParamUserSuggest:PrmModel
    {
        public string Content { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
    }

}
