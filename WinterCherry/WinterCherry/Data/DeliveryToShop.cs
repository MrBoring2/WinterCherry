namespace WinterCherry.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DeliveryToShop")]
    public partial class DeliveryToShop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryToShop()
        {
            ProductsInDeliveryToShop = new HashSet<ProductsInDeliveryToShop>();
        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int StorageId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsInDeliveryToShop> ProductsInDeliveryToShop { get; set; }
    }
}
