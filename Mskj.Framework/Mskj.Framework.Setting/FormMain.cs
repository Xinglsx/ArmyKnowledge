using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mskj.LiteFramework.Common.Extenstions;
using Mskj.LiteFramework.Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mskj.Framework.Setting
{
    public partial class FormMain : Form
    {
        #region 构造函数
        public const string SQL_SERVER_FORMAT = "data source={0},{1};initial catalog={2};user id={3};password={4};MultipleActiveResultSets=True";
        private readonly string FileName = "dbs.json";
        public FormMain()
        {
            InitializeComponent();
            InitData();
        }
        #endregion

        #region 系统事件

        private void BtnTest_Click(object sender, EventArgs e)
        {
            string connectStr = SetConnectString();
            if(!string.IsNullOrEmpty(connectStr))
            {
                TestSqlServer(connectStr);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string connectStr = SetConnectString();
            if (!string.IsNullOrEmpty(connectStr))
            {
                string jsonData = "[{"
                    + "\"Name\":\"ArmyKnowledge\","
                    + "\"ConnectionString\":\"" + CryptoApi.AesEncrypt(connectStr) + "\","//需要加密保存
                    + "\"ProviderName\":\"System.Data.SqlClient\""
                    + "}]";
                try
                {
                    File.WriteAllText(GetDirectory() + FileName, jsonData);
                }
                catch (Exception exp)
                {
                    MessageBox.Show(this, exp.Message, "失败提示", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                MessageBox.Show(this, "数据库配置保存成功！", "成功提示", MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            string configDir = GetDirectory();
            if (!Directory.Exists(configDir))
            {
                Directory.CreateDirectory(configDir);
            }
            string baseFilePath = configDir + FileName;
            if (!File.Exists(baseFilePath))
            {
                var temp = File.Create(baseFilePath);
                temp.Close();
                //并写入空值
                string jsonData = string.Empty;
                //[{"Name":"ArmyKnowledge",
                //"ConnectionString":"5fGMkdCNq5PYpZ7Hw/EFFBTDw/sfTwmQeQNfOUvzZS2Qr4+OGoxgT1/si/k9vHXwwpTrfhQj6MDGnDIhxRIoiQdm1L1vM3DbvjgaFciMe2RgcIpsZMeBcK0mDH9Qlk290/UKZWLHO1rJc/B9oSEnsutiBpKl8QF0kXOpqMv/ca8=",
                //"ProviderName":"System.Data.SqlClient"}]
                string connectStr = string.Empty;
                connectStr += string.Format(SQL_SERVER_FORMAT,"","","","","");
                jsonData = "[{"
                    + "\"Name\":\"ArmyKnowledge\","
                    + "\"ConnectionString\":\""+ CryptoApi.AesEncrypt(connectStr) + "\","//需要加密保存
                    + "\"ProviderName\":\"System.Data.SqlClient\""
                    + "}]";
                File.WriteAllText(baseFilePath, jsonData);
            }
            else
            {
                string appInfo = File.ReadAllText(baseFilePath);
                appInfo = appInfo.Substring(1, appInfo.Length - 2);
                JObject jo = (JObject)JsonConvert.DeserializeObject(appInfo);
                string connectionStr = CryptoApi.AesDecrypt(jo["ConnectionString"].ToString());//需要解密
                DatabaseConect db = AnalysisConnectString(connectionStr);
                if(db == null)
                {
                    MessageBox.Show(this, "数据库连接字符串解析失败!");
                }
                else
                {
                    txtDbName.Text = db.Name;
                    txtDbServer.Text = db.Server;
                    txtUserId.Text = db.UserId;
                    txtPassword.Text = db.Password;
                }
            }
        }
        /// <summary>
        /// 获取配置文件目录
        /// </summary>
        /// <returns></returns>
        private string GetDirectory()
        {
            string dir = Application.StartupPath;
            while (dir.EndsWith("\\"))
            {
                dir = dir.Substring(0, dir.Length - 1);
            }
            while(dir.EndsWith("bin"))
            {
                dir = dir.Substring(0, dir.Length - 3);
            }
            dir += "\\config\\";
            return dir;
        }
        /// <summary>
        /// 设置数据库字符串
        /// </summary>
        /// <returns></returns>
        private string SetConnectString()
        {
            string connectStr = string.Empty;

            if (string.IsNullOrEmpty(txtDbServer.Text))
            {
                MessageBox.Show(this, "请输入数据库服务器地址！", "提示", MessageBoxButtons.OK);
                return null;
            }
            if (string.IsNullOrEmpty(txtUserId.Text))
            {
                MessageBox.Show(this, "请输入登录名！", "提示", MessageBoxButtons.OK);
                return null;
            }
            if (string.IsNullOrEmpty(txtDbName.Text))
            {
                MessageBox.Show(this, "请输入数据库名！", "提示", MessageBoxButtons.OK);
                return null;
            }
            connectStr = string.Format(SQL_SERVER_FORMAT,txtDbServer.Text,"1433",txtDbName.Text,
                txtUserId.Text,txtPassword.Text);
            return connectStr;
        }
        
        private DatabaseConect AnalysisConnectString(string connstr)
        {
            DatabaseConect db = new DatabaseConect();
            db.Server = GetSubString(connstr, "data source=");
            db.Port = GetSubString(connstr, ",") ?? "1433";
            db.Name = GetSubString(connstr, "initial catalog=");
            db.UserId = GetSubString(connstr, "user id=");
            db.Password = GetSubString(connstr, "password=");
            return db;
        }
        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="connString"></param>
        private void TestSqlServer(string connString)
        {
            using (var con = new SqlConnection(connString))
            {
                try
                {
                    con.Open();
                    con.Close();
                    MessageBox.Show(this, "连接成功", "提示");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "连接失败：\r\n" + ex.Message, "提示");
                }
            }
        }

        private static string GetSubString(string source, string prefix, string subfix = ",;)")
        {
            var index = source.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
                return null;

            var sub = source.Substring(index + prefix.Length);

            if (subfix.Any())
            {
                var min = sub.IndexOfAny(subfix.ToCharArray());
                if (min <= 0)
                {
                    return null;
                }

                var val = sub.Substring(0, min);
                if (val.IsNullOrEmptyOrWhiteSpace())
                    return null;
                return val;
            }

            return sub;
        }
        #endregion

    }
}
