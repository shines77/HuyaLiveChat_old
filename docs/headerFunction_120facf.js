function zhReport(n) {
    if ("undefined" == typeof YA) return ! 1;
    var n = n || {},
    e = {
        pro: "huya_web",
        pas: YA.cookie.get("username") || "",
        yyuid: YA.cookie.get("yyuid") || "",
        pageType: "root",
        rso: "",
        rso_desc: "",
        eid: "",
        eid_desc: ""
    },
    o = function(n, e, o, t, a) {
        var i = n + "=" + e + "; domain=" + o + "; path=" + t;
        a > 0 && (i = i + "; expires=" + a.toGMTString()),
        document.cookie = i
    };
    for (var t in n) e[t] = n[t];
    if ("undefined" == typeof ya) {
        window.ya = new YA(e.pro, e.pas, {
            pageType: e.pageType,
            yyuid: e.yyuid
        }),
        e.rso && o("ya_rso", e.rso, TT.domainForWriteCookie, "/"),
        e.rso = YA.cookie.get("ya_rso") || e.rso;
        var a = HUYA_UTIL.getCookieVal("guid");
        a && ya.setExtParam("sguid=" + a);
        var i = "";
        try {
            i = sessionStorage.getItem("ya_eid")
        } catch(r) {
            i = YA.cookie.get("ya_eid")
        }
        ya.reportProductStartUp({
            pro: e.pro,
            rso: e.rso,
            rso_desc: e.rso_desc,
            ref: i
        }),
        ya.startProductHeartbeat({
            pro: e.pro,
            rso: e.rso,
            ref: i,
            rso_desc: e.rso_desc
        }),
        ya.reportProductEvent({
            eid: e.eid,
            eid_desc: e.eid_desc,
            rso: e.rso,
            rso_desc: e.rso_desc,
            ref: i
        }),
        function() {
            var n = new Image,
            o = {
                eid: e.eid,
                url: location.href,
                mid: YA.mid.get(),
                t: (new Date).getTime() + "_" + Math.floor(1e9 * Math.random())
            };
            n.src = "https://www.huya.com/d.gif?" + $.param(o),
            setTimeout(function() {
                n = null
            },
            200)
        } (),
        TT.emit("reportReady"),
        $("body").on("click", ".clickstat",
        function() {
            var n = "";
            if ($(this).attr("href")) {
                n = $(this).attr("eid").substring(6);
                try {
                    sessionStorage.setItem("ya_eid", n)
                } catch(e) {
                    o("ya_eid", n, TT.domainForWriteCookie, "/")
                }
            }
            var t = "";
            try {
                t = sessionStorage.getItem("ya_eid")
            } catch(e) {
                t = YA.cookie.get("ya_eid")
            }
            ya.reportProductEvent({
                eid: $(this).attr("eid"),
                eid_desc: $(this).attr("eid_desc"),
                ref: t
            })
        }),
        $("body").on("click", ".new-clickstat",
        function() {
            var n = "",
            e = $(this);
            try {
                var t = $.parseJSON(e.attr("report"));
                if ($(this).attr("href")) {
                    n = t.position;
                    try {
                        sessionStorage.setItem("ya_eid", n)
                    } catch(a) {
                        o("ya_eid", n, TT.domainForWriteCookie, "/")
                    }
                }
                try {
                    t.ref = sessionStorage.getItem("ya_eid")
                } catch(a) {
                    t.ref = YA.cookie.get("ya_eid")
                }
                ya.reportProductEvent(t)
            } catch(a) {}
        }),
        $("body").on("click", ".third-clickstat",
        function() {
            var n = $(this).data("thirdstat"),
            e = e || new Image;
            e.src = n
        })
    }
}
function parseQueryString(n) {
    var e, o = {};
    if ( - 1 != (e = n.indexOf("?"))) for (var t = n.substring(e + 1, n.length), a = t.split("&"), i = [], r = 0, c = a.length; c > r; r++) i = a[r].split("="),
    o[i[0]] = i[1];
    return o
}
window.performanceInfo = {
    _hmt: [],
    whiteScreenTime: window.performance && window.performance.timing.responseStart || +new Date,
    firstScreenTime: 0,
    pageview: "huya-main"
},
window.performanceInfo.apiTimeOut = 5e3,
function() {
    setTimeout(function() {
        var n = document.createElement("script"),
        e = "https://a.msstatic.com/huya/hd/cdn_libs/performance_report-min.js?v=" + (new Date).toLocaleDateString().replace(/-|\//g, "");
        n.src = e;
        var o = document.getElementsByTagName("script")[0];
        o.parentNode.insertBefore(n, o)
    },
    500)
} (),
!
function() {
    var n = {},
    e = location.protocol + "//" + location.host + "/";
    n.login = {
        YY: {
            authUrl: e + "udb_web/authorizeURL.php?do=authorizeEmbedURL",
            callbackUrl: e + "udb_web/udbport2.php?do=callback",
            OAuthUrl: e + "udb_web/udbport2.php?do=dummy3AuthorizeURL&loginType={type}&protocol=" + ( - 1 != window.location.protocol.indexOf("https") ? 1 : 0),
            logoutUrl: e + "udb_web/udbport2.php?do=logout",
            regUrl: e + "udb_web/udbport2.php?do=registerEmbedURL&registerUrl=https://zc.yy.com/reg/udb/reg4udb.do&type={type}"
        },
        TT: {
            loginCallback: e + "udb_web/udbport2.php?m=HuyaHome&do=huyaLCallback",
            logoutCallback: e + "udb_web/udbport2.php?m=HuyaHome&do=huyaLogout",
            loginAuthCallback: e + "udb_web/udbport2.php?m=HuyaHome&do=thirdLCallback",
            regUrl: e + "udb_web/udbport2.php?do=registerEmbedURL&registerUrl=https://zc.yy.com/reg/udb/reg4udb.do&type=Mobile"
        },
        type: "normal",
        logoutNavTo: ""
    },
    n.req = {
        userInfo: function() {
            return $.getJSON(e + "udb_web/checkLogin.php")
        },
        userNick: function(n) {
            return $.getJSON(e + (n ? "udb_web/udbport2.php?m=HuyaHome&do=checkUserNick": "udb_web/udbport2.php?do=checkUserNick"))
        },
        userAvatar: function(n) {
            return $.ajax({
                url: "//user.huya.com/user/getUserInfo",
                data: {
                    uid: n
                },
                dataType: "jsonp",
                cache: !0
            })
        }
    },
    window.TT_CFG = $.extend(!0, n, window.TT_CFG_CUSTOM)
} (),
!
function(n) {
    var e = n.HUYA_UTIL = {
        trim: function(n) {
            var e = /^(\s|\u00A0)+|(\s|\u00A0)+$/g;
            return (n || "").replace(e, "")
        },
        xssFilter: function(n) {
            return "string" != typeof n ? n: n = n.replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/\'/g, "&#39;").replace(/\"/g, "&quot;")
        },
        isFunction: function(n) {
            return "[object Function]" === Object.prototype.toString.call(n)
        },
        createStyleSheet: function(n) {
            var e;
            return document.createStyleSheet ? (e = document.createStyleSheet(), e.cssText = n) : (e = document.createElement("style"), e.appendChild(document.createTextNode(n)), document.getElementsByTagName("head")[0].appendChild(e)),
            e
        },
        deleteCookie: function(n, e, o) {
            if (this.hasCookie(n)) {
                var t = new Date;
                t.setTime(t.getTime() - 864e5),
                document.cookie = n + "=_; expires=" + t.toGMTString() + "; domain=" + e + "; path=" + o
            }
        },
        writeCookie: function(n, e, o, t, a) {
            var i = n + "=" + e + "; domain=" + o + "; path=" + t;
            a > 0 && (i = i + "; expires=" + a.toGMTString()),
            document.cookie = i
        },
        hasCookie: function(n) {
            return !! e.getCookieVal(n)
        },
        getCookieVal: function(n) {
            for (var e = document.cookie.split("; "), o = 0; o < e.length; o += 1) {
                var t = e[o].split("=");
                if (t[0] == n) return decodeURIComponent(t[1])
            }
            return ""
        },
        getScript: function(n, e) {
            function o() {
                i.onload = i.onreadystatechange = null,
                i = null,
                "function" == typeof e && e()
            }
            var t = document,
            a = t.head || t.getElementsByTagName("head")[0] || t.documentElement,
            i = t.createElement("script");
            "onload" in i ? i.onload = o: i.onreadystatechange = function() { / loaded | complete / .test(i.readyState) && o()
            },
            i.charset = "utf-8",
            i.type = "text/javascript",
            i.async = !0,
            i.src = n,
            a.appendChild(i)
        },
        throttle: function(n, e) {
            function o() {
                i = arguments,
                r = this,
                a || (a = setTimeout(function() {
                    a = null,
                    n.apply(r, i),
                    r = i = null
                },
                t ? 0 : e), t && (t = !1))
            }
            var t = !0,
            a = null,
            i = null,
            r = null;
            return o.cancel = function() {
                clearTimeout(a),
                a = r = i = null
            },
            o
        },
        debounce: function(n, e, o) {
            var t = null,
            a = function() {
                t && clearTimeout(t);
                var a = this,
                i = arguments;
                return o ? (t || n.apply(a, i), t = setTimeout(function() {
                    t = null
                },
                e)) : t = setTimeout(function() {
                    t = null,
                    n.apply(a, i)
                },
                e),
                a
            };
            return a.cancel = function() {
                clearTimeout(t),
                t = null
            },
            a
        },
        isMobile: function() {
            return navigator.userAgent.match(/(phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone)/i) ? !0 : !1
        } (),
        extend: function(n, e) {
            for (var o in e)"undefined" != typeof e[o] && e.hasOwnProperty(o) && (n[o] = e[o]);
            return n
        },
        getLocationSearchParam: function() {
            var n = {},
            e = location.search;
            if ( - 1 != e.indexOf("?")) for (var o = e.substr(1).split("&"), t = 0, a = o.length; a > t; t++) {
                var i = o[t].split("=");
                i[0] && (n[decodeURIComponent(i[0])] = decodeURIComponent(i[1]))
            }
            return n
        },
        once: function(n) {
            var e, o;
            return function() {
                return o || (o = !0, e = n.apply(this, arguments)),
                e
            }
        }
    }
} (window),
!
function() {
    var n = HUYA_UTIL.getCookieVal,
    e = {};
    e.env = e.type = 0,
    location.search.replace(/TT_ENV=(.)/,
    function(n, o) {
        var t = parseInt(o, 10);
        return isNaN(t) || (e.env = t),
        n
    }),
    e.isProd = 0 == e.env,
    e.debug = !e.isProd,
    e.log = function() {
        var n = !1;
        try {
            n = !!localStorage.getItem("TT_DEBUG")
        } catch(o) {}
        if (e.debug || n) try {
            console.log.apply(console, arguments)
        } catch(o) {}
    },
    e.domain = {
        main: "www.huya.com",
        i: "i.huya.com",
        hd: "hd.huya.com"
    },
    e.app = {
        main: location.protocol + "//" + e.domain.main + "/",
        act: location.protocol + "//" + e.domain.main + "/act/",
        i: location.protocol + "//" + e.domain.i + "/",
        hd: location.protocol + "//" + e.domain.hd + "/"
    },
    e.domainForWriteCookie = "huya.duowan.com" == location.host ? "duowan.com": "huya.com",
    e.trimUrl = function(n) {
        return $.trim(n).replace(/^http\:/, "")
    },
    e.createRoomHost = function(n, e, o) {
        return n && /[^0-9]/.test(n) ? n: e && "0" != e ? e: "yy/" + (n || o)
    },
    e.isSupportWebp = HUYA_UTIL.once(function() {
        var n = !1;
        try {
            n = 0 == document.createElement("canvas").toDataURL("image/webp").indexOf("data:image/webp")
        } catch(e) {
            n = !1
        }
        return n
    }),
    e.createScreenshot = function(n, o) {
        if ("string" != typeof n) return "";
        o = o || {};
        var t = "live-cover.msstatic.com",
        a = "x-oss-process=",
        i = "imageview/",
        r = n;
        return - 1 != n.indexOf(t) ? -1 == n.indexOf(a) && (r = n + ( - 1 == n.indexOf("?") ? "?": "&") + (a + "image") + (o.w || o.h ? "/resize,limit_0,m_fill": "") + (o.w ? ",w_" + o.w: "") + (o.h ? ",h_" + o.h: "") + (o.r ? "/rotate," + o.r: "") + "/sharpen,80" + (e.isSupportWebp() ? "/format,webp": "/format,jpg/interlace,1") + "/quality,q_90") : -1 == n.indexOf("?" + i) && -1 == n.indexOf("&" + i) && (r = n + ( - 1 == n.indexOf("?") ? "?": "&") + (i + "4/0") + (o.w ? "/w/" + o.w: "") + (o.h ? "/h/" + o.h: "") + (o.r ? "/rotate/" + o.r: "") + "/blur/1" + (e.isSupportWebp() && /\/yysnapshot\//.test(n) ? "/format/webp": "")),
        r
    },
    e.userType = function() {
        var e = 0,
        o = n("udb_status"),
        t = parseInt(o, 10);
        return t && 1 & t ? (e = 10, n("udb_openid") && (e = 11)) : n("yyuid") && (e = 20, n("thirdCookie") && (e = 21)),
        e
    },
    e.onload = function() {
        var n = new $.Deferred(function(n) {
            $(window).on("load",
            function() {
                n.resolve()
            }),
            setTimeout(function() {
                n.resolve()
            },
            5e3)
        });
        return function(e) {
            return n.done(e)
        }
    } (),
    e.lazyload = function() {
        var n = $.when(new $.Deferred(function(n) {
            setTimeout(function() {
                n.resolve()
            },
            3500)
        }), new $.Deferred(function(n) {
            e.onload(function() {
                n.resolve()
            })
        }));
        return function(e) {
            return n.done(e)
        }
    } (),
    e.promoter = HUYA_UTIL.getLocationSearchParam().promoter,
    e.promoter && HUYA_UTIL.writeCookie("promoter", e.promoter, e.domainForWriteCookie, "/"),
    n("promoter") && $(function() {
        $("body").addClass("promoter")
    }),
    window.TT = e
} (),
TT.Event = function() {
    function n() {
        return {
            fired: !1,
            data: [],
            callbacks: new $.Callbacks
        }
    }
    function e() {
        function e(e, o, t) {
            if (!i(e) || !a(o)) return this;
            var r = s[e] || (s[e] = new n);
            return r.callbacks.add(o),
            t && r.fired && o.apply(this, r.data),
            this
        }
        function o(e) {
            if (!i(e)) return this;
            var a = s[e] || (s[e] = new n);
            return a.fired = !0,
            a.data = t.call(arguments, 1),
            a.callbacks.fireWith(this, a.data),
            "ALL" !== e && o.apply(this, ["ALL"].concat(t.call(arguments))),
            this
        }
        function r(n, e) {
            var o = arguments.length;
            if (0 === o) for (var t in s) s.hasOwnProperty(t) && s[t].callbacks.empty();
            else if (i(n)) {
                var r = s[n];
                r && (1 === o ? r.callbacks.empty() : a(e) && r.callbacks.remove(e))
            }
            return this
        }
        function c(n, o, t) {
            if (!i(n) || !a(o)) return this;
            var c = function() {
                o.apply(this, arguments),
                r(n, c)
            };
            e(n, c, t)
        }
        var s = {};
        return {
            on: e,
            one: c,
            off: r,
            emit: o
        }
    }
    var o = [],
    t = o.slice,
    a = function(n) {
        return "function" == typeof n
    },
    i = function(n) {
        return "string" == typeof n
    };
    return e
} (),
TT.event = new TT.Event,
function() {
    var n = {},
    e = {},
    o = -1;
    n.publish = function(n) {
        var o = arguments;
        e[n] || (e[n] = {
            published: !1,
            data: null,
            subscribers: []
        });
        var t = e[n];
        t.published = !0,
        t.data = Array.prototype.slice.call(o);
        for (var a = t.subscribers.length,
        i = null; a--;) i = t.subscribers[a],
        i.func.apply(i, o);
        return ! 0
    },
    n.subscribe = function(n, t, a) {
        if ("function" != typeof t) return - 1;
        e[n] || (e[n] = {
            published: !1,
            data: null,
            subscribers: []
        });
        var i = (++o).toString(),
        r = {
            token: i,
            func: t
        };
        return e[n].subscribers.push(r),
        a && e[n].published && t.apply(r, e[n].data),
        i
    },
    n.unsubscribe = function(n) {
        for (var o in e) if (e.hasOwnProperty(o)) for (var t = e[o].subscribers, a = 0, i = t.length; i > a; a++) {
            var r = t[a];
            if (r.token === n) return t.splice(a, 1),
            n
        }
        return ! 1
    },
    TT.on = function() {
        n.subscribe.apply(n, arguments)
    },
    TT.off = function() {
        n.unsubscribe.apply(n, arguments)
    },
    TT.emit = function() {
        n.publish.apply(n, arguments)
    },
    window.DUYA_SUB = n
} (),
!
function(n, e) {
    function o(n) {
        var e = new HyLogin(n);
        return e.on("loginSuccess",
        function() {
            window.location.reload()
        }),
        e.on("loginFail",
        function() {
            window.location.reload()
        }),
        e.on("loginAuthSuccess",
        function() {
            window.location.reload()
        }),
        e.on("loginAuthFail",
        function() {
            window.location.reload()
        }),
        e.on("logoutSuccess",
        function() {
            TT_CFG.login.logoutNavTo || -1 != TT.app.i.indexOf(location.hostname) && (TT_CFG.login.logoutNavTo = TT.app.main),
            TT_CFG.login.logoutNavTo ? location.href = TT_CFG.login.logoutNavTo: location.reload()
        }),
        e.on("logoutFail",
        function() {
            window.location.reload()
        }),
        e
    }
    var t = "20181101",
    a = "https:",
    i = "";
    i = "//udbres.huya.com/js/HyUDBWebSDK-w1.0.js",
    o.deps = [a + i + "?v=" + t];
    var r = "//udbres.huya.com/js/HyUDBWebSDK-e1.0.js";
    o.deps1 = [a + r + "?v=" + t];
    var c = "";
    c = "//udbres.huya.com/js/HyUDBWebSDK-w1.1.js",
    o.deps2 = [a + c + "?v=" + t],
    e.TT_LOGIN_HUYA = o
} (jQuery, window),
!
function(n, e) {
    function o(e) {
        var o = function() {
            var o = new TT_YYUDB(e),
            t = new TT_YYLOGIN(o);
            return n.extend({},
            o, t)
        } ();
        return o.on("loginOAuthCallback",
        function() {
            window.location.reload()
        }),
        o.on("loginOAuthFail",
        function(e) {
            if (e && "MUTE" === e.code) {
                var o = n("body"),
                t = '<div id="login-closure"><div class="login-closure-tit">' + e.data.title + '<span id="login-closure-close"></span></div><div class="login-closure-cnt">' + e.data.msg + '</div><span id="login-closure-sure">\u786e\u5b9a</span></div>';
                o.append(t),
                o.append('<div id="login-closure-mask"></div>'),
                n("#login-closure-sure,#login-closure-close").one("click",
                function() {
                    n("#login-closure-mask").remove(),
                    n("#login-closure").remove()
                })
            }
        }),
        o.on("logoutSuccess",
        function() {
            TT_CFG.login.logoutNavTo || -1 != TT.app.i.indexOf(location.hostname) && (TT_CFG.login.logoutNavTo = TT.app.main),
            TT_CFG.login.logoutNavTo ? location.href = TT_CFG.login.logoutNavTo: location.reload()
        }),
        o
    }
    o.deps = ["https://a.msstatic.com/huya/main/js/base/login/yy/sdk_udb_ad66392.js", "https://a.msstatic.com/huya/main/js/base/login/yy/sdk_login_e937069.js"],
    e.TT_LOGIN_YY = o
} (jQuery, window),
function(n, e) {
    function o(o) {
        var t = this,
        a = e.once(function() {
            return new n.Deferred(function(n) {
                n.resolve(1)
            })
        }),
        i = e.once(function() {
            return a().pipe(function(e) {
                return new n.Deferred(function(n) {
                    if (e) {
                        var o = HUYA_UTIL.getCookieVal("udb_passdata");
                        o ? n.resolve(o) : (HUYA_UTIL.writeCookie("udb_passdata", 3, TT.domainForWriteCookie, "/"), n.resolve(3))
                    } else n.reject()
                })
            })
        }),
        r = "",
        c = new n.Deferred;
        c.done(function(n) {
            r = n
        });
        var s = e.once(function() {
            i().done(function(n) {
                c.resolve(1 == n ? "YY": "embed" == o.type ? "TT1": 3 == n ? "TT2": "TT")
            }).fail(function() {
                c.resolve("YY")
            })
        }),
        u = function(n) {
            c.resolve(r = n)
        },
        l = {
            TT: {
                getDeps: e.once(function() {
                    return e.loadScript(TT_LOGIN_HUYA.deps)
                }),
                create: e.once(function() {
                    return new TT_LOGIN_HUYA(o.TT)
                })
            },
            TT1: {
                getDeps: e.once(function() {
                    return e.loadScript(TT_LOGIN_HUYA.deps1)
                }),
                create: e.once(function() {
                    return new TT_LOGIN_HUYA(o.TT)
                })
            },
            TT2: {
                getDeps: e.once(function() {
                    return e.loadScript(TT_LOGIN_HUYA.deps2)
                }),
                create: e.once(function() {
                    return new TT_LOGIN_HUYA(o.TT)
                })
            },
            YY: {
                getDeps: e.once(function() {
                    return e.loadScript(TT_LOGIN_YY.deps)
                }),
                create: e.once(function() {
                    return new TT_LOGIN_YY(o.YY)
                })
            }
        },
        d = function(n) {
            s(),
            c.done(function() {
                var e = r;
                l[e].getDeps().done(function() {
                    n(l[e].create(), e)
                })
            })
        };
        n.each(["login", "logout", "register"],
        function(n, e) {
            t[e] = function() {
                var n = arguments;
                d(function(o) {
                    "function" == typeof o[e] && o[e].apply(o, n)
                })
            }
        }),
        t.by = function(n) {
            d(function(e, o) {
                "YY" == o ? ("WX" == n && e.loginByWX(), "WB" == n && e.loginByWB(), "QQ" == n && e.loginByQQ()) : n && e.third({
                    type: n.toLowerCase()
                })
            })
        },
        TT.lazyload(function() {
            s(),
            c.done(function() {
                l[r].getDeps()
            })
        }),
        t.getSwitchState = a,
        t.getBypass = i,
        t.use = u
    }
    TT.login = new o(TT_CFG.login);
    var t = TT.login.logout;
    TT.login.logout = function() {
        HUYA_UTIL.getCookieVal("udb_uid") && TT.login.use("TT"),
        t.apply(this, arguments)
    },
    TT.login.check = function() {
        var o = e.once(function() {
            var e = new n.Deferred;
            return TT.login.getSwitchState().pipe(TT_CFG.req.userInfo).done(function(n) {
                e.resolve(n.isLogined, n)
            }),
            e.promise()
        });
        return function(n) {
            var e = o();
            return e.done(n),
            e
        }
    } ()
} (jQuery,
function() {
    function n(n) {
        var e, o;
        return function() {
            return o || (o = !0, e = n.apply(this, arguments)),
            e
        }
    }
    function e(n) {
        var e = [].concat(n),
        o = [];
        return $.each(e,
        function(n, e) {
            o.push($.ajax({
                url: e,
                dataType: "script",
                cache: !0
            }))
        }),
        $.when.apply($, o)
    }
    return {
        once: n,
        loadScript: e
    }
} ()),
function() {
    function n(n) {
        var e, o;
        return function() {
            return o || (o = !0, e = n.apply(this, arguments)),
            e
        }
    }
    TT.getUserInfo = function() {
        var e = n(function() {
            var n = new $.Deferred;
            return TT.login.check(function(e, o) {
                if (e) {
                    var t = {},
                    a = {},
                    i = o.userNick ? (new $.Deferred).resolve([{}]) : TT.login.getSwitchState().pipe(TT_CFG.req.userNick);
                    $.when(TT_CFG.req.userAvatar(o.uid), i).done(function(n, e) {
                        t = n[0] && 200 == n[0].code && n[0].data ? n[0].data: {},
                        a = e[0] || {}
                    }).fail(function() {
                        try {
                            console.log("\u516c\u5171\u5934\uff1a\u83b7\u53d6\u7528\u6237\u4fe1\u606f\u5931\u8d25\uff01")
                        } catch(n) {}
                    }).always(function() {
                        var e = $.extend(!0, {},
                        o);
                        e.userNick = a.userNick || o.userNick || o.userName,
                        e.userLogo = t.avatar || a.userLogo || o.userLogo || "//a.msstatic.com/huya/main/assets/img/default_avatar.jpg",
                        n.resolve(e)
                    })
                } else n.resolve(o)
            }),
            n.done(function(n) {
                window.a_userIsLogin = n.isLogined,
                window.a_userName = n.userName
            }),
            n.promise()
        });
        return function(n) {
            var o = e();
            return o.done(n),
            o
        }
    } (),
    TT.sudo = function(n, e) {
        TT.login.check(function(o) {
            o ? TT.getUserInfo(n) : e !== !1 && TT.login.login()
        })
    }
} (),
!
function() {
    var n = window.HUYA_UTIL;
    window.NAV_UTIL = window.NAV_UTIL ? window.NAV_UTIL: window.NAV_UTIL = {},
    NAV_UTIL.login = function() {
        TT.login.login()
    },
    NAV_UTIL.logout = function() {
        TT.login.logout()
    },
    NAV_UTIL.getStatus = function(n) {
        TT.getUserInfo(function() {
            "function" == typeof n && n()
        })
    },
    NAV_UTIL.isLogin = function() {
        return n.hasCookie("yyuid")
    },
    NAV_UTIL.getUserInfo = function() {
        return NAV_UTIL.isLogin() ? {
            uName: n.getCookieVal("username"),
            yyID: n.getCookieVal("yyuid")
        }: null
    },
    NAV_UTIL.checkLogin = function(n) {
        TT.sudo(function(e) {
            "function" == typeof n && n(e.uid, e.userName)
        })
    },
    NAV_UTIL.check = TT.getUserInfo
} (),
!
function() {
    var n = function() {
        var n = $.Deferred(),
        e = 0,
        o = function() {++e >= 2 && n.resolve()
        },
        t = !1,
        a = setTimeout(function() {
            t = !0,
            o()
        },
        3500);
        return $(window).on("load",
        function() {
            t || (clearTimeout(a), o())
        }),
        TT.on("reportReady",
        function() {
            o()
        },
        !0),
        function(e) {
            n.done(e)
        }
    } (),
    e = function(e) {
        var o = [];
        $.each(e,
        function(n, e) {
            if (e) {
                var t = "pos=" + e.pos + ",resid=" + (e.rsc || 0) + (e.ren ? ",uid=" + e.ren: "");
                o.push("{" + t + "}")
            }
        }),
        n(function() {
            try {
                ya.reportProductEvent({
                    eid: "pageview/position",
                    extra_ext: "[" + o.join(",") + "]"
                })
            } catch(n) {}
        })
    },
    o = function() {
        var n = [],
        o = null;
        return function(t) {
            t && (clearTimeout(o), n.push(t), n.length >= 10 ? (e(n), n = []) : o = setTimeout(function() {
                e(n),
                n = []
            },
            2e3))
        }
    } ();
    n(function() {
        $(".J_g_resource").each(function() {
            o($(this).data())
        }),
        $("body").on("click", ".J_g_resource",
        function() {
            var n = $(this).data();
            if (n) try {
                ya.reportProductEvent({
                    eid: "click/position",
                    position: n.pos,
                    resourceid: n.rsc,
                    ayyuid: n.ren
                })
            } catch(e) {}
        })
    }),
    window.TT_RSC_EXPS = o
} (),
window.TT_HD_CFG = $.extend(!0, {
    style: "normal"
},
window.TT_HD_CFG_CUSTOM);
var header_right = function(obj) {
    {
        var __t, __p = "";
        Array.prototype.join
    }
    with(obj || {}) __p += '<div class="duya-header-control clearfix">\n    <div class="hy-nav-right hy-nav-kaibo">\n        <a class="hy-nav-title clickstat" href="' + (null == (__t = TT.app.main) ? "": __t) + 'e/zhubo" eid="click/navi/kaibo" eid_desc="\u70b9\u51fb/\u5bfc\u822a/\u5f00\u64ad" target="_blank">\n            <i class="hy-nav-icon hy-nav-video-icon"></i>\n            <span class="title">\u5f00\u64ad</span>\n        </a>\n        <div class="nav-expand-list nav-expand-kaibo">\n            <i class="arrow"></i>\n            <div class="kaibo-nav">\n                <a target="_blank" href="' + (null == (__t = TT.app.main) ? "": __t) + 'e/zhubo"><i class="hy-nav-video-icon"></i><span>\u6211\u8981\u5f00\u64ad</span></a>\n                <a target="_blank" href="#"><i class="hy-nav-home-icon"></i><span>\u516c\u4f1a\u5165\u9a7b</span></a>\n            </div>\n        </div>\n    </div>\n    <div class="hy-nav-right hy-nav-download" id="hy-nav-download">\n        <i class="icon-download-light"></i>\n        <a class="hy-nav-title clickstat" href="https://www.huya.com/download/" eid="click/navi/download" eid_desc="\u70b9\u51fb/\u5bfc\u822a/\u4e0b\u8f7d" target="_blank">\n            <i class="hy-nav-icon hy-nav-download-icon"></i>\n            <span class="title">\u4e0b\u8f7d</span>\n        </a>\n        <div class="nav-expand-list nav-expand-download">\n            <i class="arrow"></i>\n            <div id="J_hyDownload" class="hy-header-loading"></div>\n        </div>\n    </div>\n    <div class="hy-nav-right nav-subscribe success-login">\n        <a class="hy-nav-title new-clickstat" href="' + (null == (__t = TT.app.main) ? "": __t) + 'myfollow" target="_blank" report=\'{"eid":"click/position","position":"click/navi/dingyue"}\'>\n            <i class="hy-nav-icon hy-nav-subscribe-icon"></i>\n            <span class="title">\u8ba2\u9605</span>\n        </a>\n        <div class="nav-expand-list nav-expand-follow">\n            <i class="arrow"></i>\n            <div id="J_hyHdFollowBox" class="hy-header-loading"></div>\n        </div>\n    </div>\n    <div class="hy-nav-right hy-nav-history">\n        <a class="hy-nav-title nav-history new-clickstat" id="nav-history" href="' + (null == (__t = TT.app.i) ? "": __t) + 'index.php?m=Subscribe&watch=1" target="_blank" report=\'{"eid":"click/position","position":"click/navi/lishi"}\'>\n            <i class="hy-nav-icon hy-nav-history-icon"></i>\n            <span class="title">\u5386\u53f2</span>\n        </a>\n        <div class="nav-expand-list nav-expand-history">\n            <i class="arrow"></i>\n            <div id="J_hyHdWatchRecordBox" class="hy-header-loading"></div>\n        </div>\n    </div>\n    <div class="hy-nav-right un-login">\n        <a class="hy-nav-title J_hdLg" href="#">\n            <i class="hy-nav-icon hy-nav-login-icon"></i>\n            <span class="title clickstat" id="nav-login" eid="click/navi/sign" eid_desc="\u70b9\u51fb/\u5bfc\u822a/\u767b\u5f55">\u767b\u5f55</span>\n        </a>\n        <i class="hy-nav-login-l">|</i>\n        <a class="hy-nav-title J_hdReg" href="#">\n            <span class="title clickstat" id="nav-regiest" eid="click/navi/login" eid_desc="\u70b9\u51fb/\u5bfc\u822a/\u6ce8\u518c">\u6ce8\u518c</span>\n        </a>\n    </div>\n    <div class="hy-nav-right nav-user success-login">\n        <a class="nav-user-title" href="' + (null == (__t = TT.app.i) ? "": __t) + '" target="_blank">\n            <img id="login-userAvatar" src="//a.msstatic.com/huya/main/assets/img/default_avatar.jpg" alt="\u5934\u50cf" />\n            <span id="login-username"></span>\n            <i></i>\n        </a>\n        <div class="nav-expand-list">\n            <i class="arrow"></i>\n            <div id="J_hyUserCard" class="hy-header-loading"></div>\n        </div>\n    </div>\n</div>';
    return __p
};
$("#J_duyaHeaderRight").html(header_right()),
$(function() {
    DUYA_SUB.publish("huyaHeaderDomReady")
}),
window.TTHD = function() {
    var n = {};
    return n.style = new TT.Event,
    n.style.val = window.TT_HD_CFG.style,
    n.style.set = function(e) {
        e != n.style.val && (n.style.val = e, n.style.emit("change", e))
    },
    n
} (),
function() {
    function n() {
        clearInterval(o),
        t().removeClass("hy-header-style-skr").addClass("hy-hd-fadeInDown hy-header-style-normal"),
        o = setTimeout(function() {
            t().removeClass("hy-hd-fadeInDown")
        },
        500)
    }
    function e() {
        clearInterval(o),
        t().removeClass("hy-hd-fadeInDown hy-header-style-normal").addClass("hy-header-style-skr")
    }
    var o = null,
    t = HUYA_UTIL.once(function() {
        return $("#duya-header")
    });
    TTHD.style.on("change",
    function(o) {
        var t = {
            skr: e,
            normal: n
        } [o];
        t()
    })
} (),
!
function(n) {
    var e = HUYA_UTIL.once(function() {
        return $.ajax({
            url: "https://a.msstatic.com/huya/main/js/header/noticeDialog_fe82c57.js",
            dataType: "script",
            cache: !0
        }).pipe(function() {
            return n.TT_NOTICE_DIALOG
        })
    }),
    o = function(n) {
        return new $.Deferred(function(e) {
            $.ajax({
                type: "get",
                dataType: "jsonp",
                url: "//user.huya.com/user/getUserInfo",
                data: {
                    uid: n
                }
            }).done(function(n) {
                e.resolve(n && 200 == n.code && n.data ? n.data.avatar: "//a.msstatic.com/huya/main/assets/img/default_avatar.jpg")
            }).fail(function() {
                e.resolve("//a.msstatic.com/huya/main/assets/img/default_avatar.jpg")
            })
        })
    };
    n.HUYA_HEAD_liveNotice = function(n) {
        e().done(function(e) {
            new e({
                data: n,
                afterRender: function() {
                    DUYA_SUB.publish("INDEX_LIVE_NOTICE", n.eventId)
                }
            })
        })
    },
    n.HUYA_SUBSCRIBE_PUSH = function(n) {
        if (!$.isEmptyObject(n)) for (var t in n) !
        function(n) {
            var t = {
                title: "\u4f60\u8ba2\u9605\u7684\u4e3b\u64ad: ",
                content: (n.introduction ? '\u5728\u64ad"' + HUYA_UTIL.xssFilter(n.introduction) + '"': "\u6b63\u5728\u5f00\u64ad\u5566\uff0c") + "\u5feb\u62a2\u4e2a\u677f\u51f3\u5750\u597d",
                action: TT.app.main + n.privateHost,
                eventId: n.aid,
                nick: HUYA_UTIL.xssFilter(n.nick),
                gameFullName: HUYA_UTIL.xssFilter(n.gameFullName)
            };
            $.when(e(), o(n.uid)).done(function(n, e) {
                t.image = e,
                new n({
                    data: t,
                    afterRender: function(n) {
                        n.addClass("subscribe-notice"),
                        n.find(".notice-close").addClass("clickstat").attr({
                            eid: "click/dytc/close",
                            eid_desc: "\u70b9\u51fb/\u8ba2\u9605\u5f39\u7a97/\u5173\u95ed"
                        }),
                        n.find(".notice-btn").addClass("clickstat").attr({
                            eid: "click/dytc/in",
                            eid_desc: "\u70b9\u51fb/\u8ba2\u9605\u5f39\u7a97/\u8fdb\u5165\u76f4\u64ad\u95f4"
                        })
                    }
                })
            })
        } (n[t])
    },
    n.HUYA_NOTICE_PUSH = function(n) {
        e().done(function(e) {
            new e({
                data: n,
                afterRender: function(n) {
                    n.addClass("normal-notice")
                }
            })
        })
    }
} (window),
window.DUYA_PUSH = {
    mapSubscribeList: function() {},
    refreshPush: function() {}
},
TT.onload(function() {
    TT.sudo(function() {
        $.ajax({
            url: "https://a.msstatic.com/huya/main/js/header/push_85bceca.js",
            dataType: "script",
            cache: !0
        })
    },
    !1)
}),
function() {
    function n() {
        var n = this;
        TT.on("huyaHeaderDomReady",
        function() {
            n.init()
        })
    }
    n.prototype.init = function() {
        var e = this;
        TT.getUserInfo(function(e) {
            function o() {
                TT.login.logout(!0);
                var n = $("body"),
                e = '<div id="offline-notice"><div class="login-closure-tit">\u6e29\u99a8\u63d0\u793a<span id="offline-notice-close"></span></div><div class="login-closure-cnt"><h2 style="text-align: center;font-size: 18px;font-weight: 800;margin-bottom: 5px;">\u4e0b\u7ebf\u901a\u77e5</h2><p style="text-align: center;">\u60a8\u7684\u8d26\u53f7' + HUYA_UTIL.getCookieVal("username") + '\uff0c\u72b6\u6001\u53d1\u751f\u53d8\u66f4\uff0c\u8bf7\u91cd\u65b0\u767b\u5f55</p></div><span id="offline-notice-sure">\u91cd\u65b0\u767b\u5f55</span></div><div id="offline-notice-mask"></div>';
                n.append(e),
                $("#offline-notice-sure,#offline-notice-close").one("click",
                function() {
                    $("#offline-notice-mask").remove(),
                    $("#offline-notice").remove()
                }),
                $("#offline-notice-sure").one("click",
                function() {
                    TT.login.login()
                })
            }
            var t = n.prototype;
            1 == e.isChanged && o(),
            t.islogin = e.isLogined,
            t.userName = e.userName,
            t.userNick = e.userNick,
            t.userLogo = e.userLogo,
            t.userId = e.uid,
            e.isLogined ? ($("#duya-header .success-login").show(), $("#login-username").attr("title", t.userNick).text(t.userNick), $("#login-userAvatar").attr("src", TT.trimUrl(t.userLogo))) : $("#duya-header .un-login").show(),
            "function" == typeof window.checkLoginCallback && window.checkLoginCallback()
        }),
        e.headEven()
    },
    n.prototype.headEven = function() {
        var n = $("#duya-header");
        n.on("click", ".J_hdLg",
        function(n) {
            n.preventDefault(),
            TT.login.login()
        }),
        n.on("click", ".J_hdReg",
        function(n) {
            n.preventDefault(),
            TT.login.register()
        })
    },
    n.prototype.myMessageShow = function() {
        var n = $("#J_huyaNavUserMsgDot");
        n.show(),
        $("#J_huyaNavUserMsg").off("click").on("click",
        function() {
            n.hide()
        })
    },
    n.prototype.myMessageHide = function() {
        $("#J_huyaNavUserMsgDot").hide()
    },
    window.DUYA_INS = new n
} (),
$(function() {
    function n(n) {
        var o = document.body.className.replace(/(^[\s\t\xa0\u3000]+|[\s\t\xa0\u3000]+$)/g, ""),
        t = o ? document.body.className.split(/\s+/) : [],
        a = e(t, "w-1000");
        1 === n ? -1 != a && (t.splice(a, 1), document.body.className = t.join(" ")) : -1 == a && (t.push("w-1000"), document.body.className = t.join(" "))
    }
    var e = function(n, e) {
        for (var o = 0,
        t = n.length; t > o; o++) if (n[o] === e) return o;
        return - 1
    },
    o = document.documentElement ? document.documentElement: document.body,
    t = function() {
        var n = o.clientWidth;
        return n >= 1480 ? 1 : 0
    },
    a = t();
    n(a);
    var i = null,
    r = function() {
        clearTimeout(i),
        i = setTimeout(function() {
            var e = t();
            e !== a && n(a = e)
        },
        60)
    };
    $(window).on("resize",
    function() {
        r(),
        DUYA_SUB.publish("resizeWide", o.clientWidth)
    })
}),
$(function() {
    function n() {
        var n = o.clientWidth;
        return n >= 1720 ? "l": n >= 1440 ? "m": "s"
    }
    function e(n) {
        var e = "hy-hd-vp-";
        $(document.body).removeClass(e + ["l", "m", "s"].join(" " + e)).addClass(e + n)
    }
    var o = document.documentElement ? document.documentElement: document.body,
    t = n();
    e(t);
    var a = null,
    i = function() {
        clearTimeout(a),
        a = setTimeout(function() {
            var o = n();
            o !== t && e(t = o)
        },
        60)
    };
    $(window).on("resize", i)
}),
$(function() {
    function n(n) {
        var e = 0,
        o = n[e],
        t = new $.Callbacks,
        a = null;
        return {
            val: function() {
                return o
            },
            onchange: function(n) {
                t.add(n)
            },
            play: function() {
                n.length && null === a && (a = setInterval(function() {
                    t.fire(o = n[++e < n.length ? e: e = 0])
                },
                5e3))
            },
            pause: function() {
                clearInterval(a),
                a = null
            }
        }
    }
    function e(n) {
        var e = $("#searchForm_id input"),
        o = e[0];
        "placeholder" in o ? (o.placeholder = n.val(), n.onchange(function() {
            o.placeholder = n.val()
        }), e.on("focus",
        function() {
            n.pause()
        }).on("blur",
        function() {
            n.play()
        })) : (e.val(n.val()), n.onchange(function() {
            e.val(n.val())
        }), e.on("focus",
        function() {
            n.pause(),
            e.val() == n.val() && e.val("")
        }).on("blur",
        function() {
            "" == e.val() && (e.val(n.val()), n.play())
        }))
    }
    function o(n) {
        var e = TT.app.main + "/cache10min.php?m=Search&do=getHotword&v=2";
        HUYA_UTIL.isMobile && (e = TT.app.main + "/cacheapp.php?m=Search&do=getHotword&v=2"),
        $.ajax({
            type: "get",
            url: e,
            dataType: "jsonp",
            jsonpCallback: "huyaNavPlaceholder",
            cache: !0
        }).done(n)
    }
    DUYA_SUB.subscribe("huyaHeaderDomReady",
    function() {
        var t = "TT_HEADER_SEARCH_PLACEHOLDER",
        a = (localStorage.getItem(t) || "\u4e3b\u64ad\u3001\u9891\u9053\u3001\u6e38\u620f").split(","),
        i = new n(a);
        i.play(),
        new e(i);
        var r = HUYA_UTIL.once(function() {
            o(function(n) {
                if (n && n.length) {
                    var e = [];
                    $.each(n,
                    function(n, o) {
                        e.push(o.hot_word)
                    }),
                    localStorage.setItem(t, e.join(","))
                } else localStorage.removeItem(t)
            })
        });
        TT.lazyload(r)
    },
    !0)
}),
TT.on("huyaHeaderDomReady",
function() {
    var n = HUYA_UTIL.once(function() {
        $.ajax({
            url: "https://a.msstatic.com/huya/main/js/header/category_1b30d23.js",
            dataType: "script",
            cache: !0
        }).done(function() {
            window.TT_HEADER_CATEGORY.init().done(function() {
                $(".nav-expand-game").removeClass("hy-header-loading")
            })
        })
    });
    TT.lazyload(n),
    $("#hy-nav-category").one("mouseenter", n)
},
!0),
TT.on("huyaHeaderDomReady",
function() {
    var n = HUYA_UTIL.once(function() {
        return $.ajax({
            url: "https://a.msstatic.com/huya/main/js/header/match_5c9fadd.js",
            dataType: "script",
            cache: !0
        }).pipe(function() {
            return window.TT_HEADER_MATCH_PANEL
        })
    });
    TT.lazyload(n),
    $("#hy-nav-match").one("mouseenter",
    function() {
        n().done(function(n) {
            n.open()
        })
    })
},
!0),
TT.on("huyaHeaderDomReady",
function() {
    var n = HUYA_UTIL.once(function() {
        return $.ajax({
            url: "https://a.msstatic.com/huya/main/js/header/video_b54a456.js",
            dataType: "script",
            cache: !0
        }).pipe(function() {
            return window.TT_HEADER_VIDEO_PANEL
        })
    });
    TT.lazyload(n),
    $("#hy-nav-video").one("mouseenter",
    function() {
        n().done(function(n) {
            n.open()
        })
    })
},
!0),
TT.on("huyaHeaderDomReady",
function() {
    var n = HUYA_UTIL.once(function() {
        return $.ajax({
            url: "https://a.msstatic.com/huya/main/js/header/download_4174979.js",
            dataType: "script",
            cache: !0
        }).pipe(function() {
            return window.TT_HEADER_DOWNLOAD_PANEL
        })
    });
    TT.lazyload(n),
    $("#hy-nav-download").one("mouseenter",
    function() {
        n().done(function(n) {
            n.open()
        })
    })
},
!0),
TT.on("huyaHeaderDomReady",
function() {
    var n = HUYA_UTIL.once(function() {
        return $.ajax({
            url: "https://a.msstatic.com/huya/main/js/header/record_9eeb076.js",
            dataType: "script",
            cache: !0
        })
    });
    TT.lazyload(n),
    $("#nav-history").one("mouseenter",
    function() {
        n().done(function() {
            window.TT_HEADER_WATCH_RECORD.init()
        })
    })
},
!0),
TT.on("huyaHeaderDomReady",
function() {
    TT.sudo(function() {
        var n = HUYA_UTIL.once(function() {
            return $.ajax({
                url: "https://a.msstatic.com/huya/main/js/header/follow_56eaa45.js",
                dataType: "script",
                cache: !0
            })
        });
        TT.lazyload(n),
        $("#duya-header .nav-subscribe").one("mouseenter",
        function() {
            n().done(function() {
                window.TT_HEADER_FOLLOW.init()
            })
        })
    },
    !1)
},
!0),
TT.on("huyaHeaderDomReady",
function() {
    TT.sudo(function() {
        var n = HUYA_UTIL.once(function() {
            return $.ajax({
                url: "https://a.msstatic.com/huya/main/js/header/userCard_e49ba96.js",
                dataType: "script",
                cache: !0
            }).pipe(function() {
                return $("#J_hyUserCard").removeClass("hy-header-loading"),
                window.TT_HEADER_USER_CARD
            })
        });
        TT.lazyload(n),
        $(".nav-user-title").one("mouseenter",
        function() {
            n().done(function(n) {
                new n
            })
        })
    },
    !1)
},
!0),
TT.onload(function() {
    $.ajax({
        url: "https://a.msstatic.com/huya/main/js/header/gg_3f7bdad.js",
        dataType: "script",
        cache: !0
    })
}),
$(function() {
    var n = $("#J_duyaHdSearch");
    n.data("disabled") || HUYA_UTIL.getScript("https://a.msstatic.com/huya/main/js/header_search_tip_3987eb7.js")
}),
$(function() {
    var n = HUYA_UTIL.getLocationSearchParam();
    "login" == n.evt_fe && TT.login.login()
});