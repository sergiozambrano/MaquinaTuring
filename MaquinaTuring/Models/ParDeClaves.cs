namespace MaquinaTuring.Models;

public class ParDeClaves
{
	public ClavePublica  ClavePublica  { get; }
	public ClavePrivada  ClavePrivada  { get; }
 
	// Parámetros internos visibles en la simulación
	public int PrimoP    { get; }
	public int PrimoQ    { get; }
	public int Modulo    { get; }   // n = p × q
	public int Totiente  { get; }   // φ(n) = (p-1)(q-1)
 
	public ParDeClaves(
		int primoP, int primoQ,
		int modulo, int totiente,
		ClavePublica clavePublica, ClavePrivada clavePrivada)
	{
		PrimoP       = primoP;
		PrimoQ       = primoQ;
		Modulo       = modulo;
		Totiente     = totiente;
		ClavePublica = clavePublica;
		ClavePrivada = clavePrivada;
	}
}