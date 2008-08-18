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
using System.Windows.Forms;
using System.IO;
using Db4objects.Db4o;
using tryIcon.HiloTareas;

namespace tryIcon
{
    public class Prueba
    {
        public void BusquedaSODA()
        {
            IObjectContainer dbcliente = Db4oFactory.OpenFile("File.yap");

            Db4objects.Db4o.Query.IQuery _Consulta = dbcliente.Query();
            _Consulta.Constrain(typeof(Foo));
            _Consulta.Descend("Numero").Constrain(21).Greater(); //Devuelve todos los objetos
                                                                //que son mayores a 21 en 'Numero'
            IObjectSet ConjuntoFoos = _Consulta.Execute();
            foreach (Foo _fooRes in ConjuntoFoos)        
            {
                _fooRes.Entero = 0;
                dbcliente.Set(_fooRes);//Actualizo el objeto
            }
            dbcliente.Close();
        }
    }


    public class Foo
    {
        private int val1;
        private string val2;

        public int Entero
        {
            set { val1 = value; }
            get { return val1; }
        }
        public string Cadena
        {
            set { val2 = value; }
            get { return val2; }
        }
    }

    public static class SearchJobs
    {
        private static string _JobsFile = Application.StartupPath + Path.DirectorySeparatorChar + "JobsFile.yap";
        private static string sMessage = "There was an error while trying to run a job ";//"Ocurrio un error cuando se intento ejecutar una tarea";


        public static IList<Tarea> Search(DateTime Time)
        {
            IList<Tarea> _jobs=null;
            try
            {
                IObjectContainer db = Db4oFactory.OpenFile(_JobsFile);

                _jobs = db.Query<Tarea>(delegate(Tarea _job)
                    {
                        return DateTime.Parse(Time.ToShortDateString()) >= 
                            DateTime.Parse(_job.NextExecutionDate) &&
                        DateTime.Parse(Time.ToShortTimeString()) >= 
                            DateTime.Parse(_job.NextExecutionHour);
                    });
                db.Close();
            }
            catch (Exception ex)
            {
                Log.AddToLog(sMessage+DateTime.Now.ToString(), ex.Message);
            }
            return _jobs;
        }
    }
}
