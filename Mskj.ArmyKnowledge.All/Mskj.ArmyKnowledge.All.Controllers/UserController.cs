using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using System;
using System.Web.Http;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using System.Collections.Generic;

namespace Mskj.ArmyKnowledge.All.Controllers
{
    [RoutePrefix("Users")]
    public class UserController : BaseController
    {
        #region 构造函数
        private readonly IUsersService _UsersService;

        public UserController(IUsersService usersService)
            : base()
        {
            _UsersService = usersService;
        }
        #endregion

        #region 用户基础信息处理
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录信息，主要传loginname和加密后的password</param>
        [Route("Login")]
        [HttpPost]
        public object Login(Users user)
        {
            return _UsersService.Login(user);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户信息</param>
        [Route("Register")]
        [HttpPost]
        public object AddUser(Users user)
        {
            return _UsersService.AddUser(user);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">新的用户信息</param>
        [Route("UpdateUser")]
        [HttpPost]
        public object UpdateUser(Users user)
        {
            return _UsersService.UpdateUser(user);
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        [Route("DeleteUser")]
        [HttpPost]
        public object DeleteUser(string id)
        {
            return _UsersService.DeleteUser(id);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        [Route("ChangePassword")]
        [HttpPost]
        public object ChangePassword(ChangeUserApiModel pwd)
        {
            return _UsersService.ChangePassword(pwd.id, pwd.oldPwd, pwd.newPwd);
        }
        #endregion

        #region 用户认证信息处理
        /// <summary>
        /// 新增用户认证信息
        /// </summary>
        [Route("AddCert")]
        [HttpPost]
        public object AddCert(Cert cert)
        {
            return _UsersService.AddCert(cert);
        }
        /// <summary>
        /// 更新用户认证信息
        /// </summary>
        [Route("UpdateCert")]
        [HttpPost]
        public object UpdateCert(Cert cert)
        {
            return _UsersService.UpdateCert(cert);
        }
        /// <summary>
        /// 审核用户认证信息
        /// </summary>
        [Route("AuditCert")]
        [HttpPost]
        public object AuditCert(Cert cert)
        {
            return _UsersService.AuditCert(cert);
        }
        /// <summary>
        /// 提交审核用户认证信息
        /// </summary>
        [Route("SubmitCert")]
        [HttpPost]
        public object SubmitCert(Cert cert)
        {
            return _UsersService.SubmitCert(cert);
        }
        /// <summary>
        /// 提交审核用户认证信息
        /// </summary>
        [Route("SaveAndSubmitCert")]
        [HttpPost]
        public object SaveAndSubmitCert(Cert cert)
        {
            return _UsersService.SaveAndSubmitCert(cert);
        }
        /// <summary>
        /// 通过用户ID获取其认证信息
        /// </summary>
        [Route("GetCert")]
        [HttpPost]
        public object GetCert(string userid)
        {
            return _UsersService.GetCert(userid);
        }
        #endregion

        #region 用户粉丝信息处理

        #endregion

    }
    public class ChangeUserApiModel
    {
        public string id { get; set; }
        public string oldPwd { get; set; }
        public string newPwd { get; set; }
    }
}
