using System.ComponentModel.DataAnnotations;

namespace MaquinaTuring.ViewModels;

public class EntradaSimulacionViewModel
{
	[Required(ErrorMessage = "El primer primo es obligatorio.")]
	[Range(2, 999, ErrorMessage = "p debe ser un primo entre 2 y 999.")]
	[Display(Name = "Primo p")]
	public int PrimoP { get; set; }
 
	[Required(ErrorMessage = "El segundo primo es obligatorio.")]
	[Range(2, 999, ErrorMessage = "q debe ser un primo entre 2 y 999.")]
	[Display(Name = "Primo q")]
	public int PrimoQ { get; set; }
 
	[Required(ErrorMessage = "El mensaje es obligatorio.")]
	[StringLength(20, MinimumLength = 1,
		ErrorMessage = "El mensaje debe tener entre 1 y 20 caracteres.")]
	[Display(Name = "Mensaje a cifrar")]
	public string Mensaje { get; set; } = string.Empty;
}