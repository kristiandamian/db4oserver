using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Db4objects.Db4o;

namespace tryIcon.HiloTareas
{
    public static class Log
    {
        private static string _LogFile = Application.StartupPath + Path.DirectorySeparatorChar + "Log.yap";

        /// <summary>
        /// Agrego al log un Mensaje
        /// </summary>
        /// <param name="ID">El nombre de la tarea</param>
        /// <param name="Message">El error</param>
        public static void AddToLog(string ID,string Message)
        {
            try
            {
                Mensaje mess = new Mensaje();
                mess.ID = ID;
                mess.Message = Message;
                IObjectContainer dbCliente = Db4oFactory.OpenFile(_LogFile);
                dbCliente.Set(mess);
                dbCliente.Close();
            }
            catch (Exception)
            {//Que hago? lo remeto al log??
            }
        }
        /// <summary>
        /// Devuelvo todos los mensajes de una tarea
        /// </summary>
        /// <param name="ID">EL nombre de la tarea</param>
        /// <returns>los mensajes de esa tarea</returns>
        public static IList<string> AllLog(string ID)
        {
            IList<string> sRetorno = null;
            try
            {
                IObjectContainer dbCliente = Db4oFactory.OpenFile(_LogFile);
                IList<Mensaje> Mensajes = dbCliente.Query<Mensaje>(delegate(Mensaje mensaje)
                    {
                        return mensaje.ID == ID;
                    });
                foreach (Mensaje _miniMensaje in Mensajes)
                {
                    sRetorno.Add(_miniMensaje.OneLineMessage);
                }
            }
            catch (Exception)
            {
            }
            return sRetorno;
        }
    }

    public class Mensaje
    {
        private string sMensaje;
        private string sID;
        private DateTime dtDate;

        public string Message
        {
            set { sMensaje = value; }
            get { return sMensaje; }
        }
        public string ID
        {
            set { sID = value; }
            get { return sID; }
        }

        public string OneLineMessage
        {
            get
            {
                return dtDate.ToShortDateString() + " " +
                       dtDate.ToShortTimeString() + "-" + sMensaje;
            } 
        }

        public Mensaje()
        {
            dtDate = DateTime.Now;
        }
    }
}
