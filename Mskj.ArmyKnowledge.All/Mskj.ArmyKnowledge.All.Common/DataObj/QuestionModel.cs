using Mskj.ArmyKnowledge.All.Domains;
using Newtonsoft.Json;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Common.DataObj
{
    public class QuestionModel: IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("authornickname")]
        public string AuthorNickname { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("publishtime")]
        public DateTime? Publishtime { get; set; }
        [JsonProperty("introduction")]
        public string Introduction { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("images")]
        public string Images { get; set; }
        [JsonProperty("isrecommend")]
        public bool IsRecommend { get; set; }
        [JsonProperty("homeimage")]
        public string HomeImage { get; set; }
        [JsonProperty("readcount")]
        public int ReadCount { get; set; }
        [JsonProperty("praisecount")]
        public int PraiseCount { get; set; }
        [JsonProperty("commentcount")]
        public int CommentCount { get; set; }
        [JsonProperty("heatcount")]
        public int HeatCount { get; set; }
        [JsonProperty("questionstate")]
        public int QuestionState { get; set; }
        [JsonProperty("answers")]
        public IPagedData<AnswerModel> Answers { get; set; }
        [JsonProperty("iscollection")]
        public bool? IsCollect { get; set; }
        [JsonProperty("updatetime")]
        public DateTime? UpdateTime { get; set; }
    }
   
}
