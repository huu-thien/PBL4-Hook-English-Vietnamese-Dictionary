using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Speech.Synthesis;
using Gma.System.MouseKeyHook;
using System.Web.Script.Serialization;
using System.Collections;

namespace TuDienHook
{
    public partial class Form1 : Form
    {
        DictionaryManager dictionary;
        private IKeyboardMouseEvents globalMouseHook;
        public Form1()
        {
            InitializeComponent();
            comboBoxWord.DisplayMember = "Key";
            dictionary = new DictionaryManager();
            dictionary.LoadDataToCombobox(comboBoxWord);
            globalMouseHook = Hook.GlobalEvents();

            comboBoxLanguage1.SelectedIndex = 0;
            comboBoxLanguage2.SelectedIndex = 0;
        }

        private async void MouseDoubleClicked(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // Save clipboard's current content to restore it later.
            IDataObject tmpClipboard = Clipboard.GetDataObject();
            Clipboard.Clear();
            // I think a small delay will be more safe.
            // You could remove it, but be careful.
            await Task.Delay(50);
            // Send Ctrl+C, which is "copy"
            System.Windows.Forms.SendKeys.SendWait("^c");
            // Same as above. But this is more important.
            // In some softwares like Word, the mouse double click will not select the word you clicked immediately.
            // If you remove it, you will not get the text you selected.
            await Task.Delay(50);
            if (Clipboard.ContainsText())
            {
                string text = Clipboard.GetText();
                bool check = false;
                // Your code
                txtLanguageKey.Text = text;
                foreach (DictionaryData i in dictionary.Items.Item)
                {
                    if (txtLanguageKey.Text.ToUpper().Trim() == i.Key.ToUpper().Trim())
                    {
                        txtLanguageKey.Text = text.Trim();
                        comboBoxWord.SelectedItem = i;
                        check = true;
                    }
                }
                if (check == false)
                {
                    txtMeaning.Text = TranslateText(txtLanguageKey.Text.Trim());
                    txtExplanation.Text = "";
                }
                txtLanguageKey.Text = text.Trim();
            }
        }

        string lang_first = "auto";
        string lang_second = "vi";
        public string TranslateText(string input)
        {
            string url = String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             lang_first, lang_second, Uri.EscapeUriString(input));
            HttpClient httpClient = new HttpClient();
            string result = httpClient.GetStringAsync(url).Result;
            var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);
            var translationItems = jsonData[0];
            string translation = "";
            foreach (object item in translationItems)
            {
                IEnumerable translationLineObject = item as IEnumerable;
                IEnumerator translationLineString = translationLineObject.GetEnumerator();
                translationLineString.MoveNext();
                translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }
            if (translation.Length > 1) { translation = translation.Substring(1); };
            return translation;
        }

        private void comboBoxLanguage1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLanguage1.Text == "Tiếng Anh") lang_first = "en";
            if (comboBoxLanguage1.Text == "Tiếng Việt") lang_first = "vi";
            if (comboBoxLanguage1.Text == "Phát hiện ngôn ngữ") lang_first = "auto";
        }

        private void comboBoxLanguage2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLanguage2.Text == "Tiếng Anh") lang_second = "en";
            if (comboBoxLanguage2.Text == "Tiếng Việt") lang_second = "vi";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("You are to close, Are you sure you want to continue?", "Exit and close?", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
                return;
            }
            dictionary.Serialize();
        }

        private void comboBoxWord_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb.DataSource == null)
            {
                return;
            }
            DictionaryData data = cb.SelectedItem as DictionaryData;
            txtMeaning.Text = data.Meaning;
            txtExplanation.Text = data.Explanation;
            txtLanguageKey.Text = comboBoxWord.SelectedText;
        }

        private void buttonSaveAddData_Click(object sender, EventArgs e)
        {
            string key = txtLanguageKey.Text.Trim();
            string meaning = txtMeaning.Text.Trim();
            string explanation = txtExplanation.Text.Trim();
            if (key != null)
            {
                dictionary.AddItemToData(key, meaning, explanation);
                dictionary.LoadDataToCombobox(comboBoxWord);
                MessageBox.Show("Add Data Successfully!!!");
            }
            else MessageBox.Show("Add Data Unsuccessfully!!!");
        }

        private void buttonSaveData_Click(object sender, EventArgs e)
        {
            dictionary.Serialize();
            MessageBox.Show("Save Data Successfully!!!");
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            // Bind MouseDoubleClick event with a function named MouseDoubleClicked.
            globalMouseHook.MouseDoubleClick -= MouseDoubleClicked;
            // Bind DragFinished event with a function.
            globalMouseHook.MouseDragFinished -= MouseDoubleClicked;
            MessageBox.Show("Stop Hooking Successfully!!!");
        }

        private void bottonHooking_Click(object sender, EventArgs e)
        {
            // Bind MouseDoubleClick event with a function named MouseDoubleClicked.
            globalMouseHook.MouseDoubleClick += MouseDoubleClicked;
            // Bind DragFinished event with a function.
            globalMouseHook.MouseDragFinished += MouseDoubleClicked;  
            MessageBox.Show("Start Hooking Successfully!!!");
        }

        private void speakEL_Click(object sender, EventArgs e)
        {
            var synth = new SpeechSynthesizer();
            synth.SelectVoice("Microsoft David Desktop");
            synth.SpeakAsync(txtLanguageKey.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var synth = new System.Speech.Synthesis.SpeechSynthesizer();
            foreach (var voice in synth.GetInstalledVoices())
            {
                txtExplanation.Text += voice.VoiceInfo.Name;
            }
        }
        private void txtLanguageKey_KeyDown(object sender, KeyEventArgs e)
        {
            bool check = false;
            string text = txtLanguageKey.Text;
            if (e.KeyCode == Keys.Enter)
            {
                foreach (DictionaryData i in dictionary.Items.Item)
                {
                    if (txtLanguageKey.Text.ToUpper().Trim() == i.Key.ToUpper().Trim())
                    {
                        txtLanguageKey.Text = text.Trim();
                        comboBoxWord.SelectedItem = i;
                        check = true;
                    }
                }
                if (check == false)
                {
                    txtLanguageKey.Text = TranslateText(txtLanguageKey.Text.Trim());
                }
                txtLanguageKey.Text = text.Trim();
            }
        }

        private void SpeakVN_Click(object sender, EventArgs e)
        {
            var synth = new SpeechSynthesizer();
            synth.SelectVoice("Microsoft An");
            synth.Speak(txtMeaning.Text);
        }

        private void txtLanguageKey1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtMeaning_TextChanged(object sender, EventArgs e)
        {

        }
        private void speakExp_Click(object sender, EventArgs e)
        {
            var synth = new SpeechSynthesizer();
            synth.SelectVoice("Microsoft An");
            synth.Speak(txtExplanation.Text);
        }
    }
}
