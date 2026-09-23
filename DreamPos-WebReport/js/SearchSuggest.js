// JScript File
var maxDivId,currentDivId,strOriginal;
 
//Our XmlHttpRequest object to get the auto suggestvar 
searchReq = getXmlHttpRequestObject();

function getXmlHttpRequestObject() 
{	
    if (window.XMLHttpRequest) 
    {
        return new XMLHttpRequest();	
    } 
    else if(window.ActiveXObject) 
    {
        return new ActiveXObject("Microsoft.XMLHTTP");	
    }
    else
    {
        alert("Time to upgrade your browser");	
      }
}

//Called from keyup on the search textbox.//Starts the AJAX request.
function searchSuggest(e) 
{

     

      var key = window.event ? e.keyCode : e.which;
      
      if (key==40 || key==38)
      {
       scrolldiv(key); 
      }
      else
      {
      if (searchReq.readyState == 4 || searchReq.readyState == 0) 
        {
        var str = escape(document.getElementById('txtSearch').value);
        strOriginal=str;
        searchReq.open("GET", 'Result.aspx?search=' + str, true);		
        searchReq.onreadystatechange = handleSearchSuggest;
       	searchReq.send(null);	
       	}		
       	
      } 	
      
      
       	
}
//Called when the AJAX response is returned.
function handleSearchSuggest()
 {
  if (searchReq.readyState == 4) 
      {
          var ss = document.getElementById('search_suggest');
          ss.innerHTML = '';
          var str = searchReq.responseText.split("~");
         
          if(str.length > 1)
          {
           
          	for(i=0; i < str.length - 1; i++) 
          	{
          	//Build our element string.  This is cleaner using the DOM, but			
          	//IE doesn't support dynamically added attributes.
       
          	   maxDivId=i;
          	   currentDivId=-1;
          	   var suggest =  '<div ';
          	       suggest += 'id=div'+ i;  
          	       suggest += '  '
          	       suggest += 'onmouseover="javascript:suggestOver(this);" ';
          	       suggest += 'onmouseout="javascript:suggestOut(this);" ';
          	       suggest += 'onclick="javascript:setSearch(this.innerHTML);" ';
          	       suggest += 'class="suggest_link">' + str[i] + '</div>';
          	       ss.innerHTML += suggest;
          	       ss.style.visibility='visible';
          	}	
           }
           else	
          	{
          	
          	    ss.style.visibility='hidden';
          	}
       }
       
 }
 
 //Mouse over function
 function suggestOver(div_value) 
 {	div_value.className = 'suggest_link_over';
   
 }
  
  function scrollOver(div_value) 
 {	
 
   div_value.className = 'suggest_link_over';
   document.getElementById('txtSearch').value=div_value.innerHTML;
 }
 
 //Mouse out function
 function suggestOut(div_value) 
 {
    div_value.className = 'suggest_link';
 }
 
 //Click function
 function setSearch(value) 
 {
    var ss = document.getElementById('search_suggest');
    
    document.getElementById('txtSearch').value = value;	
    ss.innerHTML = '';
    ss.style.visibility='hidden';
 }
 
 function scrolldiv(key)
 {
   var tempID;
   
 
    
    if(key==40)
    {
    
         if(currentDivId==-1)
         {
          scrollOver(div0);
          currentDivId=0;
         }
         else
         {
            if(currentDivId==maxDivId)
            {
                 tempID='div'+maxDivId;  
                 var a=document.getElementById(tempID);
                 currentDivId=-1;
                 suggestOut(a) 
                 document.getElementById('txtSearch').value=strOriginal;
            }
            else
            {
                tempID=currentDivId+1;
                setScroll(currentDivId,tempID)
            }
         
         }
    }
    else if (key==38)
    {
         if(currentDivId==-1)
         {
         tempID=maxDivId;
         setScroll(maxDivId,maxDivId) 
         }
         else
         {
         if (currentDivId==0)
            {
                 tempID='div'+currentDivId;  
                 var a=document.getElementById(tempID);
                 currentDivId=-1;
                 suggestOut(a) 
                 document.getElementById('txtSearch').value=strOriginal;
             
            }
          else
         {
            tempID=currentDivId-1;
            setScroll(currentDivId,tempID)  
            
         }
       
    } 
    
 
  }
 }
function setScroll(Old,New)
{
       var tempDivId; 
       currentDivId=New;
       
       tempDivId='div'+Old;  
       var a=document.getElementById(tempDivId);
       suggestOut(a)
       
       tempDivId='div'+currentDivId;
       var b=document.getElementById(tempDivId);
       scrollOver(b);

}
 
 
 
