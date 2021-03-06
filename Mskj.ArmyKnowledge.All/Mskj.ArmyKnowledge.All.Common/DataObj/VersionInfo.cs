﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Common.DataObj
{
    public class VersionInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty("versionnumber")]
        public int VersionNumber { get; set; }
        /// <summary>
        /// 版本编号
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
        /// <summary>
        /// 下载地址
        /// </summary>
        [JsonProperty("downloadaddress")]
        public string DownloadAddress { get; set; }
        /// <summary>
        /// 更新内容
        /// </summary>
        [JsonProperty("updatecontent")]
        public string UpdateContent { get; set; }
    }
}
