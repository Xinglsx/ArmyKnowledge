﻿using Mskj.ArmyKnowledge.All.Domains;
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
        #region 获取验证码及注册时使用
        /// <summary>
        /// 手机号码
        /// </summary>
        [JsonProperty("phonenumber")]
        public string PhoneNumber { get; set; }
        #endregion

        #region 注册时使用
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
        #endregion

        #region 修改密码时用
        /// <summary>
        /// 用户ID
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
        [JsonProperty("oldpwd")]
        public string OldPwd { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [JsonProperty("newpwd")]
        public string NewPwd { get; set; }
        #endregion

        #region 修改个人信息时使用
        /// <summary>
        /// 关键字
        /// </summary>
        [JsonProperty("keyname")]
        public string KeyName { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [JsonProperty("keyvalue")]
        public string KeyValue { get; set; }
        #endregion

        #region 登录时使用
        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty("loginname")]
        public string LoginName { get; set; }
        #endregion
    }
}
