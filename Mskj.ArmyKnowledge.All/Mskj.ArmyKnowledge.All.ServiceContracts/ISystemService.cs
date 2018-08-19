using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.ServiceContracts.DataObj;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
using System.Collections.Generic;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface ISystemService : IServiceContract
    {
        #region 消息信息
        /// <summary>
        /// 获取版本信息
        /// </summary>
        ReturnResult<VersionInfo> GetVersionInfo();
        #endregion

    }
}
