using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
using QuickShare.LiteFramework.Common.Extenstions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Configuration;
using System.IO;
using QuickShare.LiteFramework.Foundation;
using QuickShare.LiteFramework;
using Mskj.ArmyKnowledge.All.Common.DataObj;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class DemandService : BaseService<Demand, Demand>, IDemandService
    {

        #region 构造函数
        //private readonly IRepository<Demand> _DemandRepository;
        private readonly IRepository<Dictionary> _DicRepository;
        private readonly IRepository<Demand> _DemandRepository;
        private readonly IRepository<Users> _UserRepository;
        private readonly ILogger logger;

        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public DemandService(IRepository<Demand> demandRepository,
            IRepository<Dictionary> dicRepository, IRepository<Users> userRepository) : base(demandRepository)
        {
            _DicRepository = dicRepository;
            _UserRepository = userRepository;
            _DemandRepository = demandRepository;

            logger = AppInstance.Current.Resolve<ILogger>();
        }
        #endregion

        #region 需求相关
        /// <summary>
        /// 新增需求信息
        /// </summary>
        public ReturnResult<Demand> AddDemand(Demand demand)
        {
            bool saveResult = false;
            demand.id = Guid.NewGuid().ToString();
            demand.publishtime = DateTime.Now;
            demand.updatetime = DateTime.Now;
            try
            {
                saveResult = Add(demand);
            }
            catch (Exception exp)
            {
                logger.LogError("新增需求信息出错！", exp);
                return new ReturnResult<Demand>(-1, "系统异常，请稍后重试。");
            }
            if (saveResult)
            {
                return new ReturnResult<Demand>(1, demand);
            }
            else
            {
                return new ReturnResult<Demand>(-2, "需求信息保存失败！");
            }
        }
        /// <summary>
        /// 更新需求信息
        /// </summary>
        public ReturnResult<bool> UpdateDemand(Demand demand)
        {
            bool updateResult = false;
            demand.updatetime = DateTime.Now;
            try
            {
                updateResult = Update(demand);
            }
            catch (Exception exp)
            {
                logger.LogError("更新需求信息出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            if (updateResult)
            {
                return new ReturnResult<bool>(1, updateResult);
            }
            else
            {
                return new ReturnResult<bool>(-2, updateResult, "需求信息更新失败！");
            }
        }
        /// <summary>
        /// 删除需求信息
        /// </summary>
        /// <param name="id">需求ID</param>
        public ReturnResult<bool> DeleteDemand(string id)
        {
            bool deleteResult = false;
            try
            {
                deleteResult = this.DeleteByKey(id);
            }
            catch (Exception exp)
            {
                logger.LogError("删除需求信息出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            if (deleteResult)
            {
                return new ReturnResult<bool>(1, deleteResult);
            }
            else
            {
                return new ReturnResult<bool>(-2, deleteResult, "需求信息删除失败！");
            }
        }
        /// <summary>
        /// 审核需求信息
        /// </summary>
        public ReturnResult<bool> AuditDemand(string id)
        {
            var demand = GetOne(p => p.id == id);
            if (demand == null || string.IsNullOrEmpty(demand.id))
            {
                return new ReturnResult<bool>(-2, "未找到对应需求信息！");
            }
            else if (demand.demandstate != 1)
            {
                return new ReturnResult<bool>(-2, "需求信息状态不是[提交审核状态]！");
            }
            demand.demandstate = 2;
            demand.updatetime = DateTime.Now;
            return UpdateDemand(demand);
        }
        /// <summary>
        /// 审核不通过需求信息
        /// </summary>
        public ReturnResult<bool> AuditFailDemand(string id)
        {
            var demand = GetOne(p => p.id == id);
            if (demand == null || string.IsNullOrEmpty(demand.id))
            {
                return new ReturnResult<bool>(-2, "未找到对应需求信息！");
            }
            else if (demand.demandstate != 1)
            {
                return new ReturnResult<bool>(-2, "需求信息状态不是[提交审核状态]！");
            }
            demand.demandstate = 3;
            return UpdateDemand(demand);
        }
        /// <summary>
        /// 提交审核需求信息
        /// </summary>
        public ReturnResult<bool> SubmitDemand(string id)
        {
            var demand = GetOne(p => p.id == id);
            if (demand == null || string.IsNullOrEmpty(demand.id))
            {
                return new ReturnResult<bool>(-2, "未找到对应需求信息！");
            }
            else if (demand.demandstate != 1 && demand.demandstate != 3)
            {
                return new ReturnResult<bool>(-2, "需求信息状态不是[新建状态]或[审核不通过状态]！");
            }
            demand.demandstate = 1;
            demand.updatetime = DateTime.Now;
            return UpdateDemand(demand);
        }
        /// <summary>
        /// 保存并提交需求信息
        /// </summary>
        public ReturnResult<Demand> SaveAndSubmitDemand(Demand demand)
        {
            bool saveResult = false;
            demand.id = Guid.NewGuid().ToString();
            demand.publishtime = DateTime.Now;
            demand.updatetime = DateTime.Now;
            demand.demandstate = 1;
            try
            {
                saveResult = Add(demand);
            }
            catch (Exception exp)
            {
                logger.LogError("保存并提交需求信息出错！", exp);
                return new ReturnResult<Demand>(-1, "系统异常，请稍后重试。");
            }
            if (saveResult)
            {
                return new ReturnResult<Demand>(1, demand);
            }
            else
            {
                return new ReturnResult<Demand>(-2, "需求信息保存失败！");
            }
        }
        /// <summary>
        /// 获取已有需求分类
        /// </summary>
        public ReturnResult<List<string>> GetDemandField()
        {
            List<string> professions = new List<string> { "全部" };
            var res = _DicRepository.Find().Where(p => p.dicstate && p.dictype == 1)
                .Select(q => q.dicname);
            if (res != null && res.Count() > 0)
            {
                professions.AddRange(res.ToList());
            }
            return new ReturnResult<List<string>>(1, professions);
        }
        /// <summary>
        /// 分页获取对应需求的需求列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Demand>> GetUserDemands(string userid,
            int pageIndex = 1,int pageSize = 10, int sortType = 0)
        {
            Expression<Func<Demand, bool>> expression = x => x.author == userid;
            return GetBaseDemands(pageIndex, pageSize, sortType, expression);
        }
        /// <summary>
        /// 分页获取需求列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式 0-时间倒序 1-得分倒序</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<DemandModel>> GetDemands(string filter = "", string category = "全部",
            int state = 2, int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            var res = (from demand in _DemandRepository.Find()
                       join user in _UserRepository.Find() on demand.author equals user.id
                       select new DemandModel
                       {
                           Id = demand.id,
                           Author = user.id,
                           AuthorNickname = user.nickname ?? "",
                           Avatar = user.avatar ?? "",
                           Category = demand.category ?? "",
                           Content = demand.content ?? "",
                           DemandScores = demand.demandscores,
                           DemandState = demand.demandstate,
                           HeatCount = demand.heatcount,
                           HomeImage = demand.homeimage ?? "",
                           Images = demand.images ?? "",
                           Introduction = demand.introduction ?? "",
                           isRecommend = demand.isrecommend,
                           PublishTime = demand.publishtime,
                           ReadCount = demand.readcount,
                           Title = demand.title ?? "",
                           UpdateTime = demand.updatetime,
                           Field = demand.field ?? "",
                       });
            if(state != -1)
            {
                res = res.Where(x => x.DemandState == state);
            }
            if (!"全部".Equals(category))
            {
                res = res.Where(x => x.Category == category);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                res = res.Where(x => x.Title.Contains(filter) || x.Content.Contains(filter) ||
                     x.Category.Contains(filter));
            }
            switch (sortType)
            {
                default:
                case 0:
                    res = res.OrderByDescending(p => p.PublishTime);
                    break;
                case 1:
                    res = res.OrderByDescending(q => q.DemandScores).ThenByDescending(p => p.PublishTime);
                    break;
            }

            return new ReturnResult<IPagedData<DemandModel>>(1,res.ToPage(pageIndex,pageSize));
        }
        /// <summary>
        /// 分页获取需求列表(封装排序方式)
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式 0-时间倒序 1-综合得分倒序</param>
        /// <param name="expression">查询表达示</param>
        /// <returns></returns>
        private ReturnResult<IPagedData<Demand>> GetBaseDemands(int pageIndex,
            int pageSize, int sortType, Expression<Func<Demand, bool>> expression)
        {
            List<SortInfo<Demand>> sorts = new List<SortInfo<Demand>>();
            SortInfo<Demand> sortTime = new SortInfo<Demand>(p => new { p.publishtime },
                        SortOrder.Descending);
            switch (sortType)
            {
                case 0:
                    sorts.Add(sortTime);
                    break;
                case 1:
                default:
                    sorts.Add(new SortInfo<Demand>(p => new { p.demandscores },
                        SortOrder.Descending));
                    sorts.Add(sortTime);
                    break;
            }
            return new ReturnResult<IPagedData<Demand>>(1,GetPage(pageIndex, 
                pageSize, sorts, expression));
        }
        #endregion
        
    }
}
