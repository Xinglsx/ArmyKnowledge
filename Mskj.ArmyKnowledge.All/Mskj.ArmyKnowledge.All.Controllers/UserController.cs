using Mskj.ArmyKnowledge.Common;
using Mskj.ArmyKnowledge.All.ServiceContracts;
using System.Web.Http;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.Common.DataObject;

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
        public object Login(PostUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.LoginName) ||
                string.IsNullOrEmpty(user.Pwd))
            {
                return new ReturnResult<Users>(-4, "传入参数错误!");
            }
            return _UsersService.Login(user.LoginName,user.Pwd);
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户信息</param>
        [Route("Register")]
        [HttpPost]
        public object AddUser(PostUser user)
        {
            if(user == null)
            {
                return new ReturnResult<Users>(-4, "传入参数错误!");

            }
            if (user == null || string.IsNullOrEmpty(user.VerificationCode) || 
                string.IsNullOrEmpty(user.Pwd) || string.IsNullOrEmpty(user.PhoneNumber))
            {
                return new ReturnResult<Users>(-4, "传入参数错误!");
            }
            return _UsersService.AddUser(user.PhoneNumber, user.Pwd, user.VerificationCode);
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">新的用户信息</param>
        //[Route("UpdateUser")]
        //[HttpPost]
        public object UpdateUser(Users user)
        {
            return _UsersService.UpdateUser(user);
        }
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">新的用户信息</param>
        [Route("UpdateUserByKey")]
        [HttpPost]
        public object UpdateUserByKey(PostUser user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id) ||
                string.IsNullOrEmpty(user.KeyName))
            {
                return new ReturnResult<Users>(-4, "传入参数错误!");
            }
            var userTemp = _UsersService.GetOne(p => p.id == user.Id);
            switch(user.KeyName)
            {
                case "avatar":
                    userTemp.avatar = user.KeyValue;
                    break;
                case "nickname":
                    userTemp.nickname = user.KeyValue;
                    break;
                case "area":
                    userTemp.area = user.KeyValue;
                    break;
                case "organization":
                    userTemp.organization = user.KeyValue;
                    break;
                case "profession":
                    userTemp.profession = user.KeyValue;
                    break;
                case "signatures":
                    userTemp.signatures = user.KeyValue;
                    break;
                case "position":
                    userTemp.position = user.KeyValue;
                    break;
                default:
                    return new ReturnResult<bool>(-2, "未找到待更新的字段名称!");
            }
            return _UsersService.UpdateUser(userTemp);
        }
        /// <summary>
        /// 删除用户信息（暂时不提供调用）
        /// </summary>
        /// <param name="id">用户ID</param>
        //[Route("DeleteUser")]
        //[HttpDelete]
        public object DeleteUser(PostId user)
        {
            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
            return _UsersService.DeleteUser(user.Id);
        }
        /// <summary>
        /// 修改用户密码
        /// </summary>
        [Route("ChangePassword")]
        [HttpPost]
        public object ChangePassword(PostUser pwd)
        {
            if (pwd == null || string.IsNullOrEmpty(pwd.Id) ||
                string.IsNullOrEmpty(pwd.OldPwd) || string.IsNullOrEmpty(pwd.NewPwd))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
            return _UsersService.ChangePassword(pwd.Id, pwd.OldPwd, pwd.NewPwd);
        }
        /// <summary>
        /// 通过手机验证修改用户密码
        /// </summary>
        [Route("ChangePasswordByPhoneNumber")]
        [HttpPost]
        public object ChangePasswordByPhoneNumber(PostUser newPwd)
        {
            if (newPwd == null || string.IsNullOrEmpty(newPwd.PhoneNumber) ||
                string.IsNullOrEmpty(newPwd.NewPwd) || string.IsNullOrEmpty(newPwd.VerificationCode))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            } 
            return _UsersService.ChangePasswordByPhoneNumber(newPwd.PhoneNumber,
                newPwd.NewPwd, newPwd.VerificationCode);
        }
        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [Route("GetUserById")]
        [HttpGet]
        public object GetUserById(string id)
        {
            return _UsersService.GetUserById(id);
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
            if (cert == null || string.IsNullOrEmpty(cert.userid))
            {
                return new ReturnResult<Cert>(-4, "传入参数错误!");
            }
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
            if (cert == null || string.IsNullOrEmpty(cert.Id))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
            return _UsersService.DeleteCert(cert.Id);
        }
        /// <summary>
        /// 审核用户认证信息
        /// </summary>
        [Route("AuditCert")]
        [HttpPost]
        public object AuditCert(PostId cert)
        {
            if (cert == null || string.IsNullOrEmpty(cert.Id))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
            return _UsersService.AuditCert(cert.Id);
        }
        /// <summary>
        /// 拒绝用户认证信息
        /// </summary>
        [Route("RefuseCert")]
        [HttpPost]
        public ReturnResult<bool> RefuseCert(PostId cert)
        {
            if (cert == null || string.IsNullOrEmpty(cert.Id))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
            return _UsersService.RefuseCert(cert.Id);
        }
        /// <summary>
        /// 提交审核用户认证信息
        /// </summary>
        [Route("SubmitCert")]
        [HttpPost]
        public object SubmitCert(PostId cert)
        {
            if (cert == null || string.IsNullOrEmpty(cert.Id))
            {
                return new ReturnResult<bool>(-4, "传入参数错误!");
            }
            return _UsersService.SubmitCert(cert.Id);
        }
        /// <summary>
        /// 提交审核用户认证信息
        /// </summary>
        [Route("SaveAndSubmitCert")]
        [HttpPost]
        public object SaveAndSubmitCert(Cert cert)
        {
            if (cert == null || string.IsNullOrEmpty(cert.userid))
            {
                return new ReturnResult<Cert>(-4, "传入参数错误!");
            }
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
        /// <summary>
        /// 获取用户认证信息列表
        /// </summary>
        /// <param name="filter">查询关键字 支持真实姓名、组织、组织机构代码查询</param>
        /// <param name="state">状态 -1全部 0新建 1提交审核 2审核通过 3审核不通过</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        [Route("GetCerts")]
        [HttpGet]
        public object GetCerts(string filter = "",
            int state = 1, int pageIndex = 1, int pageSize = 10)
        {
            return _UsersService.GetCerts(filter, state, pageIndex, pageSize);
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
        public object GetUsers(string filter = "",string profession = "全部",int type = 2,
            int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            return _UsersService.GetUsers(filter,profession, type,pageIndex,pageSize,sortType);
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
            if (fans == null || string.IsNullOrEmpty(fans.userid1) ||
                string.IsNullOrEmpty(fans.userid2))
            {
                return new ReturnResult<Fans>(-4, "传入参数错误!");
            }
            else if(fans.userid1 == fans.userid2)
            {
                return new ReturnResult<Fans>(-4, "无需关注自己!");
            }
            return _UsersService.AddFans(fans);
        }
        /// <summary>
        /// 删除粉丝信息
        /// </summary>
        [Route("DeleteFans")]
        [HttpPost]
        public object DeleteFans(Fans fans)
        {
            if (fans == null || string.IsNullOrEmpty(fans.userid1) ||
                string.IsNullOrEmpty(fans.userid2))
            {
                return new ReturnResult<Fans>(-4, "传入参数错误!");
            }
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
