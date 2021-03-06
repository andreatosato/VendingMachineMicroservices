﻿var MapsControl = MapsControl || {};
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
        console.log("Marker center" + centerx + " " + centery);
        // Position
        var markerCurrentPosition = new atlas.HtmlMarker({
            color: 'Red',
            text: 'Me',
            position: [centerx, centery],
        });
        console.log(MapsControl.map.markers);
        MapsControl.map.markers.add(markerCurrentPosition);

        // Vending Machines
        for (var i = 0; i < markers.length; i++) {
            var marker = new atlas.HtmlMarker({
                color: 'DodgerBlue',
                text: markers[i].id,
                position: [markers[i].x, markers[i].y],
                popup: new atlas.Popup({
                    content: '<div style="padding:10px"><a href="/products/' + markers[i].id + '">Machine ID: ' + markers[i].id + '</a></div>',
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