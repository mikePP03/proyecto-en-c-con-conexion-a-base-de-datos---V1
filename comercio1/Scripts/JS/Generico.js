toastr.options.fadeOut = 3000;
toastr.options.timeOut = 3000;

function ponerFormatoFecha(fecha) {
    var nuevaFecha = new Date(fecha.match(/\d+/)[0] * 1);
    var dia = nuevaFecha.getDate();
    if (dia < 10)
        dia = "0" + dia;
    var mes = nuevaFecha.getMonth() + 1;
    if (mes < 10)
        mes = "0" + mes;
    var anio = nuevaFecha.getFullYear();
    return dia + "/" + mes + "/" + anio;
}

function habilitarBotonGuardar(claseInput, boton) {
    var lista = $(claseInput).toArray();
    var data = lista.where(function (valor) {
        return valor.value.length <= 0;
    });
    if (data.length > 0)
        $(boton).prop('disabled', true);
    else
        $(boton).prop('disabled', false);
}

Array.prototype.where = Array.prototype.filter || function (predicate, context) {
    context = context || window;
    var arr = [];
    var l = this.length;
    for (var i = 0; i < l; i++)
        if (predicate.call(context, this[i], i, this) === true) arr.push(this[i]);
    return arr;
};
Array.prototype.first = function (predicate, def) {
    var l = this.length;
    if (!predicate) return l ? this[0] : def == null ? null : def;
    for (var i = 0; i < l; i++)
        if (predicate(this[i], i, this))
            return this[i];
    return def == null ? null : def;
};

function solicitudAjax(solicitudUrl, onSuccess, data, tipoDato, tipo) {
    var tipoSolicitud = tipo ? 'POST' : 'GET',
        datatype = tipoDato ? 'text' : 'json';
    $.ajax({
        type: tipoSolicitud,
        datatype: datatype,
        traditional: false,
        url: solicitudUrl,
        data: data,
        success: function (responseText) {
            if (onSuccess)
                onSuccess(responseText);
        },
        error: function (exception) {
        }
    });
}

function crearControlFecha(element, cambiarFecha) {
    $.datepicker.regional['es'] = {
        closeText: 'Cerrar',
        prevText: '<Ant',
        nextText: 'Sig>',
        currentText: 'Hoy',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['es']);

    if (cambiarFecha) {
        $(element).datepicker({
            changeMonth: true,
            changeYear: true

        });
    } else {
        $(element).datepicker({});
    }
}

function adicionarOpcionesCombo(elemento, items, evento, prop) {
    prop = prop || { id: 'id', value: 'value' };
    $.each(items, function (index, item) {
        $(elemento).append($('<option>').val(item[prop.id]).text(item[prop.value]));
    });
    if (evento)
        $(elemento).change(function (e) { evento(e, $(elemento).val()); });
}

function cargarRuta(url) {
    $('#content').load(url, null, null);
}

function cambiarRutaUsuario(url) {
    window.location.replace(url);
}

function limpiarTabla(elemento) {
    $('#' + elemento).find('tbody tr').remove();
}

function crearSpan(id, cssClase, contenido) {
    return $("<span>", { id: id }).addClass(cssClase).html(contenido);
}

function td(elementData, elementClass) {
    var td = $('<td>').html(elementData);
    if (elementClass)
        return td.addClass(elementClass);
    return td;
}

function col(data) {
    return td(data).addClass(alinear(data));
}

function isNumber(valueString) {
    if (valueString == '0') return true;
    if (!valueString) return false;
    return Number(valueString);
}

function isDate(dateString) {
    var patron = new RegExp("[a-zA-Z]");
    var date = new Date(dateString);
    return date.toString() == 'NaN' || date.toString() == 'Invalid Date' ? false : (patron.test(dateString)) ? false : true;
}

function alinear(value) {
    return isNumber(value) ? 'alinearDerecha' : isDate(value) ? 'alinearCentro' : 'alinearizquierda';
}

function AccionColumna(evento, classIcono, title) {
    title = title || "";
    return $('<button>', { title: title, class: "btn btn-outline-primary btn-sm" })
        .append("<img src='../Content/open-iconic/svg/" + classIcono + ".svg' alt='Eliminar'>").click(function (e) {
        e.preventDefault();
        evento(e);
    });
}

Array.prototype.remove = function (item) {
    var i = this.indexOf(item);
    if (i != -1)
        this.splice(i, 1);
};

$.fn.soloNumeros = function () {
    validarNumeros(this);
    return this;
};

$.fn.soloNumerosPorcentaje = function () {
    validarNumerosPorcentajes(this);
    return this;
};

$.fn.soloNumerosDecimal = function () {
    validarNumeros(this, 2);
    return this;
};

function validarNumeros(elemento, conDecimal) {
    $(elemento).keydown(function (e) {
        var numeroValido = [46, 8, 9, 27, 13];
        if (conDecimal)
            numeroValido.push(190, 110);
        if ($.inArray(e.keyCode, numeroValido) != -1 ||
            (e.keyCode == 65 && e.ctrlKey == true) ||
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            return;
        }
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    });
    return $(elemento);
}

function validarNumerosPorcentajes(elemento) {
    $(elemento).keypress(function (e) {
        var numeroValido = [8, 9, 27, 13];

        if ($.inArray(e.keyCode, numeroValido) != -1 ||
            (e.keyCode == 65 && e.ctrlKey == true) ||
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            return;
        }
        var valor = parseInt(elemento.val() + e.char);
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105) || valor > 100 || (elemento.val().length >= 1 && valor === 0 && e.char === "0"))
            e.preventDefault();
    });
    return $(elemento);
}

