using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Common.DataObj
{
    public class UserFansModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("signatures")]
        public string Signatures { get; set; }
        [JsonProperty("usertype")]
        public int Usertype { get; set; }
        [JsonProperty("iscertification")]
        public int IsCertification { get; set; }
        [JsonProperty("answercount")]
        public int AnswerCount { get; set; }
        [JsonProperty("adoptedcount")]
        public int AdoptedCount { get; set; }
        [JsonProperty("compositescores")]
        public int CompositeScores { get; set; }
        [JsonProperty("followcount")]
        public int FollowCount { get; set; }
        [JsonProperty("fanscount")]
        public int FansCount { get; set; }
        [JsonProperty("fansupdatetime")]
        public DateTime? FansUpdateTime { get; set; }
    }
}
