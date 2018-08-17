using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IProductService : IBaseService<Product>,IServiceContract
    {
        /// <summary>
        /// 新增产品信息
        /// </summary>
        ReturnResult<bool> AddProduct(Product cert);
        /// <summary>
        /// 更新产品信息
        /// </summary>
        ReturnResult<bool> UpdateProduct(Product cert);
    }
}
