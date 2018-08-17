using Mskj.ArmyKnowledge.All.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class NoticeMap : AllEntityMap<Notice>
    {
        public NoticeMap()
        {
            this.ToTable("notice");
            this.HasKey(p => p.id);
            this.Property(p => p.id).HasMaxLength(36);
            this.Property(p => p.title).HasMaxLength(256);
        }
    }
}
