using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NU_Solver
{
    public partial class E_solve : Form
    {
        public string filename = "";
        public string username = "";
        public string sub_code = "";
        public string current = "";
        login caller;
        public bool value_back = false;
        public bool error_only = false;
        //File_List caller;
        public DataTable dt = new DataTable();
        SqlDataAdapter da = new SqlDataAdapter();
        public E_solve(string filename, string username, string sub, login caller, bool needToSpan = false)
        {
            //test
            this.filename = filename;
            this.sub_code = sub;
            this.username = username;
            this.caller = caller;
            InitializeComponent();
            if (needToSpan)
                span();
            CustomiseGridView();
            this.Text = filename ;    
        }
        private void CustomiseGridView()
        {
            try
            {                
                SqlConnection con = Database.GetConnectionObj();
                SqlCommand cmd = new SqlCommand("", con);
                cmd.CommandText = string.Format("SELECT id, serial, err_dscr, exam_code, pap_code, regi, qr_code, scrpt_no, litho_1, litho_2,'' as '.' FROM dbo.E_solving where file_name = '{0}' ORDER BY serial", filename);
               
                //if( error_only)
                //    cmd.CommandText = string.Format("SELECT id, serial, err_dscr, exam_code, pap_code, regi, qr_code, scrpt_no, litho_1, litho_2,'' as '.' FROM dbo.E_solving where file_name = '{0}' and err_dscr <> '' ORDER BY serial", filename);
                //else
                //    cmd.CommandText = string.Format("SELECT id, serial, err_dscr, exam_code, pap_code, regi, qr_code, scrpt_no, litho_1, litho_2,'' as '.' FROM dbo.E_solving where file_name = '{0}' ORDER BY serial", filename);
                //error_only = !error_only;
                da.SelectCommand = cmd;
                da.Fill(dt);
                this.dataGridView1.DataSource = dt;

                this.dataGridView1.Columns[0].Visible = false;
                this.dataGridView1.Columns[1].ReadOnly = true;
                this.dataGridView1.Columns[2].ReadOnly = true;
                this.dataGridView1.Columns[3].Visible = false;//exam_code

                this.dataGridView1.Columns[1].Width = 60;
                //this.dataGridView1.Columns[2].Width = 70;
                this.dataGridView1.Columns[2].Width = 250;
                //this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.Columns[3].Width = 70;
                this.dataGridView1.Columns[4].Width = 90;
                this.dataGridView1.Columns[5].Width = 150;
                this.dataGridView1.Columns[6].Width = 6;
                this.dataGridView1.Columns[6].Visible = false; // qr
                this.dataGridView1.Columns[7].Width = 200;
                this.dataGridView1.Columns[8].Width = 6;
                this.dataGridView1.Columns[9].Width = 6;
                //this.dataGridView1.Columns[10].Width = 6;
                this.dataGridView1.Columns[10].Visible = false;
                //this.dataGridView1.Columns[1].Width = 60;
                ////this.dataGridView1.Columns[2].Width = 70;
                //this.dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //this.dataGridView1.Columns[3].Width = 70;
                //this.dataGridView1.Columns[4].Width = 90;
                //this.dataGridView1.Columns[5].Width = 150;
                //this.dataGridView1.Columns[6].Width = 6;
                //this.dataGridView1.Columns[7].Width = 180;
                //this.dataGridView1.Columns[8].Width = 40;
                //this.dataGridView1.Columns[9].Width = 403;
                //this.dataGridView1.Columns[10].Width = 6;
                DataGridViewTextBoxColumn exam_code = (DataGridViewTextBoxColumn)this.dataGridView1.Columns[3];
                exam_code.MaxInputLength = 3;
                DataGridViewTextBoxColumn sub_code = (DataGridViewTextBoxColumn)this.dataGridView1.Columns[4];
                sub_code.MaxInputLength = 6;
                DataGridViewTextBoxColumn reg_no = (DataGridViewTextBoxColumn)this.dataGridView1.Columns[5];
                reg_no.MaxInputLength = 11;
                DataGridViewTextBoxColumn qr = (DataGridViewTextBoxColumn)this.dataGridView1.Columns[6];
                qr.MaxInputLength = 28;
                DataGridViewTextBoxColumn binary = (DataGridViewTextBoxColumn)this.dataGridView1.Columns[7];
                binary.MaxInputLength = 10;
                DataGridViewTextBoxColumn l1 = (DataGridViewTextBoxColumn)this.dataGridView1.Columns[8];
                l1.MaxInputLength = 32;
                DataGridViewTextBoxColumn l2 = (DataGridViewTextBoxColumn)this.dataGridView1.Columns[9];
                l1.MaxInputLength = 32;
                //this.dataGridView1.AllowUserToAddRows = false;
                //this.dataGridView1.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(dataGridView1_EditingControlShowing);
                //this.dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
                con.Close();

                //DataGridViewTextBoxColumn er = (DataGridViewTextBoxColumn)this.dataGridView1.Columns[2];
                //er.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 14F, FontStyle.Bold);
           //this.dataGridView1.Rows[0].Cells[2].d
                //DataGridViewCellStyle style = new DataGridViewCellStyle();
                //style.Font = new Font(dataGridView1.Font, FontStyle.Italic);
                //dataGridView1.Rows[6].DefaultCellStyle = style;                
                //dataGridView1.Columns[2].DefaultCellStyle.ForeColor = Color.Gray;
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
                Application.Exit();
            }
        }
        private void litho_correction()
        {

            LithoCorrection L = new LithoCorrection();

            var result = L.ShowDialog();
            if (result == DialogResult.OK)
            {
                //string val = form.ReturnValue1;            //values preserved after close
                //string dateString = form.ReturnValue2;
                ////Do something here with these values

                ////for example
                //this.txtSomething.Text = val;
                MessageBox.Show("ok");
            }
            else
                MessageBox.Show("cancel");
        }
        
        public void span()
        {
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null)
                {
                    MessageBox.Show("Connection Problem");
                    return;
                }
                SqlCommand cmd = new SqlCommand("", con);
                cmd.CommandText = string.Format("SELECT id, scanned_row, file_name FROM dbo.E_solving WHERE file_name = '{0}' order by id", filename);
                SqlDataReader reader = cmd.ExecuteReader();
                //int serial = 1;
                int affectedRow = 0;
                SqlConnection con2 = Database.GetConnectionObj();
                while (reader.Read())
                {
                    //MessageBox.Show(reader[0].ToString());
                    int id = Convert.ToInt32(reader["id"]);
                    string line = reader["scanned_row"].ToString();

                    //int n = dataGridView1.Rows.Add();
                    //string scan_sl = line.Substring(0, 50);
                    //string scan_sl = line.Substring(0, 50).Replace(' ', '*');
                    string dexcode      = line.Substring(50, 32).Replace(' ', '0');
                    string exam_code    = line.Substring(82, 3).Replace(' ', '*');
                    string reg_no       = line.Substring(85, 11).Replace(' ', '*');
                    string pap_code     = line.Substring(96, 6).Replace(' ', '*');
                    string hexcode      = line.Substring(102, 32).Replace(' ', '0');
                    //string err_dscr = "error goes here";
                    string err_dscr = checkExamCode(exam_code) +
                        checkSubjectCode(pap_code) +
                        checkRegi(reg_no) +
                        checkLitho("", "", dexcode, hexcode);

                    string update = string.Format(
                        //"UPDATE dbo.E_solving SET err_dscr = '{0}',exam_code = '{1}',pap_code = '{2}',regi = '{3}',litho_1 = '{4}',litho_2 = '{5}',updates = '{6}', serial = {7} WHERE id = '{8}'",
                        "UPDATE dbo.E_solving SET err_dscr = '{0}',exam_code = '{1}',pap_code = '{2}',regi = '{3}',litho_1 = '{4}',litho_2 = '{5}',updates = '{6}' WHERE id = '{7}'",
                        err_dscr, exam_code, pap_code, reg_no, dexcode, hexcode, "", id);
                    //MessageBox.Show(update);
                    //cmd.CommandText = update;
                    SqlCommand command = new SqlCommand(update, con2);
                    //SqlDataReader reader2 = command.ExecuteReader();
                    //command.ExecuteReader();
                    try
                    {
                        affectedRow = command.ExecuteNonQuery();
                    }
                    catch (Exception E)
                    {
                    }
                    //MessageBox.Show(affectedRow.ToString()+" rows affected for id "+ id.ToString());
                }
                reader.Dispose();
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database error.\n" + ee.StackTrace.ToString());
                Application.Exit();
            }
        }

        private void onSolveWindowClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        //private int mach_count
        private void keyPressedOnCell(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                //MessageBox.Show(this.dataGridView1.CurrentCell.RowIndex.ToString());
                //L.Show();
                dataGridView1.Rows[dataGridView1.CurrentRow.Index].ErrorText = string.Empty;
                current = dataGridView1.CurrentCell.Value.ToString();
                //MessageBox.Show(current);
                dataGridView1.BeginEdit(true);
                //litho_correction();
                e.Handled = true;
            }
            else if (e.KeyChar == 'e' || e.KeyChar == 'E')
            {
                int row = -1, count = 0,sub=0,exm = 0, reg = 0, litho = 0;
                string tmp = "";
                while (++row < dataGridView1.RowCount - 1)
                {
                    if (!string.IsNullOrEmpty(dataGridView1.Rows[row].Cells[2].Value.ToString()))
                    {
                        count++;
                        tmp = dataGridView1.Rows[row].Cells[2].Value.ToString();
                        if(tmp.Contains("Exam")) exm++;
                        if(tmp.Contains("Sub")) sub++;
                        if(tmp.Contains("Regi")) reg++;
                        if (tmp.Contains("Litho")) litho++;                        
                    }
                }

                //toolStripStatusLabel1.Text = string.Format(
                tmp = string.Format(
                    "Total Error Line       : {0}\n"+
                    "Exam Code Error     : {1}\n" +
                    "Subject Code Error : {2}\n" +
                    "Registration Error   : {3}\n" +
                    "Litho Code Error     : {4}", count, exm, sub, reg, litho);
                MessageBox.Show(tmp);
                e.Handled = true;
            }
            else if (e.KeyChar == '-')
            {
                int col = this.dataGridView1.CurrentCell.ColumnIndex;
                if (col == 4)
                {
                    if (!Regex.IsMatch(dataGridView1.CurrentCell.Value.ToString(), @"^\d+$"))
                    {
                        dataGridView1.CurrentCell.Value = this.sub_code;
                        e.Handled = true;
                    }
                }
                ////int col = this.dataGridView1.CurrentCell.ColumnIndex;
                //if (col == 3)
                //    dataGridView1.CurrentCell.Value = "201";
                //else
            }
            else if (e.KeyChar == '+' )
            {
                int maxrow = this.dataGridView1.RowCount - 1;
                string cur_str = "";
                //string search_pattern = "";
                int col = this.dataGridView1.CurrentCell.ColumnIndex;
                int row = this.dataGridView1.CurrentCell.RowIndex;// +1;
                //int next_col = 1;
                //for (; row < this.dataGridView1.RowCount; row++)
                //{
                //    if (this.dataGridView1.Rows[row].Cells["err_dscr"].Value.ToString() != "")
                //    {
                //        cur_str = this.dataGridView1.Rows[row].Cells["err_dscr"].Value.ToString();
                //        if (cur_str.Contains("Sub"))
                //            next_col = 4;
                //        else if (cur_str.Contains("Regi"))
                //            next_col = 5;
                //        else if (cur_str.Contains("Litho"))
                //            next_col = 7;
                //        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[next_col];
                //        e.Handled = true;
                //        return;
                //    }
                //}

                //int rowno,
                //next_col = 1,
                    //current_row = this.dataGridView1.CurrentCell.RowIndex,
                //if (col <= 3)
                //{ //exam code
                //    col = 3;
                //    for (; row < this.dataGridView1.RowCount; row++)
                //        if (this.dataGridView1.Rows[row].Cells[3].Value.ToString() != "201")
                //        {
                //            //MessageBox.Show(this.dataGridView1.Rows[row].Cells[3].Value.ToString());
                //            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[3];
                //            e.Handled = true;
                //            return;
                //        }
                //    col = 4;
                //    row = 0;
                //}
                //if (col == 4)
                if (col <= 4)
                    { //subject code
                        col = 4;
                        for (; row < this.dataGridView1.RowCount; row++)
                            if (this.dataGridView1.Rows[row].Cells[col].Value.ToString() != this.sub_code)
                            {
                                //MessageBox.Show(this.dataGridView1.Rows[row].Cells[col].Value.ToString());
                                this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[col];
                                e.Handled = true;
                                return;
                            }
                        col = 5;
                        row = 0;
                    }

                if (col == 5)
                { //subject code
                    for (; row < this.dataGridView1.RowCount; row++)
                        if (this.dataGridView1.Rows[row].Cells[2].Value.ToString().Contains("Regi"))
                        {
                            //MessageBox.Show(this.dataGridView1.Rows[row].Cells[col].Value.ToString());
                            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[col];
                            e.Handled = true;
                            return;
                        }
                    col = 7;
                    row = 0;
                }
                if (col > 5)
                { //subject code
                    //col = 6;//??
                    //row = 0;//??
                    for (; row < this.dataGridView1.RowCount; row++)
                        if (this.dataGridView1.Rows[row].Cells[2].Value.ToString().Contains("Litho"))
                        {
                            //MessageBox.Show(this.dataGridView1.Rows[row].Cells[col].Value.ToString());
                            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[row].Cells[col];
                            e.Handled = true;
                            return;
                        }
                    //col = 6;
                    //row = 0;
                }
                e.Handled = true;
            }
            else if (e.KeyChar == '/')
            {
                finish();
                e.Handled = true;
            }
            else if (e.KeyChar == 'r')
            {
                relese();
                e.Handled = true;
            }
            else if (e.KeyChar == 'a' || e.KeyChar == 'A')
            {
                int current_pos = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value);
                if (current_pos == 0)
                    current_pos++;
                error_only = !error_only;
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = error_only? string.Format("err_dscr <> '{0}'", ""): string.Empty;
                int row = dataGridView1.RowCount - 1;
                while( Convert.ToInt32(dataGridView1.Rows[row].Cells[1].Value ) > current_pos){
                    if(row==0)
                        break; 
                    row--;
                }
                dataGridView1.CurrentCell = dataGridView1.Rows[row].Cells[2];

                //CustomiseGridView();
                //MessageBox.Show( );
                e.Handled = true;
            }
        }

        private void relese()
        {            //throw new NotImplementedException();
            if (MessageBox.Show("Are you really want to RELESE this file?\n" +
                "All Corrections made will be lost." +
                "Press 'Yes' to RELESE this file.\n" +
                "Press 'No' to return to solving window.\n",
                "Finish Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    SqlConnection con = Database.GetConnectionObj();
                    if (con == null) throw new Exception("Can't create and open a connection");
                    SqlCommand cmd = new SqlCommand("", con);

                    cmd.CommandText = string.Format(
                        "UPDATE dbo.file_list " +
                        "SET solver_name = '__NONE', solver_comments = solver_comments +' {0} Relesed on ' + CURRENT_TIMESTAMP " +
                        "WHERE file_name = '{1}' AND solver_name = '{0}' AND solve_status = 'solving' ",
                        username, filename);
                    //cmd.CommandText = "SELECT dbo.file_list.file_name FROM dbo.file_list";
                    //SqlDataReader reader = cmd.ExecuteReader();
                    //while (reader.Read())
                    //{
                    //    this.file_list_box.Items.Add(reader[0].ToString());
                    //}
                    //reader.Dispose();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("DB Update failed");
                    con.Close();
                    con.Dispose();

                    File_List fl = new File_List(username, caller);
                    fl.Show();
                    this.Hide();
                    //this.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Database error - Finishing failed.\n" + ee.StackTrace.ToString());
                    //Application.Exit();
                }
                // save file

                if (error_only)
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                this.dataGridView1.Sort(dataGridView1.Columns["serial"], ListSortDirection.Ascending);

                StreamWriter sw = new StreamWriter(@"Backup\" + filename + ".SLV");
                for (int row = 0; row < dataGridView1.RowCount; row++)
                {
                    sw.WriteLine("{0,12}{1,4}{3,3}{4,6}{5,11}{6,28}{7,10}{8,32}{9,32}{2,40}",
                        dataGridView1.Rows[row].Cells[0].Value, //id
                        dataGridView1.Rows[row].Cells[1].Value, //serial
                        dataGridView1.Rows[row].Cells[2].Value, //err_dscr
                        dataGridView1.Rows[row].Cells[3].Value, //exam_code
                        dataGridView1.Rows[row].Cells[4].Value, //pap_code
                        dataGridView1.Rows[row].Cells[5].Value, //regi
                        dataGridView1.Rows[row].Cells[6].Value, //qr_code
                        dataGridView1.Rows[row].Cells[7].Value, //scrpt_no
                        dataGridView1.Rows[row].Cells[8].Value, //litho_1
                        dataGridView1.Rows[row].Cells[9].Value, //litho_2
                        ""
                        );
                }
                sw.Dispose();
            }
        }
        private void finish()
        {            //throw new NotImplementedException();
            if (MessageBox.Show("Are you really want to finish?\n" +
                "You will be respnsible for any remaining error." +
                "Press 'Yes' to finish solving this file.\n" +
                "Press 'No' to return to solving window.\n",
                "Finish Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    SqlConnection con = Database.GetConnectionObj();
                    if (con == null) throw new Exception("Can't create and open a connection");
                    SqlCommand cmd = new SqlCommand("", con);

                    cmd.CommandText = string.Format(
                        "UPDATE dbo.file_list " +
                        "SET solve_status = 'SOLVED', end_date = CURRENT_TIMESTAMP " +
                        "WHERE file_name = '{1}' AND solver_name = '{0}' ",
                        username, filename);
                    //cmd.CommandText = "SELECT dbo.file_list.file_name FROM dbo.file_list";
                    //SqlDataReader reader = cmd.ExecuteReader();
                    //while (reader.Read())
                    //{
                    //    this.file_list_box.Items.Add(reader[0].ToString());
                    //}
                    //reader.Dispose();
                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("DB Update failed");
                    con.Close();
                    con.Dispose();

                    File_List fl = new File_List(username, caller);
                    fl.Show();
                    this.Hide();
                    //this.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Database error - Finishing failed.\n" + ee.StackTrace.ToString());
                    //Application.Exit();
                }
                // save file

                if (error_only)
                    (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
                this.dataGridView1.Sort(dataGridView1.Columns["serial"], ListSortDirection.Ascending);

                StreamWriter sw = new StreamWriter(@"Backup\" + filename + ".SLV");
                for (int row = 0; row < dataGridView1.RowCount; row++)
                {
                    sw.WriteLine("{0,12}{1,4}{3,3}{4,6}{5,11}{6,28}{7,10}{8,32}{9,32}{2,40}",
                        dataGridView1.Rows[row].Cells[0].Value, //id
                        dataGridView1.Rows[row].Cells[1].Value, //serial
                        dataGridView1.Rows[row].Cells[2].Value, //err_dscr
                        dataGridView1.Rows[row].Cells[3].Value, //exam_code
                        dataGridView1.Rows[row].Cells[4].Value, //pap_code
                        dataGridView1.Rows[row].Cells[5].Value, //regi
                        dataGridView1.Rows[row].Cells[6].Value, //qr_code
                        dataGridView1.Rows[row].Cells[7].Value, //scrpt_no
                        dataGridView1.Rows[row].Cells[8].Value, //litho_1
                        dataGridView1.Rows[row].Cells[9].Value, //litho_2
                        ""
                        );
                }
                sw.Dispose();
            }
        }
        //private string checkExamCode(string s) { return s != "201" ? "Exam code Err. " : ""; }
        private string checkExamCode(string s) { return ""; }
        private string checkSubjectCode(string s) { return s != this.sub_code ? "Sub. Err. " : ""; }//{ return s != this.sub_code ? "Subject code is not " + this.sub_code + "." : ""; }
        private string checkRegi(string regi)
        {
            if ( !Regex.IsMatch(regi, @"^\d+$"))
                return "Regi. IC.";
            else if (regi.Length != 11)
                return "Regi. Len";
            else if (regi.Substring(0,2) != "13") 
                return "Regi. 13";
            //return "Regi Contains Invalid Caracter.";
            return "";
        }
        private string checkLitho(string qr, string scrpt_no, string litho1, string litho2)
        {
            if (qr != "")
            {
                if (scrpt_no.Length != 28)
                    return "Litho QR length should 28.";
            }
            else if(scrpt_no != "")
            {
                if (!Regex.IsMatch(scrpt_no, @"^[\d]+$"))
                    return "Litho Script IC";
                if(scrpt_no.Length != 10)
                    return "Litho Script Len";
            }
            else if (!Regex.IsMatch(litho1, @"^[10]+$"))
                return "Litho 1 IC.";
            else if (!Regex.IsMatch(litho2, @"^[10]+$"))
                return "Litho 2 IC";
            else if (litho1.Length != 32)
                return "Litho 1 32";
            else if (litho2.Length != 32)
                return "Litho 2 32";
            else if (litho1 != litho2)
                return "Litho 1,2 NE";
            else //litho1 == litho2
            {
                //MessageBox.Show(litho1.Substring(0, 3) + "-" + litho1.Substring(28, 29));
                if (litho1.Substring(0, 4) == "1111" && litho1.Substring(28, 4) == "0001") //R8 F1 E type
                    return "";
                else if (litho1.Substring(0, 4) == "1111" && litho1.Substring(28, 4) == "1100") //R8 FC E type
                    return "";
                else return "Litho Shift";
            }
            return "";
        }

        private void cell_end_edit(object sender, DataGridViewCellEventArgs e)
        {
            //int r = e.RowIndex;
            //if( r != 0 && r!= dataGridView1.RowCount)
            //    r--;
            //r--;
            //MessageBox.Show("dwsdfs");
            //dataGridView1.CurrentCell = dataGridView1.Rows[r].Cells[e.ColumnIndex];
            //MessageBox.Show("cell_end_edit" + dataGridView1.CurrentCell.RowIndex.ToString() + " - " + dataGridView1.CurrentCell.ColumnIndex.ToString());
            //dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;            
        }
        private void cellValidaTING(object sender,
        DataGridViewCellValidatingEventArgs e)
        {

        } 

        private void cellValideTED(object sender, DataGridViewCellEventArgs e)
        {
            {
                //dataGridView1.Rows[e.RowIndex].ErrorText =
                //    "Company Name must not be empty";
                //MessageBox.Show("Cell " + dataGridView1.CurrentCell.RowIndex.ToString() + " - " + dataGridView1.CurrentCell.ColumnIndex.ToString() + "  cellValideTED");
            
            }
        }

        private string getError(int row)
        {
            return
                checkExamCode(dataGridView1.Rows[row].Cells[3].Value.ToString()) +
                checkSubjectCode(dataGridView1.Rows[row].Cells[4].Value.ToString()) +
                checkRegi(dataGridView1.Rows[row].Cells[5].Value.ToString()) +
                checkLitho(dataGridView1.Rows[row].Cells[6].Value.ToString(),
                            dataGridView1.Rows[row].Cells[7].Value.ToString(),
                            dataGridView1.Rows[row].Cells[8].Value.ToString(),
                            dataGridView1.Rows[row].Cells[9].Value.ToString()
                );
                
        }

        private void cellValueChanged(object sender,
    DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 3)
                return;
            string field_name = dataGridView1.Columns[e.ColumnIndex].Name;

            if (dataGridView1.Rows[e.RowIndex].ErrorText.ToString().Contains(field_name))
                return;
            string input = dataGridView1.CurrentCell.Value.ToString();
            string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            string error = getError(e.RowIndex);
            string qry = String.Format(
                "UPDATE E_solving SET " +
                "{0} = '{1}' " +
                ",err_dscr = '{4}' " +
                ",solver = '{2}' " +
                ",last_update = CURRENT_TIMESTAMP" +
                ",updates = "+
                "'({0}{1}{2}'+CONVERT(VARCHAR,CURRENT_TIMESTAMP)+')'+ updates "+
                "WHERE id = {3}",
                field_name,
                input,
                username,
                id,
                error
                );
            try
            {
                SqlConnection con = Database.GetConnectionObj();
                if (con == null) throw new Exception("Can't create and open a connection");
                SqlCommand cmd = new SqlCommand(qry, con);
                //MessageBox.Show(con.State.ToString());
                if (cmd.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("Database Update Faild. Try again Latter.\n");
                    dataGridView1.Rows[e.RowIndex].ErrorText = String.Format(
                                    "{1} entered on {0} not updated on database." ,
                                    field_name,
                                    input                                    
                                    );
                    dataGridView1.CurrentCell.Value = current;
                    //TODO: error
                }
                else
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = error;
                    //cmd.CommandText = qry;               
                con.Close();
                con.Dispose();
                dataGridView1.Rows[e.RowIndex].Cells[2].Value = error;
            }
            catch (Exception ee)
            {
                MessageBox.Show("Database Update Faild. Try again Latter.\n" + ee.StackTrace.ToString());
                dataGridView1.Rows[e.RowIndex].ErrorText = String.Format(
                                    "{1} entered on {0} not updated on database.",
                                    field_name,
                                    input
                                    );
                dataGridView1.CurrentCell.Value = current;
            }
            //dataGridView1.Rows[0].Cells[5].Value = qry;
            //MessageBox.Show(qry);
            //update error field;

            //string msg = String.Format(
            //    "Cell at row {0}, column {1} value changed",
            //    e.RowIndex, e.ColumnIndex);
            //MessageBox.Show(msg, "Cell Value Changed");
            //MessageBox.Show(value_back.ToString());
            //if (value_back)
            //{
            //    value_back = false; 
            //    return;
            //}
            
            //string qry = "";
            //string input = dataGridView1.CurrentCell.Value.ToString();
            //switch (e.ColumnIndex)
            //{
            //    case 3:
            //        if (checkExamCode(input) == "")
            //        {
            //            // no erroror
            //            qry = String.Format(
            //                    "'({0}{1}{2}'+CONVERT(VARCHAR,CURRENT_TIMESTAMP)+')'+ updates,",
            //                    dataGridView1.Columns[e.ColumnIndex].Name,
            //                    input,
            //                    username
            //                    );
            //            dataGridView1.Rows[0].Cells[5].Value = qry;
            //            MessageBox.Show(qry);

            //        }
            //        else
            //        {
            //            //dataGridView1.CurrentCell.Value = current;
            //            //value_back = true;
            //            dataGridView1.Rows[e.RowIndex].ErrorText = 
            //                string.Format("{0} is not a valid input for Exam code, \nUpdate un successfull",input);
            //            MessageBox.Show(dataGridView1.Rows[e.RowIndex].ErrorText);
            //        }
            //        break;
            //}
            //string qry = String.Format(
            //    " {0}, column {0} value changed", 
            //    dataGridView1.Columns[e.ColumnIndex].Name
            //    );
            //MessageBox.Show(qry);
        }
    }
}
