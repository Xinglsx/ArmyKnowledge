using Mskj.ArmyKnowledge.All.Common.DataObj;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using Mskj.LiteFramework.Base;
using Mskj.LiteFramework.Common;
using System.Collections.Generic;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IDemandService : IBaseService<Demand>,IServiceContract
    {
        #region 需求信息
        /// <summary>
        /// 新增需求信息
        /// </summary>
        ReturnResult<Demand> AddDemand(Demand demand);
        /// <summary>
        /// 更新需求信息
        /// </summary>
        ReturnResult<bool> UpdateDemand(Demand demand);
        /// <summary>
        /// 删除需求信息
        /// </summary>
        /// <param name="id">需求ID</param>
        ReturnResult<bool> DeleteDemand(string id);
        /// <summary>
        /// 审核通过需求信息
        /// </summary>
        ReturnResult<bool> AuditDemand(string id);
        /// <summary>
        /// 审核不通过需求信息
        /// </summary>
        ReturnResult<bool> AuditFailDemand(string id);
        /// <summary>
        /// 提交审核需求信息
        /// </summary>
        ReturnResult<bool> SubmitDemand(string id);
        /// <summary>
        /// 保存并提交需求信息
        /// </summary>
        ReturnResult<Demand> SaveAndSubmitDemand(Demand demand);
        /// <summary>
        /// 获取已有需求分类
        /// </summary>
        ReturnResult<List<string>> GetDemandField();
        /// <summary>
        /// 分页获取对应用户的需求列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        ReturnResult<IPagedData<Demand>> GetUserDemands(string userid,
            int pageIndex = 1, int pageSize = 10, int sortType = 0);
        /// <summary>
        /// 分页获取需求列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        ReturnResult<IPagedData<DemandModel>> GetDemands(string filter = "", string category = "全部", 
            int state = -1,int pageIndex = 1, int pageSize = 10, int sortType = 0);
        #endregion
        
    }
}
