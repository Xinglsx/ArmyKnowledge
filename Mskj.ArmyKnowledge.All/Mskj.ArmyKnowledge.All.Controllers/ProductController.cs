using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.Common.DataObject;
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
            if (product == null || string.IsNullOrEmpty(product.Userid) ||
                string.IsNullOrEmpty(product.ProName) || string.IsNullOrEmpty(product.ContactPhone))
            {
                return new ReturnResult<Product>(-4, "参数传入错误！");
            }
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
        public object DeleteProduct(PostId product)
        { 
            if (product == null || string.IsNullOrEmpty(product.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _ProductService.DeleteProduct(product.Id);
        }
        /// <summary>
        /// 审核不产品信息
        /// </summary>
        [Route("AuditProduct")]
        [HttpPost]
        public object AuditPassProduct(PostId pro)
        {
            if (pro == null || string.IsNullOrEmpty(pro.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _ProductService.AuditProduct(pro.Id);
        }
        /// <summary>
        /// 审核不通过产品信息
        /// </summary>
        [Route("AuditFailProduct")]
        [HttpPost]
        public object AuditFailProduct(PostId pro)
        {
            if (pro == null || string.IsNullOrEmpty(pro.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _ProductService.AuditFailProduct(pro.Id);
        }
        /// <summary>
        /// 提交审核产品信息
        /// </summary>
        [Route("SubmitProduct")]
        [HttpPost]
        public object SubmitProduct(PostId pro)
        {
            if (pro == null || string.IsNullOrEmpty(pro.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _ProductService.SubmitProduct(pro.Id);
        }
        /// <summary>
        /// 保存并提交产品信息
        /// </summary>
        [Route("SaveAndSubmitProduct")]
        [HttpPost]
        public object SaveAndSubmitProduct(PostProduct product)
        {
            if (product == null || string.IsNullOrEmpty(product.Userid) ||
                string.IsNullOrEmpty(product.ProName) || string.IsNullOrEmpty(product.ContactPhone))
            {
                return new ReturnResult<Product>(-4, "参数传入错误！");
            }
            return _ProductService.SaveAndSubmitProduct(product.ToModel());
        }
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
        /// 获取一个产品详情
        /// </summary>
        [Route("GetOneProduct")]
        [HttpGet]
        public object GetOneProduct(string proid)
        {
            return _ProductService.GetOneProduct(proid);
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
        public object GetProducts(string filter = "",string category = "全部",
            int state = 2, int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            return _ProductService.GetProducts(filter,category, state, pageIndex, pageSize, sortType);
        }
        /// <summary>
        /// 增加阅读数
        /// </summary>
        [Route("UpdateReadCount")]
        [HttpPost]
        public object UpdateReadCount(PostId pro)
        {
            if (pro == null || string.IsNullOrEmpty(pro.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _ProductService.UpdateReadCount(pro.Id);
        }
        /// <summary>
        /// 增加阅读数
        /// </summary>
        [Route("UpdateBuyCount")]
        [HttpPost]
        public object UpdateBuyCount(PostId pro)
        {
            if (pro == null || string.IsNullOrEmpty(pro.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _ProductService.UpdateBuyCount(pro.Id);
        }
        #endregion
    }
}
