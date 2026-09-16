// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

var PROGRAMAS_POR_TIPO = {
    Auxiliar: [
        'Auxiliar administrativo en salud',
        'Auxiliar en enfermería',
        'Auxiliar en salud oral',
        'Auxiliar en salud pública',
        'Auxiliar en servicios farmacéuticos'
    ],
    Tecnico: [
        'Técnico profesional en atención pre hospitalaria',
        'Técnico profesional en citohistología'
    ],
    Tecnologo: [
        'Tecnología en atención prehospitalaria',
        'Tecnología en citohistología',
        'Tecnología en regencia de farmacia',
        'Tecnología en manejo de fuentes abiertas de uso diagnóstico y terapéutico',
        'Tecnología en radiodiagnóstico y radioterapia',
        'Tecnología en radiología e imágenes diagnósticas',
        'Tecnología en radioterapia'
    ],
    Profesional: [
        'Psicología'
    ]
};

function initCascadaPrograma(options) {
    var tipoSelect = document.getElementById(options.tipoId);
    var nombreSelect = document.getElementById(options.nombreId);
    if (!tipoSelect || !nombreSelect) {
        return;
    }

    function llenarProgramas(tipo, nombreSeleccionado) {
        nombreSelect.innerHTML = '';
        var optionVacia = document.createElement('option');
        optionVacia.value = '';
        optionVacia.text = 'Seleccione el nombre del programa';
        nombreSelect.appendChild(optionVacia);

        var programas = PROGRAMAS_POR_TIPO[tipo] || [];
        programas.forEach(function (nombre) {
            var option = document.createElement('option');
            option.value = nombre;
            option.text = nombre;
            if (nombreSeleccionado && nombreSeleccionado === nombre) {
                option.selected = true;
            }
            nombreSelect.appendChild(option);
        });

        nombreSelect.disabled = programas.length === 0;
    }

    tipoSelect.addEventListener('change', function () {
        llenarProgramas(tipoSelect.value, null);
    });

    llenarProgramas(tipoSelect.value, options.nombreSeleccionado);
}

function initConvalidacionToggle(options) {
    var origenSelect = document.getElementById(options.origenId);
    var campos = (options.campoIds || []).map(function (id) { return document.getElementById(id); }).filter(Boolean);
    if (!origenSelect || !campos.length) {
        return;
    }

    function actualizar() {
        var esExtranjero = origenSelect.value === options.valorExtranjero;
        campos.forEach(function (campo) {
            campo.disabled = !esExtranjero;
            if (!esExtranjero) {
                campo.value = '';
            }
        });
    }

    origenSelect.addEventListener('change', actualizar);
    actualizar();
}

function initCascadaUbicacion(options) {
    var paisSelect = document.getElementById(options.paisId);
    var deptoSelect = document.getElementById(options.departamentoId);
    var municipioSelect = document.getElementById(options.municipioId);

    if (!paisSelect || !deptoSelect || !municipioSelect) {
        return;
    }

    function llenarSelect(select, items, valorSeleccionado, placeholder, deshabilitar) {
        select.innerHTML = '';
        var optionVacia = document.createElement('option');
        optionVacia.value = '';
        optionVacia.text = placeholder;
        select.appendChild(optionVacia);

        items.forEach(function (item) {
            var option = document.createElement('option');
            option.value = item.id;
            option.text = item.nombre;
            if (valorSeleccionado && String(item.id) === String(valorSeleccionado)) {
                option.selected = true;
            }
            select.appendChild(option);
        });

        select.disabled = !!deshabilitar;
    }

    function cargarDepartamentos(paisId, deptoSeleccionado, municipioSeleccionado) {
        if (!paisId) {
            llenarSelect(deptoSelect, [], null, 'Departamento', false);
            llenarSelect(municipioSelect, [], null, 'Municipio', false);
            return;
        }

        fetch(options.departamentosUrl + '?paisId=' + paisId)
            .then(function (r) { return r.json(); })
            .then(function (departamentos) {
                var sinDatos = departamentos.length === 0;
                llenarSelect(deptoSelect, departamentos, deptoSeleccionado, sinDatos ? 'No aplica para este país' : 'Departamento', sinDatos);
                if (!sinDatos && deptoSeleccionado) {
                    cargarMunicipios(deptoSeleccionado, municipioSeleccionado);
                } else {
                    llenarSelect(municipioSelect, [], null, sinDatos ? 'No aplica para este país' : 'Municipio', sinDatos);
                }
            });
    }

    function cargarMunicipios(departamentoId, municipioSeleccionado) {
        if (!departamentoId) {
            llenarSelect(municipioSelect, [], null, 'Municipio', false);
            return;
        }

        fetch(options.municipiosUrl + '?departamentoId=' + departamentoId)
            .then(function (r) { return r.json(); })
            .then(function (municipios) {
                var sinDatos = municipios.length === 0;
                llenarSelect(municipioSelect, municipios, municipioSeleccionado, sinDatos ? 'No aplica para este departamento' : 'Municipio', sinDatos);
            });
    }

    paisSelect.addEventListener('change', function () {
        cargarDepartamentos(paisSelect.value, null, null);
    });

    deptoSelect.addEventListener('change', function () {
        cargarMunicipios(deptoSelect.value, null);
    });

    if (paisSelect.value) {
        cargarDepartamentos(paisSelect.value, options.departamentoSeleccionado, options.municipioSeleccionado);
    }
}

(function initPasswordToggles() {
    document.querySelectorAll('[data-toggle-password]').forEach(function (button) {
        var input = document.getElementById(button.getAttribute('data-toggle-password'));
        if (!input) {
            return;
        }

        button.addEventListener('click', function () {
            var isHidden = input.type === 'password';
            input.type = isHidden ? 'text' : 'password';
            button.classList.toggle('is-visible', isHidden);
        });
    });
})();

(function initPasswordMatchValidation() {
    document.querySelectorAll('[data-match-password]').forEach(function (confirmInput) {
        var newInput = document.getElementById(confirmInput.getAttribute('data-match-password'));
        if (!newInput) {
            return;
        }

        function validar() {
            if (confirmInput.value && confirmInput.value !== newInput.value) {
                confirmInput.setCustomValidity('Las contrasenas no coinciden');
            } else {
                confirmInput.setCustomValidity('');
            }
        }

        confirmInput.addEventListener('input', validar);
        newInput.addEventListener('input', validar);
    });
})();

(function initSignaturePreview() {
    document.querySelectorAll('[data-signature-preview]').forEach(function (input) {
        var preview = document.getElementById(input.getAttribute('data-signature-preview'));
        if (!preview) {
            return;
        }

        input.addEventListener('change', function () {
            var file = input.files && input.files[0];
            if (!file) {
                return;
            }

            var reader = new FileReader();
            reader.onload = function (e) {
                preview.innerHTML = '<img src="' + e.target.result + '" alt="Firma seleccionada" />';
            };
            reader.readAsDataURL(file);
        });
    });
})();

(function initFirmaDibujada() {
    document.querySelectorAll('[data-firma-input]').forEach(function (root) {
        var canvas = root.querySelector('[data-firma-canvas]');
        var hidden = root.querySelector('[data-firma-hidden]');
        if (!canvas || !hidden) {
            return;
        }

        var tabs = root.querySelectorAll('[data-firma-tab]');
        var panels = root.querySelectorAll('[data-firma-panel]');
        var clearBtn = root.querySelector('[data-firma-clear]');
        var errorEl = root.querySelector('[data-firma-error]');
        var ctx = canvas.getContext('2d');
        ctx.lineWidth = 2;
        ctx.lineCap = 'round';
        ctx.strokeStyle = '#0b283b';
        var dibujando = false;
        var trazado = false;

        function activarTab(nombre) {
            tabs.forEach(function (tab) {
                tab.classList.toggle('active', tab.getAttribute('data-firma-tab') === nombre);
            });
            panels.forEach(function (panel) {
                panel.classList.toggle('d-none', panel.getAttribute('data-firma-panel') !== nombre);
            });
        }

        tabs.forEach(function (tab) {
            tab.addEventListener('click', function () {
                activarTab(tab.getAttribute('data-firma-tab'));
            });
        });

        function posicion(e) {
            var r = canvas.getBoundingClientRect();
            var t = e.touches ? e.touches[0] : e;
            return {
                x: (t.clientX - r.left) * canvas.width / r.width,
                y: (t.clientY - r.top) * canvas.height / r.height
            };
        }

        function iniciarTrazo(e) {
            dibujando = true;
            trazado = true;
            var p = posicion(e);
            ctx.beginPath();
            ctx.moveTo(p.x, p.y);
            e.preventDefault();
        }

        function moverTrazo(e) {
            if (!dibujando) {
                return;
            }
            var p = posicion(e);
            ctx.lineTo(p.x, p.y);
            ctx.stroke();
            e.preventDefault();
        }

        function soltarTrazo() {
            dibujando = false;
        }

        canvas.addEventListener('mousedown', iniciarTrazo);
        canvas.addEventListener('mousemove', moverTrazo);
        window.addEventListener('mouseup', soltarTrazo);
        canvas.addEventListener('touchstart', iniciarTrazo);
        canvas.addEventListener('touchmove', moverTrazo);
        canvas.addEventListener('touchend', soltarTrazo);

        if (clearBtn) {
            clearBtn.addEventListener('click', function () {
                ctx.clearRect(0, 0, canvas.width, canvas.height);
                trazado = false;
                if (errorEl) {
                    errorEl.classList.add('d-none');
                }
            });
        }

        var form = root.closest('form');
        if (!form) {
            return;
        }

        form.addEventListener('submit', function (e) {
            var tabDibujar = root.querySelector('[data-firma-tab="dibujar"]');
            var enModoDibujo = tabDibujar && tabDibujar.classList.contains('active');

            if (!enModoDibujo) {
                hidden.value = '';
                return;
            }

            if (!trazado) {
                e.preventDefault();
                if (errorEl) {
                    errorEl.classList.remove('d-none');
                }
                return;
            }

            hidden.value = canvas.toDataURL('image/png');
        });
    });
})();

(function initAutoCloseModals() {
    document.querySelectorAll('[data-autoclose]').forEach(function (el) {
        var delay = parseInt(el.getAttribute('data-autoclose'), 10) || 5000;
        var progress = el.querySelector('.app-feedback-progress span');
        if (progress) {
            progress.style.animationDuration = delay + 'ms';
        }
        var modal = bootstrap.Modal.getOrCreateInstance(el);
        modal.show();
        var timer = setTimeout(function () { modal.hide(); }, delay);
        el.addEventListener('hidden.bs.modal', function () { clearTimeout(timer); }, { once: true });
    });
})();

(function initDismissAlerts() {
    document.querySelectorAll('[data-dismiss-alert]').forEach(function (button) {
        button.addEventListener('click', function () {
            var alertEl = button.closest('.app-alert');
            if (!alertEl) {
                return;
            }

            alertEl.style.animation = 'appAlertOut 0.2s ease forwards';
            setTimeout(function () { alertEl.remove(); }, 200);
        });
    });
})();

(function initAppSidebarToggle() {
    var shell = document.getElementById('appShell');
    var toggles = document.querySelectorAll('.js-sidebar-toggle');
    var backdrop = document.getElementById('appSidebarBackdrop');
    if (!shell || !toggles.length) {
        return;
    }

    function close() {
        shell.classList.remove('app-sidebar-open');
    }

    toggles.forEach(function (toggle) {
        toggle.addEventListener('click', function () {
            shell.classList.toggle('app-sidebar-open');
        });
    });

    if (backdrop) {
        backdrop.addEventListener('click', close);
    }
})();

(function initHomeSubnavScrollSpy() {
    var subnav = document.querySelector('.home-subnav');
    if (!subnav) {
        return;
    }

    var links = subnav.querySelectorAll('a[href^="#"]');
    var sections = [];
    links.forEach(function (link) {
        var section = document.querySelector(link.getAttribute('href'));
        if (section) {
            sections.push({ link: link, section: section });
        }
    });

    if (!sections.length) {
        return;
    }

    var observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            var match = sections.find(function (s) { return s.section === entry.target; });
            if (entry.isIntersecting && match) {
                links.forEach(function (link) { link.classList.remove('active'); });
                match.link.classList.add('active');
            }
        });
    }, { rootMargin: '-45% 0px -50% 0px' });

    sections.forEach(function (s) { observer.observe(s.section); });
})();

(function initSiteNavbarScrollEffect() {
    var navbar = document.querySelector('.site-navbar');
    if (!navbar) {
        return;
    }

    function update() {
        navbar.classList.toggle('is-scrolled', window.scrollY > 12);
    }

    update();
    window.addEventListener('scroll', update, { passive: true });
})();

(function initAutoSubmitFilters() {
    var forms = document.querySelectorAll('form[data-autosubmit]');
    if (!forms.length) {
        return;
    }

    var DEBOUNCE_MS = 500;

    forms.forEach(function (form) {
        var timer = null;

        function enviar(nombreCampo) {
            var marcador = form.querySelector('input[name="__focus"]');
            if (!marcador) {
                marcador = document.createElement('input');
                marcador.type = 'hidden';
                marcador.name = '__focus';
                form.appendChild(marcador);
            }
            marcador.value = nombreCampo || '';
            form.submit();
        }

        form.querySelectorAll('input[type="text"], input[type="search"]').forEach(function (campo) {
            campo.addEventListener('input', function () {
                clearTimeout(timer);
                timer = setTimeout(function () { enviar(campo.name); }, DEBOUNCE_MS);
            });
        });
    });

    var focoCampo = new URLSearchParams(window.location.search).get('__focus');
    if (focoCampo) {
        var elFoco = document.querySelector('[name="' + focoCampo + '"]');
        if (elFoco && typeof elFoco.focus === 'function') {
            elFoco.focus();
            var valor = elFoco.value || '';
            if (elFoco.setSelectionRange) {
                elFoco.setSelectionRange(valor.length, valor.length);
            }
        }
    }
})();

(function initLoadingOverlay() {
    var overlay = document.getElementById('siteLoadingOverlay');
    var texto = document.getElementById('siteLoadingText');
    if (!overlay) {
        return;
    }

    document.querySelectorAll('form[data-show-loading]').forEach(function (form) {
        form.addEventListener('submit', function (evento) {
            if (window.jQuery && typeof jQuery(form).valid === 'function' && !jQuery(form).valid()) {
                return;
            }

            if (form.dataset.enviando === 'true') {
                evento.preventDefault();
                return;
            }
            form.dataset.enviando = 'true';

            form.querySelectorAll('button[type="submit"]').forEach(function (boton) {
                boton.disabled = true;
            });

            if (texto) {
                texto.textContent = form.getAttribute('data-loading-text') || 'Cargando...';
            }
            overlay.classList.add('is-visible');
        });
    });
})();

(function initScrollReveal() {
    var targets = document.querySelectorAll('.home-section, .feature-card, .process-step, .floating-feature-card');
    if (!targets.length) {
        return;
    }

    if (!('IntersectionObserver' in window)) {
        targets.forEach(function (el) { el.classList.add('reveal-on-scroll', 'in-view'); });
        return;
    }

    var observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
            if (entry.isIntersecting) {
                entry.target.classList.add('in-view');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.15, rootMargin: '0px 0px -60px 0px' });

    targets.forEach(function (el) {
        el.classList.add('reveal-on-scroll');
        observer.observe(el);
    });
})();

(function syncViewportGap() {
    // En algunos navegadores móviles, position:fixed se mide contra un
    // viewport más alto que el visible (mientras la barra de direcciones
    // del navegador sigue mostrándose), dejando el contenido fijo del fondo
    // a medio cortar hasta el primer scroll. Medimos esa diferencia real y
    // la exponemos como variable CSS para que los elementos fijos la usen.
    function actualizar() {
        var visible = window.visualViewport ? window.visualViewport.height : window.innerHeight;
        var gap = Math.max(window.innerHeight - visible, 0);
        document.documentElement.style.setProperty('--app-viewport-gap', gap + 'px');
    }

    actualizar();
    window.addEventListener('resize', actualizar);
    if (window.visualViewport) {
        window.visualViewport.addEventListener('resize', actualizar);
    }
})();

(function initInstallAppBanner() {
    var banner = document.getElementById('installAppBanner');
    if (!banner) {
        return;
    }

    // Ya instalada (abierta como app) -> nunca mostrar el aviso.
    var yaInstalada = window.matchMedia('(display-mode: standalone)').matches || window.navigator.standalone === true;
    if (yaInstalada) {
        return;
    }

    // Se oculta solo para esta sesión de navegador (sessionStorage): si el
    // usuario la ignora hoy, mañana (nueva sesión) se le vuelve a mostrar.
    var CLAVE_SESION = 'rethusInstallBannerOcultoSesion';
    if (window.sessionStorage && sessionStorage.getItem(CLAVE_SESION) === '1') {
        return;
    }

    var textoEl = document.getElementById('installAppBannerText');
    var btnAceptar = document.getElementById('installAppBannerAccept');
    var btnCerrar = document.getElementById('installAppBannerDismiss');
    var deferredPrompt = null;

    var esIOS = /iphone|ipad|ipod/i.test(window.navigator.userAgent);

    function mostrar() {
        banner.hidden = false;
        banner.setAttribute('data-visible', 'true');
    }

    function ocultarPorEstaSesion() {
        banner.removeAttribute('data-visible');
        banner.hidden = true;
        if (window.sessionStorage) {
            sessionStorage.setItem(CLAVE_SESION, '1');
        }
    }

    if (esIOS) {
        // iOS no dispara beforeinstallprompt; solo se puede instalar a mano.
        textoEl.textContent = 'Toca Compartir y luego "Agregar a inicio" para instalarla.';
        btnAceptar.textContent = 'Entendido';
        btnAceptar.addEventListener('click', ocultarPorEstaSesion);
        mostrar();
    } else {
        window.addEventListener('beforeinstallprompt', function (evento) {
            evento.preventDefault();
            deferredPrompt = evento;
            mostrar();
        });

        btnAceptar.addEventListener('click', function () {
            if (!deferredPrompt) {
                ocultarPorEstaSesion();
                return;
            }
            deferredPrompt.prompt();
            deferredPrompt.userChoice.finally(function () {
                deferredPrompt = null;
                ocultarPorEstaSesion();
            });
        });
    }

    btnCerrar.addEventListener('click', ocultarPorEstaSesion);

    window.addEventListener('appinstalled', ocultarPorEstaSesion);
})();

(function registerServiceWorker() {
    if (!('serviceWorker' in navigator)) {
        return;
    }
    window.addEventListener('load', function () {
        navigator.serviceWorker.register('/sw.js').catch(function () {
            // Instalación como app es opcional; si falla el registro no afecta el resto del sitio.
        });
    });
})();
