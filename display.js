window.addEventListener("load", function() {
    var canvas = document.getElementsByTagName("canvas")[0];
    var ctx = canvas.getContext("2d");
    var keyframe = new Uint8Array([ 4 ]).buffer;
    var frame = new Uint8Array([ 5 ]).buffer;
    var frameCounter = 0;
    var keyFrameRatio = 20;

    function init() {
        var ws = new WebSocket("ws://" + location.host);
        ws.binaryType = "arraybuffer";
        var dead = false;
        ws.addEventListener("open", function() {
            ws.send(keyframe);
        });
        ws.addEventListener("message", function(ev) {
            var header = new Uint32Array(ev.data.slice(1, 17));
            if (header[2] == 0 || header[3] == 0) {
                setTimeout(function() {
                    ws.send(++frameCounter % keyFrameRatio == 0 ? keyframe : frame);
                }, 1000 / 60);
            } else {
                var image = new ImageData(new Uint8ClampedArray(ev.data, 17, header[2] * header[3] * 4), header[2], header[3]);
                ctx.putImageData(image, header[0], header[1]);
                ws.send(++frameCounter % keyFrameRatio == 0 ? keyframe : frame);
            }
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
