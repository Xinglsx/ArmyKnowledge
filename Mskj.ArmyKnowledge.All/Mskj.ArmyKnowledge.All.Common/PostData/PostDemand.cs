using Mskj.ArmyKnowledge.All.Domains;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Common.PostData
{
    /// <summary>
    /// 需求上传类
    /// </summary>
    public class PostDemand
    {
        /// <summary>
        /// 标题
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// 作者ID
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }
        /// <summary>
        /// 作者昵称
        /// </summary>
        [JsonProperty("authornickname")]
        public string AuthorNickname { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [JsonProperty("introduction")]
        public string Introduction { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; set; }
        /// <summary>
        /// 图片集合
        /// </summary>
        [JsonProperty("images")]
        public string Images { get; set; }
        /// <summary>
        /// 首页图片
        /// </summary>
        [JsonProperty("homeimage")]
        public string Homeimage { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }
        /// <summary>
        /// 领域
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }
    }

    public static class PostDemandExtensions
    {
        public static Demand ToModel(this PostDemand demand)
        {
            return new Demand
            {
                title = demand.Title,
                author = demand.Author,
                authornickname = demand.AuthorNickname,
                introduction = demand.Introduction,
                content = demand.Content,
                images = demand.Images,
                homeimage = demand.Homeimage,
                category = demand.Category,
                field = demand.Field,
            };
        }
    }
}
