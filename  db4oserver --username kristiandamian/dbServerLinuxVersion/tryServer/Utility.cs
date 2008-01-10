
using System;

namespace tryServer
{
	
	
	public class Utility
	{
		
		public Utility()
		{
		}
		
		public static string NombreArchivo(string sArchivo)
        {
        	char sSeparador='/';//Path.DirectorySeparatorChar;
            int iPos = Libreria.FuncionesCadena.BuscoDesdeUltimo(sArchivo,sSeparador);
            return sArchivo.Substring(iPos + 1, (sArchivo.Length - 1 - iPos));
        }
        
        public static string RutaSinNombreArchivo(string sArchivo)
        {
        	char sSeparador='/';//Path.DirectorySeparatorChar;
            int iPos = Libreria.FuncionesCadena.BuscoDesdeUltimo(sArchivo,sSeparador);
            return sArchivo.Substring(0, iPos);
        }
	}
}
