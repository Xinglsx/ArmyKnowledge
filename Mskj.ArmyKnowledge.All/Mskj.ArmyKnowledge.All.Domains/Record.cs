using QuickShare.LiteFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Record : IModel, IEntity
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string questionid { get; set; }
        public DateTime? lasttime { get; set; }
        public bool iscollect { get; set; }
    }
}
