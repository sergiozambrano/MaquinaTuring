namespace MaquinaTuring.ViewModels;

public class CeldaViewModel
{
	public string Simbolo   { get; set; } = "_";
	public bool   EsActiva  { get; set; }   // cabeza lectora está aquí
	public bool   EsBlanco  { get; set; }   // celda vacía
	public bool   EsSeparador { get; set; } // símbolo #
}