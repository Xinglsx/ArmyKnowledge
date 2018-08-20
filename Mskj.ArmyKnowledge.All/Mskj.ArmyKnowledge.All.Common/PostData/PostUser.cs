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
    /// 用户信息
    /// </summary>
    public class PostUser
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [JsonProperty("phonenumber")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty("pwd")]
        public string Pwd { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [JsonProperty("verificationcode")]
        public string VerificationCode { get; set; }
    }
}
