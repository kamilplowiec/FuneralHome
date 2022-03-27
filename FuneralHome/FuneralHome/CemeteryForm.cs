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
    public partial class CemeteryForm : Form
    {
        FuneralHomeDBContainer db;
        Cemetery cemetery;

        public CemeteryForm(FuneralHomeDBContainer _db, int _cemeteryId = 0)
        {
            db = _db;

            cemetery = db.Cemetery.FirstOrDefault(x => x.Id == _cemeteryId);

            if (cemetery == null)
                cemetery = new Cemetery();

            InitializeComponent();

            comboBox1.Items.AddRange(db.Person.Where(x => x.Type == (int)PersonType.Priest).Select(x => x.Name + " " + x.Surname).ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.Validate(this, new Control[] { comboBox1, textBox3, textBox8, textBox7 }))
                return;

            cemetery.Person_Id = db.Person.FirstOrDefault(x => x.Name + " " + x.Surname == comboBox1.SelectedItem.ToString()).Id;
            cemetery.Street = textBox3.Text;
            cemetery.PostCode = textBox8.Text;
            cemetery.Place = textBox7.Text;

            if(cemetery.Id == 0)
            {
                db.Cemetery.Add(cemetery);
            }

            db.SaveChanges();

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PersonForm cf = new PersonForm(db, 0, db.Dictionary.FirstOrDefault(x => x.Name == "Ksiądz").Id);
            cf.ShowDialog(this);
        }

        private void CemeteryForm_Load(object sender, EventArgs e)
        {
            if (cemetery.Id > 0)
            {
                textBox3.Text = cemetery.Street;
                textBox8.Text = cemetery.PostCode;
                textBox7.Text = cemetery.Place;

                comboBox1.SelectedItem = db.Person.Select(x => new { x.Id, PriestName = x.Name + " " + x.Surname }).FirstOrDefault(x => x.Id == cemetery.Person_Id).PriestName;
            }
        }
    }
}
