namespace MaquinaTuring.Models;

public class BloqueDescifrado
{
	public long   Cifrado    { get; }
	public long   ValorAscii { get; }
	public char   Caracter   { get; }
	public bool   Valido     { get; }   // false si el valor está fuera del rango ASCII
 
	public BloqueDescifrado(long cifrado, long valorAscii, bool valido)
	{
		Cifrado    = cifrado;
		ValorAscii = valorAscii;
		Valido     = valido;
		Caracter   = valido ? (char)valorAscii : '?';
	}
}