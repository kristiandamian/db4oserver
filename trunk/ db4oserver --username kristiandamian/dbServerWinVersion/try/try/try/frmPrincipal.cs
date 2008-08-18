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
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DB4OServer;
using Db4objects.Db4o;

namespace tryIcon
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();

            cliente = new Cliente();
            
            if (servidor == null)
                servidor = new RunServer(cliente);
            //para saner si tiene acceso o no a la bd.
            servidor.SinAcceso += Excepcion;
        }
        

        #region "Mensajes del sistema"
        string sAccessMessage = "Do not have access to this server/file";//"No tiene acceso a ese Servidor/Archivo";
        string sClientError = "There was a error with the connection data";//"Error con los datos de conexion";
        string sStopError = "There was an error while trying to stop the server";//"Error al tratar de detener el server";
        #endregion
        private static string _InstancesFile = Application.StartupPath + Path.DirectorySeparatorChar + "InstancesFile.yap";
        private string _UsersFile = Application.StartupPath + Path.DirectorySeparatorChar + "users.yap";
        private RunServer servidor;
        private Cliente cliente;
        private List<Cliente> AllMyClient;//No boxing, no casting!! santos genericos batman!!
        private frmSettings config;        
        public Cliente MyClient
        {
            set { cliente = value; }
            get { return cliente; }
        }
        public List<Cliente> AllTheInstances
        {
            set { AllMyClient = value; }
            get { return AllMyClient; }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidoDatosCliente(true))
                {
                    if (!servidor.IsRunning())
                    {
                        servidor.Client = cliente;//por si cambio en el camino despues                        
                        servidor.Run();           //de ser instanciado                                     
                        //Si, esta es una solución ñoña, pero tengo que esperar a que 
                        //termine el otro hilo y no vale la pena meter sincronizacion
                        //para que solo muestre un dibujito
                        Thread.Sleep(1000);
                        ReviewState();
                        if (servidor.IsRunning())
                            Online();
                        else
                            Offline();
                    }
                    else
                        Online();
                    //    MessageBox.Show("La base de datos ya esta iniciada");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool ValidoDatosCliente(bool ConMensaje)
        {

            bool bRetorno=false;
            if (cliente!=null &&
               cliente.File != null && cliente.File.Trim().Length > 0 &&
               cliente.FileUsers != null && cliente.FileUsers.Trim().Length > 0 &&
               cliente.Password != null && cliente.Password.Trim().Length > 0 &&
               cliente.Server != null && cliente.Server.Trim().Length > 0 &&
               cliente.User != null && cliente.User.Trim().Length > 0 &&
               cliente.Port != null && Libreria.FuncionesCadena.IsNumeric(cliente.Port))
                bRetorno = true;
            if (!bRetorno && ConMensaje)
                MessageBox.Show(sClientError);
            return bRetorno;

        }
        private void Excepcion()
        {
            //lock (this)
            {            
                MessageBox.Show(sAccessMessage);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        public void StopServer()
        {
            try
            {
                if (servidor.Stop())
                    Offline();
                else
                    MessageBox.Show(sStopError);
            }
            /*catch (DatabaseClosedException ex)
            {
                MessageBox.Show("La base de datos ya esta detenida");
            }*/
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Offline();
            GetAllInstances();
            //pa manejar toooodas mis instancias
            if (AllMyClient == null)
                AllMyClient =new  List<Cliente>();

            if (AllMyClient.Count == 0)
            {
                cliente.File = "file2.yap";
                cliente.Port = "1235";
                cliente.User = "user1";
                cliente.Server = "localhost";
                cliente.Password = "Password12";
                cliente.FileUsers = _UsersFile;
                AllMyClient.Add(cliente.Clone() as Cliente);
                cliente.File = "C:\\Datos_Check\\Data.yap";
                cliente.Port = "8888";
                cliente.User = "User";
                cliente.Server = "localhost";
                cliente.Password = "Password12";
                cliente.FileUsers = _UsersFile;
                AllMyClient.Add(cliente.Clone() as Cliente);
            }

            if (cliente!=null && cliente.Server != null)
            {
                servidor.Client = cliente;
                Cliente oldClient = cliente;
                foreach (Cliente clientecito in AllMyClient)
                {
                    cliente = clientecito;//añado de uno en uno las instancias que agregue MANUALMENTE                                 
                    AñadoInstanciaAlCombo(false);//o con el boton View del menu contextual
                }
                cliente = oldClient;//me regreso al que tenia al principio
            }
            if (ValidoDatosCliente(false))
            {              
                    
                if (servidor.IsRunning())
                {
                    Online();
                }
            }          
        }

        private void AñadoInstanciaAlCombo(bool bAñadir)
        {
            if (bAñadir)
            {
                AllMyClient.Add(cliente.Clone() as Cliente);//lo meto al final
            }
            cmbInstance.Items.Add(cliente.Server+"->"+cliente.File);
            cmbInstance.SelectedIndex = cmbInstance.Items.Count - 1;            
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (config == null || config.IsDisposed)
            {
                config = new frmSettings();
                config.FormClosing += new FormClosingEventHandler(config_FormClosing);
            }
            else
                config.Activate();
            config.MiCliente = cliente;
            config.ShowDialog();
        }

        //guardo los datos almacenados
        private void config_FormClosing(object sender, EventArgs e)
        {            
            cliente = config.MiCliente;            
            //cmbInstance.Items.Clear();
            if (cliente.Server != null)
            {
                if(ClienteNuevo(cliente))
                    AñadoInstanciaAlCombo(true);
            }
            if (IsRunning())
                Online();
            else
                Offline();
        }
        private bool ClienteNuevo(Cliente MyClient)
        {
            bool bRetorno = true;
            foreach (Cliente clientecito in AllMyClient)
            {
                if (clientecito.Server == MyClient.Server &&
                    clientecito.File == MyClient.File &&
                    clientecito.Port == MyClient.Port)
                {
                    bRetorno = false;//no quiero añadirlo le movi algo
                    break;
                }
            }
            return bRetorno;
        }
        private void Offline()
        {
            imgOffline.Visible = true;
            imgOnline.Visible = !imgOffline.Visible;
        }

        private void Online()
        {
            imgOffline.Visible = false;
            imgOnline.Visible = !imgOffline.Visible;
        }

        public bool IsRunning()
        {
            bool bRetorno = false;
            if(ValidoDatosCliente(true))
                bRetorno=servidor.IsRunning();
            return bRetorno;
        }
        private void cmbInstance_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (AllMyClient.Count > 0 )
                {
                    cliente = AllMyClient[cmbInstance.SelectedIndex].Clone() as Cliente;
                    servidor.Client = cliente;
                }
                if (cliente.Server != null)
                {
                    if (IsRunning())
                        Online();
                    else
                        Offline();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        #region "Registered items cejilla"
        private void btnCancel_Click(object sender, EventArgs e)
        {
            ClearEdit();
            Clear();
        }

        private void Clear()
        {
            foreach (Control cn in tabPage2.Controls)
            {
                if (cn.GetType().Name == "TextBox")
                    ((TextBox)cn).Text = "";
                if (cn.GetType().Name == "CheckBox")
                    ((CheckBox)cn).Checked = false;
            }
        }
        private void ReviewState()
        {
            try
            {
                IObjectContainer dbcliente = Db4oFactory.OpenFile(_InstancesFile);
                IObjectSet objetitos = dbcliente.Get(new Instancia());
                treeFiles.Nodes[0].Nodes.Clear();
                while (objetitos.HasNext())
                {
                    Instancia _miniInstancia = (Instancia)objetitos.Next();
                    TreeNode Nodo = new TreeNode();
                    Nodo.Text = NombreArchivo(_miniInstancia.File);
                    RunServer _tempServer = new RunServer(_miniInstancia);
                    if (_tempServer.IsRunning())
                    {
                        Nodo.ImageIndex = 1;
                        Nodo.SelectedImageIndex = 1;
                    }
                    else
                    {
                        Nodo.ImageIndex = 0;
                        Nodo.SelectedImageIndex = 0;
                    }
                    Nodo.ContextMenu = MenuContextual();
                    treeFiles.Nodes[0].Nodes.Add(Nodo);
                }
                treeFiles.Nodes[0].Expand();
                dbcliente.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void BeginAllInstances()
        {
            try
            {
                IObjectContainer dbcliente = Db4oFactory.OpenFile(_InstancesFile);
                IObjectSet objetitos = dbcliente.Get(new Instancia());//obtengo tooodos los objetos

                while (objetitos.HasNext())
                {
                    Instancia _miniInstancia = (Instancia)objetitos.Next();
                    if (_miniInstancia.AutoInit)
                    {
                        RunServer _tempServer = new RunServer(_miniInstancia);
                        _tempServer.Run();
                    }           
                }
                objetitos = null;
                dbcliente.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetAllInstances()
        {
            try
            {
                IObjectContainer dbcliente = Db4oFactory.OpenFile(_InstancesFile);
                IObjectSet objetitos = dbcliente.Get(new Instancia());
                //treeFiles.Nodes.Clear();   
                while (objetitos.HasNext())
                {
                    Instancia _miniInstancia = (Instancia)objetitos.Next();
                    TreeNode Nodo = new TreeNode();
                    Nodo.Text = NombreArchivo(_miniInstancia.File);
                    RunServer _tempServer = new RunServer(_miniInstancia);

                    if (_tempServer.IsRunning())
                    {
                        Nodo.ImageIndex = 1;
                        Nodo.SelectedImageIndex = 1;
                    }
                    else
                    {
                        Nodo.ImageIndex = 0;
                        Nodo.SelectedImageIndex = 0;
                    }                    
                    Nodo.ContextMenu = MenuContextual();
                    treeFiles.Nodes[0].Nodes.Add(Nodo);                    
                }
                treeFiles.Nodes[0].Expand();
                dbcliente.Close();   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void btnOk_Click(object sender, EventArgs e)
        {            
            if (ValidoDatos())
            {
                AgregoInstancia();
                TreeNode Nodo = new TreeNode();
                Nodo.Text = NombreArchivo(txtFile.Text);
                Nodo.ImageIndex = 0;
                Nodo.SelectedImageIndex = 0;
                Nodo.ContextMenu = MenuContextual();
                treeFiles.Nodes[0].Nodes.Add(Nodo);
                treeFiles.Nodes[0].Expand();
                Clear();
            }
            else
                MessageBox.Show("Datos Invalidos");
        }

        private void AgregoInstancia()
        {
            
            try
            {                
                if (!bEdit)
                {//es una bonita alta                    
                    IObjectContainer dbcliente = Db4oFactory.OpenFile(_InstancesFile);
                    Instancia _tmpInstancia = new Instancia();
                    _tmpInstancia.File = txtFile.Text;
                    _tmpInstancia.AutoInit = chkStart.Checked;
                    _tmpInstancia.Password = txtPassword.Text;
                    _tmpInstancia.Port = txtPort.Text;
                    _tmpInstancia.Server = txtServer.Text;
                    _tmpInstancia.User = txtUser.Text;
                    _tmpInstancia.FileUsers = _UsersFile;
                    dbcliente.Set(_tmpInstancia);
                    dbcliente.Commit();
                    dbcliente.Close();
                }
                else
                {//es un bonito edit
                    if (instanciasEdit != null && sOldFile.Trim().Length > 0)
                    {                        
                        foreach (Instancia Ins in instanciasEdit)
                        {
                            RunServer _tempServer = new RunServer(Ins);
                            if (_tempServer.IsRunning())//detengo el servicio
                            {
                                _tempServer.Stop();
                            }
                            Ins.File = txtFile.Text.Trim();
                            Ins.Password = txtPassword.Text;
                            Ins.User = txtUser.Text.Trim();
                            Ins.Port = txtPort.Text.Trim();
                            Ins.Server = txtServer.Text.Trim();
                            Ins.AutoInit = chkStart.Checked;
                            if (Ins.FileUsers == null)
                                Ins.FileUsers = _UsersFile;
                            dbclienteEdit.Set(Ins);
                            foreach (TreeNode nodo in treeFiles.Nodes[0].Nodes)
                            {
                                if (nodo.Text == NombreArchivo(sOldFile))
                                {
                                    nodo.Remove();
                                    nodo.Text = NombreArchivo(Ins.File);
                                }
                            }
                        }
                        dbclienteEdit.Commit();
                        dbclienteEdit.Close();
                        ClearEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            /*finally
            {
                dbclienteEdit.Close();
            }*/
        }

        private string NombreArchivo(string sArchivo)
        {
            int iPos = Libreria.FuncionesCadena.BuscoDesdeUltimo(sArchivo,Path.DirectorySeparatorChar);
            return sArchivo.Substring(iPos + 1, (sArchivo.Length - 1 - iPos));
        }

        #region "Menu Contextual"
        private bool bEdit;
        private string sOldFile;
        private IObjectContainer dbclienteEdit;
        private IList<Instancia> instanciasEdit;
        private HiloTareas.frmAddJob frmNewJob;
        private HiloTareas.frmJobsList frmJobslist;
        /// <summary>
        /// Genero el menu contextual para los elementos que se crean en el treeview
        /// </summary>
        /// <returns>El objeto ContextMenu con la informacion y los eventos correspondientes</returns>
        private ContextMenu MenuContextual()
        {            
            ContextMenu menu = new ContextMenu();
            menu.MenuItems.Add("View this item");
            menu.MenuItems.Add("Edit this item");
            menu.MenuItems.Add("Delete this item");
            menu.MenuItems.Add("-");//separator
            menu.MenuItems.Add("Administrative Tasks");
            //submenu
            menu.MenuItems[4].MenuItems.Add("Add job");
            menu.MenuItems[4].MenuItems.Add("View jobs");

            menu.MenuItems[0].Click += new EventHandler(mnuView_Click);
            menu.MenuItems[1].Click += new EventHandler(mnuEdit_Click);
            menu.MenuItems[2].Click += new EventHandler(mnuDelete_Click);
            //submenu
            menu.MenuItems[4].MenuItems[0].Click+=new EventHandler(mnuAddJob_Click);
            menu.MenuItems[4].MenuItems[1].Click += new EventHandler(mnuViewJob_Click);
            
            return menu;
        }
        private void mnuViewJob_Click(object sender, EventArgs e)
        {
            Cliente _tempClient = SearchClient();
            if (frmJobslist == null || frmJobslist.IsDisposed)
            {
                frmJobslist = new tryIcon.HiloTareas.frmJobsList();
            }
            frmJobslist.MyClient = _tempClient;
            frmJobslist.ShowDialog();
        }
        private void mnuAddJob_Click(object sender, EventArgs e)
        {
            Cliente _tempClient = SearchClient();
            if (frmNewJob == null || frmNewJob.IsDisposed)
            {
                frmNewJob = new tryIcon.HiloTareas.frmAddJob();
            }
            frmNewJob.MyClient = _tempClient;
            frmNewJob.ShowDialog();
        }
        private void mnuView_Click(object sender, EventArgs e)
        {            
            try
            {
                int Total = 0;
                cliente = SearchClient(ref Total);
                if(Total>0)
                {
                    //borro los hijitos de padre
                    treeFiles.Nodes[0].Nodes.Clear();
                    /*if (cmbInstance.Items.Count > 0)
                        cmbInstance.Items.Clear();
                    */
                    tabControl1.SelectedIndex = 0;//me pongo en el primer Tab
                    if (ClienteNuevo(cliente))
                        AñadoInstanciaAlCombo(true);
                    //this.Form1_Load(this, null);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Busco en la bd el objeto CLIENTE del seleccionado en el treeview
        /// </summary>
        /// <returns>El objeto cliente instanciado</returns>
        private Cliente SearchClient()
        {
            Cliente _TempClient = null;
            IObjectContainer dbcliente = Db4oFactory.OpenFile(_InstancesFile);
            IList<Instancia> instancias = dbcliente.Query<Instancia>(delegate(Instancia instancia)
             {
                 return NombreArchivo(instancia.File) == treeFiles.SelectedNode.Text;
             });
            int Total = instancias.Count;
            if (Total > 0)
            {
                foreach (Instancia Ins in instancias)
                {
                    _TempClient = Ins;
                }
            }
            dbcliente.Close();
            return _TempClient;
        }
        private Cliente SearchClient(ref int Total)
        {
            Cliente _TempClient = null;
            IObjectContainer dbcliente = Db4oFactory.OpenFile(_InstancesFile);
            IList<Instancia> instancias = dbcliente.Query<Instancia>(delegate(Instancia instancia)
             {
                 return NombreArchivo(instancia.File) == treeFiles.SelectedNode.Text;
             });
            Total = instancias.Count;
            if (Total > 0)
            {
                foreach (Instancia Ins in instancias)
                {
                    _TempClient = Ins;
                }
            }
            dbcliente.Close();
            return _TempClient;
        }
        private void mnuEdit_Click(object sender, EventArgs e)
        {
            try
            {
                instanciasEdit = null;
                dbclienteEdit = Db4oFactory.OpenFile(_InstancesFile);
                instanciasEdit = dbclienteEdit.Query<Instancia>(delegate(Instancia instancia)
                 {
                     return NombreArchivo(instancia.File) == treeFiles.SelectedNode.Text;
                 });
                foreach (Instancia Ins in instanciasEdit)
                {
                    sOldFile = Ins.File.Trim();
                    txtFile.Text = Ins.File.Trim();
                    txtPassword.Text = "";
                    txtUser.Text = Ins.User.Trim();
                    txtPort.Text = Ins.Port.Trim();
                    txtServer.Text = Ins.Server.Trim();
                    chkStart.Checked = Ins.AutoInit;
                }
                //dbclienteEdit.Close();
                bEdit = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                dbclienteEdit.Close();
            }
        }
        private void mnuDelete_Click(object sender, EventArgs e)
        {
            try
            {
                IObjectContainer dbcliente = Db4oFactory.OpenFile(_InstancesFile);
                IList<Instancia> instancias = dbcliente.Query<Instancia>(delegate(Instancia instancia)
                 {
                     return NombreArchivo(instancia.File) == treeFiles.SelectedNode.Text;
                 });
                foreach (Instancia Ins in instancias)
                {
                    RunServer _tempServer = new RunServer(Ins);

                    if (_tempServer.IsRunning())
                    {
                        _tempServer.Stop();
                    }
                    foreach (TreeNode nodo in treeFiles.Nodes[0].Nodes)
                    {
                        if (nodo.Text == NombreArchivo(Ins.File))
                            nodo.Remove();
                    }
                    dbcliente.Delete(Ins);
                }
                dbcliente.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearEdit()
        {
            bEdit = false;
            instanciasEdit = null;
            sOldFile = null;
            if(dbclienteEdit!=null)
                dbclienteEdit.Close();
            dbclienteEdit = null;
        }
        #endregion

        private bool ValidoDatos()
        {
            bool bRetorno = false;
            if (Libreria.FuncionesCadena.IsNumeric(txtPort.Text))
                bRetorno = true;
            foreach (Control cn in this.Controls)
            {
                if (cn.GetType().Name == "TextBox")
                {
                    if (((TextBox)cn).Text.Trim().Length > 0)
                    {
                        bRetorno = false;
                        break;
                    }
                }
            }
            return bRetorno;
        }
        #endregion       
        
    }

    public class Instancia : Cliente
    {
        private bool _AutoInit;

        public bool AutoInit
        {
            set { _AutoInit = value; }
            get { return _AutoInit; }
        }
    }
}