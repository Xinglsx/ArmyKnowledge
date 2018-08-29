using QuickShare.LiteFramework.Base;
using System;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Notice : IModel, IEntity
    {
        public string id { get; set; }
        public string title { get; set; }
        public int noticetype { get; set; }
        public DateTime? publishtime { get; set; }
        public string content { get; set; }
        public int noticestate { get; set; }
        public DateTime? updatetime { get; set; }
    }
}