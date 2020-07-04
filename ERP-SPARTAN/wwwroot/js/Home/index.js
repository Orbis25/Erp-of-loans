


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


/*Calcultorjs*/

const changeElementsInputs = () => {
    if (Number($('#amortitationType').val()) === 0) {
        $('#container-amountRate').show();
        $('#container-numberRate').hide();
    } else {
        $('#container-amountRate').hide();
        $('#container-numberRate').show();

    }

};

const getCalculatorResult = () => {
    const inputs = $('#calculator-form').find(":input");
    const amortizationType = inputs[0].value;
    const initCapital = inputs[1].value;
    const typeOfTasa = inputs[2].value;
    const interest = inputs[3].value;
    const paymentModality = inputs[5].value;

    const amountDeb = inputs[4].value;


    const shares = inputs[6].value;



    const querystr = `?InitialCapital=${initCapital}&ActualCapital=${initCapital}&Interest=${interest}
                        &Shares=${shares}&RateType=${typeOfTasa}&AmortitationType=${amortizationType}
                         &PaymentModality=${paymentModality}&AmountDeb=${amountDeb}&contractDate=${new Date()}&`;
    fetch(`/Loan/GetAmortization${querystr}"`).then(result => result.text()).then((response) => {
        $('#calculator-result').empty();
        $('#calculator-result').html(response);
    });
};


/*end calculatorjs*/