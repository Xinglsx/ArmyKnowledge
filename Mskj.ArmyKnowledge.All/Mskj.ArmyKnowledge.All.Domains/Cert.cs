using Mskj.LiteFramework.Base;
using System;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Cert : IModel, IEntity
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string realname { get; set; }
        public string idcardno { get; set; }
        public string idcardfrontimage { get; set; }
        public string idcardbackimage { get; set; }
        public string organization { get; set; }
        public string creditcode { get; set; }
        public string creditimage { get; set; }
        public string creditexpirydate { get; set; }
        public string othercredite1 { get; set; }
        public string otherexpirydate1 { get; set; }
        public string othercredite2 { get; set; }
        public string otherexpirydate2 { get; set; }
        public string othercredite3 { get; set; }
        public string otherexpirydate3 { get; set; }
        public int certstate { get; set; }
        public int usertype { get; set; }
        public DateTime? updatetime { get; set; }
    }
}