﻿using System;
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
    public partial class CommandUpdateRecord : Form
    {
        private DatabaseManager dB;

        public CommandUpdateRecord(DatabaseManager dB)
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

        public void SetTextBoxInteraction(bool input)
        {
            insertTB2.Enabled = input;
            insertTB3.Enabled = input;
            insertTB4.Enabled = input;
            insertTB5.Enabled = input;
        }

        private void CommandUpdateRecord_Load(object sender, EventArgs e)
        {
            SetTextBoxInteraction(false);
        }

        private void schoolRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (schoolRadio.Checked)
            {
                insertLB1.Text = "School ID";
                insertLB2.Text = "School Name";
                insertLB3.Text = "School Address";
                insertLB4.Text = "School Phone";
                insertLB5.Text = "Course ID*";
                ClearTB();
            }
        }

        private void CourseRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (CourseRadio.Checked)
            {
                insertLB1.Text = "Course ID";
                insertLB2.Text = "Course Name";
                insertLB3.Text = "Course Code";
                insertLB4.Text = "Course Fee";
                insertLB5.Text = "Course Location";
                ClearTB();
            }
        }

        private void clearBN_Click(object sender, EventArgs e) => ClearTB();

        public void ClearTB()
        {
            insertTB1.Text = "";
            insertTB2.Text = "";
            insertTB3.Text = "";
            insertTB4.Text = "";
            insertTB5.Text = "";
            SetTextBoxInteraction(false);
        }

        private void cancelBN_Click(object sender, EventArgs e) => Close();

        private void refreshBN_Click(object sender, EventArgs e)
        {
            if (schoolRadio.Checked)
            {
                dB.RefreshRecordSchool(this);
            }
            else
            {
                dB.RefreshRecordCourse(this);
            }
        }

        private void updateBN_Click(object sender, EventArgs e)
        {
            if (schoolRadio.Checked)
            {
                dB.UpdateRecordSchool(this);
            }
            else
            {
                dB.UpdateRecordCourse(this);
            }
        }

    }
}
