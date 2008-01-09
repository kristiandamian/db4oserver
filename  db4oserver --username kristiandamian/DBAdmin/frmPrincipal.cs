using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DB4OServer;
using Db4objects.Db4o;

namespace DBAdmin
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();

            cliente = new Cliente();
            cliente.File = "file.yap";
            cliente.Port = "1234";
            cliente.User = "user1";
            cliente.Server = "localhost";
            cliente.Password = "password";

            if (servidor == null)
                servidor = new RunServer(cliente);
            
        }

        private RunServer servidor;
        private Cliente cliente;
        private frmSettings config;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!servidor.IsRunning())
                {
                    servidor.Run();
                    offline.Visible = false;
                    online.Visible = !offline.Visible;
                }
                else
                    MessageBox.Show("La base de datos ya esta iniciada");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                servidor.Stop();
                offline.Visible = true;
                online.Visible = !offline.Visible;
            }
            catch (DatabaseClosedException ex)
            {
                MessageBox.Show("La base de datos ya esta detenida");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
       }

        private void Form1_Load(object sender, EventArgs e)
        {
            offline.Visible = true;
            online.Visible = !offline.Visible;
            if (cliente != null)
            {
                cmbInstance.Items.Add(cliente.Server);
                cmbInstance.SelectedIndex = 0;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (config == null || config.IsDisposed)
            {
                config = new frmSettings();
                config.FormClosing += new FormClosingEventHandler(config_FormClosing);
            }
            config.MiCliente = cliente;
            config.Show();
        }

        //guardo los datos almacenados
        private void config_FormClosing(object sender, EventArgs e)
        {
            cliente = config.MiCliente;
            cmbInstance.Items.Clear();
            cmbInstance.Items.Add(cliente.Server);
            cmbInstance.SelectedIndex = 0;
        }
    }
}