using QuickShare.LiteFramework.Base;
using System;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Msg : IModel, IEntity
    {
        public string id { get; set; }
        public string userid1 { get; set; }
        public string nickname1 { get; set; }
        public string userid2 { get; set; }
        public string nickname2 { get; set; }
        public string lastcontent { get; set; }
        public DateTime updatetime { get; set; }
    }
}