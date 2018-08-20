using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.Common;
using System.Web.Http;

namespace Mskj.ArmyKnowledge.All.Controllers
{
    [RoutePrefix("Product")]
    public class ProductController : BaseController
    {
        #region 构造函数
        private readonly IProductService _ProductService;

        public ProductController(IProductService productService)
        {
            _ProductService = productService;
        }
        #endregion

        #region 产品信息
        /// <summary>
        /// 新增产品信息
        /// </summary>
        [Route("AddProduct")]
        [HttpPost]
        public object AddProduct(PostProduct product)
        {
            return _ProductService.AddProduct(product.ToModel());
        }
        /// <summary>
        /// 更新产品信息
        /// </summary>
        [Route("UpdateProduct")]
        [HttpPost]
        public object UpdateProduct(Product product)
        {
            return _ProductService.UpdateProduct(product);
        }
        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="id">产品ID</param>
        [Route("DeleteProduct")]
        [HttpPost]
        public object DeleteProduct(string id)
        {
            return _ProductService.DeleteProduct(id);
        }
        /// <summary>
        /// 审核产品信息
        /// </summary>
        [Route("AuditProduct")]
        [HttpPost]
        public object AuditProduct(Product product)
        {
            return _ProductService.AuditProduct(product);
        }
        /// <summary>
        /// 提交审核产品信息
        /// </summary>
        [Route("SubmitProduct")]
        [HttpPost]
        public object SubmitProduct(Product product)
        {
            return _ProductService.SubmitProduct(product);
        }
        /// <summary>
        /// 保存并提交产品信息
        /// </summary>
        [Route("SaveAndSubmitProduct")]
        [HttpPost]
        public object SaveAndSubmitProduct(Product product)
        {
            return _ProductService.SaveAndSubmitProduct(product);
        }
        /// <summar
        /// <summary>
        /// 获取已有产品分类
        /// </summary>
        [Route("GetProductCategory")]
        [HttpGet]
        public object GetProductCategory()
        {
            return _ProductService.GetProductCategory();
        }
        /// <summary>
        /// 分页获取对应用户的产品列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        [Route("GetUserProducts")]
        [HttpGet]
        public object GetUserProducts(string userid,int pageIndex = 1, int pageSize = 10,
            int sortType = 0)
        {
            return _ProductService.GetUserProducts(userid, pageIndex, pageSize, sortType);
        }
        /// <summary>
        /// 分页获取产品列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        [Route("GetProducts")]
        [HttpGet]
        public object GetProducts(string category = "全部",int state = 0, int pageIndex = 1, 
            int pageSize = 10, int sortType = 0)
        {
            return _ProductService.GetProducts(category, state, pageIndex, pageSize, sortType);
        }
        #endregion
    }
}
