using QuickShare.LiteFramework.Base;
using System;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Product : IModel, IEntity
    {
        public string id { get; set; }
        public string proname { get; set; }
        public string price { get; set; }
        public string introduction { get; set; }
        public string protype { get; set; }
        public DateTime publishtime { get; set; }
        public decimal compositescore { get; set; }
        public string materialcode { get; set; }
        public string productiondate { get; set; }
        public string prodetail { get; set; }
        public string category { get; set; }
        public string contacts { get; set; }
        public string contactphone { get; set; }
        public int prostate { get; set; }
    }
}