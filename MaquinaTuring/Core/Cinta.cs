using System.Text;

namespace MaquinaTuring.Core;

/// <summary>
/// Cinta de la Máquina de Turing: lista dinámica de celdas con
/// acceso indexado. Se expande automáticamente en ambas direcciones.
/// </summary>
public class Cinta(string titulo)
{
	// Offset interno para soportar índices negativos (expansión izquierda)
    private const int DesplazamientoInterno = 100;
 
    private readonly Dictionary<int, Celda> _cells = new();
    private int _indiceMin = 0;
    private int _indiceMax = 0;
 
    public string Titulo { get; } = titulo;
 
    // ── Acceso a celdas ────────────────────────────────────────────
    public Celda Leer(int posicion)
    {
        return _cells.TryGetValue(posicion + DesplazamientoInterno, out var cell) ? cell : new Celda(Celda.Vacio);
    }
 
    public void Escribir(int position, string symbol)
    {
        int key = position + DesplazamientoInterno;
        _cells[key] = new Celda(symbol);
 
        if (position < _indiceMin) _indiceMin = position;
        if (position > _indiceMax) _indiceMax = position;
    }
 
    // ── Escritura de secuencias ─────────────────────────────────────
 
    /// <summary>
    /// Escribe una lista de valores separados por '#' en la cinta
    /// a partir de la posición indicada. Devuelve la posición final.
    /// </summary>
    public int EscribirSecuencia(int posicionInicial, IEnumerable<string> valores)
    {
        int pos = posicionInicial;
        bool primero = true;
 
        foreach (var value in valores)
        {
            if (!primero)
                Escribir(pos++, Celda.Separador);
            
            foreach (var c in value)
                Escribir(pos++, c.ToString());
            
            primero = false;
        }
 
        Escribir(pos++, Celda.Separador);
        return pos;
    }
 
    /// <summary>
    /// Lee valores separados por '#' desde posicionInicial hasta el
    /// primer blanco o doble separador.
    /// </summary>
    public List<string> LeerSecuencia(int posicionInicial)
    {
        var result = new List<string>();
        var current = new StringBuilder();
        int pos = posicionInicial;
 
        while (true)
        {
            var cell = Leer(pos++);
            
            if (cell.EsVacio) break;
            
            if (cell.EsSeparador)
            {
                if (current.Length > 0)
                {
                    result.Add(current.ToString());
                    current.Clear();
                }
            }
            else
                current.Append(cell.Simbolo);
        }
 
        if (current.Length > 0)
            result.Add(current.ToString());
 
        return result;
    }
 
    // ── Propiedades de rango ────────────────────────────────────────
    public int IndiceMin => _indiceMin;
    public int IndiceMax => _indiceMax;
    public bool EsVacio => !_cells.Any(c => !c.Value.EsVacio);
 
    public void Limpiar()
    {
        _cells.Clear();
        _indiceMin = 0;
        _indiceMax = 0;
    }
}