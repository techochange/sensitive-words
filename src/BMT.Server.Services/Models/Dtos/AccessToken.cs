using System;
using System.Collections.Generic;
using System.Text;

namespace BMT.Server.Services.Models.Dtos
{
    public class AccessToken
    {
        public string access_token { get; set; }

        public string errmsg { get; set; }

        public int expires_in { get; set; }

        public int errcode { get; set; }
    }
}
