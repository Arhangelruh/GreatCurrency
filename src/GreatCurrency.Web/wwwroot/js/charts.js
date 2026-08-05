var viewModelData = JSON.parse(document.getElementById('requestModelData').value);
var comparePercent = 0.0005;

async function showCharts() {
    var link = "/Chart/GetStockRates?";
    const params = new URLSearchParams(viewModelData);
    const responce = await fetch(link + params,
        {
            method: "Get"
        })
        .then(responce => responce.json())
    responce.forEach(element => {
        if (element.name == "USD") {
            const usdChart = new Chart(
                document.getElementById('USD'),
                {
                    type: 'line',
                    data: {
                        labels: element.list.map(row => row.time),
                        datasets: [{
                            label: 'Курс продажи',
                            data: element.list.map(row => row.ourSellRate === 0 ? null : row.ourSellRate),
                            fill: false,
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate),
                            fill: false,
                            borderColor: 'rgb(235, 52, 140)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс покупки',
                            data: element.list.map(row => row.ourBuyRate === 0 ? null : row.ourBuyRate),
                            fill: false,
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам +' + comparePercent * 100 + '%',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate + row.dealRate * comparePercent),
                            fill: false,
                            borderColor: 'rgb(79, 19, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам -' + comparePercent * 100 + '%',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate - row.dealRate * comparePercent),
                            fill: false,
                            borderColor: 'rgb(79, 19, 192)',
                            tension: 0.1
                        }
                        ]
                    },
                    options: {
                        plugins: {
                            title: {
                                display: true,
                                text: 'График USD курсов'
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        return `${context.dataset.label}: ${context.parsed.y.toFixed(4)}`;
                                    }
                                }
                            }
                        },
                        scales: {
                            y: {
                                ticks: {
                                    callback(value) {
                                        return value.toFixed(4);
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }
        if (element.name == "EUR") {
            const usdChart = new Chart(
                document.getElementById('EUR'),
                {
                    type: 'line',
                    data: {
                        labels: element.list.map(row => row.time),
                        datasets: [{
                            label: 'Курс продажи',
                            data: element.list.map(row => row.ourSellRate === 0 ? null : row.ourSellRate),
                            fill: false,
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate),
                            fill: false,
                            borderColor: 'rgb(235, 52, 140)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс покупки',
                            data: element.list.map(row => row.ourBuyRate === 0 ? null : row.ourBuyRate),
                            fill: false,
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам +' + comparePercent * 100 + '%',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate + row.dealRate * comparePercent),
                            fill: false,
                            borderColor: 'rgb(79, 19, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам -' + comparePercent * 100 + '%',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate - row.dealRate * comparePercent),
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
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        return `${context.dataset.label}: ${context.parsed.y.toFixed(4)}`;
                                    }
                                }
                            }
                        },
                        scales: {
                            y: {
                                ticks: {
                                    callback(value) {
                                        return value.toFixed(4);
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }
        if (element.name == "RUB") {
            const usdChart = new Chart(
                document.getElementById('RUB'),
                {
                    type: 'line',
                    data: {
                        labels: element.list.map(row => row.time),
                        datasets: [{
                            label: 'Курс продажи',
                            data: element.list.map(row => row.ourSellRate === 0 ? null : row.ourSellRate * 100),
                            fill: false,
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate * 100),
                            fill: false,
                            borderColor: 'rgb(235, 52, 140)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс покупки',
                            data: element.list.map(row => row.ourBuyRate === 0 ? null : row.ourBuyRate * 100),
                            fill: false,
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам +' + comparePercent * 100 + '%',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate * 100 + row.dealRate * 100 * comparePercent),
                            fill: false,
                            borderColor: 'rgb(79, 19, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам -' + comparePercent * 100 + '%',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate * 100 - row.dealRate * 100 * comparePercent),
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
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        return `${context.dataset.label}: ${context.parsed.y.toFixed(4)}`;
                                    }
                                }
                            }
                        },
                        scales: {
                            y: {
                                ticks: {
                                    callback(value) {
                                        return value.toFixed(4);
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }
        if (element.name == "CNY") {
            const usdChart = new Chart(
                document.getElementById('CNY'),
                {
                    type: 'line',
                    data: {
                        labels: element.list.map(row => row.time),
                        datasets: [{
                            label: 'Курс продажи',
                            data: element.list.map(row => row.ourSellRate === 0 ? null : row.ourSellRate * 10),
                            fill: false,
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate * 10),
                            fill: false,
                            borderColor: 'rgb(235, 52, 140)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс покупки',
                            data: element.list.map(row => row.ourBuyRate === 0 ? null : row.ourBuyRate * 10),
                            fill: false,
                            borderColor: 'rgb(75, 192, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам +' + comparePercent * 100 + '%',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate * 10 + row.dealRate * 10 * comparePercent),
                            fill: false,
                            borderColor: 'rgb(79, 19, 192)',
                            tension: 0.1
                        },
                        {
                            label: 'Курс биржи по сделкам -' + comparePercent * 100 + '%',
                            data: element.list.map(row => row.dealRate === 0 ? null : row.dealRate * 10 - row.dealRate * 10 * comparePercent),
                            fill: false,
                            borderColor: 'rgb(79, 19, 192)',
                            tension: 0.1
                        }]
                    },
                    options: {
                        plugins: {
                            title: {
                                display: true,
                                text: 'График CNY курсов'
                            },
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        return `${context.dataset.label}: ${context.parsed.y.toFixed(4)}`;
                                    }
                                }
                            }
                        },
                        scales: {
                            y: {
                                ticks: {
                                    callback(value) {
                                        return value.toFixed(4);
                                    }
                                }
                            }
                        }
                    }
                }
            );
        }
    });
}

document.addEventListener("DOMContentLoaded", showCharts);