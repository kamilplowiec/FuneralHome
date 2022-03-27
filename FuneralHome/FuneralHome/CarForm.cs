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
    public partial class CarForm : Form
    {
        FuneralHomeDBContainer db;
        Car car;

        public CarForm(FuneralHomeDBContainer _db, int _carId = 0)
        {
            db = _db;

            car = db.Car.FirstOrDefault(x => x.Id == _carId);

            if (car == null)
                car = new Car();

            InitializeComponent();
        }

        private void CarForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = car.Mark;

            textBox2.Text = car.Model;

            textBox3.Text = car.RegNo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidationHelper.Validate(this, new Control[] { textBox1, textBox2, textBox3 }))
                return;

            car.Mark = textBox1.Text;

            car.Model = textBox2.Text;

            car.RegNo = textBox3.Text;

            if (car.Id == 0)
                db.Car.Add(car);

            db.SaveChanges();

            this.Close();
        }
    }
}
