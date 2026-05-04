using MaquinaTuring.Math.Module1;
using MaquinaTuring.Models;
using MaquinaTuring.Modules;
using MaquinaTuring.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace MaquinaTuring.Controllers;

public class Simulacioncontroller : Controller
{
	// ── Claves de sesión ───────────────────────────────────────────
    private const string SessionIndice1 = "indice_modulo1";
    private const string SessionIndice2 = "indice_modulo2";
    private const string SessionIndice3 = "indice_modulo3";
    private const string SessionEntrada = "entrada_simulacion";
 
    // ── GET: /Simulacion ──────────────────────────────────────────
    public IActionResult Index()
    {
        return View(new SimulacionViewModel());
    }
 
    // ── POST: /Simulacion/Iniciar ─────────────────────────────────
    [HttpPost]
    public IActionResult Iniciar(EntradaSimulacionViewModel entrada)
    {
        if (!ModelState.IsValid)
            return View("Index", new SimulacionViewModel());
 
        var errorPrimos = ValidarPrimos.ValidarNumeros(entrada.PrimoP, entrada.PrimoQ);
        if (errorPrimos != null)
        {
            ModelState.AddModelError(string.Empty, errorPrimos);
            return View("Index", new SimulacionViewModel());
        }
 
        int n = entrada.PrimoP * entrada.PrimoQ;
        if (entrada.Mensaje.Any(c => (int)c >= n))
        {
            ModelState.AddModelError(string.Empty,
                $"Todos los caracteres deben tener valor ASCII menor que n={n}.");
            return View("Index", new SimulacionViewModel());
        }
 
        // Guardar solo la entrada en sesión — los módulos se reconstruyen por request
        HttpContext.Session.SetInt32(SessionIndice1, 0);
        HttpContext.Session.SetInt32(SessionIndice2, 0);
        HttpContext.Session.SetInt32(SessionIndice3, 0);
        HttpContext.Session.SetString(SessionEntrada,
            $"{entrada.PrimoP}|{entrada.PrimoQ}|{entrada.Mensaje}");
 
        var (pasos1, pasos2, pasos3, claves, cifrado, descifrado) =
            _ejecutarModulos(entrada.PrimoP, entrada.PrimoQ, entrada.Mensaje);
 
        var vm = _construirViewModel(
            entrada.PrimoP, entrada.PrimoQ, entrada.Mensaje,
            pasos1, pasos2, pasos3, claves, cifrado, descifrado,
            moduloActivo: 1, indice1: 0, indice2: 0, indice3: 0);
 
        return View("Index", vm);
    }
 
    // ── POST: /Simulacion/Navegar ─────────────────────────────────
    [HttpPost]
    public IActionResult Navegar(int modulo, string direccion)
    {
        var entrada = HttpContext.Session.GetString(SessionEntrada);
        if (entrada == null) return RedirectToAction("Index");
 
        var (p, q, mensaje) = _parsearEntrada(entrada);
 
        int indice1 = HttpContext.Session.GetInt32(SessionIndice1) ?? 0;
        int indice2 = HttpContext.Session.GetInt32(SessionIndice2) ?? 0;
        int indice3 = HttpContext.Session.GetInt32(SessionIndice3) ?? 0;
 
        var (pasos1, pasos2, pasos3, claves, cifrado, descifrado) =
            _ejecutarModulos(p, q, mensaje);
 
        if (modulo == 1)
            indice1 = _ajustarIndice(indice1, pasos1.Count, direccion);
        else if (modulo == 2)
            indice2 = _ajustarIndice(indice2, pasos2.Count, direccion);
        else if (modulo == 3)
            indice3 = _ajustarIndice(indice3, pasos3.Count, direccion);
 
        HttpContext.Session.SetInt32(SessionIndice1, indice1);
        HttpContext.Session.SetInt32(SessionIndice2, indice2);
        HttpContext.Session.SetInt32(SessionIndice3, indice3);
 
        var vm = _construirViewModel(
            p, q, mensaje,
            pasos1, pasos2, pasos3, claves, cifrado, descifrado,
            moduloActivo: modulo,
            indice1: indice1, indice2: indice2, indice3: indice3);
 
        return View("Index", vm);
    }
 
    // ── Helpers: ejecución de módulos ──────────────────────────────
 
    private (List<PasoSimulacion>, List<PasoSimulacion>, List<PasoSimulacion>,
             ParDeClaves, MensajeCifrado, MensajeDescifrado)
        _ejecutarModulos(int p, int q, string mensaje)
    {
        var modulo1 = new ModuloGeneracionClaves(p, q);
        var pasos1  = modulo1.ObtenerTodosLosPasos();
        modulo1.Ejecutar();
        var claves  = modulo1.Resultado!;
 
        var modulo2 = new ModuloCifrado(mensaje, claves.ClavePublica);
        var pasos2  = modulo2.ObtenerTodosLosPasos();
        modulo2.Ejecutar();
        var cifrado = modulo2.Resultado!;
 
        var modulo3 = new ModuloDescifrado(cifrado, claves.ClavePrivada);
        var pasos3  = modulo3.ObtenerTodosLosPasos();
        modulo3.Ejecutar();
        var descifrado = modulo3.Resultado!;
 
        return (pasos1, pasos2, pasos3, claves, cifrado, descifrado);
    }
 
    // ── Helpers: construcción del ViewModel ────────────────────────
 
    private SimulacionViewModel _construirViewModel(
        int p, int q, string mensaje,
        List<PasoSimulacion> pasos1,
        List<PasoSimulacion> pasos2,
        List<PasoSimulacion> pasos3,
        ParDeClaves          claves,
        MensajeCifrado       cifrado,
        MensajeDescifrado    descifrado,
        int moduloActivo,
        int indice1, int indice2, int indice3)
    {
        return new SimulacionViewModel
        {
            PrimoP             = p,
            PrimoQ             = q,
            Mensaje            = mensaje,
            ModuloActivo       = moduloActivo,
            SimulacionIniciada = true,
            Modulo1 = _construirModulo(1, "Módulo 1 — Generación de Claves", pasos1, indice1),
            Modulo2 = _construirModulo(2, "Módulo 2 — Cifrado",              pasos2, indice2),
            Modulo3 = _construirModulo(3, "Módulo 3 — Descifrado",           pasos3, indice3),
            Resumen = new ResumenSimulacion
            {
                PrimoP            = p,
                PrimoQ            = q,
                Modulo            = claves.Modulo,
                Totiente          = claves.Totiente,
                ExponentePublico  = claves.ClavePublica.Exponente,
                ExponentePrivado  = claves.ClavePrivada.Exponente,
                MensajeOriginal   = mensaje,
                MensajeCifrado    = cifrado.BloquesCifradosTexto,
                MensajeDescifrado = descifrado.MensajeRecuperado,
                Exitoso           = descifrado.Exitoso
            }
        };
    }
 
    private ModuloViewModel _construirModulo(
        int numero, string nombre,
        List<PasoSimulacion> pasos, int indiceActual)
    {
        var pasoVisible = pasos[indiceActual];
 
        return new ModuloViewModel
        {
            Numero       = numero,
            Nombre       = nombre,
            PasoActual   = indiceActual,
            TotalPasos   = pasos.Count,
            EstaCompleto = indiceActual == pasos.Count - 1,
            EstadoActual = pasoVisible.EstadoActual,
            MensajeError = pasoVisible.EstadoActual.Contains("ERROR")
                ? pasoVisible.Descripcion : null,
            Cintas = pasoVisible.Cintas.Select(c => new CintaViewModel
            {
                Etiqueta       = c.Etiqueta,
                Descripcion    = _descripcionCinta(numero, c.Etiqueta),
                PosicionCabeza = c.PosicionCabeza - c.IndiceInicio,
                Celdas         = c.Simbolos.Select((s, i) => new CeldaViewModel
                {
                    Simbolo     = s,
                    EsActiva    = i == (c.PosicionCabeza - c.IndiceInicio),
                    EsBlanco    = s == "_",
                    EsSeparador = s == "#"
                }).ToList()
            }).ToList(),
            Instrucciones = pasos.Select((p, i) => new InstruccionViewModel
            {
                Numero      = i + 1,
                EstadoDesde = p.EstadoAnterior,
                EstadoHacia = p.EstadoActual,
                Descripcion = p.Descripcion ?? string.Empty,
                Estado      = i < indiceActual  ? EstadoInstruccion.Completada
                            : i == indiceActual ? EstadoInstruccion.Activa
                            : EstadoInstruccion.Pendiente
            }).ToList()
        };
    }
 
    private string _descripcionCinta(int modulo, string etiqueta) =>
        (modulo, etiqueta) switch
        {
            (1, "ENTRADA") => "Primos p y q",
            (1, "TRABAJO") => "n, φ(n), e, d",
            (1, "SALIDA")  => "Claves generadas",
            (2, "ENTRADA") => "Mensaje original",
            (2, "CLAVE")   => "Clave pública (e, n)",
            (2, "TRABAJO") => "ASCII y cifrado actuales",
            (2, "SALIDA")  => "Bloques cifrados",
            (3, "ENTRADA") => "Bloques cifrados",
            (3, "CLAVE")   => "Clave privada (d, n)",
            (3, "TRABAJO") => "Bloque y valor descifrado",
            (3, "SALIDA")  => "Mensaje recuperado",
            _              => etiqueta
        };
 
    // ── Helpers: sesión ────────────────────────────────────────────
 
    private int _ajustarIndice(int actual, int total, string direccion) =>
        direccion == "siguiente"
            ? System.Math.Min(actual + 1, total - 1)
            : System.Math.Max(actual - 1, 0);
 
    private (int P, int Q, string Mensaje) _parsearEntrada(string entrada)
    {
        var partes = entrada.Split('|');
        return (int.Parse(partes[0]), int.Parse(partes[1]), partes[2]);
    }
}