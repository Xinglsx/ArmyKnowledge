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
            this.Property(p => p.userid).HasMaxLength(36);
            this.Property(p => p.nickname).HasMaxLength(36);
            this.Property(p => p.proname).HasMaxLength(64);
            this.Property(p => p.introduction).HasMaxLength(2000);
            this.Property(p => p.images).HasMaxLength(4000);
            this.Property(p => p.homeimage).HasMaxLength(512);
            this.Property(p => p.materialcode).HasMaxLength(32);
            this.Property(p => p.productiondate).HasMaxLength(10);
            this.Property(p => p.category).HasMaxLength(64);
            this.Property(p => p.contacts).HasMaxLength(32);
            this.Property(p => p.contactphone).HasMaxLength(11);

            this.Property(p => p.procategory).HasMaxLength(16);
            this.Property(p => p.contacttelephone).HasMaxLength(16);
            this.Property(p => p.appsituation).HasMaxLength(64);
            this.Property(p => p.appadvancement).HasMaxLength(64);
            this.Property(p => p.appachievement).HasMaxLength(512);
            this.Property(p => p.exhibitsdisplay).HasMaxLength(128);
            this.Property(p => p.exhibitssize).HasMaxLength(128);
            this.Property(p => p.exhibitsweight).HasMaxLength(128);
            this.Property(p => p.requirement).HasMaxLength(512);
            this.Property(p => p.providefree).HasMaxLength(128);
            this.Property(p => p.area).HasMaxLength(128);
        }
    }
}
