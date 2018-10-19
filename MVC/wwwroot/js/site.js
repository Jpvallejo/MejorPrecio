// Write your JavaScript code.
var map;
var markers = [];
$(document).ready(function() {
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
    mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP,
    zoomControl: true,
    mapTypeControl: true,
    scaleControl: true,
    streetViewControl: true,
    rotateControl: true,
    fullscreenControl: true
  };

  // This makes the div with id "map_canvas" a google map
  map = new google.maps.Map(document.getElementById("map_canvas"), mapOptions);
}

function GetDataName(name) {
  var tmp = null;
  $.ajax({
    dataType: "json",
    type: "GET",
    async: false,
    url: "http://localhost:5000/Prices/SearchWithName?name=" + name,
    success: function(result) {
      tmp = result;
    }
  });
  return tmp;
}

function GetDataBarcode(barcode) {
    var tmp = null;
    $.ajax({
      dataType: "json",
      type: "GET",
      async: false,
      url: "http://localhost:5000/Prices/SearchWithBarcode?barcode=" + barcode,
      success: function(result) {
        tmp = result;
      }
    });
    return tmp;
  }

function SearchWithName(name) {
    clearMarkers();
  var data = GetDataName(name);
  if(data.length == 0){
    document.getElementById("modalButton").click();
  }
  else{
    DrawMarkers(data);
  }
}

function SearchWithBarcode(barcode){
    clearMarkers();
    var data = GetDataBarcode(barcode);
    if(data.length == 0){
      document.getElementById("modalButton").click();
    }
    else{
      DrawMarkers(data);
    }
}

function DrawMarkers(data) {
  var labels = "12345";
  var labelIndex = 0;

  for (var i = 0, length = data.length; i < length; i++) {
    // addMarkerWithTimeout(data[i], 20);

    var marker = new google.maps.Marker({
      position: new google.maps.LatLng(data[i].latitude, data[i].longitude),
      map: map,
      animation: google.maps.Animation.DROP,
      label: labels[labelIndex++ % labels.length],
      title: data[i].price
    });
    var contentString =
      "<div class='infoDiv'><h3>" +
      data[i].productName +
      "</h3><h4><em>" +
      data[i].productBrand +
      " - " +
      data[i].productBarcode +
      "</em></h4><p>Aqui el producto se encuentra a <em>$" +
      data[i].price +
      '</em></p> <br> <a href="https://www.google.com/maps/search/?api=1&query=' +
      data[i].adress+
      '" target = "_blank">Ver en Google Maps </a></div>';
    google.maps.event.addListener(
      marker,
      "click",
      getInfoCallback(map, contentString + data[i].adress)
    );
    markers.push(marker);
    function getInfoCallback(map, content) {
      var infowindow = new google.maps.InfoWindow({ content: content });
      return function() {
        infowindow.setContent(content);
        infowindow.open(map, this);
      };
    }
  }
}


function clearMarkers() {
    for (var i = 0; i < markers.length; i++) {
      markers[i].setMap(null);
    }
    markers = [];
  }
