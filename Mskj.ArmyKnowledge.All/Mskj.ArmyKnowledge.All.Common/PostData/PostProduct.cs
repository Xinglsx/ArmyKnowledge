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
    /// 产品上传类
    /// </summary>
    public class PostProduct
    {
        /// <summary>
        /// 产品名称
        /// </summary>
        [JsonProperty("proname")]
        public string ProName { get; set; }
        /// <summary>
        /// 产品价格
        /// </summary>
        [JsonProperty("price")]
        public string Price { get; set; }
        /// <summary>
        /// 物资编码
        /// </summary>
        [JsonProperty("materialcode")]
        public string MaterialCode { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        [JsonProperty("productiondate")]
        public string ProductionDate { get; set; }
        /// <summary>
        /// 产品明细信息
        /// </summary>
        [JsonProperty("prodetail")]
        public string ProDetail { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        [JsonProperty("contacts")]
        public string Contacts { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        [JsonProperty("contactphone")]
        public string ContactPhone { get; set; }
        /// <summary>
        /// 发布者ID
        /// </summary>
        [JsonProperty("userid")]
        public string Userid { get; set; }
        /// <summary>
        /// 发布者昵称
        /// </summary>
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [JsonProperty("introduction")]
        public string Introduction { get; set; }
        /// <summary>
        /// 图片集合
        /// </summary>
        [JsonProperty("images")]
        public string Images { get; set; }
        /// <summary>
        /// 首页图片
        /// </summary>
        [JsonProperty("homeimage")]
        public string Homeimage { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }
    }

    public static class PostProductExtensions
    {
        public static Product ToModel(this PostProduct pro)
        {
            return new Product
            {
                proname = pro.ProName,
                price = pro.Price,
                materialcode = pro.MaterialCode,
                productiondate = pro.ProductionDate,
                prodetail = pro.ProDetail,
                contacts = pro.Contacts,
                contactphone = pro.ContactPhone,
                userid = pro.Userid,
                nickname = pro.Nickname,
                introduction = pro.Introduction,
                images = pro.Images,
                homeimage = pro.Homeimage,
                category = pro.Category,
            };
        }
    }
}
