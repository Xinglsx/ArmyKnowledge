using Mskj.ArmyKnowledge.All.Common.DataObj;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;
using QuickShare.LiteFramework.Common;
using System.Collections.Generic;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface ISystemService : IServiceContract
    {
        #region 版本信息
        /// <summary>
        /// 获取版本信息
        /// </summary>
        ReturnResult<VersionInfo> GetVersionInfo();
        #endregion

        #region 阿里云发送消息
        /// <summary>
        /// 阿里云往指定手机号发送验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        ReturnResult<bool> SendMobileMessage(string phoneNumber);
        #endregion
    }
}
