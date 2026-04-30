namespace MaquinaTuring.Math.Module1;

/// <summary>
/// Válida si un número es primo y genera listas de primos
/// usando la Criba de Eratóstenes.
///
/// Límite de trabajo: primos menores a 1000 (scope educativo).
/// </summary>
public class ValidarPrimos
{
	private const int Limite = 1000;
 
    // Caché estático: se calcula una sola vez al primer uso
    // private static readonly bool[] _sieve = BuildSieve(Limite);

    private static List<int> Primos =
    [
        2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53,
        59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131,
        137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199,
        211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281,
        283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373,
        379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457,
        461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557,
        563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641,
        643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733,
        739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827,
        829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929,
        937, 941, 947, 953, 967, 971, 977, 983, 991, 997
    ];   
 
    // ── Criba de Eratóstenes ───────────────────────────────────────
 
    /// <summary>
    /// Construye la criba: _sieve[i] = true significa que i es primo.
    ///
    /// Algoritmo:
    ///   1. Marca todos como primos inicialmente.
    ///   2. Para cada número desde 2, si sigue marcado como primo,
    ///      marca todos sus múltiplos como no-primos.
    ///   3. Al terminar, solo quedan marcados los primos reales.
    /// </summary>
    private static bool[] BuildSieve(int Limite)
    {
        var sieve = new bool[Limite + 1];
 
        // Paso 1: todos son candidatos
        Array.Fill(sieve, true);
        sieve[0] = false;
        sieve[1] = false;
 
        // Paso 2: eliminar múltiplos
        // Solo necesitamos llegar hasta √Limite porque cualquier
        // factor compuesto mayor ya fue eliminado por uno menor
        for (int i = 2; i * i <= Limite; i++)
        {
            if (!sieve[i]) continue;
 
            // i es primo → eliminar todos sus múltiplos desde i²
            for (int multiple = i * i; multiple <= Limite; multiple += i)
                sieve[multiple] = false;
        }
 
        return sieve;
    }
 
    /// <summary>
    /// Devuelve true si n es un número primo.
    /// </summary>
    public static bool EsPrimo(int n)
    {
        if (n < 2 || n > Limite)
            return false;
 
        return Primos.Contains(n);
    }
 
    /// <summary>
    /// Devuelve todos los primos hasta el límite indicado.
    /// </summary>
    public static int[] ObtenerNumerosPrimos(int max)
    {
        if (max > Limite)
            throw new ArgumentOutOfRangeException(
                nameof(max), $"El límite máximo es {Limite}.");
 
        return Enumerable.Range(2, max - 1).Where(EsPrimo).ToArray();
    }
 
    /// <summary>
    /// Valida que p y q sean primos distintos y que n = p*q sea manejable.
    /// Devuelve el motivo del rechazo o null si son válidos.
    /// </summary>
    public static string? ValidarNumeros(int p, int q)
    {
        if (p == q) return $"p y q deben ser distintos.";
        
        if (!EsPrimo(p)) return $"{p} no es primo.";
        
        if (!EsPrimo(q)) return $"{q} no es primo.";
 
        return null;
    }
}