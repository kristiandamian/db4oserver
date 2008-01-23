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

namespace tryIcon.HiloTareas
{
    /// <summary>
    /// En esta clase se define cuale es la periodicidad de la tarea y cuando se ejecutara
    /// </summary>
    public abstract class Schedule : ISchedule
    {
        protected DateTime dtNextExecutionTime;
        protected bool bOnlyRunOnce;
        protected int iEvery;
        /// <summary>
        /// Representa cada cuando se ejecuta el proceso (cada cuatos dias, meses o semanas)
        /// </summary>
        public int Every
        {
            get { return iEvery; }
            set { iEvery = value; }
        }
        public string NextExecutionDate
        {
            get { return dtNextExecutionTime.ToShortDateString(); }
        }
        public string NextExecutionHour
        {
            get { return dtNextExecutionTime.ToShortTimeString(); }
        }
        public bool OnlyRunOnce
        {
            get { return bOnlyRunOnce; }
            set { bOnlyRunOnce = value; }
        }

        public abstract void MoveExecutionTime();
        //define cuando se ejecutara
        public abstract void DefineTime(DateTime ExeTime);
    }

    public interface ISchedule
    {
        string NextExecutionDate { get;}
        string NextExecutionHour { get;}
        bool OnlyRunOnce { get;set;}
        /// <summary>
        /// Actualiza el tiempo de ejecucion al siguient dependiendo de la programación
        /// de la tarea
        /// </summary>
        void MoveExecutionTime();
        void DefineTime(DateTime ExeTime);
    }

    public class OnlyOnce : Schedule
    {
        public OnlyOnce()
        {
            bOnlyRunOnce = true;
        }

        //define cuando se ejecutara
        public override void DefineTime(DateTime ExeTime)
        {
            dtNextExecutionTime = ExeTime;
        }
        
        public override void MoveExecutionTime()
        {//Esta tarea ya no debe ejcutarse jamas, nunca de los nunca, never de los never
            dtNextExecutionTime=DateTime.Parse( "0000-00-00 00:00:00");//no se puede guardar como null
        }
    }
    public class SomeDaysOfWeeks : Schedule
    {
        #region "propiedades y metodos"
        private bool[] bWeek;
        private bool bMonday;
        private bool bTuesday;
        private bool bWednesday;
        private bool bThursday;
        private bool bFriday;
        private bool bSaturday;
        private bool bSunday;
        /// <summary>
        /// Esta tarea ¿se ejecuta el Lunes?
        /// </summary>
        public bool Monday
        {
            get { return bMonday; }
            set { bMonday = value; }
        }
        /// <summary>
        /// Esta tarea ¿se ejecuta el Martes?
        /// </summary>
        public bool Tuesday
        {
            get { return bTuesday; }
            set { bTuesday = value; }
        }
        /// <summary>
        /// Esta tarea ¿se ejecuta el Miercoles?
        /// </summary>
        public bool Wednesday
        {
            get { return bWednesday; }
            set { bWednesday = value; }
        }
        /// <summary>
        /// Esta tarea ¿se ejecuta el Jueves?
        /// </summary>
        public bool Thursday
        {
            get { return bThursday; }
            set { bThursday = value; }
        }
        /// <summary>
        /// Esta tarea ¿se ejecuta el Viernes?
        /// </summary>
        public bool Friday
        {
            get { return bFriday; }
            set { bFriday = value; }
        }
        /// <summary>
        /// Esta tarea ¿se ejecuta el Sabado?
        /// </summary>
        public bool Saturday
        {
            get { return bSaturday; }
            set { bSaturday = value; }
        }
        /// <summary>
        /// Esta tarea ¿se ejecuta el Domingo?
        /// </summary>
        public bool Sunday
        {
            get { return bSunday; }
            set { bSunday = value; }
        }
        public bool[] Week
        {
            set { InitValues(value); }
        }
        #endregion
        private string sNumItemsWrong="Cantidad de elementos incorrecta";
        private void InitValues(bool[] bSemana )
        {
            if(bWeek==null)
                bWeek=new bool[7];
            if(bSemana.Length>7)
                throw new Exception(sNumItemsWrong);
            else
            {
                bWeek=bSemana;
                for (int x = 0; x <= 6; x++)
                {
                    InitVar(x, bSemana[x]);
                }
            }
        }
        private void InitVar(int Day, bool bValue)
        {
            switch (Day)
            {
                case 0:
                    bSunday = bValue;
                    break;
                case 1:
                    bMonday = bValue;
                    break;
                case 2:
                    bTuesday = bValue;
                    break;
                case 3:
                    bWednesday = bValue;
                    break;
                case 4:
                    bThursday = bValue;
                    break;
                case 5:
                    bFriday = bValue;
                    break;
                case 6:
                    bSaturday = bValue;
                    break;

            }
        }
        public SomeDaysOfWeeks()
        {
            bOnlyRunOnce = false;
            bWeek = new bool[7];
        }

        public override void MoveExecutionTime()
        {          
            int SeEjecuto=(int)dtNextExecutionTime.DayOfWeek;
            SeEjecuto++;
            //se supone que ya se ejecuto, entonces me fijo que dia se ejecuto
            for (int x = 0; x <= 6; x++) //all the week men!!
            {//comparo todos, Empezando un dias mas DEL DIA EN QUE ESTOY EJECUTANDO
                //Me paso del rango?
                if (SeEjecuto + x > 6)
                    SeEjecuto = 0;//comienzo del inicio!!
                if (bWeek[SeEjecuto])
                {
                    dtNextExecutionTime.AddDays(x+1);
                    break;
                }
                SeEjecuto++;
            }
        }
        public override void DefineTime(DateTime ExeTime)
        {
            dtNextExecutionTime = ExeTime;
        }
    }
    public class SomeDays : Schedule
    {
        private int iDays;
        /// <summary>
        /// Define cada cuandos dias se ejcuta la tarea, por ejemplo cada 10 dias, o cada 15
        /// </summary>
        public int Days
        {
            set { iDays = value; }
            get { return iDays; }
        }
        public SomeDays()
        {
            bOnlyRunOnce = false;
        }

        public override void MoveExecutionTime()
        {
            dtNextExecutionTime.AddDays(iDays);
        }
        public override void DefineTime(DateTime ExeTime)
        {
            dtNextExecutionTime = ExeTime;
        }
    }
}
