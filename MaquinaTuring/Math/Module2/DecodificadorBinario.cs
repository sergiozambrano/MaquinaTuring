namespace MaquinaTuring.Math.Module2;

/// <summary>
/// Descompone un exponente en su representación binaria
/// para la exponenciación modular por cuadrados repetidos.
/// </summary>
public class DecodificadorBinario
{
	// <summary>
	/// Devuelve los bits del exponente de LSB a MSB (derecha a izquierda).
	/// Es el orden en que la exponenciación modular los consume.
	/// </summary>
	public static int[] ObtenerBits(int exponente)
	{
		if (exponente < 1)
			throw new ArgumentOutOfRangeException(
				nameof(exponente), "El exponente debe ser mayor que 0.");
 
		var bits = new List<int>();
		int e = exponente;
 
		while (e > 0)
		{
			bits.Add(e & 1);   // bit menos significativo
			e >>= 1;           // desplazar a la derecha
		}
 
		return bits.ToArray();
	}
 
	/// <summary>
	/// Devuelve la representación binaria como string (MSB a LSB)
	/// para mostrarla en la vista de forma legible.
	/// </summary>
	public static string CadenaBinaria(int exponente) =>
		Convert.ToString(exponente, 2);
 
	/// <summary>
	/// Cantidad de bits (iteraciones que tendrá la exponenciación).
	/// </summary>
	public static int ContadorBit(int exponente) =>
		ObtenerBits(exponente).Length;
}