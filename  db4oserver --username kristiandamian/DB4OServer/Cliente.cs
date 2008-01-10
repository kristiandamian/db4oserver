using System;
using System.Collections.Generic;
using System.Text;

namespace DB4OServer
{
    [Serializable]
    public class Cliente
    {
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
            set { sPassword = EncriptoPassword(value); }
        }
        public string File
        {
            get { return sFile; }
            set { sFile = value; }
        }
        public string FileUsers
        {
            get { return sFileUsers; }
            set { sFileUsers = value; }
        }
        private string sServer;
        private string sPort;
        private string sUsuario;
        private string sPassword;
        private string sFile;
        private string sFileUsers;

        private string EncriptoPassword(string valor)
        {            
            return EncriptacionAsimetrica.EncriptacionHash.Encriptar(valor);
        }
    }
}
