namespace MaquinaTuring.Models;

public class BloqueCifrado
{
	public char   Caracter   { get; }
	public int    ValorAscii { get; }
	public long   Cifrado    { get; }
 
	public BloqueCifrado(char caracter, int valorAscii, long cifrado)
	{
		Caracter   = caracter;
		ValorAscii = valorAscii;
		Cifrado    = cifrado;
	}
}