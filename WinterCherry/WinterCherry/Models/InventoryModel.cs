using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterCherry.Data;

namespace WinterCherry.Models
{
    public class InventoryModel : BaseEntity
    {
        private IceCream iceCream;
        private int factAmount;
        private int buhAmount;
        private decimal price;

        public InventoryModel(IceCream _iceCream, int _buhAmount, decimal _price)
        {
            IceCream = _iceCream;
            BuhAmount = _buhAmount;
            Price = _price;
            OnPropertyChanged(nameof(Deviation));
        }

        public IceCream IceCream { get => iceCream; private set { iceCream = value; OnPropertyChanged();  } }
        public int FactAmount { get => factAmount; set { factAmount = value; OnPropertyChanged(); OnPropertyChanged(nameof(FactTotalPrice)); OnPropertyChanged(nameof(Deviation)); } }
        public int BuhAmount { get => buhAmount; private set { buhAmount = value; OnPropertyChanged(); } }
        public decimal Price { get => price; private set { price = value; OnPropertyChanged(); } }
        public decimal FactTotalPrice => FactAmount == 0 ? 0 : Price * FactAmount;
        public decimal BuhTotalPrice => BuhAmount == 0 ? 0 : Price * BuhAmount;
        public decimal Deviation => FactAmount == 0 ? 0 : FactAmount - BuhAmount;
    }
}
