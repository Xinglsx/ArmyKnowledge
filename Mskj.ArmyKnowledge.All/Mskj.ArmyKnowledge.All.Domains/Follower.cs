using QuickShare.LiteFramework.Base;

namespace Mskj.ArmyKnowledge.All.Domains
{
    public class Follower : IModel, IEntity
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string followeruserid { get; set; }
        public int followerstate { get; set; }
    }
}