using Mskj.ArmyKnowledge.All.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class AnswerMap : AllEntityMap<Answer>
    {
        public AnswerMap()
        {
            this.ToTable("answer");
            this.HasKey(p => p.id);
            this.Property(p => p.id).HasMaxLength(36);
            this.Property(p => p.questionid).HasMaxLength(36);
            this.Property(p => p.userid).HasMaxLength(36);
            this.Property(p => p.nickname).HasMaxLength(32);
            this.Property(p => p.content).HasMaxLength(8000);
            this.Property(p => p.images).HasMaxLength(4000);
        }
    }
}
