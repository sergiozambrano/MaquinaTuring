namespace MaquinaTuring.Models;

public class ClavePublica
{
	/// <summary>Exponente público (e).</summary>
    public int Exponente { get; }
 
    /// <summary>Módulo público n = p × q.</summary>
    public int Modulo { get; }
 
    public ClavePublica(int exponente, int modulo)
    {
        Exponente = exponente;
        Modulo    = modulo;
    }
 
    public override string ToString() => $"(e={Exponente}, n={Modulo})";
}