using Mskj.ArmyKnowledge.All.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class MsgDetailMap : AllEntityMap<MsgDetail>
    {
        public MsgDetailMap()
        {
            this.ToTable("msgdetail");
            this.HasKey(p => p.id);
            this.Property(p => p.id).HasMaxLength(36);
            this.Property(p => p.msgid).HasMaxLength(36);
            this.Property(p => p.senduserid).HasMaxLength(36);
            this.Property(p => p.sendnickname).HasMaxLength(32);
            this.Property(p => p.content).HasMaxLength(512);
        }
    }
}
