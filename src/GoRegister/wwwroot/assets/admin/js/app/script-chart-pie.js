document.addEventListener('DOMContentLoaded', (event) => {

    var elementchartdata = document.getElementById("piechartdata");    

    var totalaccepted = elementchartdata.attributes["data-totalaccepted"].value;
    var totaldeclined = elementchartdata.attributes["data-totaldeclined"].value;
    var totalcancelled = elementchartdata.attributes["data-totalcancelled"].value;
    var cutoutper = elementchartdata.attributes["data-cutoutper"].value;

    // Pie Chart Example
    var ctx = document.getElementById("pieChart");
    var pieChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: ["Accepted", "Declined", "Cancelled"],
            datasets: [{
                data: [totalaccepted, totaldeclined, totalcancelled],
    backgroundColor: ['#1cc88a', '#36b9cc', '#f6c23e'],
        hoverBackgroundColor: ['#1cc88a', '#36b9cc', '#f6c23e'],
            hoverBorderColor: "rgba(234, 236, 244, 1)"
}]
                },
    options: {
    maintainAspectRatio: false,
    tooltips: {
        backgroundColor: "rgb(255,255,255)",
        bodyFontColor: "#858796",
        borderColor: '#dddfeb',
        borderWidth: 1,
        xPadding: 15,
        yPadding: 15,
        displayColors: false,
        caretPadding: 10
    },
    legend: {
        display: false
    },
    cutoutPercentage: cutoutper
                }
            });

        });
