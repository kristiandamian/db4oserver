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
using System.Text;
using DB4OServer;
using Db4objects.Db4o;
using Db4objects.Db4o.Defragment;
using System.Threading;

namespace tryIcon.HiloTareas
{
    public abstract class Job
    {
        #region "propiedades y variables"
        public string Server
        {
            get { return sServer; }
            set { sServer = value; }
        }
        public string Port
        {
            get { return sPort; }
            set { sPort = value; }
        }
        public string User
        {
            get { return sUsuario; }
            set { sUsuario = value; }
        }
        public string Password
        {
            get { return sPassword; }
            set { sPassword = value; }//no lo encripto porque lo tomare de un objeto Cliente 
        }
        public string File
        {
            get { return sFile; }
            set { sFile = value; }
        }
        public string JobName
        {
            set { sJobName = value; }
        }
        protected string sServer;
        protected string sPort;
        protected string sUsuario;
        protected string sPassword;
        protected string sFile;
        protected string sJobName;
        private Cliente _MyClient;
        public Cliente MyClient
        {
            set
            {
                _MyClient = value;
                UpdateValues();
            }
            get { return _MyClient; }
        }

        protected DB4OServer.RunServer servidor;
        #endregion
        
        public Job(Cliente cliente)
        {
            servidor = new RunServer(cliente);
            _MyClient = cliente;
            UpdateValues();
        }

        private void UpdateValues()
        {
            this.Server = _MyClient.Server;
            this.Port = _MyClient.Port;
            this.User = _MyClient.User;
            this.File = _MyClient.File;
            servidor.Client = _MyClient;
        }

        public abstract bool RunJob();
        
    }

    public class Defrag : Job
    {
        private Thread hilo;
        private bool bServerWasRunning ;
        //TODO añadir todo al log

        public Defrag(Cliente cliente)
            :base(cliente)
        {
            
        }
        public override bool RunJob()
        {
            bool bError = false;
            hilo=new Thread(new ThreadStart(RunningDefrag));
            bServerWasRunning = false;
            try
            {
                if (servidor.IsRunning())
                {
                    bServerWasRunning = true;
                    if (!servidor.Stop())
                    {
                        bError = true;
                    }
                }
                if (!bError)
                {
                    hilo.Start();                   
                }                        
            }
            catch (Exception)
            {
                bError = true;
                //TODO add al log
            }
            return bError;
        }
        /// <summary>
        /// Ejecuto el hilo que defragmentara la base de datos
        /// </summary>
        private void RunningDefrag()
        {
            try
            {                
                Defragment.Defrag(base.sFile);
                if (bServerWasRunning)
                    servidor.Run();
            }
            catch (Exception ex)
            {
                Log.AddToLog(sJobName, ex.Message);
            }
        }
    }


    public class Backup : Job
    {
        public Backup(Cliente cliente)
            : base(cliente)
        {
            
        }
        private string sBackupFile;
        public string BackupFile
        {
            set { sBackupFile = value; }
            get { return sBackupFile; }
        }

        public override bool RunJob()
        {
            bool bError = false;
            try
            {
                if (servidor.IsRunning())
                {
                    IObjectContainer dbcliente = Db4oFactory.OpenClient(this.sServer, Convert.ToInt32(this.sPort),
                                                  this.sUsuario, this.sPassword);

                    dbcliente.Ext().Backup(sBackupFile);
                    dbcliente.Close();
                }
            }
            catch (Exception ex)
            {
                bError = true;
                Log.AddToLog(sJobName, ex.Message);
            }
            return bError;
        }
    }

    #region "abstract factory de job"
    //Fabricacion del tipo de la tarea
    public abstract class AbstractFactoryJob
    {
        public abstract Job CreateJob(Cliente cliente);
    }
    public class ConcreteFactoryDefrag : AbstractFactoryJob
    {
        public override Job CreateJob(Cliente cliente)
        {
            return new Defrag(cliente);
        }
    }
    public class ConcreteFactoryBackup : AbstractFactoryJob
    {
        public override Job CreateJob(Cliente cliente)
        {
            return new Backup(cliente);
        }
    }
    #endregion
     
}
