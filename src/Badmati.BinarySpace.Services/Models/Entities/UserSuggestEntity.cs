using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Services.Models.Entities
{
    [Table("UserSuggest")]
    public class UserSuggestEntity
    {
        [Key]
        public int Id { get; set; }

        public int AppType { get; set; }
        public DateTime CreateTime { get; set; }
        public string Content { get; set; }
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public bool EmailReply { get; set; }

        public bool PhoneReply { get; set; }
        public int State { get; set; }
        public bool IsReply { get; set; }
        public string ReplyContent { get; set; }
        public DateTime? ReplyTime { get; set; }
        public string UserId { get; set; }
    }
}
