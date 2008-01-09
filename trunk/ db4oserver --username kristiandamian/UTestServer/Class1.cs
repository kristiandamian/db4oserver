using System;
using System.Collections.Generic;
using System.Text;
using DB4OServer;
using Db4objects.Db4o;
using NUnit.Framework;

namespace UTestServer
{
    [TestFixture]
    public class Class1
    {
        private RunServer servidor;
        private Cliente cliente;

        private Cliente cliente2;

        [TestFixtureSetUp]
        public void Iniciar()
        {
            //Inicializo el cliente
            cliente = new Cliente();
            cliente.File = "file.yap";
            cliente.Port = "1234";
            cliente.User = "user1";
            cliente.Server = "localhost";
            cliente.Password = "password12";

            //Inicializo el cliente
            cliente2 = new Cliente();
            cliente2.File = "file2.yap";
            cliente2.Port = "1235";
            cliente2.User = "user1";
            cliente2.Server = "localhost";
            cliente2.Password = "password";
            /*
            //The server AUCH!!!
            servidor = new RunServer(cliente);
            servidor.Run();*/
        }

        [Test]
        public void AddDB()
        {
            IObjectContainer dbcliente = Db4oFactory.OpenClient(cliente.Server, Convert.ToInt32(cliente.Port),
                                                              cliente.User, cliente.Password);

            IObjectContainer dbcliente2 = Db4oFactory.OpenClient(cliente2.Server, Convert.ToInt32(cliente2.Port),
                                                              cliente2.User, cliente2.Password);
            //añado una prueba a la bd
            dbcliente.Set(new Prueba(1, "ABC", true));
            dbcliente2.Set(new Prueba(1, "ABC", true));
            dbcliente.Set(new Prueba(2, "DEF", false));
            dbcliente2.Set(new Prueba(2, "DEF", false));
            dbcliente.Set(new Prueba(3, "GHI", true));
            dbcliente2.Set(new Prueba(3, "GHI", true));
            dbcliente.Set(new Prueba(4, "JKM", false));
            dbcliente2.Set(new Prueba(4, "JKM", false));
            dbcliente.Set(new Prueba(5, "NÑO", true));
            dbcliente2.Set(new Prueba(5, "NÑO", true));
            dbcliente.Set(new Prueba(6, "PQR", false));
            dbcliente2.Set(new Prueba(6, "PQR", false));
            dbcliente.Commit();
            dbcliente2.Commit();
            dbcliente.Close();
            dbcliente2.Close();
            dbcliente.Dispose();
            dbcliente2.Dispose();
        }

        /*[Test]
        public void UpdateDB()
        {
            IObjectContainer dbcliente = Db4oFactory.OpenClient(cliente.Server, Convert.ToInt32(cliente.Port),
                                                              cliente.User, cliente.Password);
            IObjectSet objetitos=dbcliente.Get(new Prueba(1, "ABC", true));
            Prueba pupdate = (Prueba) objetitos.Next();
            pupdate.ValorCadena = "XYZ";
            dbcliente.Set(pupdate);
            dbcliente.Commit();
            objetitos.Reset();
            dbcliente.Close();
            dbcliente.Dispose();
        }

        [Test] //las devuelvo al estado inicial
        public void Update2DB()
        {
            IObjectContainer dbcliente = Db4oFactory.OpenClient(cliente.Server, Convert.ToInt32(cliente.Port),
                                                              cliente.User, cliente.Password);
            IObjectSet objetitos = dbcliente.Get(new Prueba(1, "XYZ", true));
            Prueba pupdate = (Prueba)objetitos.Next();
            pupdate.ValorCadena = "ABC";
            dbcliente.Set(pupdate);
            dbcliente.Commit();
            dbcliente.Close();
            dbcliente.Dispose();
        }*/
        /*
        [Test]
        public void DeleteDB()
        {
            IObjectContainer dbcliente = Db4oFactory.OpenClient(cliente.Server, Convert.ToInt32(cliente.Port),
                                                              cliente.User, cliente.Password);
            dbcliente.Set(new Prueba(99, "XYZ", true));
            dbcliente.Commit();
            IObjectSet objetitos = dbcliente.Get(new Prueba(99, "XYZ", true));
            Prueba pdelete = (Prueba)objetitos.Next();

            dbcliente.Delete(pdelete);
            dbcliente.Commit();
            dbcliente.Close();
            dbcliente.Dispose();
        }

        [Test]
        public void CleanDB()
        {
            IObjectContainer dbcliente = Db4oFactory.OpenClient(cliente.Server, Convert.ToInt32(cliente.Port),
                                                              cliente.User, cliente.Password);
            IObjectSet objetitos = dbcliente.Get(new Prueba());
            while (objetitos.HasNext())
            {
                Prueba pdelete = (Prueba)objetitos.Next();

                dbcliente.Delete(pdelete);                
            }
            dbcliente.Commit();
            dbcliente.Close();
            dbcliente.Dispose();
        }
        */
        [TestFixtureTearDown]
        public void Finalizar()
        {
            //servidor.Stop();
        }

    }

    public class Prueba
    {
        private int _ValorInt;
        private string _ValorString;
        private bool _ValorBool;

        public int ValorEntero
        {
            get { return _ValorInt; }
            set { _ValorInt = value; }
        }
        public string ValorCadena
        {
            get { return _ValorString; }
            set { _ValorString = value; }
        }
        public bool ValorBooleano
        {
            get { return _ValorBool; }
            set { _ValorBool = value; }
        }

        public Prueba(int Entero, string Cadena, bool Boleano)
        {
            _ValorInt = Entero;
            _ValorString = Cadena;
            _ValorBool = Boleano;
        }

        public Prueba()
        {
            //para devolver todos los objetos en db4o
        }

    }
}
