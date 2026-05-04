namespace MaquinaTuring.Math.Module2_3;

/// <summary>
/// Implementa la exponenciación modular por cuadrados repetidos:
///   resultado = base^exponente mod modulo
/// </summary>
public class PotenciaModular
{
    
	/// <summary>
    /// Calcula base^exponent mod modulus usando cuadrados repetidos.
    /// </summary>
    /// <param name="base_">Valor a elevar (M para cifrar, C para descifrar).</param>
    /// <param name="exponent">Exponente (e para cifrar, d para descifrar).</param>
    /// <param name="modulus">Módulo n = p*q.</param>
    /// <returns>base^exponent mod modulus</returns>
    public static long Calcular(long base_, long exponent, long modulus)
    {
        if (modulus == 1) return 0;
 
        long result = 1;
        base_ = base_ % modulus;
 
        while (exponent > 0)
        {
            // Si el bit menos significativo es 1, acumular en resultado
            if ((exponent & 1) == 1)
                result = result * base_ % modulus;
 
            // Avanzar al siguiente bit
            exponent >>= 1;
            base_ = base_ * base_ % modulus;
        }
 
        return result;
    }
    
    /// <summary>
    /// Versión con traza: devuelve el resultado y cada iteración
    /// para que el StateLogger pueda mostrar el proceso en la vista.
    /// Cada entrada de la traza representa una iteración:
    ///   (número de iteración, bit procesado, resultado parcial, base al cuadrado)
    /// </summary>
    public static (long Result, List<StepTrace> Trace) CalcularConTraza(
        long base_, long exponent, long modulus)
    {
        if (modulus == 1) return (0, []);
 
        var trace  = new List<StepTrace>();
        long result = 1;
        long currentBase = base_ % modulus;
        int iteration = 1;
 
        long exp = exponent;
        while (exp > 0)
        {
            int bit = (int)(exp & 1);
            long prevResult = result;
 
            if (bit == 1)
                result = result * currentBase % modulus;
 
            long nextBase = currentBase * currentBase % modulus;
 
            trace.Add(new StepTrace(iteration, bit, prevResult, currentBase, result, nextBase));
 
            exp >>= 1;
            currentBase = nextBase;
            iteration++;
        }
 
        return (result, trace);
    }
}

/// <summary>
/// Representa una iteración individual de la exponenciación modular.
/// Usada por la vista para mostrar el proceso paso a paso.
/// </summary>
public record StepTrace(
    int  Iteracion,
    int  Bit,
    long ResultadoAnterios,
    long Base,
    long NuevoResultado,
    long BaseSiguiente);