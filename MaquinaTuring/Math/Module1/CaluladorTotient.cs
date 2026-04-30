namespace MaquinaTuring.Math.Module1;

/// <summary>
/// Calcula la función de Euler φ(n) para un par de primos p, q.
///
/// φ(n) cuenta cuántos enteros en [1, n] son coprimos con n.
/// Para n = p*q (producto de dos primos distintos):
///   φ(n) = (p - 1) * (q - 1)
///
/// Este valor es el secreto estructural del sistema RSA:
/// conocer φ(n) permite calcular d, y calcular φ(n) requiere
/// conocer p y q, lo que equivale a factorizar n.
/// </summary>
public class CaluladorTotient
{
	/// <summary>
	/// Calcula φ(n) dado que n = p * q, con p y q primos distintos.
	/// </summary>
	/// <param name="p">Primer primo.</param>
	/// <param name="q">Segundo primo.</param>
	/// <returns>φ(p*q) = (p-1)*(q-1)</returns>
	public static int Calcular(int p, int q)
	{
		string? resultado = ValidarPrimos.ValidarNumeros(p, q);

		if (resultado != null) throw new ArgumentException(resultado);
 
		// Fórmula directa para el caso RSA (p y q primos):
		// φ(p*q) = (p-1)(q-1)
		return (p - 1) * (q - 1);
	}
 
	/// <summary>
	/// Calcula n = p * q (el módulo público).
	/// Se agrupa aquí porque n y φ(n) siempre se calculan juntos.
	/// </summary>
	public static int CalcularModulo(int p, int q) => p * q;
 
	/// <summary>
	/// Devuelve n y φ(n) en una sola llamada.
	/// </summary>
	public static (int N, int Phi) CalcularAmbos(int p, int q) =>
		(CalcularModulo(p, q), Calcular(p, q));
}