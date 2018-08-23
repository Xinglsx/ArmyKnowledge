using Mskj.ArmyKnowledge.All.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class DictionaryMap : AllEntityMap<Dictionary>
    {
        public DictionaryMap()
        {
            this.ToTable("dictionary");
            this.HasKey(p => p.id);
            this.Property(p => p.id).HasMaxLength(36);
            this.Property(p => p.diccode).HasMaxLength(2);
            this.Property(p => p.dicname).HasMaxLength(36);
        }
    }
}
