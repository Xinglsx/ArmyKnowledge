
using Mskj.ArmyKnowledge.All.Domains;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class UsersMap : AllEntityMap<Users>
    {
        public UsersMap()
        {
            this.ToTable("Users");
        }
    }
}
