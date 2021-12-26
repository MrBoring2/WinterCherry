namespace WinterCherry.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("IceCream")]
    public partial class IceCream : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public IceCream()
        {
            ProductsInOrder = new HashSet<ProductsInOrder>();
            ProductsInDeliveryToStorage = new HashSet<ProductsInDeliveryToStorage>();
            ProductsInDeliveryToShop = new HashSet<ProductsInDeliveryToShop>();
            ProductsInStorage = new HashSet<ProductsInStorage>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int TypeId { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public double Weight { get; set; }

        public int Amount { get; set; }

        public string PriceDisplay => $"{Math.Round(Price, 2)} ₽";
        public string WeightDisplay => Weight < 1 ? $"{Weight * 1000} г." : $"{Weight} кг.";
        public virtual IceCreamType IceCreamType { get; set; }
        public string AmountInStorages
        {
            get
            {
                using (var db = new WinterCherryContext())
                {
                    var storages = db.Storage.ToList();
                    string amount = "";

                    foreach (var storage in storages)
                    {
                        var icecream = ProductsInStorage.FirstOrDefault(p => p.StorageId == storage.Id);
                        string count = icecream == null || icecream.Amount == 0 ? "Нет в наличии" : icecream.Amount.ToString() + " шт.";
                        amount += $"{storage.Name}: {count}\n";
                    }
                    return amount;
                }
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsInOrder> ProductsInOrder { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsInDeliveryToStorage> ProductsInDeliveryToStorage { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsInDeliveryToShop> ProductsInDeliveryToShop { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductsInStorage> ProductsInStorage { get; set; }
    }
}
