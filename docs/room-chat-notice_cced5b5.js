define("widget/room-chat-notice",
function(n) {
    function e(n) {
        TTP.isH5 ? TTP.ready(function(e) {
            var t = new HUYA.GetGameAdReq;
            t.tUserId = e.userId,
            t.lTid = T.id,
            t.lSid = T.sid,
            t.lPid = o.lp,
            e.sendWup2("liveui", "getCurrentGameAd", t,
            function(e) {
                n && n(e)
            })
        }) : TTP.isFlash && TTP.ready(function() {
            TTP.one("getCurrentGameAdRsp",
            function(e) {
                n && n(e.param)
            }),
            TTP.call("getCurrentGameAd")
        })
    }
    function t(n) {
        var e = parseInt(n.tJump.lTid) || 0,
        t = parseInt(n.tJump.lSid) || 0,
        i = n.tJump.lYYId || 0,
        a = n.tJump.iRoomId || 0,
        r = n.sGameUrl;
        e && t && (r = TT.app.main + TT.createRoomHost(null, a, i));
        var o = {
            url: r,
            banner: n.sWebLogoUrl
        };
        return o
    }
    function i(n) {
        if (n.banner) {
            var e = "";
            n.url && (e = 'href="' + n.url + '"');
            var t = "<a " + e + ' target="_blank"><img class="img-notice" src="' + n.banner + '" width="338"></a>';
            $("#wrap-notice").html(t),
            u.online.push("notice")
        }
    }
    function a(n) {
        n.lPid == o.lp && i(t(n))
    }
    function r() {
        TTP.isFlash ? TTP.on("gameAdNotice",
        function(n) {
            a(n.param)
        },
        !0) : TTP.isH5 && TTP.ready(function(n) {
            n.addTafListener("6201",
            function(n) {
                a(n)
            })
        })
    }
    var o = n("TTA"),
    T = n("TTR"),
    u = n("chatPanelFilter"),
    c = n("tafPreListen");
    c.on("6201",
    function(n) {
        a(n)
    },
    !0),
    r(),
    TT.getUserInfo(function(n) {
        n.isLogined || e(function(n) {
            i(t(n))
        })
    })
});