using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuDienHook
{
    public class DictionaryData
    {
        private string key;
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        private string meaning;
        public string Meaning
        {
            get { return meaning; }
            set { meaning = value; }
        }

        private string explanation;
        public string Explanation
        {
            get { return explanation; }
            set { explanation = value; }
        }
    }
}
