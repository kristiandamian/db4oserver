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
        private string sMessageError = "Error al crear usuario";
        private string sMessageData = "Error con los datos capturados, por favor verifique";
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
            if (_MiCliente != null &&
                _MiCliente.Server != null &&
                _MiCliente.File != null)
            {
                lblServer.Text = _MiCliente.Server;
                lblFile.Text = _MiCliente.File;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidoDatos())
                {
                    IObjectContainer dbcliente = Db4oFactory.OpenFile(MiCliente.FileUsers);
                    Usuario user = new Usuario();
                    if (user.CreoUsuario(txtUser.Text, txtPassword.Text, lblServer.Text, lblFile.Text))
                    {
                        dbcliente.Set(user);
                        dbcliente.Commit();
                    }
                    else
                        MessageBox.Show(sMessageError);
                    dbcliente.Close();
                    Clear();
                }
                else
                    MessageBox.Show(sMessageData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool ValidoDatos()
        {
            bool bRetorno = false;
            if (lblFile.Text.Trim().Length > 0 &&
                lblServer.Text.Trim().Length > 0 &&
                txtUser.Text.Trim().Length > 0 &&
                txtPassword.Text.Trim().Length > 0)
                bRetorno = true;
            return bRetorno;
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