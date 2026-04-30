namespace MaquinaTuring.Core;

/// <summary>
/// Representa una celda individual en la cinta de la Máquina de Turing.
/// Cada celda contiene un símbolo o está en blanco.
/// </summary>
public class Celda(String simbolo = "_")
{
	public const string Vacio = "_";
	public const string Separador = "#";
 
	public string Simbolo { get; set; } = simbolo;
	public bool EsVacio => Simbolo == Vacio;
	public bool EsSeparador => Simbolo == Separador;
 
	public override string ToString() => Simbolo;
}