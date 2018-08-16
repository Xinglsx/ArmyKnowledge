using QuickShare.LiteFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Users : IModel, IEntity
    {
        public string id { get; set; }
        public string loginname { get; set; }
        public string password { get; set; }
        public string nickname { get; set; }
        public string profession { get; set; }
        public string organization { get; set; }
        public string creditcode { get; set; }
        public string phonenumber { get; set; }
        public string signature { get; set; }
        public string usertype { get; set; }
        public int iscertification { get; set; }
        public bool isadmin { get; set; }
        public int answercount { get; set; }
        public int adoptedcount { get; set; }
        public int compositescores { get; set; }
        public int followcount { get; set; }
        public int fanscount { get; set; }
        public int state { get; set; }
    }
}
