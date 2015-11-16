namespace NU_Solver
{
    partial class File_List
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.file_list_box = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // file_list_box
            // 
            this.file_list_box.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.file_list_box.FormattingEnabled = true;
            this.file_list_box.ItemHeight = 24;
            this.file_list_box.Location = new System.Drawing.Point(4, 3);
            this.file_list_box.Name = "file_list_box";
            this.file_list_box.Size = new System.Drawing.Size(267, 388);
            this.file_list_box.TabIndex = 1;
            this.file_list_box.DoubleClick += new System.EventHandler(this.file_double_clicked);
            // 
            // File_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 400);
            this.Controls.Add(this.file_list_box);
            this.Name = "File_List";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Available Files";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.file_list_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox file_list_box;
    }
}