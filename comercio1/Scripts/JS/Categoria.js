var _datosCategoria;
function eliminarCategoriaExitoso(resultado, e, elemento) {
    $("#eliminarCategoria").modal("hide");
    if (resultado.Success) {
        $(e.currentTarget).closest('tr').remove();
        _datosCategoria.remove(elemento);
        toastr.success("La Categoria'" + elemento.Nombre + " '   se ha eliminado satisfactoriamente");
    } else {
        toastr.error(resultado.Mensaje);
    }
}

function confirmarEliminarCategoria(e, elemento) {
    var url = "/Categoria/EliminarCategoria";
    var tipo = 'GET';
    var datos = { id: elemento.IdCategoria };
    var tipoDatos = 'JSON';
    solicitudAjax(url, function (response) { eliminarCategoriaExitoso(response, e, elemento); }, datos, tipoDatos, tipo);
}

function mostrarModalEliminarCategoria(e, elemento) {
    var modal = '#eliminarCategoria';
    $(modal).find(".modal-title").html('Eliminar Categoria');
    $(modal).find(".text-mensaje-modal").html('Esta seguro que desea eliminar la Categoria  '
        + "'" + elemento.Nombre + "'    ?");
    $(modal).find(".modal-body").css({ 'min-height': 100 + "px" });
    $(modal).modal({ backdrop: 'static', keyboard: false });
    $("#btnConfirmarEliminarCategoria").unbind('click').click(function () {
        confirmarEliminarCategoria(e, elemento);
    });
}
function editarDatosCategoria(elemento) {
    $("#txtNombreCategoria").val(elemento.Nombre);
    $("#txtDescripcionCategoria").val(elemento.Descripcion);
    $("#btnAbmGuardarCategoria").hide();
    $("#btnAbmEditarCategoria").show();
}

function eventoActualizarCategoria(input, elemento) {
    $(input).unbind('click').click(function () {
        var modal = '#agregarAbmCategoria';
        editarDatosCategoria(elemento);
        $(modal).find(".modal-title").html('Editar Categoria :   ' + "'" + elemento.Nombre + "'");
        $(modal).find(".modal-dialog").css({ 'width': 700 + "px" });
        $(modal).modal({ backdrop: 'static', keyboard: false });
        $("#btnAbmEditarCategoria").unbind('click').click(function (event) {
            event.preventDefault();
            guardarCategoria(elemento.IdCategoria, elemento);
        });
    });
}
function guardarCategoriaExitoso(respuesta, elemento) {
    $("#agregarAbmCategoria").modal("hide");
    if (respuesta.Success) {
        if (!elemento) {
            var categoria = {
                IdCategoria: parseInt(respuesta.Data),
                Nombre: $("#txtNombreCategoria").val(),
                Descripcion: $("#txtDescripcionCategoria").val()
            };
            _datosCategoria.push(categoria);
        } else {
            elemento.Nombre = $("#txtNombreCategoria").val();
            elemento.Descripcion = $("#txtDescripcionCategoria").val();
        }
        mostrarDatosCategoria();
        toastr.success("La Categoria se ha guardado satisfactoriamente ");
    } else {
        toastr.error(respuesta.Mensaje);
    }
}

function guardarCategoria(id, elemento) {
    var url = "/Categoria/GuardarCategoria";
    var tipo = 'GET';
    var datos = {
        id: id, nombre: $("#txtNombreCategoria").val(), descripcion:
            $("#txtDescripcionCategoria").val()
    };
    var tipoDatos = 'JSON';
    solicitudAjax(url, function (response) { guardarCategoriaExitoso(response, elemento); }
        , datos, tipoDatos, tipo);
}

$("#btnAbmGuardarCategoria").click(function () {
    guardarCategoria(0);
});

function limpiarDatosCategoria() {
    $("#txtNombreCategoria").val("");
    $("#txtDescripcionCategoria").val("");
    $("#btnAbmEditarCategoria").hide();
    $("#btnAbmGuardarCategoria").show();
}

$("#btnAdicionarCategoria").click(function () {
    var modal = '#agregarAbmCategoria';
    limpiarDatosCategoria();
    $(modal).find(".modal-title").html('Adicionar Categoria');
    $(modal).find(".modal-dialog").css({ 'width': 700 + "px" });
    $(modal).find(".modal-body").css({ 'min-height': 150 + "px" });
    $(modal).modal({ backdrop: 'static', keyboard: false });
});

function mostrarDatosCategoria() {
    limpiarTabla("tblCategoria");
    $.each(_datosCategoria, function (index, elemento) {
        var fila = $('<tr>').attr('id', elemento.IdCategoria);
        var input = crearSpan('lblEditCategoria' + index, 'spanHyperLink', elemento.Nombre);
        eventoActualizarCategoria(input, elemento);
        fila.append(col(input));
        fila.append(col(elemento.Descripcion));
        fila.append(col(AccionColumna(function (e) { mostrarModalEliminarCategoria(e, elemento) }, 'trash', 'eliminar')).addClass('alinearCentro'));
        $('#tblCategoria tbody').append(fila);
    });
}

function obtenerCategoriaExitoso(resultado) {
    if (resultado.Success) {
        _datosCategoria = resultado.Data;
        mostrarDatosCategoria();

    } else {
        toastr.error(resultado.Mensaje);
    }
}
function init() {
    var url = "/Categoria/ObtenerCategorias";
    var tipo = "GET";
    var datos = {};
    var tipoDatos = "JSON";
    solicitudAjax(url, obtenerCategoriaExitoso, datos, tipoDatos, tipo);
}
$(document).ready(function () {
    init();
});