namespace MaquinaTuring.Math.Module1;

/// <summary>
/// Calcula el inverso multiplicativo modular de e respecto a φ(n).
/// </summary>
public class Inverso
{
	/// <param name="e">Exponente público.</param>
	/// <param name="phi">φ(n) = (p-1)(q-1).</param>
	/// <returns>Exponente privado d.</returns>
	public static int Calcular(int e, int phi)
	{
		var (gcd, x, _) = Euclides.GCDExtendido(e, phi);
 
		if (gcd != 1)
			throw new InvalidOperationException(
				$"mcd({e}, {phi}) = {gcd} ≠ 1. " +
				$"El inverso modular no existe. e y φ(n) deben ser coprimos.");
 
		// x puede ser negativo (resultado del algoritmo de Euclides).
		// Aplicar mod phi garantiza un d positivo equivalente.
		int d = ((x % phi) + phi) % phi;
 
		// Verificación explícita — nunca confiamos solo en el algoritmo
		if ((long)d * e % phi != 1)
			throw new ArithmeticException(
				$"Error de verificación: ({d} × {e}) mod {phi} ≠ 1. " +
				$"Resultado: {(long)d * e % phi}");
 
		return d;
	}
}