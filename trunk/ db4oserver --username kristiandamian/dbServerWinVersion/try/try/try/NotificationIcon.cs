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
using DB4OServer;

namespace tryIcon
{
	public sealed class NotificationIcon
	{
		private NotifyIcon notifyIcon;
		private ContextMenu notificationMenu;

        private frmPrincipal frmprincipal;
        private Cliente MyClient;
		
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
		#endregion
		
		#region Event Handlers
		private void menuAboutClick(object sender, EventArgs e)
		{
			MessageBox.Show("KreationTech\n\rSome rights are wrong.\n\rReporting bugs to dcastrok@homex.com.mx");
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
            if (frmprincipal == null || frmprincipal.IsDisposed)
            {
                frmprincipal = new frmPrincipal();
                frmprincipal.FormClosing += new FormClosingEventHandler(frmprincipal_FormClosing);
            }
            else
                frmprincipal.Activate();
            if (MyClient != null)
                frmprincipal.MyClient = MyClient;
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
