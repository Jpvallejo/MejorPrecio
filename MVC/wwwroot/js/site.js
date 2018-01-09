// Write your JavaScript code.

$(document).ready(function () { 
    Initialize(); 
}); 

// Where all the fun happens 
function Initialize() { 

    // Google has tweaked their interface somewhat - this tells the api to use that new UI 
    google.maps.visualRefresh = true; 
    var BsAs = new google.maps.LatLng(-34.603722, -58.381592); 

    // These are options that set initial zoom level, where the map is centered globally to start, and the type of map to show 
    var mapOptions = { 
        zoom: 12, 
        center: BsAs, 
        mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP ,
        zoomControl: true,
        mapTypeControl: true,
        scaleControl: true,
        streetViewControl: true,
        rotateControl: true,
        fullscreenControl: true

    }; 

    // This makes the div with id "map_canvas" a google map 
    var map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions); 
    
} 



