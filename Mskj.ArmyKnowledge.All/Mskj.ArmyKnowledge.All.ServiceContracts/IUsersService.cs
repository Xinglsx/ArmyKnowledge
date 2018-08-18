using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Base;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IUsersService : IBaseService<Users>,IServiceContract
    {
        #region 用户基础信息
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录信息，主要传loginname和加密后的password</param>
        ReturnResult<Users> Login(Users user);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户信息</param>
        ReturnResult<Users> AddUser(Users user);
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">新的用户信息</param>
        ReturnResult<bool> UpdateUser(Users user);
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        ReturnResult<bool> DeleteUser(string id);
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        ReturnResult<bool> ChangePassword(string id, string oldPwd, string newPwd);
        #endregion

        #region 用户认证信息
        /// <summary>
        /// 新增用户认证信息
        /// </summary>
        ReturnResult<bool> AddCert(Cert cert);
        /// <summary>
        /// 更新用户认证信息
        /// </summary>
        ReturnResult<bool> UpdateCert(Cert cert);
        /// <summary>
        /// 审核用户认证信息
        /// </summary>
        ReturnResult<bool> AuditCert(Cert cert);
        /// <summary>
        /// 提交审核用户认证信息
        /// </summary>
        ReturnResult<bool> SubmitCert(Cert cert);
        /// <summary>
        /// 保存并提交用户认证信息
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        ReturnResult<Cert> SaveAndSubmitCert(Cert cert);
        /// <summary>
        /// 通过用户ID获取用户认证信息
        /// </summary>
        ReturnResult<Cert> GetCert(string userid);
        #endregion
    }
}
