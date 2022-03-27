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
    public partial class PersonForm : Form
    {
        FuneralHomeDBContainer db;
        Person person;
        int personTypeId;

        public PersonForm(FuneralHomeDBContainer _db, int _personId = 0, int _personTypeId = 0)
        {
            db = _db;

            personTypeId = _personTypeId;

            person = db.Person.FirstOrDefault(x => x.Id == _personId);

            if (person == null)
                person = new Person();

            InitializeComponent();

            if (_personId == 0 && _personTypeId == 0)
            {
                label9.Visible = true;
                comboBox1.Visible = true;

                comboBox1.Items.AddRange(db.Dictionary.Where(x => x.Type == (int)DictionaryType.PersonType).ToList().Select(x => x.Name).ToArray());
            }
        }

        private void PersonForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = person.Name;
            textBox2.Text = person.Surname;
            textBox6.Text = person.Phone;
            textBox5.Text = person.Email;

            textBox3.Text = person.Street;
            textBox4.Text = person.HouseNo;
            textBox7.Text = person.Place;
            textBox8.Text = person.PostCode;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.Validate(this, new Control[] { comboBox1, textBox1, textBox2, textBox1, textBox3, textBox4, textBox7, textBox8 }))
                return;

            person.Name = textBox1.Text;
            person.Surname = textBox2.Text;
            person.Phone = textBox6.Text;
            person.Email = textBox5.Text;

            person.Street = textBox3.Text;
            person.HouseNo = textBox4.Text;
            person.Place = textBox7.Text;
            person.PostCode = textBox8.Text;



            if (person.Id == 0)
            {
                if (personTypeId > 0)
                {
                    person.Type = personTypeId;
                }
                else
                {
                    person.Type = db.Dictionary.FirstOrDefault(x => x.Type == (int)DictionaryType.PersonType && x.Name == comboBox1.Text).Id;
                }

                db.Person.Add(person);
            }

            db.SaveChanges();

            this.Close();
        }
    }
}
