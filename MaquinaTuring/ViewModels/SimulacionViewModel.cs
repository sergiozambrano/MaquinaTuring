namespace MaquinaTuring.ViewModels;

public enum ModoNavegacion
{
	Manual,     // el usuario presiona "Siguiente paso"
	Automatico  // avanza solo con delay configurable
}

public class SimulacionViewModel
{
	// ── Entrada del usuario ────────────────────────────────────────
 
	public int    PrimoP   { get; set; }
	public int    PrimoQ   { get; set; }
	public string Mensaje  { get; set; } = string.Empty;
 
	// ── Los tres módulos ───────────────────────────────────────────
 
	public ModuloViewModel Modulo1 { get; set; } = new();
	public ModuloViewModel Modulo2 { get; set; } = new();
	public ModuloViewModel Modulo3 { get; set; } = new();
 
	// ── Estado global ──────────────────────────────────────────────
 
	/// <summary>
	/// Módulo que está activo en este momento (1, 2 ó 3).
	/// La vista resalta la sección correspondiente.
	/// </summary>
	public int ModuloActivo { get; set; } = 1;
 
	/// <summary>
	/// Modo de navegación seleccionado por el usuario.
	/// </summary>
	public ModoNavegacion Modo { get; set; } = ModoNavegacion.Manual;
 
	/// <summary>
	/// Velocidad del modo automático en milisegundos entre pasos.
	/// </summary>
	public int VelocidadMs { get; set; } = 800;
 
	/// <summary>
	/// Indica si la simulación ya fue iniciada (hay datos que mostrar).
	/// </summary>
	public bool SimulacionIniciada { get; set; }
 
	/// <summary>
	/// Resultados textuales para mostrar en el resumen final.
	/// </summary>
	public ResumenSimulacion? Resumen { get; set; }
}

/// <summary>
/// Resumen final visible cuando los tres módulos completaron su ejecución.
/// </summary>
public class ResumenSimulacion
{
	public int    PrimoP            { get; set; }
	public int    PrimoQ            { get; set; }
	public int    Modulo            { get; set; }   // n
	public int    Totiente          { get; set; }   // φ(n)
	public int    ExponentePublico  { get; set; }   // e
	public int    ExponentePrivado  { get; set; }   // d
	public string MensajeOriginal   { get; set; } = string.Empty;
	public string MensajeCifrado    { get; set; } = string.Empty;
	public string MensajeDescifrado { get; set; } = string.Empty;
	public bool   Exitoso           { get; set; }
}