using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.IO;

namespace TuDienHook
{
    public class DictionaryManager
    {
        private string filePath = "data.xml";
        #region properties
        private DictionaryItem items;
        public DictionaryItem Items
        {
            get { return items; }
            set { items = value; }
        }
        #endregion
        #region methods
        public DictionaryManager()
        {
            Items = (DictionaryItem)DeserializeFromXML(filePath);
        }
        public List<DictionaryData> GetDataItems()
        {
            return Items.Item;
        }
        public void LoadDataToCombobox(ComboBox combo)
        {
            combo.DataSource = null;
            combo.DataSource = Items.Item; //Items.Item
            combo.DisplayMember = "Key";
        }
        public List<DictionaryData> ReLoadDataXml()
        {
            List<DictionaryData> data = Items.Item;
            return data;
        }
        public void Serialize()
        {
            SerializeToXML(Items, filePath);
        }
        public void SerializeToXML(object data, string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                XmlSerializer sr = new XmlSerializer(typeof(DictionaryItem));
                sr.Serialize(fs, data);
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public object DeserializeFromXML(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                XmlSerializer sr = new XmlSerializer(typeof(DictionaryItem));
                object obj = sr.Deserialize(fs);
                //return sr.Deserialize(fs);
                fs.Close();
                return obj;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public void AddItemToData(string key, string meaning, string explanation)
        {
            DictionaryData data = new DictionaryData();
            data.Key = key;
            data.Meaning = meaning;
            data.Explanation = explanation;
            Items.Item.Add(data);
        }
        #endregion
    }
}
