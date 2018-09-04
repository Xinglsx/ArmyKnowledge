using Newtonsoft.Json;
using Mskj.LiteFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Common.DataObj
{
    public class DemandModel : IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("authornickname")]
        public string AuthorNickname { get; set; }
        [JsonProperty("publishtime")]
        public DateTime? PublishTime { get; set; }
        [JsonProperty("introduction")]
        public string Introduction { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("images")]
        public string Images { get; set; }
        [JsonProperty("isrecommend")]
        public bool isRecommend { get; set; }
        [JsonProperty("homeimage")]
        public string HomeImage { get; set; }
        [JsonProperty("readcount")]
        public int ReadCount { get; set; }
        [JsonProperty("heatcount")]
        public int HeatCount { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("demandstate")]
        public int DemandState { get; set; }
        [JsonProperty("demandscores")]
        public int DemandScores { get; set; }
        [JsonProperty("updatetime")]
        public DateTime? UpdateTime { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        /// <summary>
        /// 领域
        /// </summary>
        [JsonProperty("field")]
        public string Field { get; set; }
    }
}
