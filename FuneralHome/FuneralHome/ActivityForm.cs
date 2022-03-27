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
    public partial class ActivityForm : Form
    {
        FuneralHomeDBContainer db;
        Activity activity;

        public ActivityForm(FuneralHomeDBContainer _db, int _activityId = 0)
        {
            db = _db;

            activity = db.Activity.FirstOrDefault(x => x.Id == _activityId);

            if (activity == null)
                activity = new Activity();

            InitializeComponent();
        }

        private void ActivityForm_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(db.Dictionary.Where(x => x.Type == (int)DictionaryType.ActivityType).Select(x => x.Name).ToArray());

            comboBox2.Items.AddRange(db.Car.Select(x => x.Mark + " " + x.Model + " - " + x.RegNo).ToArray());

            comboBox3.Items.AddRange(db.Person.Where(x => x.Type == (int)PersonType.Driver).Select(x => x.Name + " " + x.Surname).ToArray());

            comboBox4.Items.AddRange(db.Cemetery.Select(x => x.Street + ", " + x.Place).ToArray());


            if (activity.Id > 0) //Edycja
            {
                comboBox1.SelectedItem = db.Dictionary.FirstOrDefault(x => x.Id == activity.Type).Name;

                comboBox2.SelectedItem = db.Car.Select(x => new { x.Id, Car = x.Mark + " " + x.Model + " - " + x.RegNo }).FirstOrDefault(x => x.Id == activity.Car_Id).Car;

                comboBox3.SelectedItem = db.Person.Select(x => new { x.Id, Driver = x.Name + " " + x.Surname }).FirstOrDefault(x => x.Id == activity.Driver_Id).Driver;

                dateTimePicker1.Value = activity.DateStart;

                if (activity.Time.HasValue)
                    numericUpDown1.Value = activity.Time.Value.Hours;

                if (comboBox1.SelectedItem.ToString() == "Pogrzeb")
                {
                    var person = db.Person.FirstOrDefault(p => p.Id == db.Funeral.FirstOrDefault(x => x.Activity_Id == activity.Id).Person_Id);

                    textBox1.Text = person.Name;

                    textBox2.Text = person.Surname;

                    if (person.BirthDate.HasValue)
                        dateTimePicker2.Value = person.BirthDate.Value;

                    if (person.DieDate.HasValue)
                        dateTimePicker3.Value = person.DieDate.Value;

                    comboBox4.SelectedItem = db.Cemetery.Select(x => new { x.Id, CemeteryName = x.Street + ", " + x.Place }).FirstOrDefault(x => x.Id == db.Funeral.FirstOrDefault(f => f.Activity_Id == activity.Id).Cemetery_Id).CemeteryName;
                }

                textBox3.Text = activity.Description;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Pogrzeb")
            {
                if (!ValidationHelper.Validate(this, new Control[] { comboBox1, comboBox2, comboBox3, textBox1, textBox2, comboBox4 }))
                    return;
            }
            else
            {
                if (!ValidationHelper.Validate(this, new Control[] { comboBox1, comboBox2, comboBox3 }))
                    return;
            }

            activity.Type = db.Dictionary.FirstOrDefault(x => x.Type == (int)DictionaryType.ActivityType && x.Name == comboBox1.Text).Id;

            activity.Driver_Id = db.Person.FirstOrDefault(x => x.Name + " " + x.Surname == comboBox3.Text).Id;

            activity.Car_Id = db.Car.FirstOrDefault(x => x.Mark + " " + x.Model + " - " + x.RegNo == comboBox2.Text).Id;

            var dateStart = dateTimePicker1.Value;

            activity.DateStart = dateStart;

            TimeSpan ts = new TimeSpan(int.Parse(numericUpDown1.Value.ToString()), 0, 0);

            activity.Time = ts;

            activity.Description = textBox3.Text;

            if (activity.Id == 0) //nowe dzialanie
            {
                activity = db.Activity.Add(activity);
                db.SaveChanges();

                if (comboBox1.SelectedItem.ToString() == "Pogrzeb")
                {
                    groupBox2.Enabled = true;

                    var person = new Person();

                    person.Name = textBox1.Text;
                    person.Surname = textBox2.Text;

                    person.Type = 3;

                    person.BirthDate = dateTimePicker2.Value;
                    person.DieDate = dateTimePicker3.Value;

                    person = db.Person.Add(person);
                    db.SaveChanges();


                    var funeral = new Funeral();

                    funeral.Activity_Id = activity.Id;
                    funeral.Person_Id = person.Id;

                    funeral.Cemetery_Id = db.Cemetery.FirstOrDefault(x => x.Street + ", " + x.Place == comboBox4.SelectedItem.ToString()).Id;

                    db.Funeral.Add(funeral);
                    db.SaveChanges();
                }
            }
            else
                db.SaveChanges();

            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;

            var selected = cb.SelectedItem.ToString();

            if (selected == "Pogrzeb")
            {
                groupBox2.Enabled = true;
            }
            else
            {
                groupBox2.Enabled = false;
            }
        }
    }
}
