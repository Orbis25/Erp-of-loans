﻿/**
 * Dialogo de confirmacion para eliminar
 * @param {any} id,
 * @param {any} controller,
 * @param {any} action,
 * @param {any} text,
 * @param {any} title,
 */
const ShowSweetConfirmRemoveDialog = (id, controller, action, text = "esta operación no puede ser revertida", title = "Seguro que desea eliminarlo?") => {
    Swal.fire({
        title: title,
        text: text,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Eliminar'
    }).then((result) => {
        if (result.value) {
            fetch(`${controller}/${action}?id=${id}`, {
                method: 'post',
                body: JSON.stringify(id)
            }).then((response) => {
                if (response.status === 200) {
                    Swal.fire("Removido con exito", '', 'success').then(result => {
                        location.reload();
                    });
                } else {
                    Swal.fire("Ocurrio un error", "intente de nuevo", 'error');
                }
            });
        }
    });
};

const ShowSweetConfirmEnableOrDisableUser = (id, controller, action, text = "esta operación no puede ser revertida", title = "Seguro que deseas realizar esta operación?") => {
    Swal.fire({
        title: title,
        text: text,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Confirmar'
    }).then((result) => {
        if (result.value) {
            fetch(`${controller}/${action}?id=${id}`, {
                method: 'post',
                body: JSON.stringify(id)
            }).then((response) => {
                if (response.status === 200) {
                    Swal.fire("Operación realizada con exito", '', 'success').then(result => {
                        location.reload();
                    });
                } else {
                    Swal.fire("Ocurrio un error", "intente de nuevo", 'error');
                }
            });
        }
    });
};