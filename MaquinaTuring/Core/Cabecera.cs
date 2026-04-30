using System.Reflection.PortableExecutable;

namespace MaquinaTuring.Core;

public enum Direccion { Izquierda, Derecha, Quieto }

public class Cabecera(Cinta cinta, int posicionInicial = 0)
{
	public Cinta Cinta { get; } = cinta;
	public int Posicion { get; private set; } = posicionInicial;
 
	// ── Operaciones básicas ────────────────────────────────────────
	public Celda Leer() => Cinta.Leer(Posicion);
 
	public void Escribir(string simbolo) => Cinta.Escribir(Posicion, simbolo);
 
	public void Move(Direccion direccion)
	{
		Posicion = direccion switch
		{
			Direccion.Izquierda  => Posicion - 1,
			Direccion.Derecha => Posicion + 1,
			Direccion.Quieto  => Posicion,
			_ => Posicion
		};
	}
 
	public void MoverA(int posicion) => Posicion = posicion;
 
	public void Restablecer() => Posicion = 0;
 
	// ── Lectura avanzada ───────────────────────────────────────────
	/// <summary>
	/// Lee el símbolo actual y avanza a la derecha.
	/// </summary>
	public string LeerAvanzada()
	{
		var simbolo = Leer().Simbolo;
		Move(Direccion.Derecha);
		return simbolo;
	}
 
	/// <summary>
	/// Escribe un símbolo en la posición actual y avanza a la derecha.
	/// </summary>
	public void EscribirAvanzada(string simbolo)
	{
		Escribir(simbolo);
		Move(Direccion.Derecha);
	}
 
	/// <summary>
	/// Avanza hasta el siguiente separador '#' o blanco.
	/// </summary>
	public void SiguienteSeparador()
	{
		while (!Leer().EsVacio && !Leer().EsSeparador)
			Move(Direccion.Derecha);
		
		if (Leer().EsSeparador)
			Move(Direccion.Derecha);
	}
}