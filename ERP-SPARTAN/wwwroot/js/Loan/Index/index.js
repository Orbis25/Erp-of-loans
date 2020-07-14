// Advertencia para marcar y desmarcar todos los prestamos como si estuvieran al dia */
const setAllIsUpToUpadateLoans = (e) => {
    Swal.fire({
        title: "Advertencia",
        text: "Cuidado!, va a marcar o desmarcar todos los prestamos al dia",
        icon: "warning"
    }).then((result) => {
        if (result.value) {
            $('#DisableIsUpToDateAll').submit();
        } else {
            const isCheked = $('#checkAll').is(':checked');
            if (isCheked) {
                $('#checkAll').prop("checked", false);
            } else {
                $('#checkAll').prop("checked", true);
            }
        }
    });
};

/**
 * Obtiene los prestamos saldados y lo asigna al tab de saldados
 * */
const getSoldOut = () => {
    fetch("/loan/GetAllSoldOut").then((response) => response.text()).then((result) => {
        $('#soldOutContent').empty();
        $('.loading').hide();
        $("#soldOutContent").append(result);
    });
};


/**
 * Obtiene los prestamos saldados por reenganche y lo asigna al tab de saldados  por reenganche
 * */
const getSoldOutReenclosing = () => {
    fetch("/loan/GetAllRenclosing").then((response) => response.text()).then((result) => {
        $('#RenclosingContent').empty();
        $('.loading').hide();
        $("#RenclosingContent").append(result);
    });
};

const showLoading = () => {
    $('.loading').show();
};