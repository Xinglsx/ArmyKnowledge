using Mskj.ArmyKnowledge.All.Common.DataObj;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using Mskj.LiteFramework.Base;
using Mskj.LiteFramework.Common;
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

        #region 获取字典信息
        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="dictype">字典类型</param>
        List<string> GetDictionarys(int dictype);
        /// <summary>
        /// 增加字典
        /// </summary>
        ReturnResult<Dictionary> AddDictionary(Dictionary dic);
        /// <summary>
        /// 更新字典
        /// </summary>
        ReturnResult<bool> UpdateDictionary(Dictionary dic);
        /// <summary>
        /// 删除字典
        /// </summary>
        ReturnResult<bool> DeleteDictionary(string dicId);
        /// <summary>
        /// 获取字典
        /// </summary>
        ReturnResult<IPagedData<Dictionary>> GetDictionarys(int dictype = -1,
            int pageIndex = 1, int pageSize = 30);
        #endregion

        #region 图片上传
        /// <summary>
        /// 图片上传
        /// </summary>
        ReturnResult<string> UploadFile();
        #endregion


        #region 阿里云发送消息
        /// <summary>
        /// 阿里云往指定手机号发送验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        ReturnResult<bool> SendMobileMessage(string phoneNumber);
        /// <summary>
        /// 验证手机号及获取到的验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="verificationCode">验证码</param>
        ReturnResult<bool> ValidationCode(string phoneNumber, string verificationCode);
        #endregion
    }
}
