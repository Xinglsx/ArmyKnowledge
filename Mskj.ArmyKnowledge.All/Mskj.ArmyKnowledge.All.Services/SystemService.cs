﻿using Mskj.ArmyKnowledge.All.ServiceContracts;
using Mskj.ArmyKnowledge.Common.DataObject;
using System;
using System.Configuration;
using System.IO;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Aliyun.Acs.Core.Exceptions;
using Mskj.LiteFramework.Foundation;
using Mskj.LiteFramework;
using Mskj.ArmyKnowledge.All.Common.DataObj;
using System.Web;
using System.Collections.Generic;
using Mskj.LiteFramework.Base;
using Mskj.ArmyKnowledge.All.Domains;
using System.Linq;
using Mskj.LiteFramework.Common;
using Mskj.LiteFramework.Common.Extenstions;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class SystemService : ISystemService
    {
        #region 构造函数
        private readonly IRepository<Dictionary> _DicRepository;
        private readonly ILogger logger;
        public SystemService(IRepository<Dictionary> dicRepository)
        {
            _DicRepository = dicRepository;

            logger = AppInstance.Current.Resolve<ILogger>();
        }
        #endregion

        #region 版本信息
        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        public ReturnResult<VersionInfo> GetVersionInfo()
        {
            VersionInfo version = new VersionInfo();
            version.VersionNumber = 0;
            try
            {
                //获取txt文件中的版本号和更新内容
                string[] lines = File.ReadAllLines(GetVersionFilePath());
                bool isVersionNumber = false;
                bool isVersion = false;
                bool isUpdateContent = false;
                foreach (string line in lines)
                {
                    if (isVersionNumber)
                    {
                        try
                        {
                            version.VersionNumber = Convert.ToInt32(line);
                        }
                        catch { }//转错不处理

                        isVersionNumber = false;
                    }
                    else if (isVersion)
                    {
                        version.Version = line;
                        isVersion = false;
                    }
                    else if (isUpdateContent)
                    {
                        version.UpdateContent += line + "\r\n";
                    }

                    if (line != string.Empty && line.Contains("VersionNumber"))
                    {
                        isVersionNumber = true;
                    }
                    if (line != string.Empty && line.Contains("Version"))
                    {
                        isVersion = true;
                    }
                    if (line != string.Empty && line.Contains("UpdateContent"))
                    {
                        isUpdateContent = true;
                    }
                }
                if (string.IsNullOrEmpty(version.Version))
                {
                    version.Version = "1.0.0";
                }
                if (string.IsNullOrEmpty(version.UpdateContent))
                {
                    version.UpdateContent = "优化了主要流程！";
                }
                version.DownloadAddress = ConfigurationManager.AppSettings["Localhost"]
                    + @"/Download/junzhi.apk";

                return new ReturnResult<VersionInfo>(1, version);
            }
            catch (Exception ex)
            {
                return new ReturnResult<VersionInfo>(-1, ex.Message);
            }
        }

        private string GetVersionFilePath()
        {
            try
            {
                string versionFilePath = ConfigurationManager.AppSettings["AppVersionFilePath"];
                return versionFilePath;
            }
            catch (Exception ex)
            {
                return @"C:\AppVersion.txt";
            }
        }
        #endregion

        #region 获取字典信息
        /// <summary>
        /// 获取字典值
        /// </summary>
        /// <param name="dictype">字典类型</param>
        public List<string> GetDictionarys(int dictype)
        {
            var res = _DicRepository.Find().Where(x => x.dictype == dictype)
                .OrderBy(q => q.diccode).Select(p => p.dicname).ToList();
            return res;
        }
        /// <summary>
        /// 增加字典
        /// </summary>
        public ReturnResult<Dictionary> AddDictionary(Dictionary dic)
        {
            dic.id = Guid.NewGuid().ToString();
            bool res = false;
            try
            {
                res = _DicRepository.Add(dic);
            }
            catch (Exception exp)
            {
                logger.LogError("AddDictionary增加字典", exp);
                return new ReturnResult<Dictionary>(-1, "系统异常，请稍后重试。");
            }
            if (res)
            {
                return new ReturnResult<Dictionary>(1, dic);
            }
            else
            {
                return new ReturnResult<Dictionary>(-1, "字典数据插入失败！");
            }
        }
        /// <summary>
        /// 更新字典
        /// </summary>
        public ReturnResult<bool> UpdateDictionary(Dictionary dic)
        {
            bool res = false;
            try
            {
                res = _DicRepository.Update(dic);
            }
            catch (Exception exp)
            {
                logger.LogError("UpdateDictionary更新字典", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            return new ReturnResult<bool>(res ? 1 : -2, res);
        }
        /// <summary>
        /// 删除字典
        /// </summary>
        public ReturnResult<bool> DeleteDictionary(string dicId)
        {
            bool res = false;
            try
            {
                res = _DicRepository.Delete(dicId);
            }
            catch (Exception exp)
            {
                logger.LogError("DeleteDictionary删除字典", exp);
                return new ReturnResult<bool>(-1, "系统异常，请稍后重试。");
            }
            return new ReturnResult<bool>(res ? 1 : -2, res);

        }
        /// <summary>
        /// 获取字典
        /// </summary>
        public ReturnResult<IPagedData<Dictionary>> GetDictionarys(int dictype = -1,
            int pageIndex = 1,int pageSize= 30)
        {
            var resTemp = _DicRepository.Find();
            if(dictype != -1)
            {
                resTemp = resTemp.Where(p => p.dictype == dictype);
            }
            var res = resTemp.OrderBy(p => p.dictype).ThenBy(q => q.diccode).ToPage(pageIndex,pageSize);
            return new ReturnResult<IPagedData<Dictionary>>(1,res);
        }
        #endregion

        #region 图片上传
        /// <summary>
        /// 图片上传
        /// </summary>
        public ReturnResult<string> UploadFile()
        {
            HttpContext context = HttpContext.Current;
            string imagePath = ConfigurationManager.AppSettings["ImagePath"];
            string localhost = ConfigurationManager.AppSettings["ImageLocalhost"];
            string fileUrls = string.Empty;
            if (!imagePath.EndsWith("\\"))
            {
                imagePath += "\\";
            }
            if (!localhost.EndsWith("/"))
            {
                localhost += "/";
            }
            var dir = context.Request.Params["dir"];
            if(!string.IsNullOrEmpty(dir))
            {
                imagePath += dir+"\\";
                localhost += dir +"/";
            }
            else
            {
                imagePath += "default\\";
                localhost += "default/";
            }
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
            if (context.Request.Files.Count > 0)
            {
                try
                {
                    for (int i = 0; i < context.Request.Files.Count; i++)
                    {
                        string fileTempName = Guid.NewGuid().ToString() + ".jpg";
                        string fileName = imagePath + fileTempName;
                        context.Request.Files[i].SaveAs(fileName);
                        if (i != 0)
                        {
                            fileUrls += ",";
                        }
                        fileUrls += localhost + fileTempName;
                    }
                }
                catch (Exception exp)
                {
                    logger.LogError("UploadFile图片上传", exp);
                    return new ReturnResult<string>(-1,"", "系统异常，请稍后重试。");
                }

            }
            return new ReturnResult<string>(1, fileUrls, "上传成功");
        }
        #endregion

        #region 阿里云发送消息
        /// <summary>
        /// 发送信息到指定的手机号码
        /// </summary>
        /// <param name="phoneNumber">指定的手机号码</param>
        /// <returns></returns>
        public ReturnResult<bool> SendMobileMessage(string phoneNumber)
        {
            if(string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 11)
            {
                return new ReturnResult<bool>(-2, "手机号不正确！");
            }
            ReturnResult<bool> result = new ReturnResult<bool>();
            String product = "Dysmsapi";//短信API产品名称
            String domain = "dysmsapi.aliyuncs.com";//短信API产品域名
            String accessKeyId = ConfigurationManager.AppSettings["AliyunAccessKeyId"];//"LTAIWm8fNYkE3W5b";//你的accessKeyId
            String accessKeySecret = ConfigurationManager.AppSettings["AliyunAccessKeySecret"];// "WG5MsiwrKqnrIlTJtlWeJM3GolcAnU";//你的accessKeySecret

            //accessKeyId = "LTAIoae9O4UxOJIG";
            //accessKeySecret = "qtFsyoEWYpobLbSoC8TzWiyjvp6vhK";
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();

            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = phoneNumber;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = ConfigurationManager.AppSettings["AliyunSignName"];// "闪荐";
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = ConfigurationManager.AppSettings["AliyunTemplateCode"];// "SMS_110400008";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                //随机生成6位数字的验证码。
                string code = string.Empty;
                Random random = new Random();
                while (code.Length < 6)
                {
                    code += random.Next(10).ToString();
                }
                request.TemplateParam = "{\"code\":\"" + code + "\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                request.OutId = "21212121211";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);
                //Test
                //sendSmsResponse.Code = "OK";
                if ("OK".Equals(sendSmsResponse.Code))
                {
                    //发送成功之后，将发送成功的code和手机号保存在缓存中
                    ICache cache = AppInstance.Current.Resolve<ICache>();
                    cache.SetObject("mobilecode-" + phoneNumber, code, new TimeSpan(0, 15, 0));
                    result.code = 1;
                    result.data = true;
                }
                else
                {
                    result.code = -3;
                    result.message = sendSmsResponse.Message;
                }
            }
            catch (ServerException e)
            {
                //LogUtil.WebError(e);
                result.code = -1;
                result.message = e.Message;
            }
            catch (ClientException e)
            {
                //LogUtil.WebError(e);
                result.code = -1;
                result.message = e.Message;
            }
            return result;
        }
        /// <summary>
        /// 验证手机号及获取到的验证码
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public ReturnResult<bool> ValidationCode(string phoneNumber,string verificationCode)
        {
            ICache cache = AppInstance.Current.Resolve<ICache>();
            var tempCode = cache.GetObject("mobilecode-" + phoneNumber);
            if (tempCode == null || !tempCode.ToString().Equals(verificationCode))
            {
                return new ReturnResult<bool>(-2, "验证码错误！");
            }
            else
            {
                return new ReturnResult<bool>(1, true, "验证通过");
            }
        }
        #endregion
    }
}
