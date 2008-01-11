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


namespace tryIcon
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private frmUsers usuarios;
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
            //txtPassword.Text = MiCliente.Password;
            if (MiCliente.FileUsers == null || MiCliente.FileUsers.Trim().Length == 0)
            {
                txtFileUsers.Text = Application.StartupPath + Path.DirectorySeparatorChar + "users.yap";
                MiCliente.FileUsers = txtFileUsers.Text;
            }
            else
                txtFileUsers.Text = MiCliente.FileUsers;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            MiCliente.Server = txtServer.Text;
            MiCliente.Port = txtPort.Text;
            MiCliente.File = txtFile.Text;
            MiCliente.User = txtUser.Text;
            MiCliente.Password = txtPassword.Text;
            MiCliente.FileUsers = txtFileUsers.Text;
            this.Close();
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            if (txtFile.Text.Trim().Length > 0)
            {
                if (usuarios == null || usuarios.IsDisposed)
                    usuarios = new frmUsers();
                else
                    usuarios.Activate();                
                usuarios.MiCliente = _MiCliente;                
                usuarios.ShowDialog();
            }
        }
    }


}