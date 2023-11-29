var deleteArray = document.querySelectorAll(".Delete_City");

function check(){
   deleteArray.forEach(
       function (element){                             
       element.addEventListener("click",deleteRequest)}
   );
}

async function deleteRequest(){        
    var link = "/City/DeleteCity/"+ this.name;   
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
      addblock.innerHTML = "Удаление города не возможно, пока есть курсы привязанные к городу.";
      // addblock.innerHTML = "You can't delete Status. Because find request whith this status.";
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
       window.location.href='/City/Cities'
    }
}

document.addEventListener("DOMContentLoaded", check);