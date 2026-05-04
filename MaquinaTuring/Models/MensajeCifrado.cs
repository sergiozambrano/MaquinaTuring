namespace MaquinaTuring.Models;

public class MensajeCifrado
{
	public string   MensajeOriginal { get; }
	public int[]    ValoresAscii    { get; }
	public long[]   BloquesCifrados { get; }
 
	/// <summary>
	/// Bloques individuales con su traza de exponenciación.
	/// Usados por la vista para mostrar el proceso paso a paso.
	/// </summary>
	public List<BloqueCifrado> Bloques { get; }
 
	public MensajeCifrado(string mensajeOriginal, List<BloqueCifrado> bloques)
	{
		MensajeOriginal = mensajeOriginal;
		Bloques         = bloques;
		ValoresAscii    = bloques.Select(b => b.ValorAscii).ToArray();
		BloquesCifrados = bloques.Select(b => b.Cifrado).ToArray();
	}
 
	public string BloquesCifradosTexto =>
		string.Join(", ", BloquesCifrados.Select(c => c.ToString()));
}