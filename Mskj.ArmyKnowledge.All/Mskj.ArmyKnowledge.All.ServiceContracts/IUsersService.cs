﻿using Mskj.ArmyKnowledge.All.Common.DataObj;
using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using Mskj.LiteFramework.Base;
using Mskj.LiteFramework.Common;
using System.Collections.Generic;

namespace Mskj.ArmyKnowledge.All.ServiceContracts
{
    public interface IUsersService : IBaseService<Users>,IServiceContract
    {
        #region 用户基础信息
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="loginname">登录名</param>
        /// <param name="pwd">密码</param>
        ReturnResult<Users> Login(string loginname, string pwd);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="phoneNumber">电话号码</param>
        /// <param name="pwd">密码</param>
        /// <param name="verificationCode">验证码</param>
        ReturnResult<Users> AddUser(string phoneNumber,
            string pwd, string verificationCode);
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
        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="mobileNumber">手机号</param>
        /// <returns></returns>
        bool ExistsUserByPhoneNumber(string mobileNumber);
        /// <summary>
        /// 通过手机号及验证码修改密码
        /// </summary>
        /// <param name="phoneNumber">电话号码</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="verificationcode">验证码</param>
        /// <returns></returns>
        ReturnResult<bool> ChangePasswordByPhoneNumber(string phoneNumber,
            string newPwd, string verficationCode);
        /// <summary>
        /// 通过手机号获取用户信息
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        ReturnResult<Users> GetUserByPhoneNumber(string phoneNumber);
        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        ReturnResult<Users> GetUserById(string id);
        /// <summary>
        /// 更新用户收藏数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="count">增减数量</param>
        bool UpdateCollectCount(string userId, int count = 1);
        /// <summary>
        /// 更新用户被点赞数
        /// </summary>
        /// <param name="userId">问题ID</param>
        /// <param name="count">增减数量</param>
        bool UpdatePraiseCount(string questionId, int count = 1);
        #endregion

        #region 用户认证信息
        /// <summary>
        /// 新增用户认证信息
        /// </summary>
        ReturnResult<Cert> AddCert(Cert cert);
        /// <summary>
        /// 更新用户认证信息
        /// </summary>
        ReturnResult<bool> UpdateCert(Cert cert);
        /// <summary>
        /// 删除用户认证信息
        /// </summary>
        ReturnResult<bool> DeleteCert(string certid);
        /// <summary>
        /// 审核用户认证信息
        /// </summary>
        ReturnResult<bool> AuditCert(string id);
        /// <summary>
        /// 拒绝用户认证信息
        /// </summary>
        ReturnResult<bool> RefuseCert(string id);
        /// <summary>
        /// 提交审核用户认证信息
        /// </summary>
        ReturnResult<bool> SubmitCert(string id);
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
        /// <summary>
        /// 获取用户认证信息列表
        /// </summary>
        /// <param name="filter">查询关键字 支持真实姓名、组织、组织机构代码查询</param>
        /// <param name="state">状态 -1全部 0新建 1提交审核 2审核通过 3审核不通过</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        ReturnResult<IPagedData<Cert>> GetCerts(string filter = "",
            int state = 1, int pageIndex = 1, int pageSize = 10);
        #endregion

        #region 专家用户信息
        /// <summary>
        /// 获取已有专业分类
        /// </summary>
        ReturnResult<List<string>> GetProfessionCategory();
        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="filter">查寻条件</param>
        /// <param name="profession">专业</param>
        /// <param name="userType">用户类型 -1-获取全部</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式 0-综合排序 1-回答问题数 2-采纳问题数</param>
        /// <returns></returns>
        ReturnResult<IPagedData<Users>> GetUsers(string filter = "", string profession = "全部",
            int userType = 2, int pageIndex = 1, int pageSize = 10, int sortType = 0);
        #endregion

        #region 用户关注(粉丝)信息
        /// <summary>
        /// 增加粉丝信息
        /// </summary>
        ReturnResult<Fans> AddFans(Fans fans);
        /// <summary>
        /// 删除粉丝信息
        /// </summary>
        ReturnResult<Fans> DeleteFans(Fans fans);
        /// <summary>
        /// 获取用户间粉丝关系信息
        /// </summary>
        /// <param name="userid1">用户1ID</param>
        /// <param name="userid2">用户2ID</param>
        /// <returns></returns>
        ReturnResult<Fans> GetFans(string userid1, string userid2);
        /// <summary>
        /// 获取用户的粉丝信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        ReturnResult<IPagedData<UserFansModel>> GetFans(string userid,
            int pageIndex = 1, int pageSize = 10);
        /// <summary>
        /// 获取用户的关注信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        ReturnResult<IPagedData<UserFansModel>> GetFollows(string userid,
            int pageIndex = 1, int pageSize = 10);
        #endregion
    }
}
