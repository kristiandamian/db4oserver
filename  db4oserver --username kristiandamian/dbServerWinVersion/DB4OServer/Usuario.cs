using System;
using System.Collections.Generic;
using System.Text;

namespace DB4OServer
{
    public class Usuario
    {
        private string _sUsuario;
        private string _sPassword;
        private int _MinPasswordLength = 10;
        private string _sServer;
        private string _sFile;

        public Usuario()
        {
        }

        public Usuario(string sUsuario, string sPassword, string sServer, string sFile)
        {
            _sUsuario = sUsuario;
            _sPassword = sPassword;
            _sServer = sServer;
            _sFile = sFile;
        }

        public string User
        {
            set { _sUsuario = value; }
            get { return _sUsuario; }
        }
        public string Password
        {
            get { return _sPassword; }
        }

        public bool CreoUsuario(string sUsuario, string sPassword, string sServer, string sFile)
        {
            bool bRetorno = false;
            if (sPassword.Trim().Length > 0 && PasswordSettings(sPassword))
            {
                _sUsuario = sUsuario;
                _sPassword = EncriptacionAsimetrica.EncriptacionHash.Encriptar(sPassword);
                _sServer = sServer;
                _sFile = sFile;
                bRetorno = true;
            }
            return bRetorno;
        }

        public bool ChangePassword(string sPassword)
        {
            bool bRetorno = false;
            if (sPassword.Trim().Length > 0 && PasswordSettings(sPassword))
            {
                _sPassword = EncriptacionAsimetrica.EncriptacionHash.Encriptar(sPassword);
                bRetorno = true;
            }
            return bRetorno;
        }

        private bool PasswordSettings(string sPassword)
        {
            bool bRetorno = false;
            if (sPassword.Length >= _MinPasswordLength)
            {
                bRetorno = true;
                //TODO regex con Mayusculas, minusculas y numeros
            }
            return bRetorno;
        }
    }
}
