using MaquinaTuring.Core;
using MaquinaTuring.Math.Module2_3;
using MaquinaTuring.Math.Module2;
using MaquinaTuring.Models;

namespace MaquinaTuring.Modules;

public class ModuloCifrado : IModuloTuring
{
	// ── Identidad ──────────────────────────────────────────────────
    public string Nombre => "Módulo 2 — Cifrado";
 
    // ── Estado de la simulación ────────────────────────────────────
    public bool EstaCompleto => _pasoActual >= _pasos.Count;
    public int  PasoActual   => _pasoActual;
 
    // ── Datos de entrada ───────────────────────────────────────────
    private readonly string      _mensaje;
    private readonly ClavePublica _clavePublica;
 
    // ── Resultado ─────────────────────────────────────────────────
    public MensajeCifrado? Resultado { get; private set; }
 
    // ── Internos ───────────────────────────────────────────────────
    private readonly Orquestador _maquina;
    private readonly List<PasoSimulacion> _pasos = new();
    private int _pasoActual = 0;
 
    // Cintas
    private const string CintaEntrada = "ENTRADA";
    private const string CintaClave   = "CLAVE";
    private const string CintaTrabajo = "TRABAJO";
    private const string CintaSalida  = "SALIDA";
 
    // Estados
    private static readonly Estado q0 = new("q0_INICIO");
    private static readonly Estado q1 = new("q1_CARGAR_CLAVE");
    private static readonly Estado q2 = new("q2_LEER_CARACTER");
    private static readonly Estado q3 = new("q3_CONVERTIR_ASCII");
    private static readonly Estado q4 = new("q4_VALIDAR_M");
    private static readonly Estado q5 = new("q5_DESCOMPONER_EXPONENTE");
    private static readonly Estado q6 = new("q6_EXPONENCIACION");
    private static readonly Estado q7 = new("q7_ESCRIBIR_CIFRADO");
    private static readonly Estado q8 = Estado.Halt;
 
    public ModuloCifrado(string mensaje, ClavePublica clavePublica)
    {
        _mensaje      = mensaje;
        _clavePublica = clavePublica;
        _maquina      = new Orquestador(
            titulosCinta:   [CintaEntrada, CintaClave, CintaTrabajo, CintaSalida],
            estadoInicial: q0);
 
        _inicializarCintas();
    }
 
    // ── IModuloTuring ──────────────────────────────────────────────
 
    public void Ejecutar()
    {
        if (_pasos.Count == 0) _construirPasos();
        _pasoActual = _pasos.Count;
        Resultado   = _extraerResultado();
    }
 
    public PasoSimulacion? SiguientePaso()
    {
        if (_pasos.Count == 0) _construirPasos();
        if (EstaCompleto) return null;
 
        var paso = _pasos[_pasoActual++];
        if (EstaCompleto) Resultado = _extraerResultado();
        return paso;
    }
 
    public List<PasoSimulacion> ObtenerTodosLosPasos()
    {
        if (_pasos.Count == 0) _construirPasos();
        return _pasos;
    }
 
    public void Reiniciar()
    {
        _pasoActual = 0;
        Resultado   = null;
        _pasos.Clear();
        _maquina.Restablecer();
        _maquina.TransitionTo(q0);
        _inicializarCintas();
    }
 
    // ── Construcción de pasos ──────────────────────────────────────
 
    private void _construirPasos()
    {
        int e = _clavePublica.Exponente;
        int n = _clavePublica.Modulo;
 
        // q0 → q1: cargar clave pública
        _maquina.TransitionTo(q1);
        _agregarPaso(q1, $"Clave pública cargada: e={e}, n={n}");
 
        // q1 → q2: inicio del loop por cada carácter
        foreach (char caracter in _mensaje)
        {
            // q2: leer carácter
            _maquina.TransitionTo(q2);
            _agregarPaso(q2, $"Carácter leído: '{caracter}'");
 
            // q3: convertir a ASCII
            _maquina.TransitionTo(q3);
            int ascii = ConversorAscii.ToInt(caracter);
            _actualizarTrabajo(ascii.ToString(), "");
            _agregarPaso(q3, $"'{caracter}' → ASCII = {ascii}");
 
            // q4: validar M < n
            _maquina.TransitionTo(q4);
            if (ascii >= n)
            {
                _agregarPaso(Estado.Error($"M={ascii} >= n={n}"),
                    $"Error: el valor ASCII {ascii} no es menor que n={n}");
                return;
            }
            _agregarPaso(q4, $"¿{ascii} < {n}? Sí ✓");
 
            // q5: descomponer exponente en binario
            _maquina.TransitionTo(q5);
            string binario = DecodificadorBinario.CadenaBinaria(e);
            int[]  bits    = DecodificadorBinario.ObtenerBits(e);
            _agregarPaso(q5, $"e={e} en binario: {binario}  →  {bits.Length} iteraciones");
 
            // q6: exponenciación modular
            _maquina.TransitionTo(q6);
            var (cifrado, traza) = PotenciaModular.CalcularConTraza(ascii, e, n);
            _actualizarTrabajo(ascii.ToString(), cifrado.ToString());
 
            // Un sub-paso por iteración de la exponenciación
            foreach (var t in traza)
            {
                _agregarPaso(q6,
                    $"iter {t.Iteracion}: bit={t.Bit} │ " +
                    $"res={t.ResultadoAnterios}→{t.NuevoResultado} │ " +
                    $"base={t.Base}→{t.BaseSiguiente}");
            }
 
            // q7: escribir bloque cifrado en cinta de salida
            _maquina.TransitionTo(q7);
            var cabezaSalida = _maquina.ObtenerCabecera(CintaSalida);
            if (cabezaSalida.Posicion > 0)
                cabezaSalida.EscribirAvanzada(Celda.Separador);
            foreach (char c in cifrado.ToString())
                cabezaSalida.EscribirAvanzada(c.ToString());
 
            _agregarPaso(q7, $"C({caracter}) = {cifrado} → escrito en cinta SALIDA");
        }
 
        // HALT
        _maquina.TransitionTo(q8);
        _agregarPaso(q8, "Cifrado completo ✓", esFinal: true);
    }
 
    // ── Helpers ────────────────────────────────────────────────────
 
    private void _inicializarCintas()
    {
        // Cinta ENTRADA: cada carácter separado por #
        _maquina.ObtenerCinta(CintaEntrada)
                .EscribirSecuencia(0, _mensaje.Select(c => c.ToString()));
 
        // Cinta CLAVE: e # n
        _maquina.ObtenerCinta(CintaClave)
                .EscribirSecuencia(0, [
                    _clavePublica.Exponente.ToString(),
                    _clavePublica.Modulo.ToString()]);
    }
 
    private void _actualizarTrabajo(string m, string c)
    {
        var cinta = _maquina.ObtenerCinta(CintaTrabajo);
        cinta.Limpiar();
        if (c == "")
            cinta.EscribirSecuencia(0, [m]);
        else
            cinta.EscribirSecuencia(0, [m, c]);
    }
 
    private void _agregarPaso(Estado estado, string descripcion, bool esFinal = false)
    {
        _pasos.Add(new PasoSimulacion(
            numeroPaso:    _pasos.Count + 1,
            estadoAnterior: _pasos.Count > 0
                ? _pasos[^1].EstadoActual
                : q0.Nombre,
            estadoActual:  estado.Nombre,
            cintas:        _capturarCintas(),
            descripcion:   descripcion,
            esEstadoFinal: esFinal));
    }
 
    private List<EstadoCinta> _capturarCintas() =>
        _maquina.Cintas.Zip(_maquina.Cabeceras).Select(par =>
        {
            var (cinta, cabeza) = par;
            var simbolos = Enumerable
                .Range(cinta.IndiceMin, System.Math.Max(1, cinta.IndiceMax - cinta.IndiceMin + 1))
                .Select(i => cinta.Leer(i).Simbolo)
                .ToList();
            return new EstadoCinta(cinta.Titulo, simbolos, cabeza.Posicion, cinta.IndiceMin);
        }).ToList();
 
    private MensajeCifrado _extraerResultado()
    {
        var bloques = _mensaje.Select(c =>
        {
            int ascii   = ConversorAscii.ToInt(c);
            long cifrado = PotenciaModular.Calcular(
                ascii, _clavePublica.Exponente, _clavePublica.Modulo);
            return new BloqueCifrado(c, ascii, cifrado);
        }).ToList();
 
        return new MensajeCifrado(_mensaje, bloques);
    }
}