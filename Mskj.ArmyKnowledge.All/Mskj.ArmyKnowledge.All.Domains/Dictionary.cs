using QuickShare.LiteFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Dictionary : IModel, IEntity
    {
        public string id { get; set; }
        public int dictype { get; set; }
        public string diccode { get; set; }
        public string dicname { get; set; }
        public bool dicstate { get; set; }
    }
}
