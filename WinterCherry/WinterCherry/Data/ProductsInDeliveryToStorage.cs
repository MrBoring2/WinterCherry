namespace WinterCherry.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductsInDeliveryToStorage")]
    public partial class ProductsInDeliveryToStorage
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DeliveryToStorageId { get; set; }

        public int Amount { get; set; }

        public virtual DeliveryToStorage DeliveryToStorage { get; set; }

        public virtual IceCream IceCream { get; set; }
    }
}
