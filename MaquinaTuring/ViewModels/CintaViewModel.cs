namespace MaquinaTuring.ViewModels;

public class CintaViewModel
{
	// <summary>Etiqueta de la cinta: ENTRADA, TRABAJO, SALIDA, CLAVE.</summary>
	public string Etiqueta     { get; set; } = string.Empty;
 
	/// <summary>Descripción del rol de esta cinta en el módulo actual.</summary>
	public string Descripcion  { get; set; } = string.Empty;
 
	/// <summary>Celdas de la cinta en orden de izquierda a derecha.</summary>
	public List<CeldaViewModel> Celdas     { get; set; } = new();
 
	/// <summary>Posición actual de la cabeza lectora (índice en Celdas).</summary>
	public int PosicionCabeza { get; set; }
}