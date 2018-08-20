using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using System.Web.Http;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.Common.PostData;

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
        public object AddUser(PostUser user)
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
        /// 删除用户信息（暂时不提供调用）
        /// </summary>
        /// <param name="id">用户ID</param>
        //[Route("DeleteUser")]
        //[HttpDelete]
        public object DeleteUser(PostId user)
        {
            return _UsersService.DeleteUser(user.Id);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        [Route("ChangePassword")]
        [HttpPost]
        public object ChangePassword(PostUser pwd)
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
        /// 删除用户认证信息
        /// </summary>
        [Route("DeleteCert")]
        [HttpDelete]
        public object DeleteCert(PostId cert)
        {
            return _UsersService.DeleteCert(cert.Id);
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
        [HttpGet]
        public object GetCert(string userid)
        {
            return _UsersService.GetCert(userid);
        }
        #endregion

        #region 专家用户信息
        /// <summary>
        /// 获取已有专业分类
        /// </summary>
        [Route("GetProfessionCategory")]
        [HttpGet]
        public object GetProfessionCategory()
        {
            return _UsersService.GetProfessionCategory();
        }
        /// <summary>
        /// 分页获取专家用户列表
        /// </summary>
        /// <param name="type">用户类型 100-获取全部</param>
        /// <param name="state">状态</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式</param>
        /// <returns></returns>
        [Route("GetUsers")]
        [HttpGet]
        public object GetUsers(int type = 2,
            int state = 0, int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            return _UsersService.GetUsers();
        }
        #endregion

        #region 用户关注(粉丝)信息
        /// <summary>
        /// 增加粉丝信息
        /// </summary>
        [Route("AddFans")]
        [HttpPost]
        public object AddFans(Fans fans)
        {
            return _UsersService.AddFans(fans);
        }
        /// <summary>
        /// 删除粉丝信息
        /// </summary>
        [Route("DeleteFans")]
        [HttpPost]
        public object DeleteFans(Fans fans)
        {
            return _UsersService.DeleteFans(fans);
        }
        /// <summary>
        /// <summary>
        /// 获取用户间粉丝关系信息
        /// </summary>
        /// <param name="userid1">用户1ID</param>
        /// <param name="userid2">用户2ID</param>
        /// <returns></returns>
        [Route("GetFansContact")]
        [HttpGet]
        public object GetFansContact(string userid1, string userid2)
        {
            return _UsersService.GetFans(userid1, userid2);
        }
        /// <summary>
        /// <summary>
        /// 获取用户的粉丝信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        [Route("GetUserFans")]
        [HttpGet]
        public object GetUserFans(string userid,int pageIndex = 1, int pageSize = 10)
        {
            return _UsersService.GetFans(userid, pageIndex, pageSize);
        }
        /// <summary>
        /// <summary>
        /// 获取用户的关注信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        [Route("GetUserFollows")]
        [HttpGet]
        public object GetFollows(string userid,int pageIndex = 1, int pageSize = 10)
        {
            return _UsersService.GetFollows(userid, pageIndex, pageSize);
        }
        #endregion

    }
}
