namespace MaquinaTuring.Models;

public class PasoSimulacion
{
	public int               NumeroPaso      { get; }
	public string            EstadoAnterior  { get; }
	public string            EstadoActual    { get; }
	public string?           SimboloLeido    { get; }
	public string?           SimboloEscrito  { get; }
	public string?           Descripcion     { get; }   // texto legible de la operación
	public List<EstadoCinta> Cintas          { get; }   // snapshot de todas las cintas
	public bool              EsEstadoFinal   { get; }
 
	public PasoSimulacion(
		int               numeroPaso,
		string            estadoAnterior,
		string            estadoActual,
		List<EstadoCinta> cintas,
		string?           simboloLeido   = null,
		string?           simboloEscrito = null,
		string?           descripcion    = null,
		bool              esEstadoFinal  = false)
	{
		NumeroPaso     = numeroPaso;
		EstadoAnterior = estadoAnterior;
		EstadoActual   = estadoActual;
		Cintas         = cintas;
		SimboloLeido   = simboloLeido;
		SimboloEscrito = simboloEscrito;
		Descripcion    = descripcion;
		EsEstadoFinal  = esEstadoFinal;
	}
 
	public string ResumenTransicion =>
		$"[{EstadoAnterior}] → [{EstadoActual}]" +
		(Descripcion != null ? $"  —  {Descripcion}" : string.Empty);
}