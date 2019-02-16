using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_Project
{
    public partial class CommandInsertRecord : Form
    {
        private DatabaseManager dB;

        public CommandInsertRecord(DatabaseManager dB)
        {
            this.dB = dB;
            InitializeComponent();
        }

        public string TB1
        {
            get { return insertTB1.Text; }
            set { insertTB1.Text = value; }
        }

        public string TB2
        {
            get { return insertTB2.Text; }
            set { insertTB2.Text = value; }
        }

        public string TB3
        {
            get { return insertTB3.Text; }
            set { insertTB3.Text = value; }
        }

        public string TB4
        {
            get { return insertTB4.Text; }
            set { insertTB4.Text = value; }
        }

        public string TB5
        {
            get { return insertTB5.Text; }
            set { insertTB5.Text = value; }
        }

        private void SchoolRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (schoolRadio.Checked)
            {
                ClearTB();
                insertLB1.Text = "School Name";
                insertLB2.Text = "School Address";
                insertLB3.Text = "School Phone";
                insertLB4.Text = "Course ID*";
                insertLB5.Visible = false;
                insertTB5.Visible = false;
            }
        }

        private void CourseRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (CourseRadio.Checked)
            {
                ClearTB();
                insertLB1.Text = "Course ID";
                insertLB2.Text = "Course Name";
                insertLB3.Text = "Course Code";
                insertLB4.Text = "Course Fee";
                insertLB5.Visible = true;
                insertTB5.Visible = true;
                insertLB5.Text = "Course Location";
            }
        }

        private void ClearBN_Click(object sender, EventArgs e) => ClearTB();

        public void ClearTB()
        {
            insertTB1.Text = "";
            insertTB2.Text = "";
            insertTB3.Text = "";
            insertTB4.Text = "";
            insertTB5.Text = "";
        }

        private void CancelBN_Click(object sender, EventArgs e) => Close();

        private void SaveBN_Click(object sender, EventArgs e)
        {
            if (schoolRadio.Checked)
            {
                dB.InsertRecordSchool(this);
            }
            else
            {
                dB.InsertRecordCourse(this);
            }       
        }
    }
}
