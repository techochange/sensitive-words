using Badmati.BinarySpace.Infrastructure;
using Badmati.BinarySpace.Infrastructure.BadmatiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badmati.BinarySpace.WebApi
{
    public class UserGroup: CompareBase
    {
        public string openGId { get; set; }
        public string nickname { get; set; }
        public string headimg { get; set; }
    }

    public class MyGroup : CompareBase
    {
        public string openGId { get; set; }

        public string groupImg { get; set; }

        //public string groupName { get; set; }

        public int MsgCount { get; set; }

        public string MsgWords { get; set; }
    }
    

    public class UserInfo : CompareBase
    {
        public string headimg { get; set; }

        public string nickname { get; set; }
    }

    public class UserContent:CompareBase
    {
        public string Comment { get; set; }
        public List<Picture> Pictures { get; set; } 

        public List<string> OpenGIds { get; set; }

        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    public class UserActionContent:UserContent
    {

        public string nickname { get; set; }
        public string headimg { get; set; }
    }

    public class Picture
    {
        public int Inx { get; set; }
        public string Pic { get; set; }
    }

    public class CloudPicture: Picture
    {
        public string Link { get; set; }
    }

    public class ParamGetList : ParamPageBase
    {
        public string openid { get; set; }
        public string openGId { get; set; }
    }
}
