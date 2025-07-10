var deleteArray = document.querySelectorAll(".Delete_Service");

function check(){
   deleteArray.forEach(
       function (element){                             
       element.addEventListener("click",deleteRequest)}
   );
}

async function deleteRequest(){        
    var link = "/ServiceRates/DeleteService?serviceid="+ this.name;   
    var el = this; 
    const responce = await fetch(link,
    {
      method:"Get"
    }
    )
    .then(responce => responce.json())
        if (responce == "error"){              
      var checkdiv =  el.querySelector(".tooltiptext");
      if(checkdiv == null){              
      let addblock = document.createElement('div')
      addblock.innerHTML = "Удаление сервиса не возможно, пока есть курсы привязанные к сервису.";
      addblock.className = "tooltiptext";
      addblock.style.display="inline";          
      el.append(addblock);
      
      document.onmouseout = function(e){
        if(addblock){                      
            el.removeChild(addblock);            
            addblock = null;
        }
      }   
     }        
    }
    else{
       window.location.href='/ServiceRates/Services'
    }
}

document.addEventListener("DOMContentLoaded", check);