using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class ProductService : BaseService<Product, Product>, IProductService
    {

        #region 构造函数
        private readonly IRepository<Product> _ProductRepository;
        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public ProductService(IRepository<Product> productRepository) : base(productRepository)
        {

        }
        #endregion

        #region 产品相关
        /// <summary>
        /// 新增产品信息
        /// </summary>
        public ReturnResult<Product> AddProduct(Product product)
        {
            bool saveResult = false;
            product.id = Guid.NewGuid().ToString();
            try
            {
                saveResult = Add(product);
            }
            catch (Exception exp)
            {
                return new ReturnResult<Product>(-1, exp.Message);
            }
            if (saveResult)
            {
                return new ReturnResult<Product>(1, product);
            }
            else
            {
                return new ReturnResult<Product>(-2, "用户信息保存失败！");
            }
        }
        /// <summary>
        /// 更新产品信息
        /// </summary>
        public ReturnResult<bool> UpdateProduct(Product product)
        {
            bool updateResult = false;
            try
            {
                updateResult = this.Update(product);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, false, exp.Message);
            }
            if (updateResult)
            {
                return new ReturnResult<bool>(1, updateResult);
            }
            else
            {
                return new ReturnResult<bool>(-2, updateResult, "用户信息更新失败！");
            }
        }
        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="id">产品ID</param>
        public ReturnResult<bool> DeleteProduct(string id)
        {
            bool deleteResult = false;
            try
            {
                deleteResult = this.DeleteByKey(id);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, exp.Message);
            }
            if (deleteResult)
            {
                return new ReturnResult<bool>(1, deleteResult);
            }
            else
            {
                return new ReturnResult<bool>(-2, deleteResult, "用户信息删除失败！");
            }
        }
        /// <summary>
        /// 审核产品信息
        /// </summary>
        public ReturnResult<bool> AuditProduct(Product product)
        {
            if (product.prostate != 1)
            {
                return new ReturnResult<bool>(-2, "待审核的产品信息状态不是[提交审核状态]！");
            }
            product.prostate = 2;
            return UpdateProduct(product);
        }
        /// <summary>
        /// 提交审核产品信息
        /// </summary>
        public ReturnResult<bool> SubmitProduct(Product product)
        {
            if (product.prostate != 0)
            {
                return new ReturnResult<bool>(-2, "待提交审核的认证信息状态不是[新建状态]！");
            }
            else
            {
                product.prostate = 1;
                return UpdateProduct(product);
            }
        }
        /// <summary>
        /// 保存并提交产品信息
        /// </summary>
        public ReturnResult<Product> SaveAndSubmitProduct(Product product)
        {
            product.prostate = 1;
            return this.AddProduct(product);
        }
        /// <summary>
        /// 获取已有产品分类
        /// </summary>
        public ReturnResult<List<string>> GetProductCategory()
        {
            List<string> categorys = new List<string> { "全部" };
            var res = _ProductRepository.Find().Select(p => p.category).Distinct().ToList();
            if(res != null && res.Count > 0)
            {
                categorys.AddRange(res);
                return new ReturnResult<List<string>>(1, res);
            }
            else
            {
                return new ReturnResult<List<string>>(1, categorys);
            }
        }
        /// <summary>
        /// 分页获取产品列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="category">分类</param>
        /// <param name="sortType">排序方式 0-综合排序 1-最新发布 2-价格升序 3-价格降序</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Product>> GetProducts(int pageIndex = 1,
            int pageSize = 10, int sortType = 0, string category = "全部")
        {
            List<SortInfo<Product>> sorts = new List<SortInfo<Product>>();
            SortInfo<Product> sort;
            switch (sortType)
            {
                case 0:
                    sort = new SortInfo<Product>(p => p.proscores,
                        SortOrder.Descending);
                    break;
                case 1:
                    sort = new SortInfo<Product>(p => p.publishtime,
                        SortOrder.Descending);
                    break;
                case 2:
                    sort = new SortInfo<Product>(p => p.price,
                        SortOrder.Ascending);
                    break;
                default:
                    sort = new SortInfo<Product>(p => p.price,
                        SortOrder.Descending);
                    break;
            }
            sorts.Add(sort);
            //所有排序之后，再按时间降序
            sorts.Add(new SortInfo<Product>(p => p.publishtime,SortOrder.Descending));
            if ("全部".Equals(category))
            {
                return new ReturnResult<IPagedData<Product>>(1,
                    GetPage(pageIndex, pageSize, sorts));
            }
            else
            {
                Expression<Func<Product, bool>> expression = x => x.category == category;
                return new ReturnResult<IPagedData<Product>>(1,
                    GetPage(pageIndex, pageSize, sorts, expression));
            }
        }
        #endregion
    }
}
