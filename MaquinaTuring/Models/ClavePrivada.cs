namespace MaquinaTuring.Models;

public class ClavePrivada
{
	/// <summary>Exponente privado (d) = e⁻¹ mod φ(n).</summary>
	public int Exponente { get; }
 
	/// <summary>Módulo público n = p × q (compartido con la clave pública).</summary>
	public int Modulo { get; }
 
	public ClavePrivada(int exponente, int modulo)
	{
		Exponente = exponente;
		Modulo    = modulo;
	}
 
	public override string ToString() => $"(d={Exponente}, n={Modulo})";
}