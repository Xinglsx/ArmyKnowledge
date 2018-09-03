using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.Framework.Setting
{
    public class DatabaseConect
    {
        /// <summary> 数据库名 </summary>
        public string Name { get; set; }
        /// <summary> 服务器名 </summary>
        public string Server { get; set; }
        /// <summary> 端口号 </summary>
        public string Port { get; set; }
        /// <summary> 用户名 </summary>
        public string UserId { get; set; }
        /// <summary> 密码 </summary>
        public string Password { get; set; }
    }
}
