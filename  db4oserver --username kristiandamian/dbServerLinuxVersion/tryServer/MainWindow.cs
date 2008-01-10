using System;
using Gtk;
//this is my shiet
using System.Threading;
//using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.IO;
using DB4OServer;
using Db4objects.Db4o;
 
namespace tryServer
{
	public partial class MainWindow: Gtk.Window
	{	
		public MainWindow (): base (Gtk.WindowType.Toplevel)
		{
			Build ();	
			
			 cliente = new Cliente();
		     cliente.File = "file.yap";
	    	 cliente.Port = "1234";
		     cliente.User = "user1";
	    	 cliente.Server = "localhost";
	       	 cliente.Password = "password12";
	       
	       if (servidor == null)
	           servidor = new RunServer(cliente);
	       //para saner si tiene acceso o no a la bd.
	       servidor.SinAcceso += Excepcion;
	       
	       //
	       Offline();		
	       string sSeparador="/";//Path.DirectorySeparatorChar;
	       
	       _InstancesFile=Utility.RutaSinNombreArchivo(System.Reflection.Assembly.GetExecutingAssembly().Location) + sSeparador + "InstancesFile.yap";
	       _UsersFile=Utility.RutaSinNombreArchivo(System.Reflection.Assembly.GetExecutingAssembly().Location) + sSeparador + "users.yap";
		}	
				
#region "Mensajes del sistema"
        string sAccessMessage = "No tiene acceso a ese Servidor/Archivo";
        string sClientError = "Error con los datos de conexion";
#endregion
#region "Variables"
        private static string _InstancesFile;// = System.Reflection.Assembly.GetExecutingAssembly().Location + Path.DirectorySeparatorChar + "InstancesFile.yap";
        private string _UsersFile ;//= System.Reflection.Assembly.GetExecutingAssembly().Location + Path.DirectorySeparatorChar + "users.yap";
        private RunServer servidor;
        private Cliente cliente;
        private frmSettings config;
#endregion  
		public Cliente MyClient
	    {        		
			set { cliente = value; }
			get { return cliente; }
		} 
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			//Application.Quit ();	
	             	
			a.RetVal = true;		
			this.Destroy();			 			
		}


		protected virtual void OnBtnStartClicked(object sender, System.EventArgs e)
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
	                   Thread.Sleep(3000);
	                   //ReviewState();
	                   if (servidor.IsRunning())
	                       Online();
	                   else
	                       Offline();
	               }
	            }
	       }
	       catch (Exception ex)
	       {
	           Message(ex.Message);
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
	            {
	                Message(sClientError);
	            }
	            return bRetorno;
	    }
	    private void Excepcion()
	    {
	        Message(sAccessMessage);
	    }
	    
		protected virtual void OnBtnStopClicked(object sender, System.EventArgs e)
		{
		 	StopServer();
		}
		public void StopServer()
	   	{
	       try
	       {
	           servidor.Stop();
	           Offline();
	       }       
	       catch (Exception ex)
	       {
		        Message(ex.Message);     
	       }
	   	}
		protected virtual void OnBtnSettingsClicked(object sender, System.EventArgs e)
		{
			//TODO
			if (config == null )
	       {
	           config = new frmSettings();
	           config.DeleteEvent+=new DeleteEventHandler(this.DeleteForm);
	           //config.FormClosing += new FormClosingEventHandler(config_FormClosing);
	       }
	       
	       config.MiCliente = cliente;
	       config.Show();
		}
		
		private void DeleteForm(object o,EventArgs args)
		{
		 	cliente = config.MiCliente;    
		 	cmbInstance.Clear();            
            if (cliente.Server != null)
            {
            	cmbInstance.InsertText(0,cliente.Server);
	            cmbInstance.Active=0;
            }
            if (IsRunning())
                Online();
            else
                Offline();
			config=null;
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

		///Funcion que ejecuta al Mostrarse la forma
		///Sobreescribo la funcion base porque no encontre un evento
		///OnLoado o algo similar	
		public new void Show()
		{		
			base.Show();
			
	       //GetAllInstances();
	       if (cliente!=null && cliente.Server != null)
	       {	       		
	           cmbInstance.InsertText(0,cliente.Server);
	           cmbInstance.Active=0;	           	           
	       }	       
	       if (ValidoDatosCliente(false))
	       {
	           if (servidor.IsRunning())
	           {
	               Online();
	           }
	       }
		}
		
		private void Message(string sMensaje)
        {
        	MessageDialog md=new MessageDialog(this,
	          							DialogFlags.DestroyWithParent,
	           							MessageType.Error,
	           							ButtonsType.Close,
	           							sMensaje);
	       	md.Run();
	        md.Destroy();
	    }	     
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
