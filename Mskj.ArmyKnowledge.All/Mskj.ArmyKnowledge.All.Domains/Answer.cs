﻿using Mskj.LiteFramework.Base;
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
        public string questionid { get; set; }
        public string userid { get; set; }
        public string nickname { get; set; }
        public DateTime? publishtime { get; set; }
        public string content { get; set; }
        public string images { get; set; }
        public bool isadopt { get; set; }
        public int praisecount { get; set; }
        public DateTime? updatetime { get; set; }
    }
}
