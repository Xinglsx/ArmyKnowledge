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
        public decimal Price { get; set; }
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

        /// <summary>
        /// 产品类别(海军、陆军、空军、火箭军、其他)
        /// </summary>
        [JsonProperty("procategory")]
        public string ProCategory { get; set; }
        /// <summary>
        /// 联系人电话
        /// </summary>
        [JsonProperty("contacttelephone")]
        public string ContactTelephone { get; set; }
        /// <summary>
        /// 应用情况(A：已应用 B：正在洽谈 C：已立项)
        /// </summary>
        [JsonProperty("appsituation")]
        public string AppSituation { get; set; }
        /// <summary>
        /// 应用先进行(A：解决卡脖子问题 B：填报国内空白 C：创新型应用)
        /// </summary>
        [JsonProperty("appadvancement")]
        public string AppAdvancement { get; set; }
        /// <summary>
        /// 产品成就
        /// </summary>
        [JsonProperty("appachievement")]
        public string AppAchievement { get; set; }
        /// <summary>
        /// 展示方式
        /// </summary>
        [JsonProperty("exhibitsdisplay")]
        public string ExhibitsDisplay { get; set; }
        /// <summary>
        /// 展品尺寸
        /// </summary>
        [JsonProperty("exhibitssize")]
        public string ExhibitsSize { get; set; }
        /// <summary>
        /// 展品重量
        /// </summary>
        [JsonProperty("exhibitsweight")]
        public string ExhibitsWeight { get; set; }
        /// <summary>
        /// 展示需求
        /// </summary>
        [JsonProperty("requirement")]
        public string Requirement { get; set; }
        /// <summary>
        /// 是否愿意无偿提供
        /// </summary>
        [JsonProperty("providefree")]
        public string ProvideFree { get; set; }
        /// <summary>
        /// 所在区域
        /// </summary>
        [JsonProperty("area")]
        public string Area { get; set; }
        /// <summary>
        /// 电子邮件
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
        /// <summary>
        /// 行业类别
        /// </summary>
        [JsonProperty("industrycategories")]
        public string IndustryCategories { get; set; }
        /// <summary>
        /// 性能参数
        /// </summary>
        [JsonProperty("performance")]
        public string Performance { get; set; }
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

                procategory = pro.ProCategory,
                appachievement = pro.AppAchievement,
                appadvancement = pro.AppAdvancement,
                appsituation = pro.AppSituation,
                area = pro.Area,
                contacttelephone = pro.ContactTelephone,
                exhibitsdisplay = pro.ExhibitsDisplay,
                exhibitssize = pro.ExhibitsSize,
                exhibitsweight = pro.ExhibitsWeight,
                providefree = pro.ProvideFree,
                requirement = pro.Requirement,
                email = pro.Email,
                industrycategories = pro.IndustryCategories,
                performance = pro.Performance,
            };
        }
    }
}
