using QuickShare.LiteFramework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Question : IModel, IEntity
    {
        public string id { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string authornickname { get; set; }
        public DateTime? publishtime { get; set; }
        public string introduction { get; set; }
        public string content { get; set; }
        public string images { get; set; }
        public bool isrecommend { get; set; }
        public string homeimage { get; set; }
        public int readcount { get; set; }
        public int praisecount { get; set; }
        public int commentcount { get; set; }
        public int heatcount { get; set; }
        public int questionstate { get; set; }
        public DateTime? updatetime { get; set; }
    }
}
