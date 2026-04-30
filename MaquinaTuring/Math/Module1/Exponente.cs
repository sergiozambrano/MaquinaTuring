namespace MaquinaTuring.Math.Module1;

/// <summary>
/// Encuentra un exponente público válido e para RSA.
///
/// La búsqueda comienza en 3 (el menor primo > 1) y avanza
/// probando solo números impares para reducir iteraciones.
/// </summary>
public class Exponente
{
	
	/// <summary>
	/// Encuentra el primer e válido mayor o igual a startFrom.
	/// Devuelve también el historial de candidatos rechazados
	/// para que el StateLogger pueda mostrarlo en consola.
	/// </summary>
	
	public static int EncontrarEulerValido(int phi, int iniciaDesde = 3)
	{
		for (int candidato = iniciaDesde; candidato < phi; candidato += 2)
		{
			int gcd = Euclides.GCD(candidato, phi);
 
			if (gcd == 1)
				return candidato;
		}
 
		throw new InvalidOperationException(
			$"No se encontró un e válido para φ(n)={phi}. " +
			$"Verifica que p y q sean primos distintos.");
	}
}