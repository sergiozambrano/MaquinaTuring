namespace MaquinaTuring.Math.Module1;

/// <summary>
/// Implementa el Algoritmo de Euclides en dos variantes:
///
/// 1. GCD(a, b)          → máximo común divisor
/// 2. ExtendedGCD(a, b)  → además devuelve los coeficientes de Bézout
///                         tales que: a*x + b*y = gcd(a, b)
///
/// El algoritmo extendido es el que permite calcular el inverso
/// modular d = e⁻¹ mod φ(n), núcleo del Módulo 1.
/// </summary>
public class Euclides
{
	// ── Euclides estándar ──────────────────────────────────────────
	/// <summary>
	/// Calcula el MCD de a y b.
	/// Idea: gcd(a, b) = gcd(b, a mod b)
	/// La recursión termina cuando b = 0, y gcd(a, 0) = a.
	/// </summary>
	public static int GCD(int a, int b)
	{
		while (b != 0)
		{
			(a, b) = (b, a % b);
		}
		return a;
	}
 
	/// <summary>
	/// Verifica si a y b son coprimos (mcd = 1).
	/// </summary>
	public static bool SonCoprimos(int a, int b) => GCD(a, b) == 1;
 
	// ── Euclides extendido ─────────────────────────────────────────
	/// <summary>
	/// Calcula gcd(a, b) y los coeficientes x, y tales que:
	///   a*x + b*y = gcd(a, b)   ← Identidad de Bézout
	/// </summary>
	public static (int Gcd, int X, int Y) GCDExtendido(int a, int b)
	{
		// Casos base
		if (b == 0) return (a, 1, 0);
 
		var (gcd, x1, y1) = GCDExtendido(b, a % b);
 
		// Reconstruir coeficientes en el retorno de la recursión
		int x = y1;
		int y = x1 - (a / b) * y1;
 
		return (gcd, x, y);
	}
}