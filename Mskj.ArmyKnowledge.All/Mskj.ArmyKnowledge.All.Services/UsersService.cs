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
using Mskj.ArmyKnowledge.All.Domains;
using QuickShare.LiteFramework.Foundation;
using QuickShare.LiteFramework;
using Mskj.ArmyKnowledge.All.Common.PostData;
using Mskj.ArmyKnowledge.All.Common.DataObj;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class UsersService : BaseService<Users, Users>,IUsersService
    {
        #region 构造函数
        private readonly IRepository<Users> _UsersRepository;
        private readonly IRepository<Cert> _CertRepository;
        private readonly IRepository<Fans> _FansRepository;
        private readonly IRepository<Follower> _FollowerRepository;//暂时先不用，只用Fans表
        private readonly IRepository<Dictionary> _DicRepository;
        ILogger logger;
        ICache cache;


        /// <summary>
        /// 构造函数，必须要传一个实参给repository
        /// </summary>
        /// <param name="goodsRepository"></param>
        public UsersService(IRepository<Users> usersRepository,IRepository<Cert> certRepository,
            IRepository<Fans> fansRepository,IRepository<Follower> followerRepository,
            IRepository<Dictionary> dicRepository) : base(usersRepository)
        {
            _UsersRepository = usersRepository;
            _CertRepository = certRepository;
            _FansRepository = fansRepository;
            _FollowerRepository = followerRepository;
            _DicRepository = dicRepository;

            logger = AppInstance.Current.Resolve<ILogger>();
            cache = AppInstance.Current.Resolve<ICache>();
        }
        #endregion

        #region 用户基础信息
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录信息，主要传loginname和加密后的pwd</param>
        public ReturnResult<Users> Login(Users user)
        {
            logger.LogInfo("loginname:"+ user.loginname+" pwd"+user.pwd);
            logger.LogInfo(user);
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
                //将PWD别成token
                string token = Guid.NewGuid().ToString();
                existUser.pwd = token;
                return new ReturnResult<Users>(1, existUser);
            }
        }
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户信息</param>
        public ReturnResult<Users> AddUser(string phoneNumber,
            string pwd, string verificationCode)
        {
            logger.LogInfo(string.Format("PhoneNumber:{0} Pwd:{1} VerificationCode:{2}",
                phoneNumber, pwd, verificationCode));
            //识别验证码
            //发送成功之后，将发送成功的code和手机号保存在缓存中
            var  tempCode = cache.GetObject("mobilecode-" + phoneNumber);
            if(tempCode == null || !tempCode.ToString().Equals(verificationCode))
            {
                return new ReturnResult<Users>(-2, "验证码错误！");
            }

            Users user = new Users();
            user.phonenumber = phoneNumber;
            user.pwd = pwd;
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
                logger.LogError("新增用户信息出错！", exp);
                return new ReturnResult<Users>(-1, "系统异常，请稍后重试。");
            }
            if (saveResult)
            {
                user.pwd = "";
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
            logger.LogInfo(string.Format("phonenumber:{0} id:{1}",
                user.phonenumber, user.id));
            var existUser = this.GetOne(p => p.phonenumber == user.phonenumber &&
                p.id != user.id);
            if (existUser != null)
            {
                return new ReturnResult<bool>(-2, false, "手机号已被使用，请更换！");
            }
            existUser = this.GetOne(p => p.loginname == user.loginname &&
                p.id != user.id);
            if (existUser != null)
            {
                return new ReturnResult<bool>(-2, false, "登录名已被使用，请更换！");
            }
            bool updateResult = false;
            try
            {
                //user.isadmin = false;已不再对接口公布些字段//接口不可将些字段更新为true,只能后台update
                user.updatetime = DateTime.Now;
                updateResult = this.Update(user);
            }
            catch (Exception exp)
            {
                logger.LogError("更新用户信息出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
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
                logger.LogError("删除用户信息出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
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
            logger.LogInfo(string.Format("id:{0} oldPwd:{1} newPwd:{2}",
                id, oldPwd,newPwd));
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
                    logger.LogError("修改用户密码时信息出错！", exp);
                    return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
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
        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns>true 已经被使用 false</returns>
        public bool ExistsUserByPhoneNumber(string mobileNumber)
        {
            logger.LogInfo(string.Format("mobileNumber:{0}", mobileNumber));
            var user = this.GetOne(p => p.phonenumber == mobileNumber);
            if(user != null && !string.IsNullOrEmpty(user.id))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 通过手机号及验证码修改密码
        /// </summary>
        /// <param name="phoneNumber">电话号码</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="verificationcode">验证码</param>
        /// <returns></returns>
        public ReturnResult<bool> ChangePasswordByPhoneNumber(string phoneNumber,
            string newPwd,string verficationCode)
        {
            var tempCode = cache.GetObject("mobilecode-" + phoneNumber);
            if (tempCode == null || !tempCode.ToString().Equals(verficationCode))
            {
                return new ReturnResult<bool>(-2, "验证码错误！");
            }

            var temp = GetOne(p => p.phonenumber == phoneNumber);
            if (temp == null || string.IsNullOrEmpty(temp.id))
            {
                return new ReturnResult<bool>(-2, "电话号码未注册！");
            }
            temp.pwd = newPwd;
            return UpdateUser(temp);
        }
        /// <summary>
        /// 通过手机号获取用户信息
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        public ReturnResult<Users> GetUserByPhoneNumber(string phoneNumber)
        {
            var temp = GetOne(p => p.phonenumber == phoneNumber);
            if(temp == null || string.IsNullOrEmpty(temp.id))
            {
                return new ReturnResult<Users>(-2, "电话号码未注册！");
            }
            temp.pwd = null;
            return new ReturnResult<Users>(1, temp);
        }
        /// <summary>
        /// 通过用户ID获取用户信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public ReturnResult<Users> GetUserById(string id)
        {
            var temp = GetOne(p => p.id == id);
            if (temp == null || string.IsNullOrEmpty(temp.id))
            {
                return new ReturnResult<Users>(-2, "用户ID不存在！");
            }
            temp.pwd = null;
            return new ReturnResult<Users>(1, temp);
        }
        /// <summary>
        /// 更新用户收藏数
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="count">增减数量</param>
        public bool UpdateCollectCount(string userId,int count = 1)
        {
            var user = GetOne(p => p.id == userId);
            if(user == null || string.IsNullOrEmpty(user.id))
            {
                return false;
            }
            user.collectcount += count;
            user.compositescores += 2 * count;
            bool updateResult = false;
            try
            {
                //user.isadmin = false;已不再对接口公布些字段//接口不可将些字段更新为true,只能后台update
                user.updatetime = DateTime.Now;
                updateResult = this.Update(user);
            }
            catch (Exception exp)
            {
                logger.LogError("更新用户信息出错！", exp);
                return false;
            }
            return updateResult;
        }
        #endregion

        #region 用户认证信息
        /// <summary>
        /// 新增用户认证信息
        /// </summary>
        public ReturnResult<Cert> AddCert(Cert cert)
        {
            //先要判断是否已经存在认证信息
            var existUser = _CertRepository.Find().Where(p => p.userid == cert.userid).FirstOrDefault();
            if(existUser != null && !string.IsNullOrEmpty(existUser.id))
            {
                return new ReturnResult<Cert>(-2, "已存在认证信息，无法新增!");
            }

            cert.id = Guid.NewGuid().ToString();
            cert.certstate = 4;
            cert.updatetime = DateTime.Now;
            bool addRes = false;
            try
            {
                addRes = _CertRepository.Add(cert);
            }
            catch (Exception exp)
            {
                logger.LogError("新增用户认证信息出错！", exp);
                return new ReturnResult<Cert>(-1, "系统异常，请稍后重试。");
            }
            if(addRes)
            {
                var user = GetOne(p => p.id == cert.userid);
                user.iscertification = 4;
                UpdateUser(user);
                return new ReturnResult<Cert>(1, cert);
            }
            else
            {
                return new ReturnResult<Cert>(-2, "新增认证信息失败,请稍后重试");
            }
        }
        /// <summary>
        /// 更新用户认证信息
        /// </summary>
        public ReturnResult<bool> UpdateCert(Cert cert)
        {
            bool res = false;
            cert.updatetime = DateTime.Now;
            try
            {
                res = _CertRepository.Update(cert);
            }
            catch (Exception exp)
            {
                logger.LogError("更新用户认证信息出错！", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            if (res)
            {
                return new ReturnResult<bool>(1, true);
            }
            else
            {
                return new ReturnResult<bool>(-2, "更新认证信息失败,请稍后重试");
            }
        }
        /// <summary>
        /// 删除用户认证信息
        /// </summary>
        public ReturnResult<bool> DeleteCert(string certid)
        {
            bool res = false;
            try
            {
                res = _CertRepository.Delete(certid);
            }
            catch (Exception exp)
            {
                logger.LogError("删除用户认证信息出错!", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            if (res)
            {
                var user = (from users in _UsersRepository.Find()
                            join cert in _CertRepository.Find() on users.id equals cert.userid
                            where cert.id == certid
                            select users).FirstOrDefault();
                user.iscertification = 0;
                UpdateUser(user);
                return new ReturnResult<bool>(1, true);
            }
            else
            {
                return new ReturnResult<bool>(-2, "更新认证信息失败,请稍后重试");
            }
        }
        /// <summary>
        /// 审核用户认证信息
        /// </summary>
        public ReturnResult<bool> AuditCert(string id)
        {
            Cert cert = _CertRepository.Find().Where(p => p.id == id).FirstOrDefault();
            if(cert == null || string.IsNullOrEmpty(cert.id))
            {
                return new ReturnResult<bool>(-2, "未找到认证信息！");
            }
            else if (cert.certstate != 1)
            {
                return new ReturnResult<bool>(-2, "认证信息状态不是[提交审核状态]！");
            }
            cert.certstate = 2;
            var res = UpdateCert(cert);
            if(res.code > 0)
            {
                Users user = GetOne(p => p.id == cert.userid);
                user.usertype = cert.usertype;
                user.iscertification = 3;
                user.compositescores += 100;//认证通过+100分
                UpdateUser(user);
            }
            return res;
        }
        /// <summary>
        /// 拒绝用户认证信息
        /// </summary>
        public ReturnResult<bool> RefuseCert(string id)
        {
            Cert cert = _CertRepository.Find().Where(p => p.id == id).FirstOrDefault();
            if (cert == null || string.IsNullOrEmpty(cert.id))
            {
                return new ReturnResult<bool>(-2, "未找到认证信息！");
            }
            else if (cert.certstate != 1)
            {
                return new ReturnResult<bool>(-2, "认证信息状态不是[提交审核状态]！");
            }
            cert.certstate = 3;
            var res = UpdateCert(cert);
            if (res.code > 0)
            {
                Users user = GetOne(p => p.id == cert.userid);
                user.iscertification = 2;
                user.compositescores -= 10;//认证不通过-10分
                UpdateUser(user);
            }
            return res;
        }
        /// <summary>
        /// 提交审核用户认证信息
        /// </summary>
        public ReturnResult<bool> SubmitCert(string id)
        {
            Cert cert = _CertRepository.Find().Where(p => p.id == id).FirstOrDefault();
            if (cert == null || string.IsNullOrEmpty(cert.id))
            {
                return new ReturnResult<bool>(-2, "未找到认证信息！");
            }
            else if (cert.certstate != 4 && cert.certstate != 2)
            {
                return new ReturnResult<bool>(-2, "认证信息状态不是[新建状态]或[审核不通过状态]！");
            }
            else
            {
                cert.certstate = 1;
                var res = UpdateCert(cert);
                if (res.code > 0)
                {
                    Users user = GetOne(p => p.id == cert.userid);
                    user.iscertification = 1;
                    user.compositescores += 10;//提交认证+10分
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
            //先要判断是否已经存在认证信息
            var existUser = _CertRepository.Find().Where(p => p.userid == cert.userid).FirstOrDefault();
            if (existUser != null && !string.IsNullOrEmpty(existUser.id))
            {
                return new ReturnResult<Cert>(-2, "已存在认证信息，无法新增!");
            }

            cert.id = Guid.NewGuid().ToString();
            cert.certstate = 1;
            cert.updatetime = DateTime.Now;
            bool addRes = false;
            try
            {
                addRes = _CertRepository.Add(cert);
            }
            catch (Exception exp)
            {
                logger.LogError(string.Format("保存并提交用户认证信息时更新用户信息出错！,用户ID：{0}"
                    , cert.userid), exp);
                return new ReturnResult<Cert>(-1, "系统异常，请稍后重试。");
            }
            if (addRes)
            {
                Users user = GetOne(p => p.id == cert.userid);
                user.iscertification = 1;
                user.compositescores += 10;//提交认证+10分
                UpdateUser(user);
                return new ReturnResult<Cert>(1, cert);
            }
            else
            {
                return new ReturnResult<Cert>(-2, "新增认证信息失败,请稍后重试");
            }
        }
        /// <summary>
        /// 通过用户ID获取用户认证信息
        /// </summary>
        public ReturnResult<Cert> GetCert(string userid)
        {
            var cert = _CertRepository.Find().Where(p => p.userid == userid).FirstOrDefault();
            if(cert == null || string.IsNullOrEmpty(cert.id))
            {
                return new ReturnResult<Cert>(-2, "未找到该用户认证信息！");
            }
            return new ReturnResult<Cert>(1, cert);
        }
        /// <summary>
        /// 获取用户认证信息列表
        /// </summary>
        /// <param name="filter">查询关键字 支持真实姓名、组织、组织机构代码查询</param>
        /// <param name="state">状态 -1全部 0新建 1提交审核 2审核通过 3审核不通过</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<Cert>> GetCerts(string filter = "",
            int state = 1, int pageIndex = 1, int pageSize = 10)
        {
            var res = _CertRepository.Find();
            if (state != -1)
            {
                res = res.Where(x => x.certstate == state);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                res = res.Where(x => x.realname.Contains(filter) || x.organization.Contains(filter) ||
                    x.creditcode.Contains(filter));
            }
            var result = res.OrderByDescending(p => p.updatetime).ToPage(pageIndex, pageSize);

            return new ReturnResult<IPagedData<Cert>>(1,result);
        }
        #endregion

        #region 专家用户信息
        /// <summary>
        /// 获取已有专业分类
        /// </summary>
        public ReturnResult<List<string>> GetProfessionCategory()
        {
            List<string> professions = new List<string> { "全部" };
            var res = _DicRepository.Find().Where(p => p.dicstate && p.dictype == 0)
                .Select(q => q.dicname);

            if (res != null && res.Count() > 0)
            {
                professions.AddRange(res.ToList());
            }
            return new ReturnResult<List<string>>(1, professions);
        }
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
        public ReturnResult<IPagedData<Users>> GetUsers(string filter = "",string profession = "全部",
            int userType = 2, int pageIndex = 1, int pageSize = 10, int sortType = 0)
        {
            Expression<Func<Users, bool>> exp1 = x => true;
            Expression<Func<Users, bool>> exp2 = x => true;
            Expression<Func<Users, bool>> exp3 = x => true;
            if (userType != -1)
            {
                exp1 = x => x.usertype == userType;
            }
            if(!"全部".Equals(profession) && !string.IsNullOrEmpty(profession))
            {
                exp2 = x => x.profession == profession;
            }
            if(!string.IsNullOrEmpty(filter))
            {
                exp3 = x => x.nickname.Contains(filter) || x.organization.Contains(filter) || 
                    x.signatures.Contains(filter);
            }

            return GetBaseUsers(pageIndex, pageSize, sortType, exp1.AndAlso(exp2).AndAlso(exp3));
        }
        /// <summary>
        /// 分页获取用户列表(封装排序方式)
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="sortType">排序方式 0-综合排序 1-回答问题数 2-采纳问题数</param>
        /// <param name="expression">查询表达示</param>
        /// <returns></returns>
        private ReturnResult<IPagedData<Users>> GetBaseUsers(int pageIndex,
            int pageSize, int sortType, Expression<Func<Users, bool>> expression)
        {
            List<SortInfo<Users>> sorts = new List<SortInfo<Users>>();
            SortInfo<Users> sort;
            switch (sortType)
            {
                case 0:
                    sort = new SortInfo<Users>(p => new { p.compositescores },
                        SortOrder.Descending);
                    break;
                case 1:
                    sort = new SortInfo<Users>(p => new { p.answercount },
                        SortOrder.Descending);
                    break;
                case 2:
                    sort = new SortInfo<Users>(p => new { p.adoptedcount },
                        SortOrder.Ascending);
                    break;
                default:
                    sort = new SortInfo<Users>(p => new { p.compositescores },
                        SortOrder.Descending);
                    break;
            }
            sorts.Add(sort);
            //所有排序之后，再按时间降序
            sorts.Add(new SortInfo<Users>(p => new { p.registertime }, SortOrder.Descending));
            var res = GetPage(pageIndex, pageSize, sorts, expression);
            if(res.Data.Count() > 0)
            {
                for(int i = 0; i < res.Data.Count(); i++)
                {
                    res.Data[i].pwd = null;
                }
            }
            return new ReturnResult<IPagedData<Users>>(1,res);
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
            var user2 = GetOne(p => p.id == followuserid);
            user2.followcount += count; 
            user2.compositescores += count * 3;
            try
            {
                _UsersRepository.Update(new List<Users> { user1, user2 });

            }
            catch (Exception exp)
            {
                logger.LogError("更新粉丝数量出错！", exp);
            }
        }
        /// <summary>
        /// 增加粉丝信息
        /// </summary>
        public ReturnResult<Fans> AddFans(Fans fans)
        {
            bool res = false;
            //是否存在用户1关注用户2的粉丝信息，存在则更新成互粉
            var existFans = _FansRepository.Find().Where(p => p.userid1 == fans.userid1
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
                        logger.LogError("增加粉丝信息时更新出错！", exp);
                        return new ReturnResult<Fans>(-1, "系统异常，请稍后重试。");
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
            existFans = _FansRepository.Find().Where(p => p.userid1 == fans.userid2
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
                        logger.LogError("增加粉丝信息时更新出错2！", exp);
                        return new ReturnResult<Fans>(-1, "系统异常，请稍后重试。");
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
            fans.updatetime = DateTime.Now;
            fans.fansstate = 1;
            try
            {
                res = _FansRepository.Add(fans);
            }
            catch (Exception exp)
            {
                logger.LogError("增加粉丝信息时新增出错！", exp);
                return new ReturnResult<Fans>(-1, "系统异常，请稍后重试。");
            }
            if (res)
            {
                //更新用户基本信息中的粉丝数
                UpdateFansCount(fans.userid1, fans.userid2, 1);
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
            var existFans = _FansRepository.Find().Where(p => p.userid1 == fans.userid1
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
                        logger.LogError("删除粉丝信息时删除出错！", exp);
                        return new ReturnResult<Fans>(-1, "系统异常，请稍后重试。");
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
                        logger.LogError("删除粉丝信息时更新出错！", exp);
                        return new ReturnResult<Fans>(-1, "系统异常，请稍后重试。");
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
            existFans = _FansRepository.Find().Where(p => p.userid1 == fans.userid2
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
                        logger.LogError("删除粉丝信息时删除出错2！", exp);
                        return new ReturnResult<Fans>(-1, "系统异常，请稍后重试。");
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
                        logger.LogError("删除粉丝信息时更新出错2！", exp);
                        return new ReturnResult<Fans>(-1, "系统异常，请稍后重试。");
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
            var existFans = _FansRepository.Find().Where(p => 
                (p.userid1 == userid1 && p.userid2 == userid2) || 
                (p.userid1 == userid2 && p.userid2 == userid1)).FirstOrDefault();
            if(existFans == null || string.IsNullOrEmpty(existFans.id))
            {
                return new ReturnResult<Fans>(-2, "两位用户均没有关注过对方！");
            }
            return new ReturnResult<Fans>(1, existFans);
        }
        /// <summary>
        /// 获取用户的粉丝信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<UserFansModel>> GetFans(string userid,
            int pageIndex = 1, int pageSize = 10)
        {
            //var res = _FansRepository.Find()
            //    .Where(p =>
            //    (p.userid1 == userid && (p.fansstate == 0 || p.fansstate == 1)) ||
            //    (p.userid2 == userid && (p.fansstate == 0 || p.fansstate == 2)))
            //    .OrderByDescending(p => p.updatetime).ToPage(pageIndex,pageSize);

            var res = (from fans in _FansRepository.Find()
                        join user1 in _UsersRepository.Find() on fans.userid1 equals user1.id
                        join user in _UsersRepository.Find() on fans.userid2 equals user.id
                        where fans.userid1 == userid && (fans.fansstate == 0 || fans.fansstate == 1)
                        select new UserFansModel{
                            Id = user.id,
                            Nickname =user.nickname,
                            Avatar = user.avatar,
                            AnswerCount = user.answercount,
                            FansCount = user.fanscount,
                            FollowCount = user.followcount,
                            AdoptedCount = user.adoptedcount,
                            CompositeScores = user.compositescores,
                            IsCertification = user.iscertification,
                            Signatures = user.signatures,
                            Usertype = user.usertype,
                            FansUpdateTime = fans.updatetime,
                        })
                        .Concat
                        (from fans in _FansRepository.Find()
                         join user1 in _UsersRepository.Find() on fans.userid1 equals user1.id
                         join user in _UsersRepository.Find() on fans.userid2 equals user.id
                         where fans.userid2 == userid && (fans.fansstate == 0 || fans.fansstate == 2)
                         select new UserFansModel
                         {
                             Id = user.id,
                             Nickname = user.nickname,
                             Avatar = user.avatar,
                             AnswerCount = user.answercount,
                             FansCount = user.fanscount,
                             FollowCount = user.followcount,
                             AdoptedCount = user.adoptedcount,
                             CompositeScores = user.compositescores,
                             IsCertification = user.iscertification,
                             Signatures = user.signatures,
                             Usertype = user.usertype,
                             FansUpdateTime = fans.updatetime,
                         }).OrderByDescending(p => new { p.FansUpdateTime }).ToPage(pageIndex, pageSize);

            return new ReturnResult<IPagedData<UserFansModel>>(1, res);
        }
        /// <summary>
        /// 获取用户的关注信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns></returns>
        public ReturnResult<IPagedData<UserFansModel>> GetFollows(string userid,
            int pageIndex = 1, int pageSize = 10)
        {
            //var res = _FansRepository.Find()
            //    .Where(p =>
            //    (p.userid1 == userid && (p.fansstate == 0 || p.fansstate == 2)) ||
            //    (p.userid2 == userid && (p.fansstate == 0 || p.fansstate == 1)))
            //    .OrderByDescending(p => p.updatetime).ToPage(pageIndex, pageSize);

            var res = (from fans in _FansRepository.Find()
                       join user in _UsersRepository.Find() on fans.userid1 equals user.id
                       where fans.userid1 == userid && (fans.fansstate == 0 || fans.fansstate == 2)
                       select new UserFansModel
                       {
                           Id = user.id,
                           Nickname = user.nickname,
                           Avatar = user.avatar,
                           AnswerCount = user.answercount,
                           FansCount = user.fanscount,
                           FollowCount = user.followcount,
                           AdoptedCount = user.adoptedcount,
                           CompositeScores = user.compositescores,
                           IsCertification = user.iscertification,
                           Signatures = user.signatures,
                           Usertype = user.usertype,
                           FansUpdateTime = fans.updatetime,
                       })
                        .Concat
                        (from fans in _FansRepository.Find()
                         join user in _UsersRepository.Find() on fans.userid1 equals user.id
                         where fans.userid2 == userid && (fans.fansstate == 0 || fans.fansstate == 1)
                         select new UserFansModel
                         {
                             Id = user.id,
                             Nickname = user.nickname,
                             Avatar = user.avatar,
                             AnswerCount = user.answercount,
                             FansCount = user.fanscount,
                             FollowCount = user.followcount,
                             AdoptedCount = user.adoptedcount,
                             CompositeScores = user.compositescores,
                             IsCertification = user.iscertification,
                             Signatures = user.signatures,
                             Usertype = user.usertype,
                             FansUpdateTime = fans.updatetime,
                         }).OrderByDescending(p => new { p.FansUpdateTime }).ToPage(pageIndex, pageSize);

            return new ReturnResult<IPagedData<UserFansModel>>(1, res);
        }
        #endregion
    }
}
