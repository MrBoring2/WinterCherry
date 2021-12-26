using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinterCherry.Models
{
    public class SortItem
    {
        public SortItem(string title, string property)
        {
            Title = title;
            Property = property;
        }

        public string Title { get; private set; }
        public string Property { get; private set; }
    }
}
