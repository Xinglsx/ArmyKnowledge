using QuickShare.LiteFramework.Common.Extenstions;
using QuickShare.LiteFramework.WebApi.Authorization;

namespace Mskj.ArmyKnowledge.Common
{
    /// <summary> 用户信息 </summary>
    public class UserInfo : User
    {
        /// <summary> 机构代码 </summary>
        public string AgentCode { get; set; }
        /// <summary> 机构名称 </summary>
        public string AgentName { get; set; }
        /// <summary> 科室代码 </summary>
        public string RoomCode { get; set; }
        /// <summary> 科室名称 </summary>
        public string RoomName { get; set; }
        /// <summary> 是否超级管理员 </summary>
        public bool IsSuperAdmin { get; set; }
        /// <summary> 是否机构管理员 </summary>
        public bool IsAgentAdmin { get; set; }
        public bool Valid()
        {
            return !Name.IsNullOrEmptyOrWhiteSpace()
                   && !Code.IsNullOrEmptyOrWhiteSpace();
        }
    }
}