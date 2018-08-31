using Newtonsoft.Json;
using QuickShare.LiteFramework.Base;
using System;

namespace Mskj.ArmyKnowledge.All.Common.DataObj
{
    public class UserModel : IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("loginname")]
        public string LoginName { get; set; }
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        [JsonProperty("profession")]
        public string Profession { get; set; }
        [JsonProperty("organization")]
        public string Organization { get; set; }
        [JsonProperty("sex")]
        public string Sex { get; set; }
        [JsonProperty("area")]
        public string Area { get; set; }
        [JsonProperty("position")]
        public string Position { get; set; }
        [JsonProperty("goodpoint")]
        public string GoodPoint { get; set; }
        [JsonProperty("registrationid")]
        public string RegistrationId { get; set; }
        [JsonProperty("creditcode")]
        public string CreditCode { get; set; }
        [JsonProperty("phonenumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("signatures")]
        public string Signatures { get; set; }
        [JsonProperty("usertype")]
        public int UserType { get; set; }
        [JsonProperty("iscertification")]
        public int IsCertification { get; set; }
        [JsonProperty("isadmin")]
        public bool Isadmin { get; set; }
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
        [JsonProperty("userstate")]
        public int UserState { get; set; }
        [JsonProperty("registertime")]
        public DateTime? RegisterTime { get; set; }
        [JsonProperty("updatetime")]
        public DateTime? UpdateTime { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
