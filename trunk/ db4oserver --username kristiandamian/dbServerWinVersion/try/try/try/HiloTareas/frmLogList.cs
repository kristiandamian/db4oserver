using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace tryIcon.HiloTareas
{
    public partial class frmLogList : Form
    {
        private string sJobName;
        public string JobName
        {
            set { sJobName = value; }
        }

        public frmLogList()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogList_Load(object sender, EventArgs e)
        {
            IList<string> Mensajes= Log.AllLog(sJobName);
            if (Mensajes != null)
            {
                foreach (string cadena in Mensajes)
                {
                    lstLog.Items.Add(cadena);
                }
            }
        }
    }
}