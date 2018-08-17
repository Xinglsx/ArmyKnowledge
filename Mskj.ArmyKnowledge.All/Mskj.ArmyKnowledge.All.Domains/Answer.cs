using QuickShare.LiteFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Answer : IModel, IEntity
    {
        public string id { get; set; }
        public string articleid { get; set; }
        public string userid { get; set; }
        public string nickname { get; set; }
        public DateTime publishtime { get; set; }
        public string content { get; set; }
        public string images { get; set; }
        public bool isaccept { get; set; }
        public int praisecount { get; set; }
    }
}
