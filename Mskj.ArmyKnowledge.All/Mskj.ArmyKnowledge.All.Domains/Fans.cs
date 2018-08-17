using QuickShare.LiteFramework.Base;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Fans : IModel, IEntity
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string fansuserid { get; set; }
        public int fansstate { get; set; }
    }
}