using Mskj.ArmyKnowledge.All.ServiceContracts;
using QuickShare.LiteFramework.Base;
using Mskj.ArmyKnowledge.All.Domains;
using Mskj.ArmyKnowledge.Common.DataObject;
using System;
using System.Collections.Generic;
using System.Linq;
using QuickShare.LiteFramework.Common;
using System.Linq.Expressions;
using System.Data.SqlClient;
using QuickShare.LiteFramework.Common.Extenstions;
using System.Configuration;
using Mskj.ArmyKnowledge.All.ServiceContracts.DataObj;
using System.IO;

namespace Mskj.ArmyKnowledge.All.Services
{
    public class SystemService : ISystemService
    {
        #region 构造函数
        public SystemService() 
        {
        }
        #endregion

        #region 版本信息
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
                    + @"/Download/junzi.apk";

                return new ReturnResult<VersionInfo>(1, version);
            }
            catch (Exception ex)
            {
                return new ReturnResult<VersionInfo>(1, ex.Message);
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
    }
}
