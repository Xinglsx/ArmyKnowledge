using Mskj.LiteFramework.Base;
using System;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Fans : IModel, IEntity
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 用户ID1
        /// </summary>
        public string userid1 { get; set; }
        /// <summary>
        /// 用户ID2
        /// </summary>
        public string userid2 { get; set; }
        /// <summary>
        /// 关注状态
        /// 0-互粉
        /// 1-用户2关注用户1
        /// 2-用户1关注用户2
        /// </summary>
        public int fansstate { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? updatetime{get;set;}
    }
}