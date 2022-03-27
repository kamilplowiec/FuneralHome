using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuneralHome
{
    public partial class DeathInfoForm : Form
    {
        FuneralHomeDBContainer db;

        public DeathInfoForm(FuneralHomeDBContainer _db, int _personId)
        {
            db = _db;

            InitializeComponent();

            Person person = db.Person.FirstOrDefault(x => x.Id == _personId);
            Funeral funeral = db.Funeral.FirstOrDefault(x => x.Person_Id == _personId);
            Cemetery cemetery = db.Cemetery.FirstOrDefault(x => x.Id == funeral.Cemetery_Id);
            Activity activity = db.Activity.FirstOrDefault(x => x.Id == funeral.Activity_Id);

            label1.Text = person.Name + " " + person.Surname;
            label2.Text = person.BirthDate.Value.ToShortDateString();
            label4.Text = person.DieDate.Value.ToShortDateString();
            label5.Text = activity.DateStart.ToShortDateString();
            label8.Text = cemetery.Street + ", " + cemetery.PostCode + " " + cemetery.Place;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(panel1.Width, panel1.Height);
            panel1.DrawToBitmap(bmp, panel1.ClientRectangle);

            SaveFileDialog saveDialog = new SaveFileDialog();

            if (saveDialog.ShowDialog() == DialogResult.OK)
            { 
                using (FileStream fs = new FileStream(saveDialog.FileName, FileMode.OpenOrCreate))
                {
                    bmp.Save(fs, ImageFormat.Jpeg);
                }
            }
        }
    }
}
