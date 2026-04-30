namespace MaquinaTuring.Core;

/// <summary>
/// Define una regla de transición de la Máquina de Turing:
/// (estado_actual, símbolo_leído) → (nuevo_estado, símbolo_escrito, dirección)
///
/// El símbolo de lectura puede ser null para indicar una transición
/// que aplica independientemente del símbolo leído (transición épsilon).
/// </summary>
public class Regla(Estado estadoDesde, string? leerSimbolo, Estado estadoA, string? escribirSimbolo = null, Direccion movCabecera = Direccion.Quieto, int indiceCinta = 0)
{
	public Estado EstadoDesde  { get; } = estadoDesde;
	public string? LeerSimbolo { get; } = leerSimbolo;  // null = cualquier símbolo
	public Estado EstadoA { get; } = estadoA;
	public string? EscribirSimbolo { get; } = escribirSimbolo; // null = no escribe (stay)
	public Direccion MovCabecera   { get; } = movCabecera;
	public int IndiceCinta { get; } = indiceCinta;   // qué cinta afecta (0-based)
 
	/// <summary>
	/// Evalúa si esta regla aplica para el estado y símbolo dados.
	/// </summary>
	public bool Matches(Estado state, string symbol)
	{
		return EstadoDesde.Equals(state) && (LeerSimbolo == null || LeerSimbolo == symbol);
	}
 
	public override string ToString()
	{
		string write = EscribirSimbolo != null ? $"escribe '{EscribirSimbolo}'" : "no escribe";
		string move  = MovCabecera.ToString().ToLower();
		return $"[{EstadoDesde}] + '{LeerSimbolo ?? "*"}' → [{EstadoA}] | {write} | mueve {move} | cinta {IndiceCinta}";
	}
}