
using Mskj.ArmyKnowledge.All.Domains;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class UsersMap : AllEntityMap<Users>
    {
        public UsersMap()
        {
            this.ToTable("Users");
            this.HasKey(p => p.id);
            this.Property(p => p.loginname).HasMaxLength(20);
            this.Property(p => p.password).HasMaxLength(64);
            this.Property(p => p.nickname).HasMaxLength(32);
            this.Property(p => p.profession).HasMaxLength(64);
            this.Property(p => p.organization).HasMaxLength(64);
            this.Property(p => p.creditcode).HasMaxLength(32);
            this.Property(p => p.phonenumber).HasMaxLength(11);
            this.Property(p => p.signature).HasMaxLength(256);
            this.Property(p => p.usertype).HasMaxLength(128);
        }
    }
}
