using Mskj.ArmyKnowledge.All.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class CertMap : AllEntityMap<Cert>
    {
        public CertMap()
        {
            this.ToTable("cert");
            this.HasKey(p => p.id);
            this.Property(p => p.id).HasMaxLength(36);
            this.Property(p => p.userid).HasMaxLength(36);
            this.Property(p => p.realname).HasMaxLength(32);
            this.Property(p => p.idcardno).HasMaxLength(18);
            this.Property(p => p.idcardfrontimage).HasMaxLength(512);
            this.Property(p => p.idcardbackimage).HasMaxLength(512);
            this.Property(p => p.organization).HasMaxLength(64);
            this.Property(p => p.creditcode).HasMaxLength(32);
            this.Property(p => p.creditimage).HasMaxLength(512);
            this.Property(p => p.creditexpirydate).HasMaxLength(10);
            this.Property(p => p.othercredite1).HasMaxLength(512);
            this.Property(p => p.otherexpirydate1).HasMaxLength(10);
            this.Property(p => p.othercredite2).HasMaxLength(512);
            this.Property(p => p.otherexpirydate2).HasMaxLength(10);
            this.Property(p => p.othercredite3).HasMaxLength(512);
            this.Property(p => p.otherexpirydate3).HasMaxLength(10);
        }
    }
}
