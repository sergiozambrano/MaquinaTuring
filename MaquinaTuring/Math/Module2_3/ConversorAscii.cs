namespace MaquinaTuring.Math.Module2_3;

/// <summary>
/// Convierte entre caracteres y sus valores numéricos ASCII.
/// Rango válido: 32 (espacio) a 126 (~) — caracteres imprimibles.
/// Fuera de ese rango el valor no representa un carácter legible,
/// lo que en M3 indica que la clave privada es incorrecta.
/// </summary>
public class ConversorAscii
{
	public const int MinPrintable = 32;
	public const int MaxPrintable = 126;
	
	/// <summary>
	/// Convierte un carácter a su valor ASCII entero.
	/// </summary>
	public static int ToInt(char c) => c;
 
	/// <summary>
	/// Convierte cada carácter de un mensaje a su valor ASCII.
	/// </summary>
	public static int[] ToIntArray(string mensaje) =>
		mensaje.Select(ToInt).ToArray();
 
	// ── Módulo 3: int → char ───────────────────────────────────────
 
	/// <summary>
	/// Convierte un valor entero a su carácter ASCII.
	/// </summary>
	public static char ToChar(int valor) => (char)valor;
 
	/// <summary>
	/// Convierte un array de enteros de vuelta a string.
	/// </summary>
	public static string ToString(int[] valors) =>
		new string(valors.Select(ToChar).ToArray());
 
	/// <summary>
	/// Verifica que el valor descifrado corresponda a un carácter
	/// ASCII imprimible. Si falla, la clave privada es incorrecta
	/// o el mensaje cifrado fue alterado.
	/// </summary>
	public static bool EsValido(int valor) =>
		valor >= MinPrintable && valor <= MaxPrintable;
 
	/// <summary>
	/// Valida un valor descifrado y devuelve el motivo si es inválido.
	/// </summary>
	public static string? Desencriptador(int valor)
	{
		if (!EsValido(valor))
			return $"Valor {valor} fuera del rango ASCII imprimible " +
			       $"[{MinPrintable}-{MaxPrintable}]. " +
			       $"Posible clave incorrecta o mensaje alterado.";
		return null;
	}
}