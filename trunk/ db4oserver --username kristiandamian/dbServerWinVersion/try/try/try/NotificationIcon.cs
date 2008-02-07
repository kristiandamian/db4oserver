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
/*
 * Creado por SharpDevelop.
 * Creado por : kristian // Usuario: Administrador
 * Fecha: 20/12/2007
 * Hora: 09:27 a.m.
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Timers;
using System.Collections.Generic;//pa manejar tooodas mis instancias
using DB4OServer;

namespace tryIcon
{
	public sealed class NotificationIcon
	{
		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;

        private frmPrincipal frmprincipal;
        private Cliente MyClient;
        private List<Cliente> AllMyInstances;

        //private System.Windows.Forms.Timer _cron;

		#region Initialize icon and menu
		public NotificationIcon()
		{
			notifyIcon = new NotifyIcon();
			notificationMenu = new ContextMenu(InitializeMenu());
			
			notifyIcon.DoubleClick += IconDoubleClick;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));

            System.Reflection.Assembly p = System.Reflection.Assembly.GetExecutingAssembly();
            notifyIcon.Icon = (Icon)resources.GetObject("applications");//"$this.icon");
			notifyIcon.ContextMenu = notificationMenu;

            notifyIcon.Text = "Unknown state";
		}
		
		private MenuItem[] InitializeMenu()
		{
			MenuItem[] menu = new MenuItem[] {
                new MenuItem("Show", menuShowClick),
                new MenuItem("-"),
				new MenuItem("About", menuAboutClick),                
				new MenuItem("Exit", menuExitClick)
			};
			return menu;
		}
		#endregion
		
		#region Main - Program entry point
		/// <summary>Program entry point.</summary>
		/// <param name="args">Command Line Arguments</param>        
		[STAThread]        
		public static void Main(string[] args)
		{
            string sMensajeAlreadyRunning = "Ya se encuentra corriendo otra instancia de este programa";
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			
			bool isFirstInstance;
			// Please use a unique name for the mutex to prevent conflicts with other programs
            using (Mutex mtx = new Mutex(true, "tryIcon", out isFirstInstance))
            {
				if (isFirstInstance) {
					NotificationIcon notificationIcon = new NotificationIcon();
					notificationIcon.notifyIcon.Visible = true;
                    frmPrincipal.BeginAllInstances();
                    //Cronometro pa las tareas
                    System.Timers.Timer _cron=null;
                    if (_cron == null)
                    {
                        
                        //Un segundo*Los segundos de un minuto*
                        //Los minutos de una hora*cada 1 hora
                        Double _intervalo = 1000 * 60 * 60 * 1;//deje el uno pa cambiar mas rapido en caso de hacerlo mas a lo largooooo
                        _cron = new System.Timers.Timer(_intervalo);
                        _cron.Elapsed += new ElapsedEventHandler(_cron_Tick);
                        _cron.AutoReset = true;
                        _cron.Start();
                    }
					Application.Run();                    
                    notificationIcon.notifyIcon.Dispose();
				} else {
					// The application is already running
					// TODO: Display message box or change focus to existing application instance
                    MessageBox.Show(sMensajeAlreadyRunning);
                    Application.Exit();
				}
			} // releases the Mutex
		}

        static void _cron_Tick(object sender, EventArgs e)
        {
            try
            {
                IList<tryIcon.HiloTareas.Tarea> _tareas = SearchJobs.Search(DateTime.Now);
                if (_tareas != null)//Hubo algo?
                {
                    foreach (tryIcon.HiloTareas.Tarea _tarea in _tareas)
                    {
                        _tarea.Run();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
		#endregion
		
		#region Event Handlers
        private void menuShowClick(object sender, EventArgs e)
		{
            LlamoPantallaPrincipal();
		}
        private void menuAboutClick(object sender, EventArgs e)
        {
            MessageBox.Show("KreationTech\n\rSome rights are wrong.\n\rReport bugs to dcastrok@homex.com.mx");
        }
		
		private void menuExitClick(object sender, EventArgs e)
		{//si se cierra la aplicacion se detiene el server
            if (frmprincipal != null)
            {                    
                    frmprincipal.StopServer();                    
            }
			Application.Exit();
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{          
            LlamoPantallaPrincipal();
		}

        private void LlamoPantallaPrincipal()
        {
            /*HiloTareas.frmAddJob p = new tryIcon.HiloTareas.frmAddJob();
            p.ShowDialog();*/
            
            if (frmprincipal == null || frmprincipal.IsDisposed)
            {
                frmprincipal = new frmPrincipal();
                frmprincipal.FormClosing += new FormClosingEventHandler(frmprincipal_FormClosing);                
            }
            else
                frmprincipal.Activate();
            if (MyClient != null)
                frmprincipal.MyClient = MyClient;
            if(AllMyInstances!=null)
                frmprincipal.AllTheInstances = AllMyInstances;//pa manejar tooodas mis instancias
            frmprincipal.Show();
        }

        private void frmprincipal_FormClosing(object sender, EventArgs e)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NotificationIcon));
            notifyIcon.Icon = (Icon)resources.GetObject("disconnected");
            notifyIcon.Text = "Server disconnected";            
            if (frmprincipal != null)
            {
                if (!frmprincipal.IsDisposed)
                {
                    MyClient = frmprincipal.MyClient;
                    AllMyInstances = frmprincipal.AllTheInstances;//pa manejar tooodas mis instancias
                    if (frmprincipal.IsRunning())
                    {                        
                        notifyIcon.Icon = (Icon)resources.GetObject("connected");
                        notifyIcon.Text = "Server connected";
                    }
                }
            }

        }

		#endregion
	}
}
