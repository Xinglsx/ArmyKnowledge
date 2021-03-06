﻿using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.Common.DataObject;
using System.Web.Http;

namespace Mskj.ArmyKnowledge.All.Controllers
{
    [RoutePrefix("Demand")]
    public class DemandController: BaseController
    {
        #region 构造函数
        private readonly IDemandService _DemandService;

        public DemandController(IDemandService demandService)
        {
            _DemandService = demandService;
        }
        #endregion

        #region 需求信息
        /// <summary>
        /// 新增需求信息
        /// </summary>
        [Route("AddDemand")]
        [HttpPost]
        public object AddDemand(PostDemand demand)
        {
            if (demand == null || string.IsNullOrEmpty(demand.Author) ||
                string.IsNullOrEmpty(demand.Content))
            {
                return new ReturnResult<Demand>(-4, "参数传入错误！");
            }
            return _DemandService.AddDemand(demand.ToModel());
        }
        /// <summary>
        /// 更新需求信息
        /// </summary>
        [Route("UpdateDemand")]
        [HttpPost]
        public object UpdateDemand(Demand demand)
        {
            return _DemandService.UpdateDemand(demand);
        }
        /// <summary>
        /// 删除需求信息
        /// </summary>
        /// <param name="id">需求ID</param>
        [Route("DeleteDemand")]
        [HttpPost]
        public object DeleteDemand(PostId demand)
        {
            if (demand == null || string.IsNullOrEmpty(demand.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _DemandService.DeleteDemand(demand.Id);
        }
        /// <summary>
        /// 审核通过需求信息
        /// </summary>
        [Route("AuditDemand")]
        [HttpPost]
        public object AuditPassDemand(PostId demand)
        {
            if(demand == null || string.IsNullOrEmpty(demand.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _DemandService.AuditDemand(demand.Id);
        }
        /// <summary>
        /// 审核不通过需求信息
        /// </summary>
        [Route("AuditFailDemand")]
        [HttpPost]
        public object AuditFailDemand(PostId demand)
        {
            if (demand == null || string.IsNullOrEmpty(demand.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _DemandService.AuditFailDemand(demand.Id);
        }
        /// <summary>
        /// 提交审核需求信息
        /// </summary>
        [Route("SubmitDemand")]
        [HttpPost]
        public object SubmitDemand(PostId demand)
        {
            if (demand == null || string.IsNullOrEmpty(demand.Id))
            {
                return new ReturnResult<bool>(-4, "参数传入错误！");
            }
            return _DemandService.SubmitDemand(demand.Id);
        }
        /// <summary>
        /// 保存并提交需求信息
        /// </summary>
        [Route("SaveAndSubmitDemand")]
        [HttpPost]
        public object SaveAndSubmitDemand(Demand demand)
        {
            return _DemandService.SaveAndSubmitDemand(demand);
        }
        /// <summar
        /// <summary>
        /// 获取已有需求领域
        /// </summary>
        [Route("GetDemandField")]
        [HttpGet]
        public object GetDemandField()
        {
            return _DemandService.GetDemandField();
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
        [Route("GetUserDemands")]
        [HttpGet]
        public object GetUserDemands(string userid,int pageIndex = 1, int pageSize = 10,
            int sortType = 0)
        {
            return _DemandService.GetUserDemands(userid, pageIndex, pageSize, sortType);
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
        [Route("GetDemands")]
        [HttpGet]
        public object GetDemands(string filter = "",string category = "全部",int state = 0, 
            int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            return _DemandService.GetDemands(filter,category, state, pageIndex, pageSize, sortType);
        }
        #endregion
    }
}
