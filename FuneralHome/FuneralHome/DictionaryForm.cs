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
    public partial class DictionaryForm : Form
    {
        FuneralHomeDBContainer db;
        private int dictType;

        public DictionaryForm(FuneralHomeDBContainer _db, int _dictType)
        {
            db = _db;
            dictType = _dictType;

            InitializeComponent();

            LoadData();
        }

        private void LoadData(bool reload = false)
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.DataSource = db.Dictionary.Where(x => x.Type == dictType).Select(x => new { x.Id, Nazwa = x.Name }).ToList();

            if (!reload)
            {
                string dictName = db.Dictionary_Type.FirstOrDefault(x => x.Id == dictType).Name;

                label1.Text += " " + dictName;
            }
                //dataGridView1.Columns[2].Visible = false;

                dataGridView1.Columns[0].HeaderText = "Id";
                dataGridView1.Columns[0].ReadOnly = true;

                dataGridView1.Columns[1].HeaderText = "Nazwa";
                dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
                deleteButton.Name = "dataGridViewDeleteButton";
                deleteButton.HeaderText = "";
                deleteButton.Text = "Usuń";
                deleteButton.UseColumnTextForButtonValue = true;
                deleteButton.Width = 60;
                dataGridView1.Columns.Add(deleteButton);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            db.SaveChanges();

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary_NewItem newItem = new Dictionary_NewItem();

            string itemName;

            var result = newItem.ShowDialog();
            if(result == DialogResult.OK)
            {
                itemName = newItem.ItemName;

                db.Dictionary.Add(new Dictionary() { Type = dictType, Name = itemName });
                db.SaveChanges();

                dataGridView1.DataSource = null;

                LoadData(true);
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView1.NewRowIndex || e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dataGridView1.Columns["dataGridViewDeleteButton"].Index)
            {
                int dictId;
                if (int.TryParse(dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["Id"].Index].Value.ToString(), out dictId))
                {
                    var dr = MessageBox.Show("Czy na pewno usunąć: " + dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["Nazwa"].Index].Value.ToString() + " ?", "", MessageBoxButtons.YesNo);

                    if (dr == DialogResult.Yes)
                    {
                        var dict = db.Dictionary.FirstOrDefault(x => x.Id == dictId);

                        if (dict != null)
                            db.Dictionary.Remove(dict);

                        db.SaveChanges();

                        dataGridView1.DataSource = null;

                        LoadData(true);
                    }
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == dataGridView1.NewRowIndex || e.RowIndex < 0)
                return;

            if (e.ColumnIndex == dataGridView1.Columns["Nazwa"].Index)
            {
                int dictId;
                if (int.TryParse(dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["Id"].Index].Value.ToString(), out dictId))
                {
                    var dict = db.Dictionary.FirstOrDefault(x => x.Id == dictId);

                    Dictionary_NewItem newItem = new Dictionary_NewItem();
                    newItem.ItemName = dict.Name;

                    var result = newItem.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        dict.Name = newItem.ItemName;

                        db.SaveChanges();

                        dataGridView1.DataSource = null;

                        LoadData(true);
                    }
                }
            }
        }
    }
}
