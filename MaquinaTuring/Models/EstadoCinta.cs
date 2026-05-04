namespace MaquinaTuring.Models;

public class EstadoCinta
{
	public string        Etiqueta        { get; }
	public List<string>  Simbolos        { get; }
	public int           PosicionCabeza  { get; }
	public int           IndiceInicio    { get; }   // índice lógico de la primera celda visible
 
	public EstadoCinta(
		string etiqueta,
		List<string> simbolos,
		int posicionCabeza,
		int indiceInicio = 0)
	{
		Etiqueta       = etiqueta;
		Simbolos       = simbolos;
		PosicionCabeza = posicionCabeza;
		IndiceInicio   = indiceInicio;
	}
 
	/// <summary>
	/// Devuelve true si la posición dada corresponde a la cabeza lectora.
	/// Usado en la vista para resaltar la celda activa.
	/// </summary>
	public bool EsPosicionActiva(int posicion) =>
		posicion == PosicionCabeza;
}