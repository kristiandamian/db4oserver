// project created on 03/01/2008 at 17:02
using System;
using Gtk;

namespace tryServer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			Application.Init ();
			//MainWindow win = new MainWindow ();
			//win.Show ();			
			TrayTest test = new TrayTest();			
			Application.Run ();
		}
	}
	
	public class TrayTest
	{
		private EventBox eb;
		private MainWindow win;
		private Image Nel;
		private Image Simon;
		
		
		private TrayIcon icon;
		public  TrayTest()
		{
			/* in order to receive signals, we need a eventbox,
	                because Gtk.Image doesn't receive signals */
			eb = new EventBox();
			
			//Stock.No para corriendo
			//Stock.Yes para detenido
			//		
			Nel=new Image(Stock.No
					, IconSize.Menu);
			Simon=new Image(Stock.Yes
					, IconSize.Menu);
			eb.Add(Nel); // using stock icon
			// hooking event
			eb.ButtonPressEvent += new ButtonPressEventHandler (this.OnImageClick);			
			icon = new TrayIcon("Test");
			icon.Add(eb);
			// showing the trayicon
			icon.ShowAll();
		}
		
		private void OnImageClick (object o, ButtonPressEventArgs args) // handler for mouse click
		{
	   		if (args.Event.Button == 3) //right click
	   		 {
	      		Menu popupMenu = new Menu(); // creates the menu  
	                // creates a menu item with no image as default
	      		ImageMenuItem menuPopup1 = new ImageMenuItem ("Quit");
	      		ImageMenuItem menuView = new ImageMenuItem("Show");
	                // creates a image for the menu item
	      		Image appimg = new Image(Stock.Quit, IconSize.Menu);
	      		Image appView=new Image(Stock.Find,IconSize.Menu);
	      		menuPopup1.Image = appimg; // sets the menu item's image
	      		menuView.Image=appView;
	      		popupMenu.Add(menuPopup1); // adds the menu item to the menu
	      		popupMenu.Add(menuView);
	                // hooks a event when the user clicks the icon
	      		menuPopup1.Activated += new EventHandler(this.OnPopupClick);
	      		menuView.Activated+=new EventHandler(this.OnViewClick);
			popupMenu.ShowAll(); // shows everything
	                // pops up the actual menu when the user right clicks
	      		popupMenu.Popup(null, null, null, IntPtr.Zero, args.Event.Button, args.Event.Time);
	   		}
		}
		
		private void OnPopupClick(object o, EventArgs args)
		{
			Application.Quit(); // quits the application when the users clicks the popup menu
		}
		private void OnViewClick(object o, EventArgs args)
		{
			
			if(win==null )
			{				
				win = new MainWindow ();	
				
				win.DeleteEvent+=new DeleteEventHandler(this.DeleteForm);
				
			}
			
			win.Show ();
		}	
		
		private void DeleteForm(object o,EventArgs args)
		{
		//TODO hacer esto de forma generica para qie no me marque execepciones
		//en un Windget guardar la imagen actual (simon o nel)....
			eb.Remove(Nel);
			icon.Remove(eb);
			
			eb.Add(Simon); // using stock icon
			// hooking event
			
			//eb.ButtonPressEvent += new ButtonPressEventHandler (this.OnImageClick);			
			
			icon.Add(eb);
			// showing the trayicon
			icon.ShowAll();
			
			win=null;
		}
		
	}
}