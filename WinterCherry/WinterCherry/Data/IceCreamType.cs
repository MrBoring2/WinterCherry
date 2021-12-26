namespace WinterCherry.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("IceCreamType")]
    public partial class IceCreamType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IceCreamType()
        {
            IceCream = new HashSet<IceCream>();
        }

        public int IceCreamTypeId { get; set; }

        [StringLength(50)]
        public string IceCreamTypeName { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IceCream> IceCream { get; set; }
    }
}
