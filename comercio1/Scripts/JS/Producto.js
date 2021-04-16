var _datosProducto;
var _datosCategoria;
function eliminarProductoExitoso(resultado, e, elemento) {
    $("#eliminarProducto").modal("hide");
    if (resultado.Success) {
        $(e.currentTarget).closest('tr').remove();
        _datosProducto.remove(elemento);
        toastr.success("La Producto'" + elemento.Nombre + " '   se ha eliminado satisfactoriamente");
    } else {
        toastr.error(resultado.Mensaje);
    }
}

function confirmarEliminarProducto(e, elemento) {
    var url = "/Producto/EliminarProducto";
    var tipo = 'GET';
    var datos = { id: elemento.IdProducto };
    var tipoDatos = 'JSON';
    solicitudAjax(url, function (response) { eliminarProductoExitoso(response, e, elemento); }, datos, tipoDatos, tipo);
}

function mostrarModalEliminarProducto(e, elemento) {
    var modal = '#eliminarProducto';
    $(modal).find(".modal-title").html('Eliminar Producto');
    $(modal).find(".text-mensaje-modal").html('Esta seguro que desea eliminar la Producto  '
        + "'" + elemento.Nombre + "'    ?");
    $(modal).find(".modal-body").css({ 'min-height': 100 + "px" });
    $(modal).modal({ backdrop: 'static', keyboard: false });
    $("#btnConfirmarEliminarProducto").unbind('click').click(function () {
        confirmarEliminarProducto(e, elemento);
    });
}
function editarDatosProducto(elemento) {
    $("#txtNombreProducto").val(elemento.Nombre);
    $("#txtDescripcionProducto").val(elemento.Descripcion);
    $("#txtPrecioProducto").val(elemento.Precio);
    $("#txtStockProducto").val(elemento.Stock);
    $("#cmbCategoria").val(elemento.IdCategoria);
    $("#btnAbmGuardarProducto").hide();
    $("#btnAbmEditarProducto").show();
}

function eventoActualizarProducto(input, elemento) {
    $(input).unbind('click').click(function () {
        var modal = '#agregarAbmProducto';
        editarDatosProducto(elemento);
        $(modal).find(".modal-title").html('Editar Producto :   ' + "'" + elemento.Nombre + "'");
        $(modal).find(".modal-dialog").css({ 'width': 700 + "px" });
        $(modal).modal({ backdrop: 'static', keyboard: false });
        $("#btnAbmEditarProducto").unbind('click').click(function (event) {
            event.preventDefault();
            guardarProducto(elemento.IdProducto, elemento);
        });
    });
}
function guardarProductoExitoso(respuesta, elemento) {
    $("#agregarAbmProducto").modal("hide");
    if (respuesta.Success) {
        if (!elemento) {
            var producto = {
                IdProducto: parseInt(respuesta.Data),
                Nombre: $("#txtNombreProducto").val(),
                Descripcion: $("#txtDescripcionProducto").val(),
                Precio: $("#txtPrecioProducto").val(),
                Stock: $("#txtStockProducto").val(),
                IdCategoria: $("#cmbCategoria").val(),
                Categoria: $("#cmbCategoria option:selected").html()
            };
            _datosProducto.push(producto);
        } else {
            elemento.Nombre = $("#txtNombreProducto").val();
            elemento.Descripcion = $("#txtDescripcionProducto").val();
            elemento.Precio = $("#txtPrecioProducto").val();
            elemento.Stock = $("#txtStockProducto").val();
            elemento.IdCategoria = $("#cmbCategoria").val();
            elemento.Categoria = $("#cmbCategoria option:selected").html()
        }
        mostrarDatosProducto();
        toastr.success("La Producto se ha guardado satisfactoriamente ");
    } else {
        toastr.error(respuesta.Mensaje);
    }
}

function guardarProducto(id, elemento) {
    var url = "/Producto/GuardarProducto";
    var tipo = 'GET';
    var datos = {
        idProducto: id,
        nombre: $("#txtNombreProducto").val(),
        descripcion: $("#txtDescripcionProducto").val(),
        precio: $("#txtPrecioProducto").val(),
        stock: $("#txtStockProducto").val(),
        idCategoria: $("#cmbCategoria").val()
    };
    var tipoDatos = 'JSON';
    solicitudAjax(url, function (response) { guardarProductoExitoso(response, elemento); }
        , datos, tipoDatos, tipo);
}

$("#btnAbmGuardarProducto").click(function () {
    guardarProducto(0);
});

function limpiarDatosProducto() {
    $("#txtNombreProducto").val("");
    $("#txtDescripcionProducto").val("");
    $("#txtPrecioProducto").val("");
    $("#txtStockProducto").val("");
    $("#btnAbmEditarProducto").hide();
    $("#btnAbmGuardarProducto").show();
}

$("#btnAdicionarProducto").click(function () {
    var modal = '#agregarAbmProducto';
    limpiarDatosProducto();
    $(modal).find(".modal-title").html('Adicionar Producto');
    $(modal).find(".modal-dialog").css({ 'width': 700 + "px" });
    $(modal).find(".modal-body").css({ 'min-height': 150 + "px" });
    $(modal).modal({ backdrop: 'static', keyboard: false });
});

function mostrarDatosProducto() {
    limpiarTabla("tblProducto");
    $.each(_datosProducto, function (index, elemento) {
        var fila = $('<tr>').attr('id', elemento.IdProducto);
        var input = crearSpan('lblEditProducto' + index, 'spanHyperLink', elemento.Nombre);
        eventoActualizarProducto(input, elemento);
        fila.append(col(input));
        fila.append(col(elemento.Descripcion));
        fila.append(col(elemento.Precio));
        fila.append(col(elemento.Stock));
        fila.append(col(elemento.Categoria));
        fila.append(col(AccionColumna(function (e) { mostrarModalEliminarProducto(e, elemento) }, 'trash', 'eliminar')).addClass('alinearCentro'));
        $('#tblProducto tbody').append(fila);
    });
}
function cargarComboCategoria() {
    var prop = { id: 'IdCategoria', value: 'Nombre' };
    adicionarOpcionesCombo($("#cmbCategoria"), _datosCategoria, '', prop, false);
}

function obtenerProductoExitoso(resultado) {
    if (resultado.Success) {
        _datosProducto = resultado.Data.Productos;
        _datosCategoria = resultado.Data.Categorias;
        mostrarDatosProducto();
        cargarComboCategoria();

    } else {
        toastr.error(resultado.Mensaje);
    }
}
function init() {
    var url = "/Producto/ObtenerProductos";
    var tipo = "GET";
    var datos = {};
    var tipoDatos = "JSON";
    solicitudAjax(url, obtenerProductoExitoso, datos, tipoDatos, tipo);
}
$(document).ready(function () {
    init();
});