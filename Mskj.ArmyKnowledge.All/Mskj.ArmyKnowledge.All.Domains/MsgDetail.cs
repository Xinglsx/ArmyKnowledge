using QuickShare.LiteFramework.Base;
using System;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class MsgDetail : IModel, IEntity
    {
        public string id { get; set; }
        public string msgid { get; set; }
        public string senduserid { get; set; }
        public string sendnickname { get; set; }
        public DateTime sendtime { get; set; }
        public string content { get; set; }
        public int contenttype { get; set; }
    }
}