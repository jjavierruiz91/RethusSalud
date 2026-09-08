// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function initCascadaUbicacion(options) {
    var paisSelect = document.getElementById(options.paisId);
    var deptoSelect = document.getElementById(options.departamentoId);
    var municipioSelect = document.getElementById(options.municipioId);

    if (!paisSelect || !deptoSelect || !municipioSelect) {
        return;
    }

    function llenarSelect(select, items, valorSeleccionado, placeholder) {
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
    }

    function cargarDepartamentos(paisId, deptoSeleccionado, municipioSeleccionado) {
        if (!paisId) {
            llenarSelect(deptoSelect, [], null, 'Departamento');
            llenarSelect(municipioSelect, [], null, 'Municipio');
            return;
        }

        fetch(options.departamentosUrl + '?paisId=' + paisId)
            .then(function (r) { return r.json(); })
            .then(function (departamentos) {
                llenarSelect(deptoSelect, departamentos, deptoSeleccionado, 'Departamento');
                if (deptoSeleccionado) {
                    cargarMunicipios(deptoSeleccionado, municipioSeleccionado);
                } else {
                    llenarSelect(municipioSelect, [], null, 'Municipio');
                }
            });
    }

    function cargarMunicipios(departamentoId, municipioSeleccionado) {
        if (!departamentoId) {
            llenarSelect(municipioSelect, [], null, 'Municipio');
            return;
        }

        fetch(options.municipiosUrl + '?departamentoId=' + departamentoId)
            .then(function (r) { return r.json(); })
            .then(function (municipios) {
                llenarSelect(municipioSelect, municipios, municipioSeleccionado, 'Municipio');
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

(function initAppSidebarToggle() {
    var shell = document.getElementById('appShell');
    var toggle = document.getElementById('appSidebarToggle');
    var backdrop = document.getElementById('appSidebarBackdrop');
    if (!shell || !toggle) {
        return;
    }

    function close() {
        shell.classList.remove('app-sidebar-open');
    }

    toggle.addEventListener('click', function () {
        shell.classList.toggle('app-sidebar-open');
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
