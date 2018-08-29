using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Common.DataObj
{
    public class MsgModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("userid1")]
        public string Userid1 { get; set; }
        [JsonProperty("nickname1")]
        public string Nickname1 { get; set; }
        [JsonProperty("avatar1")]
        public string Avatar1 { get; set; }
        [JsonProperty("userid2")]
        public string Userid2 { get; set; }
        [JsonProperty("nickname2")]
        public string Nickname2 { get; set; }
        [JsonProperty("avatar2")]
        public string Avatar2 { get; set; }
        [JsonProperty("updatetime")]
        public DateTime Updatetime { get; set; }
        [JsonProperty("lastcontent")]
        public string LastContent { get; set; }
    }
}
