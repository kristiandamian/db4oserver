/*  This file is part of db4oserver

    db4oserver is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    db4oserver is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace tryIcon.HiloTareas
{
    partial class frmAddJob
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
            this.components = new System.ComponentModel.Container();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.gpSomeDays = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbDays = new System.Windows.Forms.ComboBox();
            this.gpDays = new System.Windows.Forms.GroupBox();
            this.chkSaturday = new System.Windows.Forms.CheckBox();
            this.chkFriday = new System.Windows.Forms.CheckBox();
            this.chkThursday = new System.Windows.Forms.CheckBox();
            this.chkWednesday = new System.Windows.Forms.CheckBox();
            this.chkTuesday = new System.Windows.Forms.CheckBox();
            this.chkMonday = new System.Windows.Forms.CheckBox();
            this.chkSunday = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtHora = new System.Windows.Forms.DateTimePicker();
            this.dtFecha = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdSomeDays = new System.Windows.Forms.RadioButton();
            this.rdDaysOfWeek = new System.Windows.Forms.RadioButton();
            this.rdOnlyOnce = new System.Windows.Forms.RadioButton();
            this.gpJobType = new System.Windows.Forms.GroupBox();
            this.btnFile = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.rdDefrag = new System.Windows.Forms.RadioButton();
            this.rdBackUp = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblServerClient = new System.Windows.Forms.Label();
            this.lblFileClient = new System.Windows.Forms.Label();
            this.gpSomeDays.SuspendLayout();
            this.gpDays.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gpJobType.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(157, 423);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(238, 423);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Job Name";
            // 
            // txtJobName
            // 
            this.txtJobName.Location = new System.Drawing.Point(72, 52);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(242, 20);
            this.txtJobName.TabIndex = 3;
            // 
            // gpSomeDays
            // 
            this.gpSomeDays.Controls.Add(this.label5);
            this.gpSomeDays.Controls.Add(this.label4);
            this.gpSomeDays.Controls.Add(this.cmbDays);
            this.gpSomeDays.Location = new System.Drawing.Point(11, 353);
            this.gpSomeDays.Name = "gpSomeDays";
            this.gpSomeDays.Size = new System.Drawing.Size(304, 54);
            this.gpSomeDays.TabIndex = 16;
            this.gpSomeDays.TabStop = false;
            this.gpSomeDays.Text = "Each N Days";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(198, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "days";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Run every ";
            // 
            // cmbDays
            // 
            this.cmbDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDays.FormattingEnabled = true;
            this.cmbDays.Location = new System.Drawing.Point(71, 19);
            this.cmbDays.Name = "cmbDays";
            this.cmbDays.Size = new System.Drawing.Size(121, 21);
            this.cmbDays.TabIndex = 0;
            // 
            // gpDays
            // 
            this.gpDays.Controls.Add(this.chkSaturday);
            this.gpDays.Controls.Add(this.chkFriday);
            this.gpDays.Controls.Add(this.chkThursday);
            this.gpDays.Controls.Add(this.chkWednesday);
            this.gpDays.Controls.Add(this.chkTuesday);
            this.gpDays.Controls.Add(this.chkMonday);
            this.gpDays.Controls.Add(this.chkSunday);
            this.gpDays.Location = new System.Drawing.Point(11, 277);
            this.gpDays.Name = "gpDays";
            this.gpDays.Size = new System.Drawing.Size(304, 70);
            this.gpDays.TabIndex = 15;
            this.gpDays.TabStop = false;
            this.gpDays.Text = "Days of week";
            // 
            // chkSaturday
            // 
            this.chkSaturday.AutoSize = true;
            this.chkSaturday.Location = new System.Drawing.Point(227, 19);
            this.chkSaturday.Name = "chkSaturday";
            this.chkSaturday.Size = new System.Drawing.Size(68, 17);
            this.chkSaturday.TabIndex = 6;
            this.chkSaturday.Text = "Saturday";
            this.chkSaturday.UseVisualStyleBackColor = true;
            // 
            // chkFriday
            // 
            this.chkFriday.AutoSize = true;
            this.chkFriday.Location = new System.Drawing.Point(157, 42);
            this.chkFriday.Name = "chkFriday";
            this.chkFriday.Size = new System.Drawing.Size(54, 17);
            this.chkFriday.TabIndex = 5;
            this.chkFriday.Text = "Friday";
            this.chkFriday.UseVisualStyleBackColor = true;
            // 
            // chkThursday
            // 
            this.chkThursday.AutoSize = true;
            this.chkThursday.Location = new System.Drawing.Point(157, 19);
            this.chkThursday.Name = "chkThursday";
            this.chkThursday.Size = new System.Drawing.Size(70, 17);
            this.chkThursday.TabIndex = 4;
            this.chkThursday.Text = "Thursday";
            this.chkThursday.UseVisualStyleBackColor = true;
            // 
            // chkWednesday
            // 
            this.chkWednesday.AutoSize = true;
            this.chkWednesday.Location = new System.Drawing.Point(78, 42);
            this.chkWednesday.Name = "chkWednesday";
            this.chkWednesday.Size = new System.Drawing.Size(83, 17);
            this.chkWednesday.TabIndex = 3;
            this.chkWednesday.Text = "Wednesday";
            this.chkWednesday.UseVisualStyleBackColor = true;
            // 
            // chkTuesday
            // 
            this.chkTuesday.AutoSize = true;
            this.chkTuesday.Location = new System.Drawing.Point(78, 19);
            this.chkTuesday.Name = "chkTuesday";
            this.chkTuesday.Size = new System.Drawing.Size(67, 17);
            this.chkTuesday.TabIndex = 2;
            this.chkTuesday.Text = "Tuesday";
            this.chkTuesday.UseVisualStyleBackColor = true;
            // 
            // chkMonday
            // 
            this.chkMonday.AutoSize = true;
            this.chkMonday.Location = new System.Drawing.Point(10, 42);
            this.chkMonday.Name = "chkMonday";
            this.chkMonday.Size = new System.Drawing.Size(64, 17);
            this.chkMonday.TabIndex = 1;
            this.chkMonday.Text = "Monday";
            this.chkMonday.UseVisualStyleBackColor = true;
            // 
            // chkSunday
            // 
            this.chkSunday.AutoSize = true;
            this.chkSunday.Location = new System.Drawing.Point(10, 19);
            this.chkSunday.Name = "chkSunday";
            this.chkSunday.Size = new System.Drawing.Size(62, 17);
            this.chkSunday.TabIndex = 0;
            this.chkSunday.Text = "Sunday";
            this.chkSunday.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dtHora);
            this.groupBox2.Controls.Add(this.dtFecha);
            this.groupBox2.Location = new System.Drawing.Point(152, 180);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(163, 91);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Start date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Time";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Date";
            // 
            // dtHora
            // 
            this.dtHora.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtHora.Location = new System.Drawing.Point(49, 62);
            this.dtHora.Name = "dtHora";
            this.dtHora.ShowUpDown = true;
            this.dtHora.Size = new System.Drawing.Size(101, 20);
            this.dtHora.TabIndex = 1;
            // 
            // dtFecha
            // 
            this.dtFecha.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFecha.Location = new System.Drawing.Point(49, 19);
            this.dtFecha.Name = "dtFecha";
            this.dtFecha.Size = new System.Drawing.Size(101, 20);
            this.dtFecha.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdSomeDays);
            this.groupBox1.Controls.Add(this.rdDaysOfWeek);
            this.groupBox1.Controls.Add(this.rdOnlyOnce);
            this.groupBox1.Location = new System.Drawing.Point(11, 180);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(135, 91);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shedule";
            // 
            // rdSomeDays
            // 
            this.rdSomeDays.AutoSize = true;
            this.rdSomeDays.Location = new System.Drawing.Point(6, 65);
            this.rdSomeDays.Name = "rdSomeDays";
            this.rdSomeDays.Size = new System.Drawing.Size(86, 17);
            this.rdSomeDays.TabIndex = 2;
            this.rdSomeDays.TabStop = true;
            this.rdSomeDays.Text = "Each N days";
            this.rdSomeDays.UseVisualStyleBackColor = true;
            this.rdSomeDays.CheckedChanged += new System.EventHandler(this.rdSomeDays_CheckedChanged_1);
            // 
            // rdDaysOfWeek
            // 
            this.rdDaysOfWeek.AutoSize = true;
            this.rdDaysOfWeek.Location = new System.Drawing.Point(6, 42);
            this.rdDaysOfWeek.Name = "rdDaysOfWeek";
            this.rdDaysOfWeek.Size = new System.Drawing.Size(90, 17);
            this.rdDaysOfWeek.TabIndex = 1;
            this.rdDaysOfWeek.TabStop = true;
            this.rdDaysOfWeek.Text = "Days of week";
            this.rdDaysOfWeek.UseVisualStyleBackColor = true;
            this.rdDaysOfWeek.CheckedChanged += new System.EventHandler(this.rdDaysOfWeek_CheckedChanged_1);
            // 
            // rdOnlyOnce
            // 
            this.rdOnlyOnce.AutoSize = true;
            this.rdOnlyOnce.Location = new System.Drawing.Point(6, 19);
            this.rdOnlyOnce.Name = "rdOnlyOnce";
            this.rdOnlyOnce.Size = new System.Drawing.Size(73, 17);
            this.rdOnlyOnce.TabIndex = 0;
            this.rdOnlyOnce.TabStop = true;
            this.rdOnlyOnce.Text = "Only once";
            this.rdOnlyOnce.UseVisualStyleBackColor = true;
            this.rdOnlyOnce.CheckedChanged += new System.EventHandler(this.rdOnlyOnce_CheckedChanged_1);
            // 
            // gpJobType
            // 
            this.gpJobType.Controls.Add(this.btnFile);
            this.gpJobType.Controls.Add(this.lblFile);
            this.gpJobType.Controls.Add(this.rdDefrag);
            this.gpJobType.Controls.Add(this.rdBackUp);
            this.gpJobType.Location = new System.Drawing.Point(11, 78);
            this.gpJobType.Name = "gpJobType";
            this.gpJobType.Size = new System.Drawing.Size(304, 76);
            this.gpJobType.TabIndex = 18;
            this.gpJobType.TabStop = false;
            this.gpJobType.Text = "Job Type";
            // 
            // btnFile
            // 
            this.btnFile.Location = new System.Drawing.Point(227, 39);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 23);
            this.btnFile.TabIndex = 3;
            this.btnFile.Text = "Select";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // lblFile
            // 
            this.lblFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFile.Location = new System.Drawing.Point(68, 39);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(159, 23);
            this.lblFile.TabIndex = 2;
            // 
            // rdDefrag
            // 
            this.rdDefrag.AutoSize = true;
            this.rdDefrag.Location = new System.Drawing.Point(6, 19);
            this.rdDefrag.Name = "rdDefrag";
            this.rdDefrag.Size = new System.Drawing.Size(57, 17);
            this.rdDefrag.TabIndex = 0;
            this.rdDefrag.TabStop = true;
            this.rdDefrag.Text = "Defrag";
            this.rdDefrag.UseVisualStyleBackColor = true;
            // 
            // rdBackUp
            // 
            this.rdBackUp.AutoSize = true;
            this.rdBackUp.Location = new System.Drawing.Point(6, 42);
            this.rdBackUp.Name = "rdBackUp";
            this.rdBackUp.Size = new System.Drawing.Size(62, 17);
            this.rdBackUp.TabIndex = 1;
            this.rdBackUp.TabStop = true;
            this.rdBackUp.Text = "Backup";
            this.rdBackUp.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(11, 158);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(304, 14);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Server :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "File :";
            // 
            // lblServerClient
            // 
            this.lblServerClient.AutoSize = true;
            this.lblServerClient.Location = new System.Drawing.Point(55, 9);
            this.lblServerClient.Name = "lblServerClient";
            this.lblServerClient.Size = new System.Drawing.Size(35, 13);
            this.lblServerClient.TabIndex = 22;
            this.lblServerClient.Text = "label8";
            // 
            // lblFileClient
            // 
            this.lblFileClient.AutoSize = true;
            this.lblFileClient.Location = new System.Drawing.Point(55, 32);
            this.lblFileClient.Name = "lblFileClient";
            this.lblFileClient.Size = new System.Drawing.Size(35, 13);
            this.lblFileClient.TabIndex = 23;
            this.lblFileClient.Text = "label9";
            // 
            // frmAddJob
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 458);
            this.Controls.Add(this.lblFileClient);
            this.Controls.Add(this.lblServerClient);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.gpJobType);
            this.Controls.Add(this.gpSomeDays);
            this.Controls.Add(this.gpDays);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtJobName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmAddJob";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Job";
            this.Load += new System.EventHandler(this.frmAddJob_Load);
            this.gpSomeDays.ResumeLayout(false);
            this.gpSomeDays.PerformLayout();
            this.gpDays.ResumeLayout(false);
            this.gpDays.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gpJobType.ResumeLayout(false);
            this.gpJobType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtJobName;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox gpSomeDays;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDays;
        private System.Windows.Forms.GroupBox gpDays;
        private System.Windows.Forms.CheckBox chkSaturday;
        private System.Windows.Forms.CheckBox chkFriday;
        private System.Windows.Forms.CheckBox chkThursday;
        private System.Windows.Forms.CheckBox chkWednesday;
        private System.Windows.Forms.CheckBox chkTuesday;
        private System.Windows.Forms.CheckBox chkMonday;
        private System.Windows.Forms.CheckBox chkSunday;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtHora;
        private System.Windows.Forms.DateTimePicker dtFecha;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdSomeDays;
        private System.Windows.Forms.RadioButton rdDaysOfWeek;
        private System.Windows.Forms.RadioButton rdOnlyOnce;
        private System.Windows.Forms.GroupBox gpJobType;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.RadioButton rdDefrag;
        private System.Windows.Forms.RadioButton rdBackUp;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblServerClient;
        private System.Windows.Forms.Label lblFileClient;
    }
}