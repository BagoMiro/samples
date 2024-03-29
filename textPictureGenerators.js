// Generates pixel art using filled-rectangle font
function generatePixelArt(){
  //showLoadingThings();
  var canvasContext = document.getElementById("canvas").getContext("2d"); // get context from canvas

  // get average of several pixels in a rectangle shape
  var verticalSize = parseInt(document.getElementById("sampling").value) * 2;
  var horizontalSize = parseInt(document.getElementById("sampling").value);
  var finalString = "";
  var pixelValues = "";
  if(document.getElementById("newLineAtStart").checked){
    // add extra new line character at the start of the string
    finalString += "<br";
  }
  for( var y=0; y < document.getElementById("canvasHeight").value; y += verticalSize){
    for( var x=0; x < document.getElementById("canvasWidth").value; x += horizontalSize){
      var pixels = canvasContext.getImageData(x, y, horizontalSize, verticalSize);
      var data = pixels.data;
      var red = 0;
      var green = 0;
      var blue = 0;
      var aRed = 0; //average
      var aGreen = 0; //average
      var aBlue = 0; // average

      var pixelValue = 0;
      for(var i = 0; i < horizontalSize*verticalSize*4; i+=4){
        red += data[i];
        green += data[i+1];
        blue += data[i+2];
        pixelValues = "R:" + String(red) + "<br>";
      }

      // average values
      aRed = red / (horizontalSize*verticalSize);
      aGreen = green / (horizontalSize*verticalSize);
      aBlue = blue / (horizontalSize*verticalSize);

      pixelValue = (red + green + blue) / (horizontalSize*verticalSize*3);

      // add a "pixel" into the final string
      if(document.getElementById("useColor").checked){
        finalString += "<span style='color: #" + (aRed.toString(16)).substring(0,2) + (aGreen.toString(16)).substring(0,2) + (aBlue.toString(16)).substring(0,2) +"'>█</span>";
      } else if(document.getElementById("useAnalyzer").checked){ 
        // analyze picture
          // detect shapes
          // go from left to right
          
          // check pixels around it that were already checkced (one behind and 3 above if they exist)
        
          // if match of the same color was found assign it to object that already was detected
        
          // after all objects or shapes are identified check how many of them will be deformed in final product which will lose many details
        
          // compromise with what to draw since not everything will be shown
        
          // fix shapes
        
          // color shapes apropriatelly to their original shade of color and also use 4 color mapping 
          // so that no two shapes of very similar colorare of the same color
      } else {
        if( pixelValue > 180){
          finalString += "░";
        }else if( pixelValue > 120){
          finalString += "▒";
        }else if( pixelValue > 60){
          finalString += "▒";
        }else{
          finalString += "█";
        }
      }
    }

    finalString += "<br>";
  }

  // add an extra line with website for reference
  if(document.getElementById("addWebsite").checked){
    var typeThis = "textpixelimage.com";
    for( var x=0; x < document.getElementById("canvasWidth").value; x += horizontalSize){
      if(typeThis.length > 0){
        if( (x/horizontalSize)%2 == 0 ){
          finalString += " ";
        }else{
          finalString += typeThis.charAt(0);
          typeThis = typeThis.substring(1);
        }
      }
    }
  }
  //hideLoadingThings();
  document.getElementById("textPicture").innerHTML = finalString; 
  document.getElementById("h2text").innerHTML = "Generated Text-Pixel Image :";
}

function showLoadingThings(){
  document.getElementById("leftSpinner").style.display = "inherit";
  document.getElementById("workingOnIt").style.display = "inherit";
}

function hideLoadingThings(){
  document.getElementById("leftSpinner").style.display = "none";
  document.getElementById("workingOnIt").style.display = "none";
}

// will copy the code into clipboard and removes the <BR> tags from it as well
function copyTextToClipboard(){
  document.getElementById("copyArea").value = document.getElementById("textPicture").innerHTML;
  var endwhile = 0
  var str = document.getElementById("copyArea").value;
  var text = str;

  if(document.getElementById("useColor").checked == false){
    while(endwhile == 0){
      var tempText = text;
      text = text.replace("<br>","\r\n");
      if(text == tempText){
        endwhile = 1; 
      }
    }
  }
  document.getElementById("copyArea").style.display = "inherit";
  document.getElementById("copyArea").value = text;
  document.getElementById("copyArea").select();
  document.execCommand("copy");
  document.getElementById("copyArea").style.display = "none";
}