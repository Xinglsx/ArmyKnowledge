using Mskj.ArmyKnowledge.All.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class RecordMap : AllEntityMap<Record>
    {
        public RecordMap()
        {
            this.ToTable("record");
            this.HasKey(p => p.id);
            this.Property(p => p.id).HasMaxLength(36);
            this.Property(p => p.questionid).HasMaxLength(36);
        }
    }
}
