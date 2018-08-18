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
        private readonly IRepository<Users> _UsersRepository;
        private readonly IRepository<Cert> _CertRepository;
        private readonly IRepository<Fans> _FansRepository;
        private readonly IRepository<Follower> _FollowerRepository;//暂时先不用，只用Fans表
        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public UsersService(IRepository<Users> usersRepository,IRepository<Cert> certRepository,
            IRepository<Fans> fansRepository,IRepository<Follower> followerRepository) 
            : base(usersRepository)
        {
            _UsersRepository = usersRepository;
            this._CertRepository = certRepository;
            _FansRepository = fansRepository;
            _FollowerRepository = followerRepository;
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
            user.registertime = DateTime.Now;
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
            var res = UpdateCert(cert);
            if(res.code > 1)
            {
                Users user = GetOne(p => p.id == cert.userid);
                user.iscertification = 2;
                res = UpdateUser(user);
            }
            return res;
        }
        /// <summary>
        /// 提交审核用户认证信息
        /// </summary>
        public ReturnResult<bool> SubmitCert(Cert cert)
        {
            if (cert.certstate != 0)
            {
                return new ReturnResult<bool>(-2, "待提交审核的认证信息状态不是[新建状态]！");
            }
            else
            {
                cert.certstate = 1;
                var res = UpdateCert(cert);
                if (res.code > 0)
                {
                    Users user = GetOne(p => p.id == cert.userid);
                    user.iscertification = 2;
                    res = UpdateUser(user);
                }
                return res;
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
            var res = AddCert(cert);
            if (res.code > 1)
            {
                Users user = GetOne(p => p.id == cert.userid);
                user.iscertification = 2;
                if(UpdateUser(user).code < 0)
                {
                    return new ReturnResult<Cert>(-2, "认证申请成功，基础信息更新失败！");
                }
            }
            return res;
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

        #region 专家用户信息
        /// <summary>
        /// 获取已有专业分类
        /// </summary>
        public ReturnResult<List<string>> GetProductCategory()
        {
            List<string> categorys = new List<string> { "全部" };
            var res = _UsersRepository.Find()
                .Where(p => p.usertype == 2 && p.iscertification >= 1)
                .Select(p => p.profession).Distinct().ToList();
            if (res != null && res.Count > 0)
            {
                categorys.AddRange(res);
                return new ReturnResult<List<string>>(1, res);
            }
            else
            {
                return new ReturnResult<List<string>>(1, categorys);
            }
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
        public ReturnResult<IPagedData<Users>> GetUserss(int type = 2,
            int state = 0, int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            Expression<Func<Users, bool>> expression;
            if (type == 100)
            {
                expression = x => x.userstate == state;
            }
            else
            {
                expression = x => x.userstate == state && x.usertype == type;

            }
            return GetBaseUserss(pageIndex, pageSize, sortType, expression);
        }
        /// <summary>
        /// 分页获取专家用户列表(封装排序方式)
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式 0-综合排序 1-最新发布</param>
        /// <param name="expression">查询表达示</param>
        /// <returns></returns>
        private ReturnResult<IPagedData<Users>> GetBaseUserss(int pageIndex,
            int pageSize, int sortType, Expression<Func<Users, bool>> expression)
        {
            List<SortInfo<Users>> sorts = new List<SortInfo<Users>>();
            SortInfo<Users> sort;
            switch (sortType)
            {
                case 0:
                    sort = new SortInfo<Users>(p => p.compositescores,
                        SortOrder.Descending);
                    break;
                case 1:
                    sort = new SortInfo<Users>(p => p.answercount,
                        SortOrder.Descending);
                    break;
                case 2:
                    sort = new SortInfo<Users>(p => p.adoptedcount,
                        SortOrder.Ascending);
                    break;
                default:
                    sort = new SortInfo<Users>(p => p.compositescores,
                        SortOrder.Descending);
                    break;
            }
            sorts.Add(sort);
            //所有排序之后，再按时间降序
            sorts.Add(new SortInfo<Users>(p => p.registertime, SortOrder.Descending));
            return new ReturnResult<IPagedData<Users>>(1,
                    GetPage(pageIndex, pageSize, sorts, expression));
        }
        #endregion

        #region 用户关注(粉丝)信息
        /// <summary>
        /// 更新粉丝数量
        /// </summary>
        /// <param name="userid">被关注都</param>
        /// <param name="followuserid">关注者</param>
        /// <param name="count">修改数量 1表示加 -1表示减</param>
        private void UpdateFansCount(string userid,string followuserid,int count)
        {
            //更新用户基本信息中的粉丝数
            var user1 = GetOne(p => p.id == userid);
            user1.fanscount += count;
            this.UpdateUser(user1);
            var user2 = GetOne(p => p.id == followuserid);
            user2.followcount += count; ;
            this.UpdateUser(user2);
        }
        /// <summary>
        /// 增加粉丝信息
        /// </summary>
        public ReturnResult<Fans> AddFans(Fans fans)
        {
            bool res = false;
            //是否存在用户1关注用户2的粉丝信息，存在则更新成互粉
            var existFans = _FansRepository.FindInclude(p => p.userid1 == fans.userid1
                && p.userid2 == fans.userid2).FirstOrDefault(); 
            if(existFans != null && !string.IsNullOrEmpty(existFans.id))
            {
                if(existFans.fansstate == 1 || existFans.fansstate == 0)
                {
                    return new ReturnResult<Fans>(-2, "已存在关注信息!");
                }
                else
                {
                    existFans.fansstate = 0;
                    existFans.updatetime = DateTime.Now;
                    try
                    {
                        res = _FansRepository.Update(existFans);
                    }
                    catch (Exception exp)
                    {
                        return new ReturnResult<Fans>(-1, exp.Message);
                    }
                    if (res)
                    {
                        UpdateFansCount(existFans.userid1, existFans.userid2, 1);
                        return new ReturnResult<Fans>(1, existFans);
                    }
                    else
                    {
                        return new ReturnResult<Fans>(-2, "粉丝信息更新为互粉失败！");
                    }
                }
            }
            //反查是否存在用户2关注用户1的粉丝信息，存在者直接更新成互粉
            existFans = _FansRepository.FindInclude(p => p.userid1 == fans.userid2
                && p.userid2 == fans.userid1).FirstOrDefault();
            if (existFans != null && !string.IsNullOrEmpty(existFans.id))
            {
                if (existFans.fansstate == 2 || existFans.fansstate == 0)
                {
                    return new ReturnResult<Fans>(-2, "已存在关注信息!");
                }
                else
                {
                    existFans.fansstate = 0;
                    existFans.updatetime = DateTime.Now;
                    try
                    {
                        //更新用户基本信息中的粉丝数
                        UpdateFansCount(existFans.userid2, existFans.userid1, 1);
                        res = _FansRepository.Update(existFans);
                    }
                    catch (Exception exp)
                    {
                        return new ReturnResult<Fans>(-1, exp.Message);
                    }
                    if (res)
                    {
                        return new ReturnResult<Fans>(1, existFans);
                    }
                    else
                    {
                        return new ReturnResult<Fans>(-2, "粉丝信息更新为互粉失败！");
                    }
                }
            }
            fans.id = Guid.NewGuid().ToString();
            existFans.updatetime = DateTime.Now;
            try
            {
                res = _FansRepository.Add(fans);
            }
            catch (Exception exp)
            {
                return new ReturnResult<Fans>(-1, exp.Message);
            }
            if (res)
            {
                //更新用户基本信息中的粉丝数
                UpdateFansCount(existFans.userid1, existFans.userid2, 1);
                return new ReturnResult<Fans>(1, fans);
            }
            else
            {
                return new ReturnResult<Fans>(-2, "粉丝信息保存失败！");
            }
        }
        /// <summary>
        /// 删除粉丝信息
        /// </summary>
        public ReturnResult<Fans> DeleteFans(Fans fans)
        {
            bool res = false;
            //是否存在用户1与用户2的粉丝信息
            var existFans = _FansRepository.FindInclude(p => p.userid1 == fans.userid1
                && p.userid2 == fans.userid2).FirstOrDefault();
            if (existFans != null && !string.IsNullOrEmpty(existFans.id))
            {
                //如果是用户2关注用户1，则删除这条记录
                if (existFans.fansstate == 1)
                {
                    try
                    {
                        res = _FansRepository.Delete(existFans);
                    }
                    catch (Exception exp)
                    {
                        return new ReturnResult<Fans>(-1, exp.Message);
                    }
                    if (res)
                    {
                        UpdateFansCount(existFans.userid1, existFans.userid2, -1);
                        return new ReturnResult<Fans>(1, existFans);
                    }
                    else
                    {
                        return new ReturnResult<Fans>(-2, "粉丝信息删除失败！");
                    }
                }
                //如果是互粉信息，则更新成被关注信息
                else if (existFans.fansstate == 0)
                {
                    existFans.fansstate = 2;
                    try
                    {
                        res = _FansRepository.Update(existFans);
                    }
                    catch (Exception exp)
                    {
                        return new ReturnResult<Fans>(-1, exp.Message);
                    }
                    if (res)
                    {
                        UpdateFansCount(existFans.userid1, existFans.userid2, -1);
                        return new ReturnResult<Fans>(1, existFans);
                    }
                    else
                    {
                        return new ReturnResult<Fans>(-2, "粉丝信息取消互粉失败！");
                    }
                }
                else
                {
                    return new ReturnResult<Fans>(-2, "不存在关注信息!");
                }
            }
            ////反查是否存在被关注信息
            existFans = _FansRepository.FindInclude(p => p.userid1 == fans.userid2
                && p.userid2 == fans.userid1).FirstOrDefault();
            if (existFans != null && !string.IsNullOrEmpty(existFans.id))
            {
                //如果是用户2关注用户1，则删除这条记录
                if (existFans.fansstate == 2)
                {
                    try
                    {
                        res = _FansRepository.Delete(existFans);
                    }
                    catch (Exception exp)
                    {
                        return new ReturnResult<Fans>(-1, exp.Message);
                    }
                    if (res)
                    {
                        UpdateFansCount(existFans.userid1, existFans.userid2, -1);
                        return new ReturnResult<Fans>(1, existFans);
                    }
                    else
                    {
                        return new ReturnResult<Fans>(-2, "粉丝信息删除失败！");
                    }
                }
                //如果是互粉信息，则更新成被关注信息
                else if (existFans.fansstate == 0)
                {
                    existFans.fansstate = 1;
                    try
                    {
                        res = _FansRepository.Update(existFans);
                    }
                    catch (Exception exp)
                    {
                        return new ReturnResult<Fans>(-1, exp.Message);
                    }
                    if (res)
                    {
                        UpdateFansCount(existFans.userid1, existFans.userid2, -1);
                        return new ReturnResult<Fans>(1, existFans);
                    }
                    else
                    {
                        return new ReturnResult<Fans>(-2, "粉丝信息取消互粉失败！");
                    }
                }
                else
                {
                    return new ReturnResult<Fans>(-2, "不存在关注信息!");
                }
            }
            return new ReturnResult<Fans>(-2, "不存在关注信息！");
        }
        /// <summary>
        /// 获取用户间粉丝关系信息
        /// </summary>
        /// <param name="userid1">用户1ID</param>
        /// <param name="userid2">用户2ID</param>
        /// <returns></returns>
        public ReturnResult<Fans> GetFans(string userid1, string userid2)
        {
            var existFans = _FansRepository.FindInclude(p => 
                (p.userid1 == userid1 && p.userid2 == userid2) || 
                (p.userid1 == userid2 && p.userid2 == userid1)).FirstOrDefault();
            return new ReturnResult<Fans>(1, existFans);
        }
        /// <summary>
        /// 获取用户的粉丝信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Fans>> GetFans(string userid,
            int pageIndex = 1, int pageSize = 10)
        {
            var res = _FansRepository.Find()
                .Where(p =>
                (p.userid1 == userid && (p.fansstate == 0 || p.fansstate == 1)) ||
                (p.userid2 == userid && (p.fansstate == 0 || p.fansstate == 2)))
                .OrderByDescending(p => p.updatetime).ToPage(pageIndex,pageSize);
            return new ReturnResult<IPagedData<Fans>>(1, res);
        }
        /// <summary>
        /// 获取用户的关注信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Fans>> GetFollows(string userid,
            int pageIndex = 1, int pageSize = 10)
        {
            var res = _FansRepository.Find()
                .Where(p =>
                (p.userid1 == userid && (p.fansstate == 0 || p.fansstate == 2)) ||
                (p.userid2 == userid && (p.fansstate == 0 || p.fansstate == 1)))
                .OrderByDescending(p => p.updatetime).ToPage(pageIndex, pageSize);
            return new ReturnResult<IPagedData<Fans>>(1, res);
        }
        #endregion
    }
}
