using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Final_Project
{
    public partial class DataBaseApp : Form
    {
        private DatabaseManager dB;

        public OleDbConnection connection = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.16.0;" + @"Data Source = .\..\..\DB.accdb");

        public delegate void DatabaseToGridView();

        public DataBaseApp()
        {
            InitializeComponent();
        }

        public string QueryTB
        {
            get { return queryTB.Text; }
            set { queryTB.Text = value; }
        }

        public DataGridView OutputGrid
        {
            get { return outputGrid; }
            set { outputGrid = value; }
        }

        private void DataBaseApp_Load(object sender, EventArgs e)
        {
            dB = new DatabaseManager(this);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dB.ConnectToDB(connection);
            connectToolStripMenuItem.Enabled = false;
            disconnectToolStripMenuItem.Enabled = true;
        }

        private void DisconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dB.DisconnectToDB(connection);
            connectToolStripMenuItem.Enabled = true;
            disconnectToolStripMenuItem.Enabled = false;
        }

        private void InsertRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                CommandInsertRecord insert = new CommandInsertRecord(dB);
                insert.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error: Please connect to the database.");
            }
        }

        private void UpdateRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                CommandUpdateRecord update = new CommandUpdateRecord(dB);
                update.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error: Please connect to the database.");
            }
        }

        private void DeleteRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                CommandDeleteRecord delete = new CommandDeleteRecord(dB);
                delete.ShowDialog();
            }
            else
            {
                MessageBox.Show("Error: Please connect to the database.");
            }
        }

        private void RunQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (connection.State == ConnectionState.Open)
            {
                dB.RunQuery(this);
            }
            else
            {
                MessageBox.Show("Error: Please connect to the database.");
            }
        }
    }
}