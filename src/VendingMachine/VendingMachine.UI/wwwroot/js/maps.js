var MapsControl = MapsControl || {};
MapsControl.map = {};
MapsControl.init = function (x, y) {
    MapsControl.map = new atlas.Map('vendingMap', {
        center: [x, y],
        zoom: 12,
        language: 'it-IT',
        authOptions: {
            authType: 'subscriptionKey',
            subscriptionKey: '57P8hPINbY_s3f83rTLqUAXimdz2S41zWPa9Jl-Fg44'
        }
    });
};
MapsControl.AddVendingMachine = function (x, y, vendingMachineName) {
    var marker = new atlas.HtmlMarker({
        color: 'DodgerBlue',
        text: '10',
        position: [x, y],
        popup: new atlas.Popup({
            content: '<div style="padding:10px"><a href="/' + vendingMachineName + '/products>Machine ID: ' + vendingMachineName + '</a></div>',
            pixelOffset: [0, -30]
        })
    });

    MapsControl.map.markers.add(marker);
}