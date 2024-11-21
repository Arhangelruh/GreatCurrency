const timeList = document.querySelectorAll(".statistic");

function check(){
    timeList.forEach(
        function (element){                             
        element.addEventListener("click", showTimes)}
    );
 }

function showTimes(e){
    const item=e.target;
    
    if(item.classList[0]==="show_times"){
     const timeField = item.parentElement
     const timeTable = timeField.children[1];
     item.setAttribute("style","display:none;");
    timeTable.setAttribute("style","display:block;");
    };

    if(item.classList[0]==="hidden_times"){        
        const timeTable = item.parentElement;        
        timeTable.setAttribute("style","display:none;");
        const timeField = timeTable.parentElement;
        timeField.children[0].setAttribute("style","display:block;");
    };
}

document.addEventListener("DOMContentLoaded", check);