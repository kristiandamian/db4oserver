using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DB4OServer;
using Db4objects.Db4o;

namespace tryIcon
{
    public partial class frmUsers : Form
    {

        
        private Cliente _MiCliente;
        public Cliente MiCliente
        {
            get { return _MiCliente; }
            set { _MiCliente = value; }
        }
        
        public frmUsers()
        {
            InitializeComponent();
        }

        private void Users_Load(object sender, EventArgs e)
        {
            lblServer.Text = _MiCliente.Server;
            lblFile.Text = _MiCliente.File;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                IObjectContainer dbcliente = Db4oFactory.OpenFile(MiCliente.FileUsers);
                Usuario user = new Usuario();
                if (user.CreoUsuario(txtUser.Text, txtPassword.Text,lblServer.Text,lblFile.Text))
                {
                    dbcliente.Set(user);
                    dbcliente.Commit();
                }
                else
                    MessageBox.Show("Error al crear el usuario");
                dbcliente.Close();
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Clear()
        {
            foreach (Control con in this.Controls)
            {
                if (con.GetType().Name == "TextBox")
                    ((TextBox)con).Text = "";
            }
        }
    }


}