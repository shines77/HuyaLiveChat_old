define("TTR",
function() {
    return window.TT_ROOM_DATA
});;
define("TTA",
function() {
    return window.TT_PROFILE_INFO
});;
define("TTPlayer/cfg",
function() {
    return window.TT_PLAYER_CFG
});;
define("TTPlayer/tools",
function(e, n) {
    n.parseQueryString = function(e) {
        var n, t = {};
        if ( - 1 != (n = e.indexOf("?"))) for (var o = e.substring(n + 1, e.length), r = o.split("&"), i = [], l = 0, u = r.length; u > l; l++) i = r[l].split("="),
        t[i[0]] = i[1];
        return t
    },
    n.getCookieVal = function(e) {
        for (var n = document.cookie ? document.cookie.split("; ") : [], t = 0; t < n.length; t += 1) {
            var o = n[t].split("=");
            if (o[0] == e) return decodeURIComponent(o[1])
        }
        return ""
    }
});;
define("TTPlayer/h5",
function(e) {
    var i = e("TTR"),
    r = e("TTA"),
    o = e("TTPlayer/cfg"),
    a = e("TTPlayer/tools");
    return function(e) {
        var n = hyPlayerConfig.WEBYYFROM,
        t = hyPlayerConfig.vappid;
        "2" == i.liveSourceType && (t = 10507);
        var l = a.parseQueryString(location.search),
        p = "";
        try {
            p = sessionStorage.getItem("ya_eid")
        } catch(y) {
            p = a.getCookieVal("ya_eid")
        }
        var d = {
            chTopId: i.id,
            subChId: i.sid,
            pyyid: r.yyid,
            profileRoom: i.profileRoom,
            pnick: r.nick,
            freezeLevel: r.freezeLevel,
            deleteOriginalPainting: o.deleteOriginalPainting,
            h5gopChannel: o.h5gopChannel,
            eu: r.lp,
            rso: l.promoter || l.rso || a.getCookieVal("ya_rso") || "",
            rso_desc: l.rso_desc || "",
            from: n,
            vappid: t,
            stream: hyPlayerConfig.stream,
            gameId: i.gid,
            screenType: i.screenType,
            furl: encodeURIComponent(document.referrer),
            ref: p,
            iSourceType: i.liveSourceType,
            avatarImg: r.avatar,
            cfg: o,
            isUnion: !!a.getCookieVal("promoter")
        };
        l.share_by && l.share_fm && (d.platform = {
            weixin: 2,
            pengyouquan: 1,
            weibo: 3,
            qq: 4,
            kongjian: 5,
            tieba: 6,
            lianjie: 7
        } [l.share_fm] || 7, d.platform = "share-" + d.platform),
        i.isReplay && $.extend(d, {
            replay: 1,
            isEu: !1,
            doJoin: 0
        }),
        $("#liveRoomObj").html('<div id="videoContainer" style="position: relative;"></div>');
        var s = window.TT_LIVE_TIMING || {};
        s.playerInitBefore = (new Date).getTime();
        var g = new VPlayer($.extend({
            idDom: "#videoContainer",
            register: function(i) {
                e.emit("reg", i)
            }
        },
        d));
        s.playerInitAfter = (new Date).getTime(),
        e.emit("initComplete", g)
    }
});;
define("TTPlayer/flash",
function(e) {
    var o = e("TTR"),
    i = e("TTA"),
    a = e("TTPlayer/cfg"),
    r = e("TTPlayer/tools");
    return function(e, t) {
        function n(e) {
            var o = this;
            $("#liveRoomObj").html('<div id="flashRoomObj"></div>'),
            $.extend(!0, this, e),
            window.initComplete = function() {
                o.init()
            },
            this.load()
        }
        var s = hyPlayerConfig.WEBYYFROM,
        l = hyPlayerConfig.WEBYYSWF,
        f = (hyPlayerConfig.WEBYYHOST, hyPlayerConfig.vappid),
        c = a.flashDomain + a.flashVersion + "/main.swf";
        "2" == o.liveSourceType && (f = 10507);
        var d = r.parseQueryString(location.search);
        window.jsTime = (new Date).getTime();
        var u = "";
        try {
            u = sessionStorage.getItem("ya_eid")
        } catch(y) {
            u = r.getCookieVal("ya_eid")
        }
        var h = {
            topSid: o.id,
            subSid: o.sid,
            pyyid: i.yyid,
            pnick: i.nick,
            eu: i.lp,
            profileRoom: o.profileRoom,
            freezeLevel: i.freezeLevel,
            type: l,
            _yyAuth: "12",
            rso: d.promoter || d.rso || r.getCookieVal("ya_rso") || "",
            rso_desc: d.rso_desc || "",
            from: s,
            vappid: f,
            gameId: o.gid,
            furl: encodeURIComponent(document.referrer),
            referer: encodeURIComponent(location.href),
            ref: u,
            pageFull: 1,
            iSourceType: o.liveSourceType,
            screenType: o.screenType,
            normalpub: 1,
            avatarImg: i.avatar,
            isEmbed: !!window.IS_UNION,
            canIuseH5: t.canIuseH5,
            isUnion: !!r.getCookieVal("promoter")
        };
        d.share_by && d.share_fm && (h.platform = {
            weixin: 2,
            pengyouquan: 1,
            weibo: 3,
            qq: 4,
            kongjian: 5,
            tieba: 6,
            lianjie: 7
        } [d.share_fm] || 7, h.platform = "share-" + h.platform),
        o.isReplay && $.extend(h, {
            replay: 1,
            isEu: !1,
            doJoin: 0
        }),
        n.prototype = {
            swfPath: "",
            swfVs: "10.1",
            swfvar: null,
            id: "flashRoomObj",
            swfSetting: {
                quality: "high",
                bgcolor: "#000000",
                allowScriptAccess: "always",
                allowFullScreen: "true",
                allowFullScreenInteractive: "true",
                wmode: "opaque",
                menu: "false"
            },
            type: "flash",
            load: function() {
                var e = this;
                return window.swfobject && swfobject.hasFlashPlayerVersion(e.swfVs) ? (swfobject.embedSWF(e.swfPath, e.id, "100%", "100%", e.swfVs, null, e.swfvar, e.swfSetting, {
                    name: e.id
                },
                function(e) {
                    e && e.ref && "onload" in e.ref && (e.ref.onload = function() {}),
                    huyacreateObject.apply(this, arguments)
                },
                huyaflashCallback), this) : this
            },
            init: function() {
                var o = this;
                o.getMovie().callAs("addJsKey", !0),
                e.emit("reg", o.getMovie()),
                e.emit("initComplete", o.getMovie())
            },
            getMovie: function() {
                var e;
                return function() {
                    return e || (e = $.browser.msie ? window[this.id] : document[this.id])
                }
            } ()
        },
        window.playerFlashGetStreamInfo = function() {
            return hyPlayerConfig.stream
        },
        new n({
            swfPath: c,
            swfvar: h
        })
    }
});;
define("TTPlayer",
function(e) {
    function n(e) {
        return "function" == typeof e
    }
    function t(e) {
        return "string" == typeof e
    }
    function r(e) {
        var r = $.extend({
            type: l,
            canIuseH5: !1
        },
        e),
        o = r.type,
        s = o === c,
        d = o === l,
        y = f[o](),
        h = new u,
        v = new u;
        v.on("reg",
        function(e) {
            if (s) e.on("all",
            function() {
                h.emit.apply(h, arguments)
            });
            else if (d) {
                var n = "TT_PLAYER_FLASH_EVENT_LISTENER_" + Math.floor(1e10 * Math.random());
                window[n] = function(e) {
                    try {
                        h.emit(e.jsKey, e)
                    } catch(n) {
                        try {
                            console.log(n)
                        } catch(n) {}
                    }
                };
                try {
                    e.registerCallBack("all", n)
                } catch(t) {}
            }
        });
        var m = null,
        T = function() {
            var e = $.Deferred();
            return v.on("initComplete",
            function(n) {
                e.resolveWith(h, [m = n])
            }),
            function(n) {
                return e.done(n),
                "resolved" === e.state()
            }
        } (),
        p = function() {
            var e = $.Deferred();
            return h.on("gamelivePubTextInitComplete",
            function() {
                e.resolveWith(h, [m])
            }),
            function(n) {
                return e.done(n),
                "resolved" === e.state()
            }
        } (),
        g = function() {
            var e = $.Deferred();
            return h.on("videoOnLoad",
            function() {
                e.resolveWith(h, [m])
            }),
            function(n) {
                return e.done(n),
                "resolved" === e.state()
            }
        } (),
        E = function(e, r) {
            if (m && e && t(e)) {
                var o;
                return s ? n(m[e]) ? o = m[e](r) : n(m.callByJs) && (o = m.callByJs(e, r)) : d && (o = m.callAs(e, r)),
                o
            }
        },
        P = function(e) {
            return m && t(e) ? m[e] : void 0
        },
        _ = function() {
            if (p()) {
                var e = E("getMyInfo");
                return e ? {
                    uid: e.lUid || e.uid,
                    nick: e.sNick || e.nickname
                }: null
            }
        },
        H = y.then(),
        I = 0;
        return y.done(function() {
            s ? I ? i(v) : setTimeout(function() {
                i(v)
            },
            0) : d && a(v, {
                canIuseH5: r.canIuseH5
            })
        }),
        I = 1,
        $.extend(h, {
            type: o,
            isH5: s,
            isFlash: d,
            onload: function(e) {
                return H.done(e),
                "resolved" === H.state()
            },
            initComplete: T,
            ready: p,
            videoOnLoad: g,
            call: E,
            get: P,
            getMyInfo: _
        }),
        h
    }
    var o = e("TTPlayer/cfg"),
    i = e("TTPlayer/h5"),
    a = e("TTPlayer/flash"),
    u = (e("TTR"), e("Event")),
    c = "H5",
    l = "FLASH",
    f = {};
    return function(e) {
        function n(e) {
            return $.ajax({
                url: e,
                dataType: "script",
                cache: !0
            })
        }
        var t = e.h5domain + e.h5PlayerIncludeSDK + "/vplayer.js",
        r = function() {
            var n = document.createElement("link");
            n.rel = "stylesheet",
            n.href = e.h5domain + e.h5PlayerIncludeSDK + "/vplayer.css",
            document.getElementsByTagName("head")[0].appendChild(n)
        };
        f[c] = function() {
            return r(),
            n(t)
        },
        f[l] = function() {
            return n("https://a.msstatic.com/huya/main/lib/swf_hiido_hiidostatic_dbd90e6.js")
        }
    } (o),
    r.TYPE_H5 = c,
    r.TYPE_FLASH = l,
    r
});;
define("TTP",
function(e) {
    function a() {
        var e = "";
        try {
            e = localStorage.getItem("h5ErrorTime")
        } catch(a) {}
        return !! (e && (new Date).getTime() - e <= 864e5)
    }
    function r() {
        var e = navigator.userAgent.toLocaleLowerCase(),
        a = e.match(/chrome\/([\d.]+)/);
        return !! a && parseInt(a[1], 10) >= 45 && -1 == e.indexOf("edge") && !t()
    }
    function t() {
        var e = !1,
        a = navigator.userAgent;
        if (/2345chrome/.test(a) || /2345Explorer/.test(a)) e = !0;
        else try {
            window.external.RCCoralGetItemCacheType(),
            e = !0
        } catch(r) {}
        return e
    }
    function o() {
        var e = "";
        try {
            e = localStorage.getItem("playerMask")
        } catch(a) {}
        return e
    }
    var n = e("TTPlayer"),
    i = (e("TTR"), -1 != location.search.indexOf("playerh5=1")),
    l = -1 != location.search.indexOf("playerflash=1"),
    c = hyPlayerConfig.html5 && !a() && r(),
    d = i || !l && "flash" != o() && c,
    f = new n({
        type: d ? n.TYPE_H5: n.TYPE_FLASH,
        canIuseH5: c
    }),
    m = window.TT_LIVE_TIMING || {};
    return m.playerLoadStart = (new Date).getTime(),
    window.performanceInfo.firstScreenTime = m.playerLoadStart,
    window.performanceInfo._hmt.push(["reportApiTime", "huya-main-room", "end", m.playerLoadStart]),
    f.onload(function() {
        m.videoLoadStart = m.playerLoadEnd = (new Date).getTime()
    }),
    f.videoOnLoad(function() {
        m.videoLoadEnd = (new Date).getTime()
    }),
    f.isH5 && f.on("playerError",
    function() {
        if (window.localStorage) {
            var e = localStorage.getItem("h5ErrorTime"),
            a = (new Date).getTime();
            e && a - e > 864e5 && localStorage.setItem("h5ErrorTime", a)
        }
        location.href = location.href + ( - 1 != location.search.indexOf("?") ? "&": "?") + "playerflash=1"
    }),
    f.on("playerSwitch",
    function(e) {
        var a = e.param ? e.param: e;
        try {
            localStorage.setItem("playerMask", a.type)
        } catch(r) {}
        location.reload()
    }),
    f.displayMode = 0,
    f.on("pageFullScreen",
    function(e) {
        var a = e.param.flag;
        a != f.displayMode && f.emit("displayModeChange", f.displayMode = a)
    }),
    window.TTP = f
});;
define("roomBeta",
function(n, e, o) {
    function t() {
        var n = this,
        e = new $.Deferred(function(n) {
            u.isOn || u.isReplay ? (i.videoOnLoad(function() {
                n.resolve()
            }), setTimeout(function() {
                n.resolve()
            },
            1500)) : setTimeout(function() {
                n.resolve()
            },
            16)
        }),
        o = e.then(),
        t = $.when(new $.Deferred(function(n) {
            $(window).on("load",
            function() {
                n.resolve()
            }),
            setTimeout(function() {
                n.resolve()
            },
            5e3)
        }), e.then()),
        c = function(n) {
            if (n && ("function" == typeof n || "function" == typeof n.executor)) {
                var e, u, i;
                "function" == typeof n ? e = n: (e = n.executor, u = n.force, i = n.type);
                var c = r.once(function() {
                    return new $.Deferred(function(n) {
                        e(n.resolve, n.reject)
                    }).promise()
                });
                "function" == typeof u && f.push(function() {
                    u(c)
                }),
                "VIDEO" === i ? o.done(function() {
                    f.push(c)
                }) : t.done(function() {
                    f.push(c)
                })
            }
        };
        return $.extend(n, {
            videoOnLoad: function(n) {
                return o.done(function() {
                    f.push(n)
                })
            },
            onload: function(n) {
                return t.done(function() {
                    f.push(n)
                })
            },
            lazyLoad: c
        })
    }
    var u = n("TTR"),
    i = n("TTP"),
    f = n("rafQue"),
    r = n("tools");
    o.exports = new t
});