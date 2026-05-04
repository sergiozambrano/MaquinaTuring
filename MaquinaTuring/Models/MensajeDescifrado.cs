namespace MaquinaTuring.Models;

public class MensajeDescifrado
{
	public string   MensajeRecuperado { get; }
	public bool     Exitoso           { get; }
	public string?  ErrorDescripcion  { get; }
 
	public List<BloqueDescifrado> Bloques { get; }
 
	public MensajeDescifrado(List<BloqueDescifrado> bloques)
	{
		Bloques = bloques;
 
		var bloquesValidos = bloques.Where(b => b.Valido).ToList();
		Exitoso = bloquesValidos.Count == bloques.Count;
 
		MensajeRecuperado = Exitoso
			? new string(bloques.Select(b => b.Caracter).ToArray())
			: string.Empty;
 
		ErrorDescripcion = Exitoso
			? null
			: $"{bloques.Count - bloquesValidos.Count} bloque(s) fuera del rango ASCII imprimible. " +
			  $"Verifique que la clave privada sea correcta.";
	}
}