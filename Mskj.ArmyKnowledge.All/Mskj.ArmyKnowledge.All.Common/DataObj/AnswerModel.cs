using Newtonsoft.Json;
using Mskj.LiteFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Common.DataObj
{
    public class AnswerModel : IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("questionid")]
        public string Questionid { get; set; }
        [JsonProperty("userid")]
        public string Userid { get; set; }
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        [JsonProperty("publishtime")]
        public DateTime? Publishtime { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("images")]
        public string Images { get; set; }
        [JsonProperty("isadopt")]
        public bool Isadopt { get; set; }
        [JsonProperty("praisecount")]
        public int Praisecount { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("updatetime")]
        public DateTime? UpdateTime { get; set; }
    }
}
