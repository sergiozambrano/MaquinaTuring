using MaquinaTuring.Core;
using MaquinaTuring.Math.Module2_3;
using MaquinaTuring.Models;

namespace MaquinaTuring.Modules;

public class ModuloDescifrado : IModuloTuring
{
	// ── Identidad ──────────────────────────────────────────────────
    public string Nombre => "Módulo 3 — Descifrado";
 
    // ── Estado de la simulación ────────────────────────────────────
    public bool EstaCompleto => _pasoActual >= _pasos.Count;
    public int  PasoActual   => _pasoActual;
 
    // ── Datos de entrada ───────────────────────────────────────────
    private readonly MensajeCifrado _mensajeCifrado;
    private readonly ClavePrivada   _clavePrivada;
 
    // ── Resultado ─────────────────────────────────────────────────
    public MensajeDescifrado? Resultado { get; private set; }
 
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
    private static readonly Estado q2 = new("q2_LEER_BLOQUE");
    private static readonly Estado q3 = new("q3_CARGAR_BLOQUE");
    private static readonly Estado q4 = new("q4_EXPONENCIACION");
    private static readonly Estado q5 = new("q5_VALIDAR_ASCII");
    private static readonly Estado q6 = new("q6_CONVERTIR_CHAR");
    private static readonly Estado q7 = new("q7_ESCRIBIR_CARACTER");
    private static readonly Estado q8 = Estado.Halt;
 
    public ModuloDescifrado(MensajeCifrado mensajeCifrado, ClavePrivada clavePrivada)
    {
        _mensajeCifrado = mensajeCifrado;
        _clavePrivada   = clavePrivada;
        _maquina        = new Orquestador(
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
        int d = _clavePrivada.Exponente;
        int n = _clavePrivada.Modulo;
 
        // q0 → q1: cargar clave privada
        _maquina.TransitionTo(q1);
        _agregarPaso(q1, $"Clave privada cargada: d={d}, n={n}");
 
        // loop por cada bloque cifrado
        foreach (var bloque in _mensajeCifrado.Bloques)
        {
            long cifrado = bloque.Cifrado;
 
            // q2: leer bloque
            _maquina.TransitionTo(q2);
            _agregarPaso(q2, $"Bloque leído: C={cifrado}");
 
            // q3: cargar en cinta de trabajo
            _maquina.TransitionTo(q3);
            _actualizarTrabajo(cifrado.ToString(), "");
            _agregarPaso(q3, $"C={cifrado} cargado en cinta TRABAJO");
 
            // q4: exponenciación modular C^d mod n
            _maquina.TransitionTo(q4);
            long valorAscii = PotenciaModular.Calcular(cifrado, d, n);
            _actualizarTrabajo(cifrado.ToString(), valorAscii.ToString());
            _agregarPaso(q4,
                $"C^d mod n = {cifrado}^{d} mod {n} = {valorAscii}");
 
            // q5: validar rango ASCII imprimible
            _maquina.TransitionTo(q5);
            var errorAscii = ConversorAscii.Desencriptador((int)valorAscii);
            if (errorAscii != null)
            {
                _agregarPaso(Estado.Error("ASCII_INVALIDO"), errorAscii);
                return;
            }
            _agregarPaso(q5,
                $"¿{ConversorAscii.MinPrintable} ≤ {valorAscii} ≤ {ConversorAscii.MaxPrintable}? Sí ✓");
 
            // q6: convertir a carácter
            _maquina.TransitionTo(q6);
            char caracter = ConversorAscii.ToChar((int)valorAscii);
            _agregarPaso(q6, $"ASCII {valorAscii} → '{caracter}'");
 
            // q7: escribir carácter en cinta de salida
            _maquina.TransitionTo(q7);
            var cabezaSalida = _maquina.ObtenerCabecera(CintaSalida);
            if (cabezaSalida.Posicion > 0)
                cabezaSalida.EscribirAvanzada(Celda.Separador);
            cabezaSalida.EscribirAvanzada(caracter.ToString());
            _agregarPaso(q7, $"'{caracter}' escrito en cinta SALIDA");
        }
 
        // HALT
        _maquina.TransitionTo(q8);
        _agregarPaso(q8, "Descifrado completo ✓", esFinal: true);
    }
 
    // ── Helpers ────────────────────────────────────────────────────
 
    private void _inicializarCintas()
    {
        // Cinta ENTRADA: bloques cifrados separados por #
        _maquina.ObtenerCinta(CintaEntrada)
                .EscribirSecuencia(0, _mensajeCifrado.BloquesCifrados
                    .Select(c => c.ToString()));
 
        // Cinta CLAVE: d # n
        _maquina.ObtenerCinta(CintaClave)
                .EscribirSecuencia(0, [
                    _clavePrivada.Exponente.ToString(),
                    _clavePrivada.Modulo.ToString()]);
    }
 
    private void _actualizarTrabajo(string c, string m)
    {
        var cinta = _maquina.ObtenerCinta(CintaTrabajo);
        cinta.Limpiar();
        if (m == "")
            cinta.EscribirSecuencia(0, [c]);
        else
            cinta.EscribirSecuencia(0, [c, m]);
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
 
    private MensajeDescifrado _extraerResultado()
    {
        int d = _clavePrivada.Exponente;
        int n = _clavePrivada.Modulo;
 
        var bloques = _mensajeCifrado.Bloques.Select(b =>
        {
            long ascii  = PotenciaModular.Calcular(b.Cifrado, d, n);
            bool valido = ConversorAscii.EsValido((int)ascii);
            return new BloqueDescifrado(b.Cifrado, ascii, valido);
        }).ToList();
 
        return new MensajeDescifrado(bloques);
    }
}