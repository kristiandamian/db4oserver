using System;
using DB4OServer;
using Db4objects.Db4o;
using Gtk;

namespace tryServer
{	
	public partial class frmUsers : Gtk.Window
	{
		private string sMessageError="Error al crear usuario";
		private string sMessageData="Error con los datos capturados, por favor verifique";
		
		public frmUsers() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
		}
		
		
        private Cliente _MiCliente;
        public Cliente MiCliente
        {
            get { return _MiCliente; }
            set { _MiCliente = value; }
        }
        
        public new void Show()
        {
        	base.Show();
        	if( _MiCliente!=null &&
        		_MiCliente.Server!=null &&
        		_MiCliente.File!=null)
        	{
        		lblServer.Text = _MiCliente.Server;
            	lblFile.Text = _MiCliente.File;
            }
        }
                
        private void Clear()
        {
        	txtUser.Text="";
        	txtPassword.Text="";
        }

        protected virtual void OnBtnOkClicked(object sender, System.EventArgs e)
        {
        	try
            {               
                if(ValidoDatos())
                {
                	Message(MiCliente.FileUsers);
	                IObjectContainer dbcliente = Db4oFactory.OpenFile(MiCliente.FileUsers);
	                Usuario user = new Usuario();
	                if (user.CreoUsuario(txtUser.Text, txtPassword.Text,lblServer.Text,lblFile.Text))
	                {
	                    dbcliente.Set(user);
	                    dbcliente.Commit();
	                }
	                else
	                {
	                	Message(sMessageError);	                   
	                }
	                dbcliente.Close();
	                Clear();
                }
                else
                	Message(sMessageData);
            }
            catch (Exception ex)
            {
           		Message(ex.Message);           
            }
        }

        protected virtual void OnBtnCancelClicked(object sender, System.EventArgs e)
        {
        	this.HideOnDelete();
        }
        
        private bool ValidoDatos()
        {
        	bool bRetorno=false;
        	if( lblFile.Text.Trim().Length>0 &&
        		lblServer.Text.Trim().Length>0 &&
        		txtUser.Text.Trim().Length>0 &&
        		txtPassword.Text.Trim().Length>0)
        		bRetorno=true;
        	return bRetorno;
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
}
