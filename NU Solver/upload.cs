using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NU_Solver
{
    public partial class upload : Form
    {
        string folder = @"D:\Scanned\";
        public upload()
        {
            InitializeComponent();
        }

        private void upload_Click(object sender, EventArgs e)
        {
            label2.Text = "Processing...";
            DirectoryInfo dr = new DirectoryInfo(folder);
            for (int i = 0; i < dr.GetFiles().Length; i++)
            {
                //this.listBox1.Items.Add(dr.GetFiles()[i].Name);
                uploadfile(dr.GetFiles()[i].Name);
            }

            label2.Text = "Finished...";
            MessageBox.Show("Finished");
        }
        private void uploadfile(string filename)
        {
            string fullpath = folder + filename;
            string sub_code = "";
            if (filename.Length >= 7)
                sub_code = "21" + filename.Substring(1, 4);
            string filename_insert = String.Format("INSERT INTO dbo.[file_list] ([file_name],[pap_code],[insert_date]) values('{0}','{1}', CURRENT_TIMESTAMP )", filename, sub_code);
            //this.textBox1.Text = this.textBox1.Text + filename_insert;
            //this.richTextBox1.Text = this.richTextBox1.Text +"\n"+ filename_insert;
            //return;
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) throw new Exception("Can't create and open a connection");
                SqlCommand cmd = new SqlCommand(filename_insert, con);
                SqlDataReader reader = cmd.ExecuteReader();
                this.richTextBox1.Text = filename + " inserted.\n" + this.richTextBox1.Text;
                SqlConnection con2 = Database.GetConnectionObj();
                StreamReader sr = new StreamReader(fullpath);

                int sl = 0;
                while (sr.EndOfStream == false)
                {
                    sl++;
                    string line = sr.ReadLine();
                    //sl = Int32.Parse(line.Substring(0,10));
                    string insert_line = string.Format(
                        "INSERT INTO [dbo].[E_solving] " +
                        "([file_name] " +
                        ",[scanned_row]" +
                        ",[serial]) VALUES ('{0}','{1}','{2}')",
                        filename, line, sl);
                    //SqlCommand command = new SqlCommand(insert_line, con2);
                    //SqlCommand command = new SqlCommand(insert_line, con2);
                    int tr = 0;
                Back:
                    tr++;
                    try
                    {
                        SqlCommand cmd1 = new SqlCommand(insert_line, con2);
                        cmd1.ExecuteNonQuery();
                        //SqlDataReader reader1 = cmd1.ExecuteReader();                        
                    }
                    catch (Exception ee)
                    {
                        //this.richTextBox1.Text = filename + "ROW insertion FAILED. " + tr + "time\n" + line + "\n" + this.richTextBox1.Text;
                        //MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
                        if (tr < 100)
                            goto Back;
                        else
                        {
                            //this.richTextBox1.Text += filename + "Permanent ROW insertion FAILED. " + ++tr + "time\n" + line + "\n" + this.richTextBox1.Text;
                            this.richTextBox1.Text = insert_line + "\n" + this.richTextBox1.Text;

                        }
                    }
                    StreamWriter lg = new StreamWriter("log.log");
                    lg.Write(this.richTextBox1.Text.ToString());
                    lg.Dispose();
                }
                sr.Dispose();
                //con2.Close();
                con.Close();
                con.Dispose();
                //if (reader.Read())
                //{                    
                //    //MessageBox.Show("something");
                //}
                //else
                //    MessageBox.Show("nothing");
                //return;
                //int retVal = cmd.ExecuteNonQuery();
                //int retVal = cmd.ExecuteNonQuery();
                //if (retVal == 1)
                //{
                    //MessageBox.Show(string.Format("inserted {} into Database", filename));
                    
                //}
                //else
                    //MessageBox.Show(string.Format("Can't insert {} into Database", filename));

            }
            catch (Exception ee)
            {
                this.richTextBox1.Text = filename + " insertion FAILED. May be same file name already exissts.\n" + this.richTextBox1.Text;
                //MessageBox.Show(string.Format("{0} May be alredy uploaded in Database", filename));
                //MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
                //Application.Exit();
            }
        }

        private void exit(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
            
            
        //    try
        //    {
        //        SqlConnection con = Database.GetConnectionObj();
        //        if (con == null) throw new Exception("Connection Creation Failed");
                
        //        SqlCommand cmd = new SqlCommand(filename_insert, con);
        //        //cmd.CommandText = 
        //        //SqlDataReader reader = cmd.ExecuteReader();
        //        int serial = 1;
        //        int affectedRow = 0;
        //        SqlConnection con2 = Database.GetConnectionObj();
        //        while (reader.Read())
        //        {
        //            //MessageBox.Show(reader[0].ToString());
        //            int id = Convert.ToInt32(reader["id"]);
        //            string line = reader["scanned_row"].ToString();

        //            //int n = dataGridView1.Rows.Add();
        //            //string scan_sl = line.Substring(0, 50);
        //            //string scan_sl = line.Substring(0, 50).Replace(' ', '*');
        //            string dexcode = line.Substring(50, 32).Replace(' ', '0');
        //            string exam_code = line.Substring(82, 3).Replace(' ', '*');
        //            string reg_no = line.Substring(85, 11).Replace(' ', '*');
        //            string pap_code = line.Substring(96, 6).Replace(' ', '*');
        //            string hexcode = line.Substring(102, 32).Replace(' ', '0');
        //            //string err_dscr = "error goes here";
        //            string err_dscr = checkExamCode(exam_code) +
        //                checkSubjectCode(pap_code) +
        //                checkRegi(reg_no) +
        //                checkLitho("", "", dexcode, hexcode);

        //            string update = string.Format(
        //                "UPDATE dbo.E_solving SET err_dscr = '{0}',exam_code = '{1}',pap_code = '{2}',regi = '{3}',litho_1 = '{4}',litho_2 = '{5}',updates = '{6}', serial = {7} WHERE id = '{8}'",
        //                err_dscr, exam_code, pap_code, reg_no, dexcode, hexcode, "", serial++, id);
        //            //MessageBox.Show(update);
        //            //cmd.CommandText = update;
        //            SqlCommand command = new SqlCommand(update, con2);
        //            //SqlDataReader reader2 = command.ExecuteReader();
        //            //command.ExecuteReader();
        //            affectedRow = command.ExecuteNonQuery();
        //            //MessageBox.Show(affectedRow.ToString()+" rows affected for id "+ id.ToString());
        //        }
        //        reader.Dispose();
        //    }
        //    catch (Exception ee)
        //    {
        //        MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
        //        Application.Exit();
        //    }
        //    StreamReader sr = new StreamReader(fullpath);
        //    int sl = 0;
        //    while (sr.EndOfStream == false)
        //    {
        //        sl++;
        //        string line = sr.ReadLine();               


        //        string sql_insert = String.Format("INSERT INTO etemp values('{0}',NULL,'{1}','{2}','{3}','{4}','{5}','{6}','{7}',NULL,'{8}','{9}')", sl, scan_sl, dexcode, exam_code, exam_roll, reg_no, sub_code, extra, hexcode, form_sl);
        //        // string sql_insert = String.Format("INSERT INTO etemp values('{0}',NULL,'{1}','{2}','{3}','{4}','{5}','{6}','{7}',NULL,'{8}',NULL)", sl, scan_sl, dexcode, exam_code, exam_roll, reg_no, sub_code, extra, hexcode);
        //        cmd[0].CommandText = sql_insert;
        //        cmd[0].ExecuteNonQuery();
        //    }

        //    sr.Dispose();
        //}
    }
}
