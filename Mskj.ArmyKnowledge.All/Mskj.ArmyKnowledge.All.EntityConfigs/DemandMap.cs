using Mskj.ArmyKnowledge.All.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class DemandMap : AllEntityMap<Demand>
    {
        public DemandMap()
        {
            this.ToTable("demand");
            this.HasKey(p => p.id);
            this.Property(p => p.id).HasMaxLength(36);
            this.Property(p => p.title).HasMaxLength(128);
            this.Property(p => p.author).HasMaxLength(36);
            this.Property(p => p.authornickname).HasMaxLength(32);
            this.Property(p => p.introduction).HasMaxLength(512);
            this.Property(p => p.images).HasMaxLength(4000);
            this.Property(p => p.homeimage).HasMaxLength(512);
            this.Property(p => p.category).HasMaxLength(16);
        }
    }
}
