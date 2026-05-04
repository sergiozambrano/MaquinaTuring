using MaquinaTuring.Core;
using MaquinaTuring.Math.Module1;
using MaquinaTuring.Models;

namespace MaquinaTuring.Modules;

public class ModuloGeneracionClaves : IModuloTuring
{
	// ── Identidad ──────────────────────────────────────────────────
    public string Nombre => "Módulo 1 — Generación de Claves";
 
    // ── Estado de la simulación ────────────────────────────────────
    public bool EstaCompleto => _pasoActual >= _pasos.Count;
    public int  PasoActual   => _pasoActual;
 
    // ── Datos de entrada ───────────────────────────────────────────
    private readonly int _primoP;
    private readonly int _primoQ;
 
    // ── Resultado ─────────────────────────────────────────────────
    public ParDeClaves? Resultado { get; private set; }
 
    // ── Internos ───────────────────────────────────────────────────
    private readonly Orquestador _maquina;
    private readonly List<PasoSimulacion> _pasos = new();
    private int _pasoActual = 0;
 
    // Cintas
    private const string CintaEntrada = "ENTRADA";
    private const string CintaTrabajo = "TRABAJO";
    private const string CintaSalida  = "SALIDA";
 
    // Estados
    private static readonly Estado q0 = new("q0_INICIO");
    private static readonly Estado q1 = new("q1_VALIDAR_PRIMOS");
    private static readonly Estado q2 = new("q2_CALCULAR_N");
    private static readonly Estado q3 = new("q3_CALCULAR_PHI");
    private static readonly Estado q4 = new("q4_ELEGIR_E");
    private static readonly Estado q5 = new("q5_CALCULAR_D");
    private static readonly Estado q6 = new("q6_ESCRIBIR_CLAVES");
    private static readonly Estado q7 = Estado.Halt;
 
    public ModuloGeneracionClaves(int primoP, int primoQ)
    {
        _primoP  = primoP;
        _primoQ  = primoQ;
        _maquina = new Orquestador(
            titulosCinta:   [CintaEntrada, CintaTrabajo, CintaSalida],
            estadoInicial: q0);
 
        _inicializarCintaEntrada();
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
        _inicializarCintaEntrada();
    }
 
    // ── Construcción de pasos ──────────────────────────────────────
 
    private void _construirPasos()
    {
        // q0 → q1: validar primos
        _maquina.TransitionTo(q1);
        var errorValidacion = ValidarPrimos.ValidarNumeros(_primoP, _primoQ);
        if (errorValidacion != null)
        {
            _agregarPaso(Estado.Error(errorValidacion), errorValidacion);
            return;
        }
        _agregarPaso(q1, $"¿{_primoP} es primo? Sí ✓  ¿{_primoQ} es primo? Sí ✓");
 
        // q1 → q2: calcular n
        _maquina.TransitionTo(q2);
        int n = CaluladorTotient.CalcularModulo(_primoP, _primoQ);
        _escribirEnTrabajo(n.ToString());
        _agregarPaso(q2, $"n = {_primoP} × {_primoQ} = {n}");
 
        // q2 → q3: calcular φ(n)
        _maquina.TransitionTo(q3);
        int phi = CaluladorTotient.Calcular(_primoP, _primoQ);
        _escribirEnTrabajo(phi.ToString());
        _agregarPaso(q3, $"φ(n) = ({_primoP}-1)×({_primoQ}-1) = {phi}");
 
        // q3 → q4: elegir e
        _maquina.TransitionTo(q4);
        int e = Exponente.EncontrarEulerValido(phi);
        _escribirEnTrabajo(e.ToString());
        _agregarPaso(q4, $"e = {e}  [mcd({e},{phi})=1 ✓]");
 
        // q4 → q5: calcular d
        _maquina.TransitionTo(q5);
        int d = Inverso.Calcular(e, phi);
        _escribirEnTrabajo(d.ToString());
        _agregarPaso(q5, $"d = {e}⁻¹ mod {phi} = {d}  [{d}×{e} mod {phi}=1 ✓]");
 
        // q5 → q6: escribir claves en cinta de salida
        _maquina.TransitionTo(q6);
        var cabezaSalida = _maquina.ObtenerCabecera(CintaSalida);
        cabezaSalida.EscribirAvanzada($"PUB:(e={e},n={n})");
        cabezaSalida.EscribirAvanzada(Celda.Separador);
        cabezaSalida.EscribirAvanzada($"PRV:(d={d},n={n})");
        _agregarPaso(q6, $"Clave pública (e={e},n={n}) y privada (d={d},n={n}) escritas");
 
        // q6 → HALT
        _maquina.TransitionTo(q7);
        _agregarPaso(q7, "Generación de claves completada ✓", esFinal: true);
    }
 
    // ── Helpers ────────────────────────────────────────────────────
 
    private void _inicializarCintaEntrada()
    {
        var cabeza = _maquina.ObtenerCabecera(CintaEntrada);
        cabeza.MoverA(0);
        _maquina.ObtenerCinta(CintaEntrada)
                .EscribirSecuencia(0, [_primoP.ToString(), _primoQ.ToString()]);
    }
 
    private void _escribirEnTrabajo(string valor)
    {
        var cinta = _maquina.ObtenerCinta(CintaTrabajo);
        var valores = cinta.LeerSecuencia(0);
        valores.Add(valor);
        cinta.Limpiar();
        cinta.EscribirSecuencia(0, valores.Select(v => v.ToString()));
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
 
    private List<EstadoCinta> _capturarCintas()
    {
        return _maquina.Cintas.Zip(_maquina.Cabeceras).Select(par =>
        {
            var (cinta, cabeza) = par;
            var simbolos = Enumerable
                .Range(cinta.IndiceMin, cinta.IndiceMax - cinta.IndiceMin + 1)
                .Select(i => cinta.Leer(i).Simbolo)
                .ToList();
 
            return new EstadoCinta(cinta.Titulo, simbolos, cabeza.Posicion, cinta.IndiceMin);
        }).ToList();
    }
 
    private ParDeClaves _extraerResultado()
    {
        int n   = CaluladorTotient.CalcularModulo(_primoP, _primoQ);
        int phi = CaluladorTotient.Calcular(_primoP, _primoQ);
        int e   = Exponente.EncontrarEulerValido(phi);
        int d   = Inverso.Calcular(e, phi);
 
        return new ParDeClaves(
            primoP:       _primoP,
            primoQ:       _primoQ,
            modulo:       n,
            totiente:     phi,
            clavePublica: new ClavePublica(e, n),
            clavePrivada: new ClavePrivada(d, n));
    }
}