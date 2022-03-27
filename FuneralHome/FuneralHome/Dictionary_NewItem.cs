using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuneralHome
{
    public partial class Dictionary_NewItem : Form
    {
        public string ItemName { get; set; }

        public Dictionary_NewItem()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ItemName = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Dictionary_NewItem_Shown(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ItemName))
            {
                textBox1.Text = ItemName;
            }
        }
    }
}
