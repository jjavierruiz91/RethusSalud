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
