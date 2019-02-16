using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Final_Project
{
    public class DatabaseManager
    {
        readonly OleDbConnection connection;

        readonly Regex justInt = new Regex("^[0-9.]*$");
        readonly Regex justStr = new Regex("^[a-zA-Z ]*$");
        readonly Regex strInt = new Regex("^[a-zA-Z0-9 ]*$");

        public DatabaseManager(DataBaseApp form)
        {
            connection = form.connection;
        }

        public void GenericTryCatchOutput(Exception ex)
        {
            if (ex is FormatException)
            {
                MessageBox.Show("Error: Please enter proper values.");
            }
            else if (ex is IndexOutOfRangeException)
            {
                MessageBox.Show("Error: Entered value cannot be found.");
            }
            else if (ex is KeyNotFoundException)
            {
                MessageBox.Show("Error: The course(s) you entered does not exist.");
            }
            else
            {
                MessageBox.Show("Error: " + ex.Source + "\n\nDescription: " + ex.Message);
            }
        }

        public void ConnectToDB(OleDbConnection connection)
        {
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                GenericTryCatchOutput(ex);
            }
        }

        public void DisconnectToDB(OleDbConnection connection)
        {
            try
            {
                connection.Close();
            }
            catch (Exception ex)
            {
                GenericTryCatchOutput(ex);
            }
        }

        public void InsertRecordSchool(CommandInsertRecord form)
        {
            string sqlInS = "INSERT INTO School (`School Name`, `School Address`, `School Phone`) " +
                "VALUES ('" + form.TB1 + "', '" + form.TB2 + "', '" + form.TB3 + "')";

                try
                {
                    List<string> SCid = form.TB4.Split(';').ToList();

                    if (!justInt.IsMatch(form.TB3) | !justStr.IsMatch(form.TB1) | !strInt.IsMatch(form.TB2))
                    {
                        throw new FormatException();
                    }

                    if (!Exists(SCid)) throw new IndexOutOfRangeException();

                    using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                    using (adapter.InsertCommand = new OleDbCommand(sqlInS, connection))
                    {
                        adapter.InsertCommand.ExecuteNonQuery();
                        foreach (string i in SCid)
                        {
                            adapter.InsertCommand.CommandText = "INSERT INTO School (`Course ID`.Value) VALUES ('" + i + "') WHERE `School Name` = '" + form.TB1 + "' ";
                            adapter.InsertCommand.ExecuteNonQuery();
                        }
                    };
                    MessageBox.Show("Success");
                    form.ClearTB();
                }
                catch (Exception ex)
                {
                    GenericTryCatchOutput(ex);
                }
        }

        public void InsertRecordCourse(CommandInsertRecord form)
        {
            string sqlIn = "INSERT INTO Course([Course ID], [Course Name], [Course Code], [Course Fee], [Course Location]) " +
                    "VALUES ('" + form.TB1 + "', '" + form.TB2 + "', '" + form.TB3 + "', '" + form.TB4 + "', '" + form.TB5 + "')";

            try
            {
                if (!justInt.IsMatch(form.TB3) | !justInt.IsMatch(form.TB4) | !strInt.IsMatch(form.TB5))
                {
                    throw new FormatException();
                }

                if (Convert.ToInt32(form.TB1) < 0 | Convert.ToDecimal(form.TB4) < 0) throw new FormatException();

                using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                using (adapter.InsertCommand = new OleDbCommand(sqlIn, connection))
                {
                    adapter.InsertCommand.ExecuteNonQuery();
                };
                MessageBox.Show("Success");
                form.ClearTB();
            }
            catch (Exception ex)
            {
                GenericTryCatchOutput(ex);
            }
        }

        public void RefreshRecordSchool(CommandUpdateRecord form)
        {
            try
            {
                if (!int.TryParse(form.TB1, out int num) | num < 0)
                {
                    throw new FormatException();
                }

                string sqlSr = "SELECT * FROM [School] Where [School ID] = " + num + " ";

                using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                using (adapter.SelectCommand = new OleDbCommand(sqlSr, connection))
                using (OleDbDataReader reader = adapter.SelectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        form.TB2 = reader[1].ToString();
                        form.TB3 = reader[2].ToString();
                        form.TB4 = reader[3].ToString();
                        form.TB5 = reader[4].ToString();
                        form.SetTextBoxInteraction(true);
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                };
            }
            catch (Exception ex)
            {
                form.SetTextBoxInteraction(false);
                form.ClearTB();
                GenericTryCatchOutput(ex);
            }
        }

        public void RefreshRecordCourse(CommandUpdateRecord form)
        {
            try
            {
                if (!int.TryParse(form.TB1, out int num) | num < 0)
                {
                    throw new FormatException();
                }

                string sqlSr = "SELECT * FROM [Course] Where [Course ID] = " + num + " ";

                using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                using (adapter.SelectCommand = new OleDbCommand(sqlSr, connection))
                using (OleDbDataReader reader = adapter.SelectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        form.TB2 = reader[1].ToString();
                        form.TB3 = reader[2].ToString();
                        form.TB4 = reader[3].ToString();
                        form.TB5 = reader[4].ToString();
                        form.SetTextBoxInteraction(true);
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                };
            }
            catch (Exception ex)
            {
                form.SetTextBoxInteraction(false);
                form.ClearTB();
                GenericTryCatchOutput(ex);
            }
        }

        public void UpdateRecordSchool(CommandUpdateRecord form)
        {
            try
            {
                if (!justStr.IsMatch(form.TB2) | !strInt.IsMatch(form.TB3) | !justInt.IsMatch(form.TB4))
                {
                    throw new FormatException();
                }

                List<string> SCid = form.TB5.Split(';').ToList();

                if (!Exists(SCid)) throw new KeyNotFoundException();

                if (!int.TryParse(form.TB1, out int id)) throw new FormatException();

                string sqlUp = "UPDATE School " +
                    "SET `School Name` = '" + form.TB2 + "', `School Address` = '" + form.TB3 + "', `School Phone` = '" + form.TB4 + "'" +
                    "WHERE `School ID` = " + id + "";

                using (OleDbCommand command = new OleDbCommand(sqlUp, connection))
                {
                    command.ExecuteNonQuery();
                    command.CommandText = "DELETE `Course ID`.Value FROM School WHERE `School Name` = '" + form.TB2 + "'";
                    command.ExecuteNonQuery();
                    foreach (string i in SCid)
                    {
                        command.CommandText = "INSERT INTO School (`Course ID`.Value) VALUES ('" + i + "') WHERE `School Name` = '" + form.TB2 + "'";
                        command.ExecuteNonQuery();
                    }
                };

                MessageBox.Show("Success");
                form.ClearTB();
            }
            catch (Exception ex)
            {
                GenericTryCatchOutput(ex);
            }
        }

        public void UpdateRecordCourse(CommandUpdateRecord form)
        {
            try
            {
                if (!justStr.IsMatch(form.TB2) | !strInt.IsMatch(form.TB3) | !justInt.IsMatch(form.TB4) | !strInt.IsMatch(form.TB5))
                {
                    throw new FormatException();
                }

                if (!int.TryParse(form.TB1, out int Cid) | !decimal.TryParse(form.TB4, out decimal Fee))
                {
                    throw new FormatException();
                }

                string sqlUpd = "UPDATE Course " +
                    "SET [Course Name] = '" + form.TB2 + "', [Course Code] = '" + form.TB3 + "', [Course Fee] = " + Fee + ", [Course Location] = '" + form.TB5 + "' " +
                    "WHERE [Course ID] = " + Cid + " ";

                using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                using (adapter.UpdateCommand = new OleDbCommand(sqlUpd, connection))
                {
                    adapter.UpdateCommand.ExecuteNonQuery();
                };
                MessageBox.Show("Success");
                form.ClearTB();
            }
            catch (Exception ex)
            {
                GenericTryCatchOutput(ex);
            }
        }

        public void RefreshRecordSchool(CommandDeleteRecord form)
        {
            try
            {
                if (!int.TryParse(form.TB1, out int num) | num < 0)
                {
                    throw new FormatException();
                }

                string sqlSr = "SELECT * FROM [School] Where [School ID] = " + num + " ";

                using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                using (adapter.SelectCommand = new OleDbCommand(sqlSr, connection))
                using (OleDbDataReader reader = adapter.SelectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        form.TB2 = reader[1].ToString();
                        form.TB3 = reader[2].ToString();
                        form.TB4 = reader[3].ToString();
                        form.TB5 = reader[4].ToString();
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                };
            }
            catch (Exception ex)
            {
                form.ClearTB();
                GenericTryCatchOutput(ex);
            }
        }

        public void RefreshRecordCourse(CommandDeleteRecord form)
        {
            try
            {
                if (!int.TryParse(form.TB1, out int num) | num < 0)
                {
                    throw new FormatException();
                }

                string sqlSr = "SELECT * FROM [Course] Where [Course ID] = " + num + " ";

                using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                using (adapter.SelectCommand = new OleDbCommand(sqlSr, connection))
                using (OleDbDataReader reader = adapter.SelectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        form.TB2 = reader[1].ToString();
                        form.TB3 = reader[2].ToString();
                        form.TB4 = reader[3].ToString();
                        form.TB5 = reader[4].ToString();
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                };
            }
            catch (Exception ex)
            {
                form.ClearTB();
                GenericTryCatchOutput(ex);
            }
        }

        public void DeleteRecordSchool(CommandDeleteRecord form)
        {
            string sqlDel = "DELETE FROM School WHERE [School ID] = " + form.TB1 + "";

            try
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                using (adapter.DeleteCommand = new OleDbCommand(sqlDel, connection))
                {
                    adapter.DeleteCommand.ExecuteNonQuery();
                };
                MessageBox.Show("Success");
                form.ClearTB();
            }
            catch (Exception ex)
            {
                GenericTryCatchOutput(ex);
            }
        }

        public void DeleteRecordCourse(CommandDeleteRecord form)
        {
            string sqlDel = "DELETE FROM Course WHERE [Course ID] = " + form.TB1 + "";

            try
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                using (adapter.DeleteCommand = new OleDbCommand(sqlDel, connection))
                {
                    adapter.DeleteCommand.ExecuteNonQuery();
                };
                MessageBox.Show("Success");
                form.ClearTB();
            }
            catch (Exception ex)
            {
                GenericTryCatchOutput(ex);
            }
        }

        public void RunQuery(DataBaseApp form)
        {
            try
            {
                if (form.QueryTB == "")
                {
                    throw new FormatException();
                }

                using (OleDbCommand command = new OleDbCommand(form.QueryTB, connection))
                using (DataTable dt = new DataTable())
                {
                    if (form.QueryTB.Split(' ').First() == "SELECT")
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dt);
                            if (dt.Rows.Count == 0)
                            {
                                form.OutputGrid.Columns.Clear();
                                throw new IndexOutOfRangeException();
                            }
                            else
                            {
                                form.OutputGrid.DataSource = dt;
                            }
                        };
                    }
                    else if (form.QueryTB.Contains("School"))
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "SELECT * FROM School";
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dt);
                            if (dt.Rows.Count == 0)
                            {
                                form.OutputGrid.Columns.Clear();
                                throw new IndexOutOfRangeException();
                            }
                            else
                            {
                                form.OutputGrid.DataSource = dt;
                            }
                        };
                    }
                    else if (form.QueryTB.Contains("Course"))
                    {
                        command.ExecuteNonQuery();
                        command.CommandText = "SELECT * FROM Course";
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                        {
                            adapter.Fill(dt);
                            if (dt.Rows.Count == 0)
                            {
                                form.OutputGrid.Columns.Clear();
                                throw new IndexOutOfRangeException();
                            }
                            else
                            {
                                form.OutputGrid.DataSource = dt;
                            }
                        };
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                };
                MessageBox.Show("Succes");
            }
            catch (Exception ex)
            {
                GenericTryCatchOutput(ex);
            }
        }

        public bool Exists(List<string> SCid)
        {
            string search = "SELECT * FROM Course";

            using (OleDbCommand command = new OleDbCommand(search, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            using (DataTable results = new DataTable())
            {
                adapter.Fill(results);
                foreach (string i in SCid)
                {
                    if (!results.AsEnumerable().Any(row => Convert.ToInt32(i) == row.Field<int>("Course ID")))
                    {
                        return false;
                    }
                }
            };
            return true;
        }
    }
}
