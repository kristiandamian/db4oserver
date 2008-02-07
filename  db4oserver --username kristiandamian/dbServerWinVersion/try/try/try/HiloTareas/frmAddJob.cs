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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DB4OServer;
using Db4objects.Db4o;

namespace tryIcon.HiloTareas
{
    public partial class frmAddJob : Form
    {
        private string sFileWindowName = "Backup File";
        private string sMessageTextBox = "Plase input the job name";
        private string sMessageFileName = "Please select the backup file";
        private string sMessageErrorClient="There was an error with the client data";


        private static string _JobsFile = Application.StartupPath + Path.DirectorySeparatorChar + "JobsFile.yap";

        private Cliente _MyClient;
        public Cliente MyClient
        {
            set { _MyClient = value; }
            get { return _MyClient; }
        }

        public frmAddJob()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddJob_Load(object sender, EventArgs e)
        {
            if (_MyClient != null)
            {
                lblFileClient.Text = _MyClient.File;
                lblServerClient.Text = _MyClient.Server;
                LlenoCombo();
                rdOnlyOnce.Checked = true;//deshabilito los demas groupBox
                rdDefrag.Checked = true;
            }
            else
            {
                MessageBox.Show(sMessageErrorClient);
                this.Close();
            }
        }

        private void LlenoCombo()
        {
            for (int x = 1; x <= 365; x++)
            {
                cmbDays.Items.Add(x);
            }
            cmbDays.SelectedIndex = 0;
        }

        private void EnableOptions()
        {
            gpDays.Enabled = rdDaysOfWeek.Checked;
            gpSomeDays.Enabled = rdSomeDays.Checked;
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            if (rdBackUp.Checked)
            {
                saveFileDialog1.Filter = "Backup Files (*.bak)| *.bak";
                saveFileDialog1.DefaultExt = ".bak";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.SupportMultiDottedExtensions = true;
                saveFileDialog1.Title = sFileWindowName;
                saveFileDialog1.ShowDialog();

                if (saveFileDialog1.FileName.Trim().Length > 0)
                {
                    lblFile.Text = saveFileDialog1.FileName;
                    toolTip1.SetToolTip(lblFile, lblFile.Text);
                }
            }
        }

        private void rdDaysOfWeek_CheckedChanged_1(object sender, EventArgs e)
        {
            EnableOptions();
        }

        private void rdSomeDays_CheckedChanged_1(object sender, EventArgs e)
        {
            EnableOptions();
        }

        private void rdOnlyOnce_CheckedChanged_1(object sender, EventArgs e)
        {
            EnableOptions();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                bool bFileName=true;
                if (txtJobName.Text.Trim().Length > 0)
                {
                    if (rdBackUp.Checked)
                        if (lblFile.Text.Trim().Length <= 0)
                            bFileName = false;//no selecciono el nombre del archivo
                    if (bFileName)
                    {
                        ScheduleType _scheduleType = new ScheduleType();
                        JobType _jobType = new JobType();
                        if (rdOnlyOnce.Checked)
                            _scheduleType = ScheduleType.OnlyOnce;
                        if (rdDaysOfWeek.Checked)
                            _scheduleType = ScheduleType.DaysOfWeek;
                        if (rdSomeDays.Checked)
                            _scheduleType = ScheduleType.SomeDays;

                        if (rdDefrag.Checked)
                            _jobType = JobType.Defrag;
                        if (rdBackUp.Checked)
                            _jobType = JobType.Backup;
                        //Creo la tarea
                        Tarea _tarea = new Tarea(_scheduleType, _jobType, _MyClient);
                        //Defino los demas valores
                        _tarea.JobName = txtJobName.Text;
                        //_tarea.MyClient = _MyClient;
                        string sFecha = dtFecha.Value.ToShortDateString() + " " + dtHora.Value.ToShortTimeString();

                        bool[] bWeek = new bool[7];
                        AssignWeek(ref bWeek);                        
                        _tarea.InitDate(DateTime.Parse(sFecha), cmbDays.SelectedIndex + 1, bWeek);
                        AddJob(_tarea);
                        Clear();
                    }
                    else
                        MessageBox.Show(sMessageFileName);
                }
                else
                    MessageBox.Show(sMessageTextBox);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// grabo el objeto en el archivo correspondiente
        /// </summary>
        /// <param name="_tarea">Job a grabar YA CON TODOS LOS DATOS</param>
        private void AddJob(Tarea _tarea)
        {
            try
            {
                IObjectContainer dbcliente = Db4oFactory.OpenFile(_JobsFile);
                dbcliente.Set(_tarea);
                dbcliente.Commit();
                dbcliente.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AssignWeek(ref bool[] bWeek)
        {
            bWeek[0] = chkSunday.Checked;//domingo
            bWeek[1] = chkMonday.Checked;//Lunes
            bWeek[2] = chkTuesday.Checked;//martes
            bWeek[3] = chkWednesday.Checked;//miercoles
            bWeek[4] = chkThursday.Checked;//Jueves
            bWeek[5] = chkFriday.Checked;//viernes
            bWeek[6] = chkSaturday.Checked;//sabado
        }

        private void Clear()
        {
            cmbDays.SelectedIndex = 0;
            txtJobName.Text = "";
            lblFile.Text = "";
            rdOnlyOnce.Checked = true;
            rdDefrag.Checked = true;
            dtFecha.Value = DateTime.Now;
            dtHora.Value = DateTime.Now;
            foreach (Control cn in gpDays.Controls)
            {
                if (cn.GetType().Name == "CheckBox")//Limpio los dias
                    ((CheckBox)cn).Checked = false;
            }
        }
    }
}