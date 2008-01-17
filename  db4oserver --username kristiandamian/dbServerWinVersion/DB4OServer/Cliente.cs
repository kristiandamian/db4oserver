/*  This file is part of db4oserver Class

    db4oserver Class is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    db4oserver Class is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Text;

namespace DB4OServer
{
   // [Serializable]
    public class Cliente:ICloneable
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

        private  string EncriptoPassword(string valor)
        {            
            return EncriptacionAsimetrica.EncriptacionHash.Encriptar(valor);
        }

        public Object Clone()
        {
            Cliente CloneClient=new Cliente();
            CloneClient.File = this.File;
            CloneClient.FileUsers = this.FileUsers;
            CloneClient.sPassword = this.sPassword;//evito volver a encriptar
            CloneClient.Port = this.Port;
            CloneClient.Server = this.Server;
            CloneClient.User = this.User;
            return CloneClient;
        }
    }
}
