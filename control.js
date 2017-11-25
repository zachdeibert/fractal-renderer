window.addEventListener("load", function() {
    var touches = {};
    var ws;
    var dead = true;
    var sending = false;
    var events = {
        "swipe": {
            "needsUpdate": false,
            "dx": 0,
            "dy": 0
        }
    };

    function sendAny() {
        if (!dead) {
            if (events.swipe.needsUpdate) {
                if (events.swipe.dx > 127) {
                    events.swipe.dx = 127;
                } else if (events.swipe.dx < -128) {
                    events.swipe.dx = -128;
                }
                if (events.swipe.dy > 127) {
                    events.swipe.dy = 127;
                } else if (events.swipe.dy < -128) {
                    events.swipe.dy = -128;
                }
                ws.send(new Int8Array([ 8, events.swipe.dx, events.swipe.dy ]));
                events.swipe = {
                    "needsUpdate": false,
                    "dx": 0,
                    "dy": 0
                };
            } else {
                return;
            }
            sending = true;
        }
    }

    document.body.addEventListener("touchstart", function(ev) {
        ev.preventDefault();
        for (var i = 0; i < ev.changedTouches.length; ++i) {
            touches[ev.changedTouches[i].identifier] = {
                "x": ev.changedTouches[i].pageX,
                "y": ev.changedTouches[i].pageY
            };
        }
    });
    document.body.addEventListener("touchend", function(ev) {
        ev.preventDefault();
        for (var i = 0; i < ev.changedTouches.length; ++i) {
            touches[ev.changedTouches[i].identifier] = null;
        }
    });
    document.body.addEventListener("touchcancel", function(ev) {
        ev.preventDefault();
        for (var i = 0; i < ev.changedTouches.length; ++i) {
            touches[ev.changedTouches[i].identifier] = null;
        }
    });
    document.body.addEventListener("touchmove", function(ev) {
        ev.preventDefault();
        for (var i = 0; i < ev.changedTouches.length; ++i) {
            var dx = ev.changedTouches[i].pageX - touches[ev.changedTouches[i].identifier].x;
            var dy = ev.changedTouches[i].pageY - touches[ev.changedTouches[i].identifier].y;
            touches[ev.changedTouches[i].identifier] = {
                "x": ev.changedTouches[i].pageX,
                "y": ev.changedTouches[i].pageY
            };
            if (!dead) {
                events.swipe.needsUpdate = true;
                events.swipe.dx += dx;
                events.swipe.dy += dy;
                if (!sending) {
                    sendAny();
                }
            }
        }
    });

    function init() {
        ws = new WebSocket("ws://" + location.host);
        ws.binaryType = "arraybuffer";
        ws.addEventListener("open", function() {
            dead = false;
        });
        ws.addEventListener("message", function(ev) {
            sending = false;
            sendAny();
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
