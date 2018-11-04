using Mskj.LiteFramework.Base;
using System;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Product : IModel, IEntity
    {
        public string id { get; set; }
        public string proname { get; set; }
        public decimal price { get; set; }
        public string introduction { get; set; }
        public string userid { get; set; }
        public bool isrecommend { get; set; }
        public string nickname { get; set; }
        public DateTime? publishtime { get; set; }
        public decimal compositescore { get; set; }
        public string materialcode { get; set; }
        public string productiondate { get; set; }
        public string prodetail { get; set; }
        public string category { get; set; }
        public string contacts { get; set; }
        public string contactphone { get; set; }
        public string images { get; set; }
        public string homeimage { get; set; }
        public int prostate { get; set; }
        public int proscores { get; set; }
        public int readcount { get; set; }
        public int buycount { get; set; }
        public DateTime? updatetime { get; set; }

        public string procategory { get;set;}
        public string contacttelephone { get;set;}
        public string appsituation { get;set;}
        public string appadvancement { get;set;}
        public string appachievement { get;set;}
        public string exhibitsdisplay { get;set;}
        public string exhibitssize { get;set;}
        public string exhibitsweight { get;set;}
        public string requirement { get;set; }
        public string providefree { get;set; }
        public string area { get;set; }
        public string email { get; set; }
        public string industrycategories { get; set; }
    }
}