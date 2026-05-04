// simulacion.js — modo automático con submit de formulario

document.addEventListener('DOMContentLoaded', () => {

    [1, 2, 3].forEach(numeroModulo => {
        const btnAuto    = document.getElementById(`btn-auto-${numeroModulo}`);
        const formSig    = document.getElementById(`form-modulo-${numeroModulo}`);
        if (!btnAuto || !formSig) return;

        let intervalo = null;
        let corriendo = false;

        btnAuto.addEventListener('click', () => {
            if (corriendo) {
                clearInterval(intervalo);
                corriendo = false;
                btnAuto.textContent = '▶ Auto';
                btnAuto.classList.remove('corriendo');
            } else {
                corriendo = true;
                btnAuto.textContent = '⏹ Detener';
                btnAuto.classList.add('corriendo');

                const velocidad = parseInt(
                    document.getElementById('velocidad')?.value ?? '800', 10);

                intervalo = setInterval(() => {
                    const btnSig = formSig.querySelector('button[value="siguiente"]');
                    if (!btnSig || btnSig.disabled) {
                        clearInterval(intervalo);
                        corriendo = false;
                        btnAuto.textContent = '▶ Auto';
                        btnAuto.classList.remove('corriendo');
                        return;
                    }
                    btnSig.click();
                }, velocidad);
            }
        });
    });

    // Scroll automático a la instrucción activa
    document.querySelectorAll('.instruccion.activa').forEach(el => {
        el.scrollIntoView({ block: 'nearest', behavior: 'smooth' });
    });
});