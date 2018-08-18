using Mskj.ArmyKnowledge.All.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using QuickShare.LiteFramework.Base;
using Mskj.ArmyKnowledge.Common.DataObject;
using QuickShare.LiteFramework.Common;
using QuickShare.LiteFramework.Common.Extenstions;
using System.Linq.Expressions;
using System.Data.SqlClient;
using Mskj.ArmyKnowledge.Core;
using Mskj.ArmyKnowledge.All.Domains;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class UsersService : BaseService<Users, Users>,IUsersService
    {
        #region 构造函数
        private readonly IRepository<Cert> _CertRepository;
        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public UsersService(IRepository<Users> usersRepository,
            IRepository<Cert> certRepository) : base(usersRepository)
        {
            this._CertRepository = certRepository;
        }
        #endregion

        #region 用户基础信息
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录信息，主要传loginname和加密后的pwd</param>
        public ReturnResult<Users> Login(Users user)
        {
            var existUser = this.GetOne(p => p.phonenumber == user.loginname ||
                p.loginname == user.loginname);
            if (existUser == null)
            {
                return new ReturnResult<Users>(-2, "用户名或手机号不存在!");
            }
            else if (!user.pwd.Equals(existUser.pwd))
            {
                return new ReturnResult<Users>(-2, "密码错误，请重试！");
            }
            else
            {
                return new ReturnResult<Users>(1, existUser);
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户信息</param>
        public ReturnResult<Users> AddUser(Users user)
        {
            bool saveResult = false;
            user.id = Guid.NewGuid().ToString();
            user.loginname = user.phonenumber;
            user.nickname = "新用户" + user.phonenumber;
            var existUser = this.GetOne(p => p.phonenumber == user.phonenumber);
            if (existUser != null)
            {
                //获取验证码的时候应该就要校验了，这个地方只是确保一下。
                return new ReturnResult<Users>(-2, "手机号已被使用，请更换！");
            }
            try
            {
                saveResult = this.Add(user);
            }
            catch (Exception exp)
            {
                return new ReturnResult<Users>(-1, exp.Message);
            }
            if (saveResult)
            {
                return new ReturnResult<Users>(1, user);
            }
            else
            {
                return new ReturnResult<Users>(-2, "用户信息保存失败！");
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user">新的用户信息</param>
        public ReturnResult<bool> UpdateUser(Users user)
        {
            var existUser = this.GetOne(p => p.phonenumber == user.phonenumber &&
                p.id != user.id);
            if (existUser != null)
            {
                //获取验证码的时候应该就要校验了，这个地方只是确保一下。
                return new ReturnResult<bool>(-2, false, "手机号已被使用，请更换！");
            }
            existUser = this.GetOne(p => p.loginname == user.loginname &&
                p.id != user.id);
            if (existUser != null)
            {
                //获取验证码的时候应该就要校验了，这个地方只是确保一下。
                return new ReturnResult<bool>(-2, false, "用户名已被使用，请更换！");
            }
            bool updateResult = false;
            try
            {
                updateResult = this.Update(user);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, false, exp.Message);
            }
            if (updateResult)
            {
                return new ReturnResult<bool>(1, updateResult);
            }
            else
            {
                return new ReturnResult<bool>(-2, updateResult, "用户信息更新失败！");
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        public ReturnResult<bool> DeleteUser(string id)
        {
            bool deleteResult = false;
            try
            {
                deleteResult = this.DeleteByKey(id);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, exp.Message);
            }
            if (deleteResult)
            {
                return new ReturnResult<bool>(1, deleteResult);
            }
            else
            {
                return new ReturnResult<bool>(-2, deleteResult, "用户信息删除失败！");
            }
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        /// <returns></returns>
        public ReturnResult<bool> ChangePassword(string id, string oldPwd, string newPwd)
        {
            var user = this.GetOne(p => p.id == id);
            if (user == null)
            {
                return new ReturnResult<bool>(-2, "不存在该用户");
            }
            else if (user.pwd != oldPwd)
            {
                return new ReturnResult<bool>(-2, "用户原密码有误");
            }
            else
            {
                user.pwd = newPwd;
                bool updateResult = false;
                try
                {
                    updateResult = this.Update(user);
                }
                catch (Exception exp)
                {
                    return new ReturnResult<bool>(-1, false, exp.Message);
                }
                if (updateResult)
                {
                    return new ReturnResult<bool>(1, updateResult);
                }
                else
                {
                    return new ReturnResult<bool>(-2, updateResult, "用户密码更新失败！");
                }
            }
        }
        #endregion

        #region 用户认证信息
        /// <summary>
        /// 新增用户认证信息
        /// </summary>
        public ReturnResult<Cert> AddCert(Cert cert)
        {
            cert.id = Guid.NewGuid().ToString();
            bool addRes = false;
            try
            {
                addRes = _CertRepository.Add(cert);
                var user = GetOne(p => p.id == cert.userid);
                user.usertype = cert.userType;
                addRes = Update(user);
            }
            catch (Exception exp)
            {
                return new ReturnResult<Cert>(-1, exp.Message);
            }
            if(addRes)
            {
                return new ReturnResult<Cert>(1, cert);
            }
            else
            {
                return new ReturnResult<Cert>(-2, "新增认证信息失败!");
            }
        }
        /// <summary>
        /// 更新用户认证信息
        /// </summary>
        public ReturnResult<bool> UpdateCert(Cert cert)
        {
            bool res = false;
            try
            {
                res = _CertRepository.Update(cert);
            }
            catch (Exception exp)
            {
                return new ReturnResult<bool>(-1, exp.Message);
            }
            if (res)
            {
                return new ReturnResult<bool>(1, true);
            }
            else
            {
                return new ReturnResult<bool>(-2, "更新认证信息失败!");
            }
        }
        /// <summary>
        /// 审核用户认证信息
        /// </summary>
        public ReturnResult<bool> AuditCert(Cert cert)
        {
            if (cert.certstate != 1)
            {
                return new ReturnResult<bool>(-2, "待审核的认证信息状态不是[提交审核状态]！");
            }
            cert.certstate = 2;
            return this.UpdateCert(cert);
        }
        /// <summary>
        /// 提交审核用户认证信息
        /// </summary>
        public ReturnResult<bool> SubmitCert(Cert cert)
        {
            if (string.IsNullOrEmpty(cert.id))
            {
                cert.certstate = 1;
                return AddCert(cert);
            }
            else if (cert.certstate != 0)
            {
                return new ReturnResult<bool>(-2, "待提交审核的认证信息状态不是[新建状态]！");
            }
            else
            {
                cert.certstate = 1;
                return UpdateCert(cert);
            }
        }
        /// <summary>
        /// 保存并提交用户认证信息
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ReturnResult<Cert> SaveAndSubmitCert(Cert cert)
        {
            cert.certstate = 1;
            return AddCert(cert);
        }
        /// <summary>
        /// 通过用户ID获取用户认证信息
        /// </summary>
        public ReturnResult<Cert> GetCert(string userid)
        {
            var cert = _CertRepository.FindInclude(p => p.userid == userid).FirstOrDefault();
            if(cert == null || string.IsNullOrEmpty(cert.id))
            {
                return new ReturnResult<Cert>(-2, "未找到该用户认证信息！");
            }
            return new ReturnResult<Cert>(1, cert);
        }
        #endregion

    }
}
