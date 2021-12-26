namespace WinterCherry.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeliveryToStorage")]
    public partial class DeliveryToStorage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryToStorage()
        {
            ProductsInDeliveryToStorage = new HashSet<ProductsInDeliveryToStorage>();
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int? SupplierId { get; set; }

        public int? StorageId { get; set; }

        public virtual Storage Storage { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsInDeliveryToStorage> ProductsInDeliveryToStorage { get; set; }
    }
}
