namespace MaquinaTuring.ViewModels;

public class ModuloViewModel
{
	/// <summary>Nombre del módulo: "Módulo 1 — Generación de Claves", etc.</summary>
	public string                    Nombre        { get; set; } = string.Empty;
 
	/// <summary>Número de módulo (1, 2 ó 3) para identificar la sección en la vista.</summary>
	public int                       Numero        { get; set; }
 
	/// <summary>Cintas activas del módulo en el paso actual.</summary>
	public List<CintaViewModel>      Cintas        { get; set; } = new();
 
	/// <summary>Lista completa de instrucciones para el panel lateral.</summary>
	public List<InstruccionViewModel> Instrucciones { get; set; } = new();
 
	/// <summary>Índice del paso actualmente visible (0-based).</summary>
	public int                       PasoActual    { get; set; }
 
	/// <summary>Total de pasos de este módulo.</summary>
	public int                       TotalPasos    { get; set; }
 
	/// <summary>Indica si el módulo ya llegó a HALT.</summary>
	public bool                      EstaCompleto  { get; set; }
 
	/// <summary>Mensaje de error si el módulo terminó en estado de rechazo.</summary>
	public string?                   MensajeError  { get; set; }
 
	/// <summary>Texto del estado actual para mostrarlo sobre las cintas.</summary>
	public string                    EstadoActual  { get; set; } = string.Empty;
 
	// ── Helpers para la vista ──────────────────────────────────────
 
	public bool HayPasoAnterior => PasoActual > 0;
	public bool HaySiguientePaso => PasoActual < TotalPasos - 1;
	public string ProgresoTexto => $"Paso {PasoActual + 1} de {TotalPasos}";
}