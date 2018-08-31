using Mskj.ArmyKnowledge.All.Domains;
using QuickShare.LiteFramework.Common;

namespace Mskj.ArmyKnowledge.All.Common.DataObj
{
    /// <summary> 全文检索返回数据 </summary>
    public class FtrModel
    {
        /// <summary> 用户信息 </summary>
        public IPagedData<Users> UserInfo { get; set; }
        /// <summary> 提问信息 </summary>
        public IPagedData<QuestionModel> QuestionInfo { get; set; }
        /// <summary> 需求信息 </summary>
        public IPagedData<DemandModel> DemandInfo { get; set; }
        /// <summary> 产品信息 </summary>
        public IPagedData<ProductModel> ProductInfo { get; set; }
    }
}
