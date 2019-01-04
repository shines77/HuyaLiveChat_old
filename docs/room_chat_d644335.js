define("chat/broadcaster",
function(e, n, a) {
    function t(e) {
        o.fire({
            data: e,
            isOwn: !(!r || e.tUserInfo.lUid != r)
        })
    }
    var i = e("TTP"),
    o = (e("TTR"), e("tools"), new $.Callbacks);
    a.exports = {
        addListener: function(e) {
            o.add(e)
        },
        removeListener: function(e) {
            o.remove(e)
        },
        addOnceListener: function(e) {
            function n() {
                e.apply(this, arguments),
                o.remove(n)
            }
            o.add(n)
        }
    };
    var r = HUYA_UTIL.getCookieVal("yyuid");
    i.isH5 ? i.ready(function(e) {
        e.addTafListener("1400", t)
    }) : i.on("onSpeakMessage",
    function(e) {
        t(e.param)
    })
});;
define("chat/barrageColor",
function(t, e, n) {
    var o = t("tools"),
    r = t("DefineValue"),
    a = t("chat/broadcaster"),
    i = new r( - 1);
    i.toRgb = function(t) {
        return - 1 === t ? "FFFFFF": o.toRgb(t)
    },
    i.rgb = function() {
        return i.toRgb(i.get())
    };
    var u = new r( - 1),
    c = new r( - 1);
    u.change(function(t) {
        i.set( - 1 !== t ? t: c.get())
    }),
    c.change(function(t) { - 1 === u.get() && i.set(t)
    }),
    n.exports = $.extend({
        defaultColor: c,
        customColor: u
    },
    i, {
        set: null
    }),
    a.addListener(function(t) {
        if (t.isOwn && -1 === u.get()) {
            var e = t.data.tBulletFormat;
            e && e.iFontColor && c.set(e.iFontColor)
        }
    })
});;
define("chat/barrageColorFans",
function(e, o, t) {
    function a(e) {
        function o() {
            if (s.isSame(s.current(), g)) {
                var e = i.get();
                p.find(".color-item").each(function(o, t) {
                    $(t)[o > e ? "addClass": "removeClass"]("locked")
                }),
                s.current().iVFlag && p.find(".color-item").last().removeClass("locked")
            } else p.find(".color-item").each(function(e, o) {
                $(o)[0 != e ? "addClass": "removeClass"]("locked")
            })
        }
        function t() {
            function e() {
                localStorage.removeItem(v),
                t.clear()
            }
            if (s.isSame(s.current(), g)) {
                var o = p.find(".color-item").eq(i.get());
                o.html('<span style="position: absolute;width: 25px;height: 20px;color: #fff;background-color: #f50;text-align: center;line-height: 20px;border-radius: 10px; top: -7px;right: -10px; z-index:2">\u65b0</span>'),
                m.one("mouseleave", e),
                o.one("mouseenter", e),
                s.onUse(function() {
                    m.off("mouseleave", e),
                    o.off("mouseenter", e)
                })
            }
        }
        function a(e) {
            var o = _.indexOf(d, e); - 1 != o ? p.find(".color-item").removeClass("current").eq(o).addClass("current") : p.find(".color-item").removeClass("current")
        }
        function n(e, o) {
            var t = $('<div class="color-grade-tip common-panel-tip">' + e + "</div>");
            m.append(t),
            t.css({
                left: o.position().left + 24 - t.outerWidth() / 2,
                top: o.position().top - 25
            })
        }
        var r = e.color,
        i = e.grade,
        s = e.badge,
        g = e.anchorBadge,
        m = $("#J-room-color-box"),
        p = $("#J-color-list-fans"),
        h = function() {
            var e = "";
            $.each(d,
            function(o, t) {
                e += '<li class="color-item' + ("FFFFFF" == t ? " color-item1": "") + '" style="background-color: #' + t + ';"></li>'
            }),
            p.html(e)
        };
        h(),
        r.defaultColor.change(function(e) {
            var o = l.toRgb(e);
            p.find(".color-item").eq(0).css("backgroundColor", "#" + o)["FFFFFF" == o ? "addClass": "removeClass"]("color-item1")
        }),
        o(),
        i.change(o),
        s.onUse(o);
        var v = "ROOM_CHAT_BARRAGE_COLOR_NEW_" + TT.env + c.lp;
        i.change(function() {
            localStorage.setItem(v, 1),
            t(),
            TT.event.emit("BARRAGE_COLOR_UNLOCK")
        }),
        localStorage.getItem(v) && t(),
        s.onUse(function() {
            s.isSame(s.current(), g) ? localStorage.getItem(v) && t() : t.clear()
        }),
        t.clear = function() {
            p.find(".color-item").eq(i.get()).html("")
        },
        a(r.rgb()),
        r.change(function() {
            a(r.rgb())
        }),
        s.onUse(function() {
            r.customColor.set(f[0])
        }),
        p.on("mouseenter", ".color-item",
        function() {
            var e = $(this),
            o = e.index();
            if (e.hasClass("locked")) {
                var t = "";
                if (g.lBadgeId > 0) {
                    var a = s.getItem(g.lBadgeId, g.iBadgeType);
                    t = a ? s.isSame(s.current(), g) ? o == d.length - 1 ? "\u94bb\u7c89\u4e13\u5c5e": "\u9700\u8981\u62e5\u6709\u3010" + s.getBadgeName(a) + '\u3011\u5fbd\u7ae0<span class="tip-mark">' + u[o] + "\u7ea7</span>": "\u8bf7\u6234\u4e0a\u60a8\u7684\u3010" + s.getBadgeName(a) + "\u3011\u5fbd\u7ae0": o == d.length - 1 ? "\u94bb\u7c89\u4e13\u5c5e": "\u9700\u8981\u62e5\u6709\u3010" + (1 == g.iBadgeType ? g.tFaithItem.sFaithName: g.sBadgeName) + '\u3011\u5fbd\u7ae0<span class="tip-mark">' + u[o] + "\u7ea7</span>"
                } else t = "\u4e3b\u64ad\u6682\u672a\u5f00\u901a\u7c89\u4e1d\u5fbd\u7ae0";
                n(t, e)
            }
        }).on("mouseleave", ".color-item",
        function() {
            n.clear()
        }).on("click", ".color-item",
        function() {
            var e = $(this),
            o = e.index();
            if (!e.hasClass("locked") && !e.hasClass("current")) {
                r.customColor.set(f[o]);
                try {
                    ya.reportProductEvent({
                        eid: "click/room/chatColor/" + o,
                        eid_desc: "\u70b9\u51fb/\u76f4\u64ad\u95f4/\u5f39\u5e55\u989c\u8272/" + d[o]
                    })
                } catch(t) {}
            }
        }),
        n.clear = function() {
            $(".color-grade-tip").remove()
        }
    }
    function n() {
        s.ready(function(e, o) {
            var t = e.getItem(o.lBadgeId, o.iBadgeType),
            n = new i(r(t ? t.iBadgeLevel: 0));
            e.onChange(function(t) {
                e.isSame(t.tBadgeInfo, o) && n.set(r(t.tBadgeInfo.iBadgeLevel))
            });
            new a({
                color: l,
                grade: n,
                badge: e,
                anchorBadge: o
            })
        })
    }
    function r(e) {
        var o = 0;
        return $.isNumeric(e) && ("number" != typeof e && (e = parseInt(e, 10)), $.each(u,
        function(t, a) {
            return e >= a ? void(o = t) : !1
        })),
        o
    }
    var c = e("TTA"),
    i = (e("tools"), e("DefineValue")),
    l = e("chat/barrageColor"),
    s = e("widget/fansbadge"),
    d = [l.toRgb(l.defaultColor.get()), "66FF99", "CCFF66", "22CECE", "FF706E", "FF66FF", "FFCC00", "FF6699"];
    l.defaultColor.change(function(e) {
        d[0] = l.toRgb(e)
    });
    var f = $.map(d,
    function(e, o) {
        return 0 === o ? -1 : parseInt(e, 16)
    }),
    u = [0, 4, 6, 8, 10, 14, 18];
    t.exports = {
        createPalette: n
    }
});;
define("chat/barragePosition",
function(e, t, n) {
    function o(e) {
        function t() {
            var e = [1, 3, 2],
            t = ["\u6b63\u5e38", "\u9876\u90e8", "\u5e95\u90e8"],
            n = "";
            $.each(s,
            function(o) {
                n += '<li class="room-select-item room-select-item' + e[o] + '">',
                n += '<div class="room-select-pic"></div><span class="room-select-text">' + t[o] + "</span>",
                n += "</li>"
            }),
            l.html(n)
        }
        function n() {
            l.on("click", ".room-select-item",
            function(e) {
                e.preventDefault();
                var t = $(this).index();
                if (a.get() >= t && c.get() != s[t]) {
                    c.set(s[t]);
                    try {
                        ya.reportProductEvent({
                            eid: "click/room/chatPos/" + t,
                            eid_desc: "\u70b9\u51fb/\u76f4\u64ad\u95f4/\u5f39\u5e55\u4f4d\u7f6e/" + s[t]
                        })
                    } catch(e) {}
                }
            }).on("mouseenter", ".room-select-item",
            function() {
                var e = $(this).index();
                e > a.get() && o(r[e], $(this).position())
            }).on("mouseleave", ".room-select-item",
            function() {
                o.clear()
            })
        }
        function o(e, t) {
            var n = $('<div class="font-grade-tip common-panel-tip"><span class="tip-mark">' + e + "\u7ea7</span>\u89e3\u9501</div>");
            n.css({
                left: t.left - 10,
                top: t.top - 35
            }),
            $("#J-room-speaking-box").append(n)
        }
        function i(e) {
            l.find(".room-select-item").removeClass("on").eq(_.indexOf(s, e)).addClass("on")
        }
        var c = e.pos,
        a = e.grade,
        l = $(".room-select-list");
        o.clear = function() {
            $(".font-grade-tip").remove()
        },
        t(),
        n(),
        i(c.get()),
        c.change(i)
    }
    function i() {
        a.get(function(e) {
            var t = new c(l(e));
            a.change(function(e) {
                var n = l(e);
                n !== t.get() && t.set(n)
            });
            new o({
                pos: f,
                grade: t
            })
        })
    }
    var c = (e("tools"), e("DefineValue")),
    a = e("userLevel"),
    s = [1, 3, 2],
    r = [0, 25, 30],
    l = function(e) {
        var t = 0;
        if ($.isNumeric(e)) {
            e = parseInt(e, 10);
            for (var n = 0,
            o = r.length; o > n && e >= r[n]; n++) t = n
        }
        return t
    },
    f = new c(s[0]);
    n.exports = {
        get: f.get,
        change: f.change,
        createPalette: i
    }
});;
define("chat/barrageSize",
function(e, t, n) {
    function o(e) {
        function t() {
            var e = "";
            $.each(c,
            function(t) {
                e += '<li class="room-font-item room-font-item' + (t + 1) + '"></li>'
            }),
            s.html(e)
        }
        function n() {
            s.on("click", ".room-font-item",
            function(e) {
                e.preventDefault();
                var t = $(this).index();
                if (r.get() >= t && a.get() != c[t]) {
                    a.set(c[t]);
                    try {
                        ya.reportProductEvent({
                            eid: "click/room/chatSize/" + t,
                            eid_desc: "\u70b9\u51fb/\u76f4\u64ad\u95f4/\u5f39\u5e55\u5927\u5c0f/" + c[t]
                        })
                    } catch(e) {}
                }
            }).on("mouseenter", ".room-font-item",
            function() {
                var e = $(this).index();
                e > r.get() && o(f[e], $(this).position())
            }).on("mouseleave", ".room-font-item",
            function() {
                o.clear()
            })
        }
        function o(e, t) {
            var n = $('<div class="font-grade-tip common-panel-tip"><span class="tip-mark">' + e + "\u7ea7</span>\u89e3\u9501</div>");
            n.css({
                left: t.left - 10,
                top: t.top - 35
            }),
            $("#J-room-speaking-box").append(n)
        }
        function i(e) {
            s.find(".room-font-item").removeClass("on").eq(_.indexOf(c, e)).addClass("on")
        }
        var a = e.size,
        r = e.grade,
        s = $(".room-font-list");
        o.clear = function() {
            $(".font-grade-tip").remove()
        },
        t(),
        n(),
        i(a.get()),
        a.change(i)
    }
    function i() {
        r.get(function(e) {
            var t = new a(s(e));
            r.change(function(e) {
                var n = s(e);
                n !== t.get() && t.set(n)
            });
            new o({
                size: m,
                grade: t
            })
        })
    }
    var a = (e("tools"), e("DefineValue")),
    r = e("userLevel"),
    c = [4, 2],
    f = [0, 20],
    s = function(e) {
        var t = 0;
        if ($.isNumeric(e)) {
            e = parseInt(e, 10);
            for (var n = 0,
            o = f.length; o > n && e >= f[n]; n++) t = n
        }
        return t
    },
    m = new a(c[0]);
    n.exports = {
        get: m.get,
        change: m.change,
        createPalette: i
    }
});;
define("chat/receive",
function(e, a, s) {
    function n(e) {
        return "string" != typeof e ? e: e = e.replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/\'/g, "&#39;").replace(/\"/g, "&quot;")
    }
    function r() {
        $.extend(this, new p)
    }
    function i(e) {
        var a = e.iColor > 0 ? " color: #" + o(e.iColor) + ";": "";
        return '<span style="display: inline-block; line-height: 20px;' + a + '">' + e.sText + "</span>"
    }
    function o(e) {
        if (e > 0) {
            var a = e.toString(16);
            if (a.length < 6) {
                for (var s = 6 - a.length,
                n = "",
                r = 0; s > r; r++) n += "0";
                e = n + a
            } else e = a
        } else e = 0;
        return e
    }
    var t = (e("TTP"), e("TTR"), e("msgManager")),
    l = e("Emot"),
    p = (e("tools"), e("Event")),
    c = e("widget/fans-icon");
    r.prototype = {
        showMessage: function(e) {
            var a = e.tUserInfo,
            s = e.sContent;
            try {
                s = decodeURIComponent(e.sContent)
            } catch(r) {}
            var p = n(s),
            c = 0,
            d = e.vDecorationPrefix.value,
            m = [],
            v = null,
            f = -1;
            if (d && _.isArray(d)) for (var u = 0,
            y = d.length; y > u; u++) {
                var T = d[u].iAppId,
                h = d[u].iViewType,
                U = d[u].vData,
                x = "";
                0 == h && g[T] && (x = g[T](U)),
                1 == h && (x = i(U)),
                2 == h && (x = '<img src="' + TT.trimUrl(U.sUrl).replace(".png", "_3.png") + '" />'),
                10100 == T && (1 == U.iType || 2 == U.iType) || 10090 == T ? v ? (10090 == v.iAppId && 10100 == T && 1 == U.iType || 10100 == v.iAppId && 2 == v.vData.iType && 10090 == T) && (v = d[u], m.splice(f, 1, x)) : (v = d[u], f = m.length, m.push(x)) : m.push(x),
                10200 == T && (c = U.iLevel)
            }
            var I = e.vDecorationSuffix.value,
            C = "",
            k = "",
            S = "";
            if (I && _.isArray(I)) for (var u = 0,
            y = I.length; y > u; u++) {
                var h = I[u].iViewType;
                2 == h && (S = I[u].vData.sUrl, S && (k = S.lastIndexOf("."), C += '<img src="' + TT.trimUrl(S.slice(0, k)) + "_3" + S.slice(k) + '">'))
            }
            var b = a.sNickName;
            try {
                b = n(decodeURIComponent(b))
            } catch(r) {
                b = n(b)
            }
            var w = {
                uid: a.lUid,
                nick: b,
                msg: p
            };
            p = l.replace(p);
            var M = o(e.tFormat.iFontColor),
            D = 0 != M ? ' style="color: #' + M + ';"': "",
            F = "";
            F = c && c > 0 ? '<div class="msg-nobleSpeak box-noble-level-' + c + '"><p class="msg-nobleSpeak-decorationPrefix">' + m.join("") + '</p><span class="name J_userMenu" title="\u70b9\u51fb\u67e5\u770b\u4e2a\u4eba\u4fe1\u606f">' + b + '</span><p class="msg-nobleSpeak-decorationSuffix">' + C + '</p><span class="colon">:</span><span class="msg"' + D + ">" + p + "</span></div>": '<div class="msg-normal"><p class="msg-normal-decorationPrefix">' + m.join("") + '</p><span class="name J_userMenu" title="\u70b9\u51fb\u67e5\u770b\u4e2a\u4eba\u4fe1\u606f">' + b + '</span><p class="msg-normal-decorationSuffix">' + C + '</p><span class="colon">:</span><span class="msg"' + D + ">" + p + "</span></div>",
            t.add(F, w)
        },
        onSpeakMessage: function(e) {
            var a = this,
            s = e.tUserInfo;
            if (s && s.lUid < 0) {
                var n = o(e.tFormat.iFontColor),
                r = 0 == n ? "": ' style="color: #' + n + ';"',
                i = s.sNickName || "";
                if (i) try {
                    i = decodeURIComponent(i)
                } catch(l) {}
                var p = e.sContent;
                if (p) try {
                    p = decodeURIComponent(p)
                } catch(l) {}
                var c = '<div class="msg-sys">' + (i ? '<span class="msg-sys-admin">' + _.escape(i) + '</span><span class="colon">:</span>': "") + '<span class="msg"' + r + ">" + _.escape(p).replace(/\n/g, "<br />") + "</span></div>";
                t.add(c)
            } else a.showMessage(e)
        }
    };
    var g = {
        10090 : function(e) {
            return '<img src="' + TT.trimUrl(e.sUrl).replace(".png", "_3.png") + '" />'
        },
        10100 : function(e) {
            return 1 == e.iType || 2 == e.iType ? '<img src="' + TT.trimUrl(e.sManagerDecorationUrl).replace(".png", "_3.png") + '" />': ""
        },
        10400 : function(e) {
            return c.create(e.iBadgeLevel, 1 == e.iBadgeType ? e.tFaithInfo.sFaithName: e.sBadgeName, e.lBadgeId, e.iBadgeType)
        }
    };
    s.exports = new r
});;
define("chat/speakLimit",
function(e, t, i) {
    function n() {
        var e = new Date;
        return e.getTime()
    }
    function a() {
        f >= u.maxNumber ? (l = !0, window.ya && ya.reportProductEvent({
            eid: "Status/BarrageTooFast",
            eid_desc: "\u6b21\u6570/\u53d1\u8a00\u9891\u7387\u9650\u5236",
            ref: YA.cookie.get("ya_eid")
        })) : l = !1
    }
    function o() {
        w ? T = n() : (v = T, T = n(), T - v < 1e3 * u.limitedTime ? f++:s()),
        w = !1
    }
    function m() {
        r()
    }
    function s() {
        f = 1,
        l = !1,
        g = u.waitTime,
        $(".room_msg_fast_tip").hide()
    }
    function r() {
        if (0 == $(".room_msg_fast_tip").length) {
            var e = $('<div class="room_msg_fast_tip">\u8bf4\u5f97\u592a\u5feb\u5566\uff0c\u4f11\u606f<span class="msg_send_time">' + g + "</span> \u79d2\u5427~</div>");
            p.append(e)
        } else $(".msg_send_time").text(g),
        $(".room_msg_fast_tip").show();
        clearTimeout(d),
        d = setTimeout(function() {
            $(".room_msg_fast_tip").hide(),
            clearTimeout(d)
        },
        3e3),
        c || (c = setInterval(function() {
            g--;
            var e = $(".msg_send_time");
            e.text(g),
            0 >= g && (clearInterval(c), c = null, s())
        },
        1e3))
    }
    function _(e) {
        l ? m() : (e(), o(), a())
    }
    var u = {
        limitedTime: 10,
        maxNumber: 5,
        waitTime: 21
    },
    c = null,
    d = null,
    f = 1,
    l = !1,
    g = u.waitTime,
    p = $(".chat-room__bd3"),
    T = 0,
    v = 0,
    w = !0;
    i.exports = _
});;
define("chat/speak",
function(e) {
    function n(e) {
        S.emit("sendBefore"),
        s.ready(function(n) {
            if (s.isH5) {
                var t = new HUYA.SendMessageReq;
                t.tUserId = n.userId,
                t.lTid = h,
                t.lSid = v,
                t.lPid = o.lp,
                t.sContent = e,
                t.tBulletFormat = $.extend(t.tBulletFormat || {},
                y),
                n.sendWup("liveui", "sendMessage", t)
            } else n.callAs("speakSendMessage", {
                lTid: h,
                lSid: v,
                sContent: e,
                tBulletFormat: y
            })
        }),
        S.emit("speak", e)
    }
    function t(e, n) {
        if (document.selection) e.focus(),
        sel = document.selection.createRange(),
        sel.text = n,
        sel.select();
        else if (e.selectionStart || "0" == e.selectionStart) {
            e.blur();
            var t = e.selectionStart,
            i = e.selectionEnd,
            s = e.scrollTop;
            e.value = e.value.substring(0, t) + n + e.value.substring(i, e.value.length),
            s > 0 && (e.scrollTop = s),
            e.focus(),
            e.selectionStart = t + n.length,
            e.selectionEnd = t + n.length
        } else e.value += n,
        e.focus()
    }
    function i() {
        var e = this;
        e.target = $("#pub_msg_input"),
        $("#msg_send_bt").click(function() {
            $(this).hasClass("enable") && e.send()
        }),
        e.target.bind("keydown",
        function(n) {
            if (13 == n.keyCode) {
                if (!n.ctrlKey) return e.send(),
                !1;
                t(e.target[0], "\r\n")
            }
        }),
        s.on("sendDanmuText",
        function(n) {
            n && n.param && n.param.danmutext && (e.target.val(n.param.danmutext), e.send())
        }),
        c.onEnter(function(n, t) {
            3 == t.iUserType && (e.silenced = !0, e.silencedTips = t.sSendMessageTips, e.silencedTips_display = t.bSendMessagePopUp, e.silencedCD = setTimeout(function() {
                e.silenced = !1
            },
            1e3 * t.iSendMessagePopUpAmtTime))
        }),
        c.change(function(n, t) {
            3 == t.iNewUserType ? (e.silenced = !0, e.silencedTips = t.sSendMessageTips, e.silencedTips_display = t.bSendMessagePopUp, e.silencedCD = setTimeout(function() {
                e.silenced = !1
            },
            1e3 * t.iSendMessagePopUpAmtTime)) : (e.silenced = !1, e.silencedCD && clearTimeout(e.silencedCD))
        }),
        $("#" + this.tipsId + " a").click(function() {
            $(this).parent().hide()
        })
    }
    var s = (e("tools"), e("TTP")),
    o = e("TTA"),
    a = e("TTR"),
    d = e("Event"),
    r = e("input_ob"),
    c = e("auditor"),
    l = e("chat/barrageColor"),
    u = e("chat/barrageSize"),
    f = e("chat/barragePosition"),
    p = e("bindPhone"),
    g = e("widget/speakTipsBindPhone"),
    m = e("chat/speakLimit"),
    T = e("Emot"),
    h = a.id,
    v = a.sid,
    S = (a.isOn, a.isReplay, a.isOff, new d),
    y = {};
    return y.iFontColor = l.customColor.get(),
    l.customColor.change(function() {
        y.iFontColor = l.customColor.get()
    }),
    y.iFontSize = u.get(),
    u.change(function() {
        y.iFontSize = u.get()
    }),
    y.iTransitionType = f.get(),
    f.change(function() {
        y.iTransitionType = f.get()
    }),
    S.send = n,
    function() {
        function e() {
            s.ready(function(e) {
                s.isH5 ? e.sendWup("mobileui", "setUserProfile", new HUYA.SetUserProfileReq) : e.callAs("setUserProfile")
            })
        }
        function n(e) {
            s.ready(function(n) {
                s.isH5 ? n.addTafListener("sendMessage", e) : s.on("speakSendMessageRsp", e)
            })
        }
        function t(e) {
            s.ready(function(n) {
                s.isH5 ? n.removeTafListener("sendMessage", e) : s.off("speakSendMessageRsp", e)
            })
        }
        n(function(i) {
            var s = i.param ? i.param: i;
            if (905 == s.iStatus && 0 == $(".speakTipsBindPhone").length) {
                var o = new g;
                o.on("todo",
                function() {
                    p().fail(function(i) {
                        function s(n) {
                            var i = n.param ? n.param: n;
                            905 != i.iStatus && (S.off("sendBefore", e), t(s))
                        }
                        "IGNORANCE" === i ? (S.off("sendBefore", e), t(s), S.on("sendBefore", e), n(s)) : alert("\u624b\u673a\u7ed1\u5b9a\u5931\u8d25\uff0c\u8bf7\u7a0d\u540e\u91cd\u8bd5\uff01(" + i + ")")
                    })
                })
            }
        })
    } (),
    i.prototype = {
        tipsId: "pubNoticMe",
        maxInputSize: 30,
        send: function() {
            var e = this;
            HUYA_UTIL.getCookieVal("yyuid") && m(function() {
                e.sendMessage()
            })
        },
        sendMessage: function() {
            var e = this,
            t = e.target.val(),
            i = a_userIsLogin,
            d = a_userName;
            if ("" == d && !i) return NAV_UTIL.login(),
            !1;
            if (e.silenced && e.silencedTips_display) return void e.noticeMe(e.silencedTips);
            if (!t) return void e.noticeMe("请输入要发言的内容！");
            if (t.length > e.maxInputSize) return void e.noticeMe("输入字数最大不能超过" + e.maxInputSize);
            n(T.parseAlt(t,
            function(e, n) {
                return n ? n.code: e
            }));
            try {
                ya.reportProductEvent($.extend({
                    eid: "click/zhibo/sendwords",
                    eid_desc: "点击/直播间/发言按钮",
                    ayyuid: o.lp,
                    gameId: a.gid
                },
                s.get("videoData")))
            } catch(c) {}
            e.target.val(""),
            r.resetAll()
        },
        noticeMe: function(e) {
            if (e) {
                var n = this;
                $("#" + n.tipsId + " p").text(e),
                $("#" + n.tipsId).show(),
                setTimeout(function() {
                    $("#" + n.tipsId).hide()
                },
                3e3)
            }
        }
    },
    s.ready(function() {
        new i
    }),
    S
});;
define("chat/mySelfSpeakCfg",
function(e) {
    function n(e) {
        f = e,
        "pending" === d.state() ? d.resolve() : u.fire(f)
    }
    var i = (e("tools"), e("TTP"), e("TTR")),
    t = e("chat/broadcaster"),
    a = e("chat/barrageColor"),
    r = e("chat/barrageSize"),
    o = e("chat/barragePosition"),
    c = e("widget/fansbadge"),
    f = (i.isOn, i.isReplay, i.isOff, null),
    u = new $.Callbacks,
    d = new $.Deferred;
    return t.addListener(function(e) {
        e.isOwn && n(e.data)
    }),
    d.done(function() {
        a.change(function() {
            f.tBulletFormat.iFontColor = a.get(),
            u.fire(f)
        }),
        r.change(function() {
            f.tBulletFormat.iFontSize = r.get(),
            u.fire(f)
        }),
        o.change(function() {
            f.tBulletFormat.iTransitionType = o.get(),
            u.fire(f)
        })
    }),
    d.done(function() {
        c.ready(function(e) {
            if (e.current()) {
                var n = !1;
                $.each(f.vDecorationPrefix.value,
                function(i, t) {
                    return t && 0 == t.iViewType && 10400 == t.iAppId ? (n = !0, t.vData = e.current(), !1) : void 0
                }),
                n || f.vDecorationPrefix.value.push({
                    iViewType: 0,
                    iAppId: 10400,
                    vData: e.current()
                }),
                u.fire(f)
            }
            e.onUse(function() {
                var n = f.vDecorationPrefix.value,
                i = !1;
                $.each(n,
                function(t, a) {
                    if (e.current()) {
                        if (a && 0 == a.iViewType && 10400 == a.iAppId) return a.vData = e.current(),
                        i = !0,
                        !1
                    } else if (a && 0 == a.iViewType && 10400 == a.iAppId) return n.splice(t, 1),
                    !1
                }),
                e.current() && !i && f.vDecorationPrefix.value.push({
                    iViewType: 0,
                    iAppId: 10400,
                    vData: e.current()
                }),
                u.fire(f)
            }),
            e.onChange(function(e, n) {
                if (n && e.iBadgeLevelChanged) {
                    var i = f.vDecorationPrefix.value;
                    $.each(i,
                    function(n, i) {
                        return i && 0 == i.iViewType && 10400 == i.iAppId ? (i.vData = e.tBadgeInfo, !1) : void 0
                    }),
                    u.fire(f)
                }
            })
        })
    }),
    {
        get: function(e) {
            d.done(function() {
                "function" == typeof e && e(f)
            })
        },
        change: function(e) {
            u.add(e)
        }
    }
});;
define("chat/historyRecord",
function(e, i, n) {
    function t() {
        return new $.Deferred(function(e) {
            d.ready(function(i) {
                if (d.isH5) {
                    var n = new HUYA.GetRctTimedMessageReq;
                    n.tUserId = i.userId,
                    n.lPid = s.lp,
                    n.lTid = r.id,
                    n.lSid = r.sid,
                    i.sendWup2("mobileui", "getRctTimedMessage", n,
                    function(i) {
                        e.resolve(i)
                    })
                }
            })
        })
    }
    var d = e("TTP"),
    r = e("TTR"),
    s = e("TTA");
    n.exports = {
        get: t
    }
});;
define("chat",
function(e) {
    var o = e("tools"),
    t = e("TTP"),
    n = e("TTR"),
    a = e("TTA"),
    c = e("TTM"),
    i = e("smog"),
    r = e("roomBeta"),
    s = e("chat/broadcaster"),
    u = e("chat/barrageColor"),
    l = e("chat/barrageColorFans"),
    d = e("chat/barrageSize"),
    f = e("chat/barragePosition"),
    m = e("chat/receive"),
    h = e("chat/speak"),
    g = e("chat/mySelfSpeakCfg"),
    v = e("roomShield"),
    T = e("roomBlockWords"),
    C = e("auditor"),
    p = e("msgManager"),
    k = e("chat/historyRecord"),
    O = {};
    return O.barrage = {
        color: u,
        size: d,
        position: f
    },
    O.receive = m,
    O.speak = h,
    O.broadcaster = s,
    n.isOff && t.isH5 && k.get().done(function(e) {
        var o = new Date(1e3 * c.time);
        o.setHours(0, 0, 0, 0); {
            var t = function(e) {
                return ("00" + e).slice((e + "").length)
            },
            n = function(e) {
                var n = new Date(1e3 * e),
                a = n.getMonth() + 1,
                c = n.getDate(),
                i = n.getHours(),
                r = n.getMinutes(),
                s = n.getTime() > o.getTime(),
                u = "";
                return u = s ? t(i) + ":" + t(r) : t(a) + "\u6708" + t(c) + "\u65e5 " + t(i) + ":" + t(r)
            },
            a = 0;
            HUYA_UTIL.getCookieVal("yyuid")
        }
        $.each(e.vTimedMesasgeNotice.value,
        function(e, o) {
            if (!T.testMsg(o.tNotice)) {
                var t = o.lCreatedTime > a + 180;
                t && (a = o.lCreatedTime, p.add('<div class="msg-timed"><span>' + n(a) + "</span></div>")),
                m.showMessage(o.tNotice)
            }
        })
    }),
    s.addOnceListener(i.stop),
    h.on("speak",
    function(e) {
        g.get(function(o) {
            o.sContent = e,
            m.showMessage(o)
        })
    }),
    s.addListener(function(e) {
        e.isOwn || T.testMsg(e.data) || m.onSpeakMessage(e.data)
    }),
    s.addListener(function(e) {
        TT.emit("LIVE_ROOM_MSG_NOTICE", e.data)
    }),
    function() {
        function e() {
            n.html(""),
            localStorage.removeItem("BARRAGE_COLOR_UNLOCK")
        }
        var t = null,
        n = $("#J-room-chat-color"),
        a = $("#J-room-color-box"),
        c = o.once(function() {
            l.createPalette()
        }),
        i = function() {
            clearTimeout(t),
            c(),
            a.show(),
            e()
        },
        r = function() {
            clearTimeout(t),
            t = setTimeout(function() {
                a.hide()
            },
            100)
        };
        n.on("mouseenter", i).on("mouseleave", r),
        a.on("mouseenter", i).on("mouseleave", r),
        n.css("backgroundColor", -1 == u.get() ? "": "#" + u.rgb()),
        u.change(function() {
            n.css("backgroundColor", -1 == u.get() ? "": "#" + u.rgb())
        }),
        TT.event.on("BARRAGE_COLOR_UNLOCK",
        function() {
            n.html("<span>\u65b0</span>"),
            localStorage.setItem("BARRAGE_COLOR_UNLOCK", 1)
        }),
        localStorage.getItem("BARRAGE_COLOR_UNLOCK") && n.html("<span>\u65b0</span>")
    } (),
    function() {
        var e = null,
        t = $("#J-room-speaking-box"),
        n = o.once(function() {
            d.createPalette(),
            f.createPalette()
        }),
        a = function() {
            clearTimeout(e),
            n(),
            t.show()
        },
        c = function() {
            clearTimeout(e),
            e = setTimeout(function() {
                t.hide()
            },
            100)
        };
        $("#J-room-chat-font").on("mouseenter", a).on("mouseleave", c),
        t.on("mouseenter", a).on("mouseleave", c)
    } (),
    function() {
        var e = null,
        t = $("#J-room-shield-box"),
        n = $("#J-room-chat-shield"),
        a = o.once(function() {
            var e = t.find("li"),
            o = function() {
                $.each(v.options,
                function(o, t) {
                    e.eq(o)[v.cfg[t] ? "addClass": "removeClass"]("checked")
                })
            };
            o(),
            v.on("change", o),
            t.on("click", "li",
            function(e) {
                e.preventDefault();
                var o = $(this).index(),
                t = v.options[o],
                n = v.cfg[t] ? 0 : 1;
                v.set(t, n);
                try {
                    ya.reportProductEvent({
                        eid: "click/zhibo/shield/" + t + "/" + n,
                        eid_desc: "点击/直播/屏蔽/" + v.desc[t] + "/" + (n ? "\u9009\u4e2d": "\u53d6\u6d88")
                    })
                } catch(e) {}
            })
        }),
        c = function() {
            function e() {
                n[v.selected.length > 0 ? "addClass": "removeClass"]("shield-on")
            }
            e(),
            v.on("change", e),
            n.on("click",
            function(e) {
                e.preventDefault();
                var o = v.selected.length > 0 ? 0 : 1;
                v.setAll(o);
                try {
                    ya.reportProductEvent({
                        eid: "click/zhibo/shield/all/" + o,
                        eid_desc: "点击/直播/屏蔽/\u6240\u6709/" + (o ? "\u9009\u4e2d": "\u53d6\u6d88")
                    })
                } catch(e) {}
            })
        };
        c();
        var i = function() {
            clearTimeout(e),
            a(),
            t.show()
        },
        r = function() {
            clearTimeout(e),
            e = setTimeout(function() {
                t.hide()
            },
            100)
        };
        n.on("mouseenter", i).on("mouseleave", r),
        t.on("mouseenter", i).on("mouseleave", r)
    } (),
    $(function() {
        var t = $("#J_roomChatIconBlockWords"),
        n = $("#J_roomChatBlockWords"),
        a = !1,
        c = o.once(function() {
            e.async("widget/roomBlockWords",
            function(e) {
                var o = new e(n);
                o.on("close", s),
                n.addClass("inited")
            })
        }),
        i = function() {
            n.show(),
            a = !0,
            t.addClass("active"),
            c();
            try {
                ya.reportProductEvent({
                    eid: "click/keywordshield",
                    eid_desc: "\u70b9\u51fb/\u5173\u952e\u8bcd\u5c4f\u853d"
                })
            } catch(e) {}
        },
        s = function() {
            n.hide(),
            a = !1,
            t.removeClass("active")
        },
        u = function() {
            a ? s() : i()
        };
        t.on("click",
        function(e) {
            e.preventDefault(),
            TT.sudo(u)
        }),
        r.lazyLoad(function() {
            e.async("widget/roomBlockWords")
        })
    }),
    function() {
        TT.getUserInfo(function(o) {
            return a.lp == o.uid ? void e.async("widget/room-inform") : void C.onEnter(function() { (C.isAuditor() || C.isSuperAuditor()) && e.async("widget/room-inform")
            })
        })
    } (),
    O
});