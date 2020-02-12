var MapsControl = MapsControl || {};
MapsControl.map = {};
MapsControl.draw = function (centerx, centery, markers) {
    console.log(centerx + " " + centery);
    console.log("Map Init");
    MapsControl.map = new atlas.Map('vendingMap', {
        view: "Auto",
        center: [centerx, centery],
        zoom: 8,
        language: 'it-IT',
        authOptions: {
            authType: 'subscriptionKey',
            subscriptionKey: '57P8hPINbY_s3f83rTLqUAXimdz2S41zWPa9Jl-Fg44'
        }
    });

    MapsControl.map.events.add('ready', function () {
        console.log("Map Ready");
        console.log(JSON.stringify(markers));
        for (var i = 0; i < markers.length; i++) {
            var marker = new atlas.HtmlMarker({
                color: 'DodgerBlue',
                text: markers[i].id,
                position: [markers[i].x, markers[i].y],
                popup: new atlas.Popup({
                    content: '<div style="padding:10px"><a href="/' + markers[i].id + '/products">Machine ID: ' + markers[i].id + '</a></div>',
                    pixelOffset: [0, -30]
                })
            });
            MapsControl.map.markers.add(marker);

            MapsControl.map.events.add('click', marker, () => {
                marker.togglePopup();
            });

            console.log("Map Pin Loaded");
        }
    }.bind(this));   
}

//MapsControl.AddVendingMachine = function (x, y, vendingMachineId) {
//    MapsControl.map.events.add('ready', function () {
//        console.log("Marker: " + x + ' ' + y + ' ' + vendingMachineId);
//        console.log('<div style="padding:10px"><a href="/' + vendingMachineId + '/products">Machine ID: ' + vendingMachineId + '</a></div>');

//        var marker = new atlas.HtmlMarker({
//            color: 'DodgerBlue',
//            text: vendingMachineId,
//            position: [x, y],
//            popup: new atlas.Popup({
//                content: '<div style="padding:10px">jj</div>',
//                pixelOffset: [0, -30]
//            })
//        });
//        MapsControl.map.markers.add(marker);

//        MapsControl.map.events.add('click', marker, () => {
//            marker.togglePopup();
//        });
//    });
//}