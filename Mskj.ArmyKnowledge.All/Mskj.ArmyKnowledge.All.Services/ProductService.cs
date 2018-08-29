﻿using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
using QuickShare.LiteFramework.Common.Extenstions;
using QuickShare.LiteFramework.Foundation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class ProductService : BaseService<Product, Product>, IProductService
    {

        #region 构造函数
        private readonly IRepository<Dictionary> _DicRepository;
        private readonly ILogger logger;

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public ProductService(IRepository<Product> productRepository,
            IRepository<Dictionary> dicRepository) : base(productRepository)
        {
            _DicRepository = dicRepository;
            logger = AppInstance.Current.Resolve<ILogger>();
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
            product.publishtime = DateTime.Now;
            product.updatetime = DateTime.Now;
            try
            {
                saveResult = Add(product);
            }
            catch (Exception exp)
            {
                logger.LogError("新增产品信息出错！", exp);
                return new ReturnResult<Product>(-1, "系统异常，请稍后重试。");
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
            product.updatetime = DateTime.Now;
            try
            {
                updateResult = this.Update(product);
            }
            catch (Exception exp)
            {
                logger.LogError("更新产品信息出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
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
                logger.LogError("删除产品信息出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
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
        public ReturnResult<bool> AuditProduct(string id)
        {
            var product = GetOne(p => p.id == id);
            if(product == null || string.IsNullOrEmpty(product.id))
            {
                return new ReturnResult<bool>(-2, "未找到对应产品信息！");
            }
            else if (product.prostate != 1)
            {
                return new ReturnResult<bool>(-2, "产品信息状态不是[提交审核状态]！");
            }
            product.prostate = 2;
            return UpdateProduct(product);
        }
        /// <summary>
        /// 提交审核产品信息
        /// </summary>
        public ReturnResult<bool> SubmitProduct(string id)
        {
            var product = GetOne(p => p.id == id);
            if (product == null || string.IsNullOrEmpty(product.id))
            {
                return new ReturnResult<bool>(-2, "未找到对应产品信息！");
            }
            else if (product.prostate != 1)
            {
                return new ReturnResult<bool>(-2, "产品信息状态不是[提交审核状态]！");
            }
            product.prostate = 1;
            return UpdateProduct(product);
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
        /// 获取一个产品信息
        /// </summary>
        public ReturnResult<Product> GetOneProduct(string id)
        {
            var res = GetOne(p => p.id == id);
            if(res == null || string.IsNullOrEmpty(res.id))
            {
                return new ReturnResult<Product>(-2, "找不到对应产品！");
            }
            return new ReturnResult<Product>(1, res);
        }
        /// <summary>
        /// 获取已有产品分类
        /// </summary>
        public ReturnResult<List<string>> GetProductCategory()
        {
            List<string> categorys = new List<string> { "全部" };
            var res = _DicRepository.Find().Where(p => p.dicstate && p.dictype == 2)
                .Select(q => q.dicname);
                if(res != null && res.Count() > 0)
            {
                categorys.AddRange(res.ToList());
            }
            return new ReturnResult<List<string>>(1, categorys);
        }
        /// <summary>
        /// 分页获取用户对应的产品列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Product>> GetUserProducts(string userid,
            int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            Expression<Func<Product, bool>> expression = x => x.userid == userid;
            return GetBaseProducts(pageIndex, pageSize, sortType, expression);
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
        public ReturnResult<IPagedData<Product>> GetProducts(string filter = "", string category = "全部",
            int state = 2, int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            Expression<Func<Product, bool>> exp1 = x => true;
            Expression<Func<Product, bool>> exp2 = x => true;
            Expression<Func<Product, bool>> exp3 = x => true;
            if (state != -1)
            {
                exp1 = x => x.prostate == state;
            }
            if (!"全部".Equals(category))
            {
                exp2 = x => x.category == category;
            }
            if (!string.IsNullOrEmpty(filter))
            {
                exp3 = x => x.prodetail.Contains(filter) || x.proname.Contains(filter) ||
                    x.category.Contains(filter) || x.contacts.Contains(filter);
            }
            return GetBaseProducts(pageIndex, pageSize, sortType, exp1.AndAlso(exp2).AndAlso(exp3));
        }
        /// <summary>
        /// 分页获取产品列表(封装排序方式)
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式 0-综合排序 1-最新发布 2-价格</param>
        /// <param name="expression">查询表达示</param>
        /// <returns></returns>
        private ReturnResult<IPagedData<Product>> GetBaseProducts(int pageIndex,
            int pageSize, int sortType, Expression<Func<Product, bool>> expression)
        {
            List<SortInfo<Product>> sorts = new List<SortInfo<Product>>();
            SortInfo<Product> sort;
            switch (sortType)
            {
                case 0:
                    sort = new SortInfo<Product>(p => new { p.proscores },
                        SortOrder.Descending);
                    break;
                case 1:
                    sort = new SortInfo<Product>(p => new { p.publishtime },
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
            sorts.Add(new SortInfo<Product>(p => new { p.publishtime }, SortOrder.Descending));
            return new ReturnResult<IPagedData<Product>>(1,
                    GetPage(pageIndex, pageSize, sorts, expression));
        }
        /// <summary>
        /// 增加阅读数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ReturnResult<bool> UpdateReadCount(string proId)
        {
            Product product = this.GetOne(p => p.id == proId);
            if (product == null || string.IsNullOrEmpty(product.id))
            {
                return new ReturnResult<bool>(-2, "产品ID不存在！");
            }
            else
            {
                product.readcount++;
                var res = UpdateProduct(product);
                if (res.code > 0)
                {
                    UpdateHeatCount(product);
                }
                return res;
            }
        }
        /// <summary>
        /// 增加购买数
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ReturnResult<bool> UpdateBuyCount(string proId)
        {
            Product product = this.GetOne(p => p.id == proId);
            if (product == null || string.IsNullOrEmpty(product.id))
            {
                return new ReturnResult<bool>(-2, "产品ID不存在！");
            }
            else
            {
                product.buycount++;
                var res = UpdateProduct(product);
                if (res.code > 0)
                {
                    UpdateHeatCount(product);
                }
                return res;
            }
        }
        /// <summary>
        /// 更新热度
        /// </summary>
        /// <param name="question">问题对象</param>
        /// <returns></returns>
        public void UpdateHeatCount(Product product)
        {
            product.compositescore = 1000 + product.readcount;
            product.compositescore += product.buycount * 100;
            //距离现在的时间越长，减分也越大。
            int hours = (DateTime.Now - product.publishtime.Value).Hours;
            //当天的，每过一个小时减5
            if (hours <= 24)
            {
                product.compositescore -= hours * 5;
            }
            //2~5天的，每过一个小时减10
            else if (hours <= 120 && hours > 24)
            {
                product.compositescore -= 120 + (hours - 24) * 10;
            }
            //5天以上的，每过一个小时减100
            else
            {
                product.compositescore -= 1080 + (hours - 120) * 100;
            }
            product.updatetime = DateTime.Now;

            this.Update(product);
        }
        #endregion
    }
}
