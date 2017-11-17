window.addEventListener("load", function() {
    var canvas = document.getElementsByTagName("canvas")[0];
    var ctx = canvas.getContext("2d");
    var url = prompt("Please enter server URL", "ws://localhost:8000");

    function init() {
        var ws = new WebSocket(url);
        ws.binaryType = "arraybuffer";
        var dead = false;
        ws.addEventListener("open", function() {
            ws.send("display");
        });
        ws.addEventListener("message", function(ev) {
            var header = new Uint32Array(ev.data, 0, 4);
            var image = new ImageData(new Uint8ClampedArray(ev.data, 16), header[2], header[3]);
            ctx.putImageData(image, 0, 0, header[0], header[1], header[2], header[3]);
        });
        ws.addEventListener("error", function(ev) {
            if (!dead) {
                dead = true;
                console.log(ev);
                if (confirm("An error has occurred.  Do you want to reconnect?")) {
                    init();
                }
            }
        });
        ws.addEventListener("close", function() {
            if (!dead) {
                dead = true;
                init();
            }
        });
    }

    init();
});
