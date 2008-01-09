using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DB4OServer;


namespace DBAdmin
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private Cliente _MiCliente;
        public Cliente MiCliente
        {
            get {return _MiCliente;}
            set {_MiCliente=value;}
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            txtServer.Text=MiCliente.Server;
            txtPort.Text = MiCliente.Port;
            txtFile.Text = MiCliente.File;
            txtUser.Text = MiCliente.User;
            txtPassword.Text = MiCliente.Password;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            MiCliente.Server = txtServer.Text;
            MiCliente.Port = txtPort.Text;
            MiCliente.File = txtFile.Text;
            MiCliente.User = txtUser.Text;
            MiCliente.Password = txtPassword.Text;
            this.Close();
        }
    }
}