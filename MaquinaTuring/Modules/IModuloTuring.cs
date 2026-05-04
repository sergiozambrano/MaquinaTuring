using MaquinaTuring.Models;

namespace MaquinaTuring.Modules;

public interface IModuloTuring
{
	// <summary>Nombre descriptivo del módulo para la vista.</summary>
	string Nombre { get; }
 
	/// <summary>Indica si el módulo ya llegó a HALT.</summary>
	bool EstaCompleto { get; }
 
	/// <summary>Paso actual dentro de la simulación.</summary>
	int PasoActual { get; }
 
	/// <summary>
	/// Ejecuta el módulo completo de una sola vez.
	/// No genera traza — usado cuando solo importa el resultado.
	/// </summary>
	void Ejecutar();
 
	/// <summary>
	/// Avanza un único paso de la simulación.
	/// Devuelve el paso ejecutado o null si ya está completo.
	/// </summary>
	PasoSimulacion? SiguientePaso();
 
	/// <summary>
	/// Devuelve todos los pasos de la simulación de una sola vez.
	/// Usado por el modo automático para pre-calcular la traza completa.
	/// </summary>
	List<PasoSimulacion> ObtenerTodosLosPasos();
 
	/// <summary>
	/// Reinicia el módulo a su estado inicial.
	/// Permite volver a simular con los mismos datos.
	/// </summary>
	void Reiniciar();
}