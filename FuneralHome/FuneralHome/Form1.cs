using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FuneralHome
{
    public partial class Form1 : Form
    {
        private FuneralHomeDBContainer _connection;
        private FuneralHomeDBContainer db
        {
            get
            {
                if (!_connection.Database.Exists())
                {
                    var dr = MessageBox.Show("Wystąpił problem z połączeniem z bazą danych", "", MessageBoxButtons.RetryCancel);

                    if(dr == DialogResult.Retry)
                    {
                        return db;
                    }
                    else
                    {
                        this.Close();
                    }
                }

                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        public Form1()
        {
            InitializeComponent();

            db = new FuneralHomeDBContainer();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            InitializeGridViews();
        }

        private void InitializeGridViews()
        {
            RefreshFuneralForDay(monthCalendar1.SelectionRange.Start);
            RefreshActivityGV();
            RefreshEmployeeGV();
            RefreshCarsGV();
            RefreshCemeteryGV();
            RefreshDiedGV();
        }

        private void RefreshFuneralForDay(DateTime? date = null)
        {
            dataGridView1.Rows.Clear();
            db.Activity.Where(x => x.Type == (int)DictionaryType.ActivityType && (!date.HasValue || (x.DateStart.Day == date.Value.Day && x.DateStart.Month == date.Value.Month && x.DateStart.Year == date.Value.Year))).ToList().ForEach(x => dataGridView1.Rows.Add(x.Id, x.DateStart.ToShortTimeString(), db.Cemetery.Select(c => new { c.Id, CemeteryName = c.Street + ", " + c.Place }).FirstOrDefault(c => c.Id == db.Funeral.FirstOrDefault(f => f.Activity_Id == x.Id).Cemetery_Id).CemeteryName));
        }

        private void RefreshActivityGV()
        {
            dataGridView2.Rows.Clear();
            db.Activity.ToList().ForEach(x => dataGridView2.Rows.Add(
                                            x.Id,
                                            db.Dictionary.FirstOrDefault(d => d.Id == x.Type).Name,
                                            x.DateStart.ToShortDateString(),
                                            x.DateStart.ToShortTimeString() + (x.Time.HasValue ? " - " + x.DateStart.Add(x.Time.Value).ToShortTimeString() : ""),
                                            x.Type == 1 ? 
                                                            db.Person.Select(p => new { p.Id, PersonName = p.Name + " " + p.Surname }).FirstOrDefault(p => p.Id == db.Funeral.FirstOrDefault(f => f.Activity_Id == x.Id).Person_Id).PersonName 
                                                        : 
                                                            db.Car.Select(c => new { c.Id, CarName = c.Mark + " " + c.Model + " - " + c.RegNo }).FirstOrDefault(c => c.Id == x.Car_Id).CarName,
                                            x.Type == 1 ? db.Cemetery.Select(c => new { c.Id, CemeteryName = c.Street + ", " + c.Place }).FirstOrDefault(c => c.Id == db.Funeral.FirstOrDefault(f => f.Activity_Id == x.Id).Cemetery_Id).CemeteryName : x.Place,
                                            x.Description
                                        ));
        }

        private void RefreshEmployeeGV()
        {
            dataGridView3.Rows.Clear();
            db.Person.Where(x => x.Type == (int)PersonType.Driver || x.Type == (int)PersonType.Worker).ToList().ForEach(x => dataGridView3.Rows.Add(x.Id, db.Dictionary.FirstOrDefault(d => d.Id == x.Type).Name, x.Name + " " + x.Surname));
        }

        private void RefreshCarsGV()
        {
            dataGridView4.Rows.Clear();
            db.Car.ToList().ForEach(x => dataGridView4.Rows.Add(x.Id, x.Mark + " " + x.Model + " - " + x.RegNo));
        }

        private void RefreshCemeteryGV()
        {
            dataGridView5.Rows.Clear();

            foreach (var cemetery in db.Cemetery.ToList())
            {
                var priest = db.Person.FirstOrDefault(p => p.Id == cemetery.Person_Id);

                dataGridView5.Rows.Add(cemetery.Id, priest.Name + " " + priest.Surname, cemetery.Street + ", " + cemetery.PostCode + " " + cemetery.Place);
            }
        }

        private void RefreshDiedGV()
        {
            dataGridView6.Rows.Clear();
            db.Person.Where(x => x.Type == (int)PersonType.Died).ToList().ForEach(x => dataGridView6.Rows.Add(x.Id, x.Name, x.Surname, x.DieDate.HasValue ? x.DieDate.Value.ToShortDateString() : "", db.Cemetery.Select(c => new { c.Id, CemeteryName = c.Street + ", " + c.Place }).FirstOrDefault(c => c.Id == db.Funeral.FirstOrDefault(f => f.Person_Id == x.Id).Cemetery_Id).CemeteryName, ""));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ActivityForm af = new ActivityForm(db);
            af.ShowDialog(this);

            RefreshActivityGV();
            RefreshDiedGV();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            RefreshFuneralForDay(monthCalendar1.SelectionRange.Start);
        }

        private void typOsóbToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DictionaryForm df = new DictionaryForm(db, (int)DictionaryType.PersonType);
            df.ShowDialog(this);
        }

        private void typDziałaniaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DictionaryForm df = new DictionaryForm(db, (int)DictionaryType.ActivityType);
            df.ShowDialog(this);

            RefreshActivityGV();
        }

        private void zamknijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dodajPojazdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CarForm cf = new CarForm(db);
            cf.ShowDialog(this);

            RefreshCarsGV();
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;

            int carId;
            if (int.TryParse(gv.Rows[e.RowIndex].Cells[0].Value.ToString(), out carId))
            {
                CarForm cf = new CarForm(db, carId);
                cf.ShowDialog(this);

                RefreshCarsGV();
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;

            int personId;
            if (int.TryParse(gv.Rows[e.RowIndex].Cells[0].Value.ToString(), out personId))
            {
                PersonForm cf = new PersonForm(db, personId);
                cf.ShowDialog(this);

                RefreshEmployeeGV();
            }
        }

        private void dodajOsobeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PersonForm cf = new PersonForm(db);
            cf.ShowDialog(this);
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;

            int activityId;
            if (int.TryParse(gv.Rows[e.RowIndex].Cells[0].Value.ToString(), out activityId))
            {
                ActivityForm af = new ActivityForm(db, activityId);
                af.ShowDialog(this);

                RefreshActivityGV();
            }
        }

        private void dodajCmentarzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CemeteryForm cf = new CemeteryForm(db);
            cf.ShowDialog(this);

            RefreshCemeteryGV();
        }

        private void dataGridView5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;

            int cemetryId;
            if (int.TryParse(gv.Rows[e.RowIndex].Cells[0].Value.ToString(), out cemetryId))
            {
                CemeteryForm cf = new CemeteryForm(db, cemetryId);
                cf.ShowDialog(this);

                RefreshCemeteryGV();
            }
        }

        private void dataGridView6_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView gv = (DataGridView)sender;

            int personId;
            if (int.TryParse(gv.Rows[e.RowIndex].Cells[0].Value.ToString(), out personId))
            {
                DeathInfoForm dif = new DeathInfoForm(db, personId);
                dif.ShowDialog(this);
            }
        }
    }
}
