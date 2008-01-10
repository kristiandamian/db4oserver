using System;
using System.IO;
using Gtk;
using DB4OServer;

namespace tryServer
{
	public partial class frmSettings : Gtk.Dialog
	{	
		public frmSettings()
		{
			this.Build();
			
		}
		
 		private frmUsers usuarios;
        private Cliente _MiCliente;
        public Cliente MiCliente
        {
            get {return _MiCliente;}
            set {_MiCliente=value;}
        }
	
		protected virtual void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
			args.RetVal=true;
			this.Destroy();
		}

		protected virtual void OnButton30Clicked(object sender, System.EventArgs e)
		{ 
			MiCliente.Server = txtServer.Text;
            MiCliente.Port = txtPort.Text;
            MiCliente.File = txtFile.Text;
            MiCliente.User = txtUser.Text;
            MiCliente.Password = txtPassword.Text;
            MiCliente.FileUsers = txtFileUsers.Text;
    		//Con esta madre es como hacerle click en la espantosa X
			this.HideOnDelete();			
		}

		protected virtual void OnBtnUsersClicked(object sender, System.EventArgs e)
		{
			if (txtFile.Text.Trim().Length > 0)
            {
				if(usuarios==null)
				{
					usuarios=new frmUsers();
					usuarios.DeleteEvent+=new DeleteEventHandler(this.DeleteForm);
				}				
				usuarios.MiCliente = _MiCliente;
				usuarios.Show();
			}				
		}
		
		private void DeleteForm(object o, EventArgs args)
		{
			usuarios=null;
		}
		
		public new void Show()
		{
			base.Show();
			if(MiCliente!=null)
			{	
				txtServer.Text=MiCliente.Server;
	            txtPort.Text = MiCliente.Port;
	            txtFile.Text = MiCliente.File;
	            txtUser.Text = MiCliente.User;	            
	            string separador="/";//Path.DirectorySeparatorChar.ToString();
	            //txtPassword.Text = MiCliente.Password;
	            if (MiCliente.FileUsers==null || MiCliente.FileUsers.Trim().Length == 0)
	            {
	                txtFileUsers.Text =  Utility.RutaSinNombreArchivo(System.Reflection.Assembly.GetExecutingAssembly().Location) +separador + "users.yap";
	                MiCliente.FileUsers=txtFileUsers.Text;
	            }
	            else
	                txtFileUsers.Text = MiCliente.FileUsers;
	         }
		}
		
	}
}

		
