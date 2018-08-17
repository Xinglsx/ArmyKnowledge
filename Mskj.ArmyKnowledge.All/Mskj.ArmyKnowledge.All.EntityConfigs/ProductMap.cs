using Mskj.ArmyKnowledge.All.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mskj.ArmyKnowledge.All.EntityConfigs
{
    public class ProductMap : AllEntityMap<Product>
    {
        public ProductMap()
        {
            this.ToTable("product");
            this.HasKey(p => p.id);
            this.Property(p => p.id).HasMaxLength(36);
            this.Property(p => p.proname).HasMaxLength(64);
            this.Property(p => p.price).HasMaxLength(32);
            this.Property(p => p.introduction).HasMaxLength(2000);
            this.Property(p => p.protype).HasMaxLength(32);
            this.Property(p => p.materialcode).HasMaxLength(32);
            this.Property(p => p.productiondate).HasMaxLength(10);
            this.Property(p => p.category).HasMaxLength(64);
            this.Property(p => p.contacts).HasMaxLength(32);
            this.Property(p => p.contactphone).HasMaxLength(11);
            this.Property(p => p.prodetail).HasMaxLength(2000);
        }
    }
}
