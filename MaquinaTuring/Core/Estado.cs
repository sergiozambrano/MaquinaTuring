namespace MaquinaTuring.Core;

public enum TipoEstado
{
	Normal,     // estado de procesamiento
	Aceptado,  // HALT exitoso
	Rechazado,  // HALT con error
	Error       // estado de error recuperable
}


/// <summary>
/// Representa un estado de la Máquina de Turing.
/// Los estados tienen un nombre descriptivo que refleja la acción
/// que están realizando (ej: "q3_VALIDAR_PRIMOS").
/// </summary>
public class Estado(string name, TipoEstado tipo = TipoEstado.Normal)
{
	public string Nombre { get; } = name;
	public TipoEstado Tipo { get; } = tipo;
 
	public bool SeDetiene   => Tipo is TipoEstado.Aceptado or TipoEstado.Rechazado;
	public bool EsAceptado => Tipo == TipoEstado.Aceptado;
	public bool EsError     => Tipo == TipoEstado.Error;
 
	public override string ToString() => Nombre;
	public override bool Equals(object? obj) => obj is Estado s && s.Nombre == Nombre;
	public override int GetHashCode() => Nombre.GetHashCode();
 
	// ── Estados predefinidos comunes ───────────────────────────────
 
	public static Estado Halt => new("q_HALT",  TipoEstado.Aceptado);
	public static Estado Error(string reason) =>
		new($"q_ERROR({reason})", TipoEstado.Rechazado);
}