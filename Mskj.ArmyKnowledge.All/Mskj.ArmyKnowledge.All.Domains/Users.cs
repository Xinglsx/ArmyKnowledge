using QuickShare.LiteFramework.Base;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Users : IModel, IEntity
    {
        public string id { get; set; }
        public string loginname { get; set; }
        public string pwd { get; set; }
        public string nickname { get; set; }
        public string profession { get; set; }
        public string organization { get; set; }
        public string creditcode { get; set; }
        public string phonenumber { get; set; }
        public string avatar { get; set; }
        public string signatures { get; set; }
        public int usertype { get; set; }
        public int iscertification { get; set; }
        public bool isadmin { get; set; }
        public int answercount { get; set; }
        public int adoptedcount { get; set; }
        public int compositescores { get; set; }
        public int followcount { get; set; }
        public int fanscount { get; set; }
        public int userstate { get; set; }
    }
}
