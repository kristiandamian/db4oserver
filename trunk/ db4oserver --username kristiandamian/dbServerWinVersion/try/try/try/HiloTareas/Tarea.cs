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
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace tryIcon.HiloTareas
{
    public enum ScheduleType { OnlyOnce, DaysOfWeek, SomeDays };
    public enum JobType { Defrag, Backup };
    public /*abstract*/ class Tarea:ITarea
    {
        #region "propiedades, variables, mensajes y ruta de archivo"
        private static string _InstancesFile = Application.StartupPath + Path.DirectorySeparatorChar + "InstancesFile.yap";

        private bool bWasCorrected;
        private bool bWasFailed;
        private bool bOnlyOnce;
        private bool bSomeDays;
        private bool bDaysOfWeek;
        private bool bDefrag;
        private bool bBackup;
        private string sJobName;
        private string sBackupFile;
        private Schedule Calendarizacion;
        
        protected Job _job;

        private Cliente _MyClient;
        public Cliente MyClient
        {
            set { _MyClient = value; }
            get { return _MyClient; }
        }

        public bool WasCorrected
        {
            get { return bWasCorrected; }
        }
        public bool WasFailed
        {
            get { return bWasFailed; }
        }
        public bool RunOnlyOnce
        {
            get { return bOnlyOnce; }
            set { bOnlyOnce = value; }
        }
        public bool RunSomeDays
        {
            get { return bSomeDays; }
            set { bSomeDays = value; }
        }
        public bool RunDaysOfWeek
        {
            get { return bDaysOfWeek; }
            set { bDaysOfWeek = value; }
        }
        public string JobName
        {
            get { return sJobName; }
            set { sJobName = value; }
        }
        public string NextExecutionDate
        {
            get { return Calendarizacion.NextExecutionDate; }
        }
        public string NextExecutionHour
        {
            get { return Calendarizacion.NextExecutionHour; }
        }
        public string BackupFile
        {
            set { sBackupFile = value; }
            get { return sBackupFile; }
        }
        #endregion
        /// <summary>
        /// Ejecuta la tarea programada
        /// </summary>
        /// <returns>true si se ejecuto correctamente, falso en caso contrario</returns>
        public bool Run()
        {
            bool bRetorno = false;
            try
            {
                CheckUpdateClient();
                if (bBackup)
                    ((Backup)_job).BackupFile = sBackupFile;
                _job.MyClient = _MyClient;//Update porsilas
                _job.RunJob();
                Calendarizacion.MoveExecutionTime();//avanzo al siguiente periodo (SOLO SI APLICA)
                bRetorno = bWasCorrected = true;
            }
            catch (Exception ex)
            {
                Log.AddToLog(sJobName, ex.Message);
                bWasFailed = false;
            }
            return bRetorno;
        }
        /// <summary>
        /// Verifico que los datos del cliente esten actualizados para evitar problemas de
        /// latencia en los bonitos datos
        /// </summary>
        private void CheckUpdateClient()
        {
            try
            {
                IObjectContainer dbcliente = Db4oFactory.OpenFile(_InstancesFile);
                IList<Cliente> instancias = dbcliente.Query<Cliente>(delegate(Cliente instancia)
                 {

                     return instancia.File == _MyClient.File &&
                            instancia.Server==_MyClient.Server;
                 });                
                if (instancias.Count > 0)
                {
                    foreach (Cliente Ins in instancias)
                    {//Solo deberia haber uno, pero por si las dudas tomo el ultimo
                        _MyClient = Ins;
                    }
                }
                dbcliente.Close();
            }
            catch (Exception ex)
            {
                Log.AddToLog(sJobName, ex.Message);
            }
        }

        /// <summary>
        /// Constructor para devolver todos los objetos en la BD DB4O
        /// </summary>
        public Tarea()
        {
        }

        public Tarea(ScheduleType sche, JobType _jobType, Cliente _cliente)
        {
            EnumToScheduleBool(sche);//defino la calendarizacion
            EnumToJobBool(_jobType);//defino el tipo
            _MyClient = _cliente;//necesario para la creacion del objeto _job
            //TODO quitar esto de aqui, no puede ir en el constructor
            //i'm still thinking that shiet up...
            AbstractFactoryTarea CuandoCalendarizo = null;
            if (Calendarizacion == null)
            {
                if(bOnlyOnce)
                    CuandoCalendarizo=new ConcreteFactoryOnlyOnce();
                if(bDaysOfWeek)
                    CuandoCalendarizo = new ConcreteFactoryDayOfWeek();
                if(bSomeDays)
                    CuandoCalendarizo = new ConcreteFactorySomeDays(); ;
                Calendarizacion = CuandoCalendarizo.CrearSchedule();
            }
            AbstractFactoryJob JobFactory=null;
            if(_job==null)
            {
                if (bDefrag)
                    JobFactory = new ConcreteFactoryDefrag();
                if (bBackup)
                    JobFactory = new ConcreteFactoryBackup();
                _job = JobFactory.CreateJob(_MyClient);
            }
            _job.JobName = sJobName;
        }
        /// <summary>
        /// Asigno los valores para determinar el tipo de los objetos correspondientes
        /// </summary>
        /// <param name="sche">Enum para saber que tipo de calendarizacion tiene el Job</param>
        private void EnumToScheduleBool(ScheduleType sche)
        {
            switch ((int)sche)
            {
                case 0:
                    bOnlyOnce = true;
                    bDaysOfWeek = !bOnlyOnce;
                    bSomeDays = !bOnlyOnce;
                    break;
                case 1:
                    bDaysOfWeek = true;
                    bOnlyOnce = !bDaysOfWeek;
                    bSomeDays = !bDaysOfWeek;
                    break;
                case 2:
                    bSomeDays = true;
                    bOnlyOnce = !bSomeDays;
                    bDaysOfWeek = !bSomeDays;
                    break;
            }
        }
        /// <summary>
        /// Asigno los valores para determinar el tipo de los objetos correspondientes
        /// </summary>
        /// <param name="_jobType">Enum para saber que tipo de trabajo es</param>
        private void EnumToJobBool(JobType _jobType)
        {
            switch ((int)_jobType)
            {
                case 0:
                    bDefrag = true;
                    bBackup = !bDefrag;
                    break;
                case 1:
                    bDefrag = false;
                    bBackup = !bDefrag;
                    break;
            }
        }

        /// <summary>
        /// Inicializo la primera ejecucion del job
        /// </summary>
        /// <param name="ExecTime">Fecha de la primera ejecucion (APLICA PARA LOS 3 tipos de calendarizacion)</param>
        /// <param name="iDays">Numero de dias a ejecutar (APLICA SOLO PARA EJECUTAR CADA N DIAS)</param>
        /// <param name="Week">Arreglo de dias que se ejecutara la tarea (APLICA SOLO PARA DIAS DE LA SEMANA)</param>
        /// <returns></returns>
        public bool InitDate(DateTime ExecTime,int iDays, bool[] Week)
        {
            bool bRetorno = false;
             if(bOnlyOnce)
                 bRetorno=InitDateOnlyOnce(ExecTime);
            if (bDaysOfWeek)
                bRetorno=InitDateDaysOfWeek(ExecTime, Week);
             if(bSomeDays)
                 bRetorno=InitDateSomeDays(ExecTime, iDays);
             return bRetorno;
        }
        /// <summary>
        /// definicion de la primera fecha de ejecución de la tarea
        /// OJO queda en manos del programado saber CUANDO llamar esta función
        /// </summary>
        /// <returns>True si no ocurrieron errores y era clase del tipo indicado</returns>
        private bool InitDateOnlyOnce(DateTime ExecTime)
        {          
            bool bRetorno=false;
            if (Calendarizacion.GetType() == typeof(OnlyOnce).GetType())
            {
                Calendarizacion.DefineTime(ExecTime);
                bRetorno = true;
            }
            return bRetorno;
        }
        /// <summary>
        /// definicion de la primera fecha de ejecución de la tarea
        /// OJO queda en manos del programado saber CUANDO llamar esta función
        /// </summary>
        /// <returns>True si no ocurrieron errores y era clase del tipo indicado</returns>
        private bool InitDateSomeDays(DateTime ExecTime, int iDays)
        {
            bool bRetorno=false;
            if (Calendarizacion.GetType() == typeof(SomeDays).GetType())
            {
                Calendarizacion.DefineTime(ExecTime);
                ((SomeDays)Calendarizacion).Days = iDays;
                bRetorno = true;
            }
            return bRetorno;
        }
        /// <summary>
        /// definicion de la primera fecha de ejecución de la tarea
        /// OJO queda en manos del programado saber CUANDO llamar esta función
        /// </summary>
        /// <param name="ExecTime">Fecha en la que se ejecutara</param>
        /// <param name="Week">Arreglo booleano que representa que dias de la semana se ejecuta, donde 0 es Domingo y 6 es Lunes (igual que el enum DayOfWeek)</param>
        /// <returns>True si no ocurrieron errores y era clase del tipo indicado</returns>
        private bool InitDateDaysOfWeek(DateTime ExecTime, bool[] Week)
        {
            bool bRetorno = false;
            if (Calendarizacion.GetType() == typeof(SomeDaysOfWeeks).GetType())
            {
                Calendarizacion.DefineTime(ExecTime);
                ((SomeDaysOfWeeks)Calendarizacion).Week = Week;
                bRetorno = true;
            }
            return bRetorno;
        }
    }

    #region "Abstrac factory de tarea"
    public interface ITarea 
    {
        bool WasCorrected { get;}
        bool WasFailed { get;}
        bool RunOnlyOnce { get;set;}
        bool RunSomeDays { get;set;}
        bool RunDaysOfWeek { get;set;}
        bool Run();
    }

    //Fabricacion del tipo de calendarización de la tarea
    public abstract class AbstractFactoryTarea
    {
        public abstract Schedule CrearSchedule();
    }
    public class ConcreteFactoryOnlyOnce : AbstractFactoryTarea
    {
        public override Schedule CrearSchedule()
        {
            return new OnlyOnce();
        }
    }
    public class ConcreteFactoryDayOfWeek : AbstractFactoryTarea
    {
        public override Schedule CrearSchedule()
        {
            return new SomeDaysOfWeeks();
        }
    }
    public class ConcreteFactorySomeDays : AbstractFactoryTarea
    {
        public override Schedule CrearSchedule()
        {
            return new SomeDays();
        }
    }
    #endregion
}
