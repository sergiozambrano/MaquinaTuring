namespace MaquinaTuring.ViewModels;

public enum EstadoInstruccion
{
	Pendiente,   // aún no ejecutada
	Activa,      // es el paso actual
	Completada,  // ya fue ejecutada
	Error        // terminó en error
}

public class InstruccionViewModel
{
	/// <summary>Número de paso (1, 2, 3...).</summary>
	public int    Numero      { get; set; }
 
	/// <summary>Estado de origen de la transición.</summary>
	public string EstadoDesde { get; set; } = string.Empty;
 
	/// <summary>Estado de destino de la transición.</summary>
	public string EstadoHacia { get; set; } = string.Empty;
 
	/// <summary>Descripción legible de la operación realizada.</summary>
	public string Descripcion { get; set; } = string.Empty;
 
	/// <summary>Estado visual de esta instrucción en la lista.</summary>
	public EstadoInstruccion Estado { get; set; } = EstadoInstruccion.Pendiente;
 
	/// <summary>Texto resumido para mostrar en la lista lateral.</summary>
	public string Resumen =>
		$"[{EstadoDesde}] → [{EstadoHacia}]";
}