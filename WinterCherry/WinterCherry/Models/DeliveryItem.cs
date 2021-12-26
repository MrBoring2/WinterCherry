using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterCherry.Data;

namespace WinterCherry.Models
{
    public class DeliveryItem
    {
        public DeliveryItem(IceCream iceCream, int amount)
        {
            IceCream = iceCream;
            Amount = amount;
        }

        public IceCream IceCream { get; private set; }
        public int Amount { get; private set; }
    }
}
