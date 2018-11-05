using Mskj.ArmyKnowledge.All.Common.DataObj;
using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.Common.DataObject;
using Mskj.LiteFramework;
using Mskj.LiteFramework.Base;
using Mskj.LiteFramework.Common;
using Mskj.LiteFramework.Common.Extenstions;
using Mskj.LiteFramework.Foundation;
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
        private readonly IRepository<Product> _ProRepository;
        private readonly IRepository<Users> _UserRepository;
        private readonly ILogger logger;

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public ProductService(IRepository<Product> productRepository, IRepository<Users> userRepository,
            IRepository<Dictionary> dicRepository) : base(productRepository)
        {
            _DicRepository = dicRepository;
            _ProRepository = productRepository;
            _UserRepository = userRepository;

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
        /// 审核通过产品信息
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
        /// 审核不通过产品信息
        /// </summary>
        public ReturnResult<bool> AuditFailProduct(string id)
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
            product.prostate = 3;
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
            else if (product.prostate != 0 && product.prostate != 3)
            {
                return new ReturnResult<bool>(-2, "产品信息状态不是[新建状态]或[审核不通过状态]！");
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
        public ReturnResult<IPagedData<ProductModel>> GetUserProducts(string userid,
            int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            var res = (from pro in _ProRepository.Find()
                       join user in _UserRepository.Find() on pro.userid equals user.id
                       where pro.userid == userid
                       select new ProductModel
                       {
                           Avatar = user.avatar,
                           BuyCount = pro.buycount,
                           Category = pro.category ?? "",
                           CompositeScore = pro.compositescore,
                           ContactPhone = pro.contactphone ?? "",
                           Contacts = pro.contacts ?? "",
                           HomeImage = pro.homeimage ?? "",
                           Id = pro.id,
                           Images = pro.images ?? "",
                           Introduction = pro.introduction ?? "",
                           IsRecommend = pro.isrecommend,
                           MaterialCode = pro.materialcode ?? "",
                           Nickname = user.nickname,
                           DecimalPrice = pro.price,
                           ProDetail = pro.prodetail ?? "",
                           ProductionDdate = pro.productiondate ?? "",
                           ProName = pro.proname ?? "",
                           ProScores = pro.proscores,
                           ProState = pro.prostate,
                           PublishTime = pro.publishtime,
                           ReadCount = pro.readcount,
                           UpdateTime = pro.updatetime,
                           UserId = user.id,

                           AppAchievement = pro.appachievement,
                           AppAdvancement = pro.appadvancement,
                           AppSituation = pro.appsituation,
                           Area = pro.area,
                           ContactTelephone = pro.contacttelephone,
                           ExhibitsDisplay = pro.exhibitsdisplay,
                           ExhibitsWeight = pro.exhibitsweight,
                           ProCategory = pro.procategory,
                           ProvideFree = pro.providefree,
                           Requirement = pro.requirement,
                           ExhibitsSize = pro.exhibitssize,
                           IndustryCategories = pro.industrycategories,
                           Email = pro.email,
                           Performance = pro.performance,
                       });
            switch (sortType)
            {
                case 1:
                    res = res.OrderByDescending(p => p.ProScores).ThenByDescending(q => q.PublishTime);
                    break;
                case 2:
                    res = res.OrderByDescending(p => p.DecimalPrice).ThenByDescending(q => q.PublishTime);
                    break;
                case 3:
                    res = res.OrderBy(p => p.DecimalPrice).ThenByDescending(q => q.PublishTime);
                    break;
                case 0:
                default:
                    res = res.OrderByDescending(q => q.PublishTime);
                    break;
            }
            return new ReturnResult<IPagedData<ProductModel>>(1, res.ToPage(pageIndex, pageSize));
        }
        /// <summary>
        /// 分页获取产品列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式 0-时间倒序 1-得分倒序 2-价格倒序 3-价格正序</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<ProductModel>> GetProducts(string filter = "", string category = "全部",
            int state = 2, int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            var res = (from pro in _ProRepository.Find()
                       join user in _UserRepository.Find() on pro.userid equals user.id
                       select new ProductModel
                       {
                           Avatar = user.avatar,
                           BuyCount = pro.buycount,
                           Category = pro.category ?? "",
                           CompositeScore = pro.compositescore,
                           ContactPhone = pro.contactphone ?? "",
                           Contacts = pro.contacts ?? "",
                           HomeImage = pro.homeimage ?? "",
                           Id = pro.id,
                           Images = pro.images ?? "",
                           Introduction = pro.introduction ?? "",
                           IsRecommend = pro.isrecommend,
                           MaterialCode = pro.materialcode ?? "",
                           Nickname = user.nickname,
                           DecimalPrice = pro.price,
                           ProDetail = pro.prodetail ?? "",
                           ProductionDdate = pro.productiondate ?? "",
                           ProName = pro.proname ?? "",
                           ProScores = pro.proscores,
                           ProState = pro.prostate,
                           PublishTime = pro.publishtime,
                           ReadCount = pro.readcount,
                           UpdateTime = pro.updatetime,
                           UserId = user.id,

                           AppAchievement = pro.appachievement,
                           AppAdvancement = pro.appadvancement,
                           AppSituation = pro.appsituation,
                           Area = pro.area,
                           ContactTelephone = pro.contacttelephone,
                           ExhibitsDisplay = pro.exhibitsdisplay,
                           ExhibitsWeight = pro.exhibitsweight,
                           ProCategory = pro.procategory,
                           ProvideFree = pro.providefree,
                           Requirement = pro.requirement,
                           ExhibitsSize = pro.exhibitssize,
                           IndustryCategories = pro.industrycategories,
                           Email = pro.email,
                           Performance = pro.performance,
                       });
            if (state != -1)
            {
                res = res.Where(x => x.ProState == state);
            }
            if (!"全部".Equals(category))
            {
                res = res.Where(x => x.Category == category);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                res = res.Where(x => x.ProDetail.Contains(filter) || x.ProName.Contains(filter) ||
                    x.Category.Contains(filter) || x.Contacts.Contains(filter));
            }
            switch (sortType)
            {
                case 1:
                    res = res.OrderByDescending(p => p.ProScores).ThenByDescending(q => q.PublishTime);
                    break;
                case 2:
                    res = res.OrderByDescending(p => p.DecimalPrice).ThenByDescending(q => q.PublishTime);
                    break;
                case 3:
                    res = res.OrderBy(p => p.DecimalPrice).ThenByDescending(q => q.PublishTime);
                    break;
                case 0:
                default:
                    res = res.OrderByDescending(q => q.PublishTime);
                    break;
            }
            return new ReturnResult<IPagedData<ProductModel>>(1,res.ToPage(pageIndex,pageSize));
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
