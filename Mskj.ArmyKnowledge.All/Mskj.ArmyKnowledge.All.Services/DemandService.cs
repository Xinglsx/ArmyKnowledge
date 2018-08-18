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
    public class DemandService : BaseService<Demand, Demand>, IDemandService
    {

        #region 构造函数
        private readonly IRepository<Demand> _DemandRepository;
        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public DemandService(IRepository<Demand> demandRepository) : base(demandRepository)
        {

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
            try
            {
                saveResult = Add(demand);
            }
            catch (Exception exp)
            {
                return new ReturnResult<Demand>(-1, exp.Message);
            }
            if (saveResult)
            {
                return new ReturnResult<Demand>(1, demand);
            }
            else
            {
                return new ReturnResult<Demand>(-2, "用户信息保存失败！");
            }
        }
        /// <summary>
        /// 更新需求信息
        /// </summary>
        public ReturnResult<bool> UpdateDemand(Demand demand)
        {
            bool updateResult = false;
            try
            {
                updateResult = this.Update(demand);
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
        /// 审核需求信息
        /// </summary>
        public ReturnResult<bool> AuditDemand(Demand demand)
        {
            if (demand.demandstate != 1)
            {
                return new ReturnResult<bool>(-2, "待审核的需求信息状态不是[提交审核状态]！");
            }
            demand.demandstate = 2;
            return UpdateDemand(demand);
        }
        /// <summary>
        /// 提交审核需求信息
        /// </summary>
        public ReturnResult<bool> SubmitDemand(Demand demand)
        {
            if (demand.demandstate != 0)
            {
                return new ReturnResult<bool>(-2, "待提交审核的认证信息状态不是[新建状态]！");
            }
            else
            {
                demand.demandstate = 1;
                return UpdateDemand(demand);
            }
        }
        /// <summary>
        /// 保存并提交需求信息
        /// </summary>
        public ReturnResult<Demand> SaveAndSubmitDemand(Demand demand)
        {
            demand.demandstate = 1;
            return this.AddDemand(demand);
        }
        /// <summary>
        /// 获取已有需求分类
        /// </summary>
        public ReturnResult<List<string>> GetDemandCategory()
        {
            List<string> categorys = new List<string> { "全部" };
            var res = _DemandRepository.Find().Select(p => p.category).Distinct().ToList();
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
        /// 分页获取对应用户的需求列表
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
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Demand>> GetDemands(string category = "全部",
            int state = 0,int pageIndex = 1,int pageSize = 10, int sortType = 0)
        {
            Expression<Func<Demand, bool>> expression;
            if ("全部".Equals(category))
            {
                expression = x => x.demandstate == state;
            }
            else
            {
                expression = x => x.demandstate == state && x.category == category;

            }
            return GetBaseDemands(pageIndex, pageSize, sortType, expression);
        }
        /// <summary>
        /// 分页获取需求列表(封装排序方式)
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式 0-综合排序 1-最新发布</param>
        /// <param name="expression">查询表达示</param>
        /// <returns></returns>
        private ReturnResult<IPagedData<Demand>> GetBaseDemands(int pageIndex,
            int pageSize, int sortType, Expression<Func<Demand, bool>> expression)
        {
            List<SortInfo<Demand>> sorts = new List<SortInfo<Demand>>();
            SortInfo<Demand> sort;
            switch (sortType)
            {
                case 0:
                    sort = new SortInfo<Demand>(p => p.demandscores,
                        SortOrder.Descending);
                    break;
                case 1:
                default:
                    sort = new SortInfo<Demand>(p => p.publishtime,
                        SortOrder.Descending);
                    break;
            }
            sorts.Add(sort);
            //所有排序之后，再按时间降序
            sorts.Add(new SortInfo<Demand>(p => p.publishtime,SortOrder.Descending));
            return new ReturnResult<IPagedData<Demand>>(1,
                    GetPage(pageIndex, pageSize, sorts, expression));
        }
        #endregion
    }
}
