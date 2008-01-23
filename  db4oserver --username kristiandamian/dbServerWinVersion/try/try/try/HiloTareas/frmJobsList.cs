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
    public partial class frmJobsList : Form
    {
        private static string _JobsFile = Application.StartupPath + Path.DirectorySeparatorChar + "JobsFile.yap";

        private Cliente _MyClient;
        public Cliente MyClient
        {
            set { _MyClient = value; }
            get { return _MyClient; }
        }

        private frmLogList frmLog;

        public frmJobsList()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmJobsList_Load(object sender, EventArgs e)
        {
            GetAllJobs();
        }

        /// <summary>
        /// Obtengo todas las tareas del objeto seleccionado solamente
        /// </summary>
        private void GetAllJobs()
        {
            try
            {
                IObjectContainer dbcliente = Db4oFactory.OpenFile(_JobsFile);
                IList<Tarea> tareas = dbcliente.Query<Tarea>(delegate(Tarea tarea)
                {
                    return  tarea.MyClient.File == _MyClient.File &&
                            tarea.MyClient.Server == _MyClient.Server;
                });
                if (tareas.Count > 0)
                {
                    foreach (Tarea tarea in tareas)
                    {
                        lstJobs.Items.Add(tarea.JobName);
                    }
                }
                dbcliente.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            if(lstJobs.SelectedIndex>=0)
            {
                if(frmLog==null || frmLog.IsDisposed)
                {
                    frmLog=new frmLogList();
                }

                frmLog.JobName = lstJobs.Items[lstJobs.SelectedIndex].ToString();
                frmLog.ShowDialog();
            }
        }
    }
}