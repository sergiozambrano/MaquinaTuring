namespace MaquinaTuring.Core;

/// <summary>
/// Máquina de Turing con múltiples cintas.
/// Orquesta Estados, transiciones y cabezas lectoras.
/// Soporta modo paso a paso (StepMode) para visualización educativa.
/// </summary>
public class Orquestador
{
	// ── Componentes ────────────────────────────────────────────────
    public Estado EstadoActual { get; private set; }
    public List<Estado> Estados { get; } = [];
    public List<Regla> ListaReglas { get; } = [];
    public Cinta[] Cintas { get; }
    public Cabecera[] Cabeceras { get; }
 
    // ── Control de ejecución ───────────────────────────────────────
    public bool Modoejecucion { get; set; } = false;  // pausa en cada transición
    
    public int ConteoPasos { get; private set; }
    // public bool SeDetiene => ;
    // public bool EsAceptado => EstadoActual.EsAceptado;
 
    // Evento que se dispara en cada transición (para Display)
    public event Action<TransitionEventArgs>? OnTransition;
 
    public Orquestador(string[] titulosCinta, Estado estadoInicial)
    {
        EstadoActual = estadoInicial;
        Estados.Add(estadoInicial);
 
        Cintas = titulosCinta.Select(l => new Cinta(l)).ToArray();
        Cabeceras = Cintas.Select(t => new Cabecera(t)).ToArray();
    }
 
    // ── Configuración ──────────────────────────────────────────────
    public void AgregarEstado(Estado estado)
    {
        if (!Estados.Contains(estado))
            Estados.Add(estado);
    }
 
    public void AgregarRegal(Regla regla)
    {
        ListaReglas.Add(regla);
    }
 
    // ── Ejecución ──────────────────────────────────────────────────
    /// <summary>
    /// Ejecuta un paso de la máquina.
    /// Devuelve false si ya está en HALT o no hay transición aplicable.
    /// </summary>
    public bool Paso()
    {
        if (EstadoActual.SeDetiene) return false;
 
        // Lee el símbolo de la cinta principal (cinta 0)
        string currentSymbol = Cabeceras[0].Leer().Simbolo;
 
        // Busca la primera regla que aplique
        var regla = ListaReglas.FirstOrDefault(r =>
            r.Matches(EstadoActual, currentSymbol));
 
        if (regla == null)
        {
            // Sin transición → error implícito
            var prev = EstadoActual;
            EstadoActual = Estado.Error($"sin transición desde {EstadoActual} con '{currentSymbol}'");
            OnTransition?.Invoke(new TransitionEventArgs(prev, EstadoActual, currentSymbol, null, Direccion.Quieto, ConteoPasos));
            return false;
        }
 
        // Ejecuta la transición
        var estadoPrevio = EstadoActual;
 
        if (regla.EscribirSimbolo != null)
            Cabeceras[regla.IndiceCinta].Escribir(regla.EscribirSimbolo);
 
        Cabeceras[regla.IndiceCinta].Move(regla.MovCabecera);
        EstadoActual = regla.EstadoA;
        ConteoPasos++;
 
        OnTransition?.Invoke(new TransitionEventArgs(
            estadoPrevio, EstadoActual,
            currentSymbol, regla.EscribirSimbolo,
            regla.MovCabecera, ConteoPasos));
 
        return !EstadoActual.SeDetiene;
    }
 
    /// <summary>
    /// Ejecuta hasta HALT o hasta alcanzar el límite de pasos.
    /// </summary>
    public void Ejecutar(int maxSteps = 100_000)
    {
        while (!EstadoActual.SeDetiene && ConteoPasos < maxSteps)
            Paso();
    }
 
    /// <summary>
    /// Transición directa de estado sin pasar por la tabla.
    /// Usada por los módulos para encapsular lógica compleja
    /// que no se puede expresar como una sola regla de tabla.
    /// </summary>
    public void TransitionTo(Estado nuevoEstado, string? mensaje = null)
    {
        var previous = EstadoActual;
        EstadoActual = nuevoEstado;
        ConteoPasos++;
 
        OnTransition?.Invoke(new TransitionEventArgs(
            previous, nuevoEstado,
            Simbolo: null,
            Escritura: null,
            Direction: Direccion.Quieto,
            Paso: ConteoPasos,
            Mensaje: mensaje));
    }
 
    /// <summary>
    /// Devuelve la cinta por su etiqueta.
    /// </summary>
    public Cinta ObtenerCinta(string titulo) =>
        Cintas.First(t => t.Titulo == titulo);
 
    /// <summary>
    /// Devuelve la cabeza de la cinta por etiqueta.
    /// </summary>
    public Cabecera ObtenerCabecera(string titulo) =>
        Cabeceras[Array.IndexOf(Cintas, ObtenerCinta(titulo))];
 
    public void Restablecer()
    {
        ConteoPasos = 0;
        foreach (var cinta in Cintas) cinta.Limpiar();
        foreach (var cabecera in Cabeceras) cabecera.Restablecer();
    }
}

public record TransitionEventArgs(
    Estado Desde,
    Estado Hacia,
    string? Simbolo,
    string? Escritura,
    Direccion Direction,
    int Paso,
    string? Mensaje = null
);