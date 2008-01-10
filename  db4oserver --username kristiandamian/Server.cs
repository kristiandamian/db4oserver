using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using Db4objects.Db4o;
using Db4objects.Db4o.Messaging;

namespace DB4OServer
{
    
    public class RunServer : IMessageRecipient
    {
        private Cliente MiCliente;
        private bool stop = false;
        //private string sAccessMessage = "No tiene acceso a ese Servidor/Archivo";
        private Publisher pub = new Publisher();

        //Propiedades
        public Cliente Client
        {
            set { MiCliente = value; }
            get { return MiCliente; }
        }
        //evento
        public delegate void UpdateEventHandler();
        public event UpdateEventHandler SinAcceso;
        
        public RunServer(Cliente cliente)
        {
            MiCliente = cliente;
        }
        
        public void Run()
        {
            try
            {           
                if(pub==null)
                    pub = new Publisher();                
                pub.RaiseCustomEvent+=pub_RaiseCustomEvent;
                 
                stop = false;
                Thread hilo = new Thread(new ThreadStart(InitServer));
                hilo.Start();
            }
            catch (Exception )
            {
            }
        }

        void pub_RaiseCustomEvent(object sender, NoAcceso e)
        {
            if(this.SinAcceso!=null)
                this.SinAcceso();
            pub.Dispose();
            pub.RaiseCustomEvent-= pub_RaiseCustomEvent;
            pub = null;            
        }

        private void InitServer()
        {
            lock (this)
            {               
                IObjectServer server = Db4oFactory.OpenServer(MiCliente.File, Convert.ToInt32(MiCliente.Port));
            
                if (ValidoUsuario())
                {
                    AgregoUsuarios(ref server);
                    server.Ext().Configure().ClientServer().SetMessageRecipient(this);
                    try
                    {
                        while (!stop)
                        {
                            Monitor.Wait(this);/*wait 60000 ??*/
                        }
                    }
                    catch (ThreadInterruptedException )
                    {
                        server.Close();
                        stop = true;
                    }
                    finally
                    {
                        server.Close();
                        stop = true;
                    }
                }
                else
                {
                    server.Close();
                    stop = true;
                    pub.SinAcceso();                    
                }                
            }
        }

        private bool ValidoUsuario()
        {
            bool bRetorno = false;            
            try
            {
                if (MiCliente.FileUsers != null)
                {
                    IObjectContainer dbcliente = Db4oFactory.OpenFile(MiCliente.FileUsers);
                    IObjectSet objetitos = dbcliente.Get(new Usuario(MiCliente.User, MiCliente.Password,
                                                                     MiCliente.Server, MiCliente.File));

                    if (objetitos.Count > 0)
                    {
                        bRetorno = true;
                    }
                    dbcliente.Close();
                }
            }
            catch (Exception )
            {                
                //Publisher LanzarEvento = new Publisher();
                pub.SinAcceso();                
            }
            return bRetorno;
        }

        private void AgregoUsuarios(ref IObjectServer server)
        {
            
            string sMessage = "Error al accesar a ese Servidor/Archivo";
            try
            {
                IObjectContainer dbcliente = Db4oFactory.OpenFile(MiCliente.FileUsers);
                IObjectSet objetitos = dbcliente.Get(new Usuario()); //todos los usuarios

                while (objetitos.HasNext())
                {
                    Usuario _miniUser = (Usuario)objetitos.Next();
                    server.GrantAccess(_miniUser.User, _miniUser.Password);
                }
                dbcliente.Close();
            }
            catch (Exception )
            {
                throw new Exception(sMessage);
            }
        }

        public void Stop()
        {
            IObjectContainer Contenedor = null;
            try
            {
                Contenedor = Db4oFactory.OpenClient(MiCliente.Server, Convert.ToInt32(MiCliente.Port), MiCliente.User, MiCliente.Password);
                if (Contenedor != null)
                {
                    IMessageSender messageSender = Contenedor.Ext().Configure().ClientServer()
                                                                   .GetMessageSender();
                    //envio el mensaje
                    messageSender.Send(new StopServer(""));
                    //cierro el contenedor                
                    if (Contenedor != null)
                        Contenedor.Close();
                    Contenedor.Dispose();
                }
            }
            catch (Exception )
            {
                //Console.WriteLine(e.ToString());
            }
        }

        public bool IsRunning()
        {
            bool bRetorno = false;
            try
            {
                //Intento abrir el archivo para ver si esta bloqueado o no sin hacer llamadas a
                // FILE porque no se si funcionara en Linux
                IObjectServer server = Db4oFactory.OpenServer(MiCliente.File, Convert.ToInt32(MiCliente.Port));
                server.Close();
            }
            catch (DatabaseFileLockedException )
            {
                bRetorno = true;
            }            
            return bRetorno;
        }
        
        public void ProcessMessage(IObjectContainer con, Object message)
        {            
            lock (this)
            {
                if (message is StopServer)
                {                    
                    stop = true;
                    Monitor.PulseAll(this);
                }
            }
        }
    }

    class StopServer
    {
        private string _info;
        public StopServer(string info)
        {
            _info = info;
        }
        public override string ToString()
        {
            return _info;
        }
    }

    class NoAcceso : EventArgs
    {
        public NoAcceso(string s)
        {
            message = s;
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }
    class Publisher : IDisposable
    {
        private IntPtr handle;
        private Component Components=new Component();
        private bool disposed = false;
        public event EventHandler<NoAcceso> RaiseCustomEvent;
        private string sAccessMessage = "No tiene acceso a ese Servidor/Archivo";
        public void SinAcceso()
        {
            OnRaiseCustomEvent(new NoAcceso(sAccessMessage));
        }        
        protected virtual void OnRaiseCustomEvent(NoAcceso e)
        {            
            EventHandler<NoAcceso> handler = RaiseCustomEvent;
            
            if (handler != null)
            {                
                e.Message += sAccessMessage;
                                
                handler(this, e);
            }
        }

        #region "para hacer el disposed"
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Components.Dispose();
                }
                //CloseHandle(handle);
                handle = IntPtr.Zero;

            }
            disposed = true;
        }
        //destructor
        ~Publisher()      
       {
          Dispose(false);
       }
        #endregion
    }
}