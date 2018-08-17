using Mskj.ArmyKnowledge.All.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class FansMap : AllEntityMap<Fans>
    {
        public FansMap()
        {
            this.ToTable("fans");
            this.HasKey(p => p.id);
            this.Property(p => p.id).HasMaxLength(36);
            this.Property(p => p.userid).HasMaxLength(36);
            this.Property(p => p.fansuserid).HasMaxLength(36);
        }
    }
}
