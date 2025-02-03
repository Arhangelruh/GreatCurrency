var viewModelData = JSON.parse(document.getElementById('requestModelData').value);
var chartsarea =  document.getElementById('USDList');

async function showCharts(){        
    var link = "/Charts/GetLineData?";   
    const params = new URLSearchParams(viewModelData);
    const responce = await fetch(link+params,
    {
      method:"Get"
    })
    .then(responce => responce.json())
    responce.forEach(element => {
      if(element.name == "USDList"){
        const usdChart = new Chart(
          document.getElementById('USDList'),
           {             
            type:'line',
             data: {                            
              labels: element.list.map(row=>row.time),
              datasets: [{
                label: 'Курс покупки у банка',
                data: element.list.map(row=>row.ourSaleRate),
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                tension: 0.1
              },
              {
                label: 'Лучший курс покупки',
                data: element.list.map(row=>row.bestSaleRate),
                fill: false,
                borderColor: 'rgb(235, 52, 140)',
                tension: 0.1
              },
              {
                label: 'Курс продажи банку',
                data: element.list.map(row=>row.ourBuyRate),
                fill: false,
                borderColor: 'rgb(211, 52, 235)',
                tension: 0.1
              },
              {
                label: 'Лучший курс продажи',
                data: element.list.map(row=>row.bestBuyRate),
                fill: false,
                borderColor: 'rgb(79, 19, 192)',
                tension: 0.1
              }]
             },
             options: {
              plugins: {
                  title: {
                      display: true,
                      text: 'График USD курсов'
                  }
              }
            }
          }
        );
      }
      if(element.name == "EURList"){
        const usdChart = new Chart(
          document.getElementById('EURList'),
           {             
            type:'line',
             data: {                            
              labels: element.list.map(row=>row.time),
              datasets: [{
                label: 'Курс покупки у банка',
                data: element.list.map(row=>row.ourSaleRate),
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                tension: 0.1
              },
              {
                label: 'Лучший курс покупки',
                data: element.list.map(row=>row.bestSaleRate),
                fill: false,
                borderColor: 'rgb(235, 52, 140)',
                tension: 0.1
              },
              {
                label: 'Курс продажи банку',
                data: element.list.map(row=>row.ourBuyRate),
                fill: false,
                borderColor: 'rgb(211, 52, 235)',
                tension: 0.1
              },
              {
                label: 'Лучший курс продажи',
                data: element.list.map(row=>row.bestBuyRate),
                fill: false,
                borderColor: 'rgb(79, 19, 192)',
                tension: 0.1
              }]
             },
             options: {
              plugins: {
                  title: {
                      display: true,
                      text: 'График EUR курсов'
                  }
              }
            }
          }
        );
      }
      if(element.name == "RUBList"){
        const usdChart = new Chart(
          document.getElementById('RUBList'),
           {             
            type:'line',
             data: {                            
              labels: element.list.map(row=>row.time),
              datasets: [{
                label: 'Курс покупки у банка',
                data: element.list.map(row=>row.ourSaleRate),
                fill: false,
                borderColor: 'rgb(75, 192, 192)',
                tension: 0.1
              },
              {
                label: 'Лучший курс покупки',
                data: element.list.map(row=>row.bestSaleRate),
                fill: false,
                borderColor: 'rgb(235, 52, 140)',
                tension: 0.1
              },
              {
                label: 'Курс продажи банку',
                data: element.list.map(row=>row.ourBuyRate),
                fill: false,
                borderColor: 'rgb(211, 52, 235)',
                tension: 0.1
              },
              {
                label: 'Лучший курс продажи',
                data: element.list.map(row=>row.bestBuyRate),
                fill: false,
                borderColor: 'rgb(79, 19, 192)',
                tension: 0.1
              }]
             },
             options: {
              plugins: {
                  title: {
                      display: true,
                      text: 'График RUB курсов'
                  }
              }
            }
          }
        );
      }
    });
}

document.addEventListener("DOMContentLoaded", showCharts);
