﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('.data-table').DataTable({ pageLength : 10 });
});



$('#phoneNumber').mask('000-000-0000');
$('.decimal').mask("#,##0.00", { reverse: true });

