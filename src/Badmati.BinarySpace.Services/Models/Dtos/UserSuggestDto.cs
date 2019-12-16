using Badmati.BinarySpace.Services.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Badmati.BinarySpace.Services.Models.Dtos
{
    public class UserSuggestDto
    {
        public DateTime CreateTime { get; set; }
        public string Content { get; set; }
        public bool IsReply { get; set; }
        public string ReplyContent { get; set; }
        public DateTime? ReplyTime { get; set; }
        public string UserId { get; set; }

        public UserSuggestDto() { }

        public UserSuggestDto(UserSuggestEntity entity)
        {
            Content = entity.Content;
            CreateTime = entity.CreateTime;
            IsReply = entity.IsReply;
            ReplyContent = entity.ReplyContent;
            ReplyTime = entity.ReplyTime;
            UserId = entity.UserId;
        }
    }
}
