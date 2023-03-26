using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuDienHook
{
    public class DictionaryItem
    {
        private List<DictionaryData> item;

        public List<DictionaryData> Item
        {
            get { return item; }
            set { item = value; }
        }
    }
}
