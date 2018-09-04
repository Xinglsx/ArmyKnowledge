using Mskj.LiteFramework.Base;
using Mskj.LiteFramework.Data;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public abstract class AllEntityMap<T>: EntityMap<T> where T:class, IEntity
    {
        protected AllEntityMap() : base("ArmyKnowledge")
        {

        }
    }

    class ArmyKnowledgeName : IDbName
    {
        public string Name { get { return "ArmyKnowledge"; } }
        public string Description { get { return "军融汇"; } }
    }
}
