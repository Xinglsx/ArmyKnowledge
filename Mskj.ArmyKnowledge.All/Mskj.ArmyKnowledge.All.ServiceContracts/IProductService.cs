using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
using System.Collections.Generic;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IProductService : IBaseService<Product>,IServiceContract
    {
        /// <summary>
        /// 新增产品信息
        /// </summary>
        ReturnResult<Product> AddProduct(Product product);
        /// <summary>
        /// 更新产品信息
        /// </summary>
        ReturnResult<bool> UpdateProduct(Product product);
        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="id">产品ID</param>
        ReturnResult<bool> DeleteProduct(string id);
        /// <summary>
        /// 审核产品信息
        /// </summary>
        ReturnResult<bool> AuditProduct(string id);
        /// <summary>
        /// 提交审核产品信息
        /// </summary>
        ReturnResult<bool> SubmitProduct(string id);
        /// <summary>
        /// 保存并提交产品信息
        /// </summary>
        ReturnResult<Product> SaveAndSubmitProduct(Product product);
        /// <summary>
        /// 获取一个产品信息
        /// </summary>
        ReturnResult<Product> GetOneProduct(string id);
        /// <summary>
        /// 获取已有产品分类
        /// </summary>
        ReturnResult<List<string>> GetProductCategory();
        /// <summary>
        /// 分页获取用户对应的产品列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        ReturnResult<IPagedData<Product>> GetUserProducts(string userid,
            int pageIndex = 1, int pageSize = 10, int sortType = 0);
        /// <summary>
        /// 分页获取产品列表
        /// </summary>
        /// <param name="filter">查寻关键字</param>
        /// <param name="category">分类</param>
        /// <param name="state">状态 -1:全部</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        ReturnResult<IPagedData<Product>> GetProducts(string filter = "", string category = "全部",
            int state = -1, int pageIndex = 1, int pageSize = 10, int sortType = 0);
    }
}
