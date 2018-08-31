using Mskj.ArmyKnowledge.All.Domains;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Common.PostData
{
    public class PostRecord
    {
        [JsonProperty("userid")]
        public string UserId { get; set; }
        [JsonProperty("questionid")]
        public string QuestionId { get; set; }
        [JsonProperty("type")]
        public int Type { get; set; }
    }

    public static class PostRecordExtensions
    {
        public static Record ToModel(this PostRecord record)
        {
            return new Record
            {
                questionid = record.QuestionId,
                userid = record.UserId,
            };
        }
    }
}
