using Newtonsoft.Json;
using Mskj.LiteFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Common.DataObj
{
    public class ProductModel : IModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("proname")]
        public string ProName { get; set; }
        [JsonProperty("price")]
        public string Price
        {
            get
            {
                return DecimalPrice == 0 ? "价格面议" : ("￥" + DecimalPrice.ToString());
            }
        }
        [JsonProperty("decimalprice")]
        public decimal DecimalPrice { get; set; }
        [JsonProperty("introduction")]
        public string Introduction { get; set; }
        [JsonProperty("userid")]
        public string UserId { get; set; }
        [JsonProperty("isrecommend")]
        public bool IsRecommend { get; set; }
        [JsonProperty("nickname")]
        public string Nickname { get; set; }
        [JsonProperty("publishtime")]
        public DateTime? PublishTime { get; set; }
        [JsonProperty("compositescore")]
        public decimal CompositeScore { get; set; }
        [JsonProperty("materialcode")]
        public string MaterialCode { get; set; }
        [JsonProperty("productiondate")]
        public string ProductionDdate { get; set; }
        [JsonProperty("prodetail")]
        public string ProDetail { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("contacts")]
        public string Contacts { get; set; }
        [JsonProperty("contactphone")]
        public string ContactPhone { get; set; }
        [JsonProperty("images")]
        public string Images { get; set; }
        [JsonProperty("homeimage")]
        public string HomeImage { get; set; }
        [JsonProperty("prostate")]
        public int ProState { get; set; }
        [JsonProperty("proscores")]
        public int ProScores { get; set; }
        [JsonProperty("readcount")]
        public int ReadCount { get; set; }
        [JsonProperty("buycount")]
        public int BuyCount { get; set; }
        [JsonProperty("updatetime")]
        public DateTime? UpdateTime { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("procategory")]
        public string ProCategory { get; set; }
        [JsonProperty("contacttelephone")]
        public string ContactTelephone { get; set; }
        [JsonProperty("appsituation")]
        public string AppSituation { get; set; }
        [JsonProperty("appadvancement")]
        public string AppAdvancement { get; set; }
        [JsonProperty("appachievement")]
        public string AppAchievement { get; set; }
        [JsonProperty("exhibitsdisplay")]
        public string ExhibitsDisplay { get; set; }
        [JsonProperty("exhibitssize")]
        public string ExhibitsSize { get; set; }
        [JsonProperty("exhibitsweight")]
        public string ExhibitsWeight { get; set; }
        [JsonProperty("requirement")]
        public string Requirement { get; set; }
        [JsonProperty("providefree")]
        public string ProvideFree { get; set; }
        [JsonProperty("area")]
        public string Area { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("industrycategories")]
        public string IndustryCategories { get; set; }
    }
}
