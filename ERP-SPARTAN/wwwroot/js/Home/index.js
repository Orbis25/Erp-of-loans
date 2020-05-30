


/*LINE CHARTJS*/
fetch('/Loan/GetLoanByMonth').then((result) => result.json()).then((response) => {
    var data = response.map(x => x.quantity);
    var labels = response.map(x => x.month);
    /*Home chartjs*/
    const ctx = document.getElementById('chartv1').getContext('2d');
    const myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: labels,//['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            datasets: [{
                label: 'Prestamos',
                data: data,
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)',
                    'rgba(255, 100, 54, 0.2)',
                    'rgba(100, 100, 54, 0.5)',
                    'rgba(76, 200, 54, 0.5)',
                    'rgba(89, 150, 54, 0.5)',
                    'rgba(125, 60, 68, 0.5)',
                    'rgba(222, 69, 68, 0.5)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)',
                    'rgba(255, 100, 54, 0.2)',
                    'rgba(100, 100, 54, 0.5)',
                    'rgba(76, 200, 54, 0.5)',
                    'rgba(89, 150, 54, 0.5)',
                    'rgba(125, 60, 68, 0.5)',
                    'rgba(222, 69, 68, 0.5)'
                ],
                borderWidth: 2
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        beginAtZero: true
                    }
                }]
            }
        }
    });
    /*En home*/
});
/*END LINE CHARTJS*/



/**PIE CHART*/
fetch("/Loan/GetBadAndGoodPayments").then(result => result.json()).then((response) => {
    if (response !== null) {
        getPie(response.good, response.bad);
    }
});


const getPie = (goods, bads) => {
    const pie = document.getElementById('pie').getContext('2d');
    const myPieChart = new Chart(pie, {
        type: 'pie',
        data: {
            labels: [
                'Solventes',
                'Morosos'
            ],
            datasets: [{
                data: [goods, bads],

                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)'
                ],
                borderColor: [
                    'rgba(255, 99, 132, 1)',
                    'rgba(54, 162, 235, 1)'
                ],

            }]
        }
        //options: options
    });
};
/**END PIE CHART*/