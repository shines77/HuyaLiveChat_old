define("widget/roomReportMsg",
function(require, exports, module) {
    function reportMsg(e, t, r) {
        importStyle(),
        new Dialog({
            title: "\u4e3e\u62a5\u5f39\u5e55",
            customClass: "miniDlg",
            content: tpl({
                nick: t,
                msg: r
            }),
            buttons: [{
                type: 0,
                name: "\u786e\u5b9a",
                click: function() {
                    report(e, t, r,
                    function() {
                        Toast.suc("\u4e3e\u62a5\u6210\u529f\uff0c\u623f\u7ba1\u4f1a\u5c3d\u5feb\u5904\u7406")
                    });
                    try {
                        ya.reportProductEvent({
                            eid: "click/videotext/report/sure",
                            eid_desc: "\u70b9\u51fb/\u89c6\u9891\u533a\u5f39\u5e55/\u4e3e\u62a5/\u786e\u5b9a"
                        })
                    } catch(o) {}
                },
                close: !0
            },
            {
                type: 1,
                name: "\u53d6\u6d88",
                click: function() {},
                close: !0
            }],
            lock: !1
        });
        try {
            ya.reportProductEvent({
                eid: "click/videotext/report",
                eid_desc: "点击/视频区弹幕/举报"
            })
        } catch(o) {}
    }
    function report(e, t, r, o) {
        TTP.ready(function(i) {
            if (TTP.isH5) {
                var s = new HUYA.ReportMessageReq;
                s.tId = i.userId,
                s.tScene.lLiveId = TTR.liveId,
                s.tScene.lPid = TTA.lp,
                s.tScene.lTid = TTR.id,
                s.tScene.lSid = TTR.sid,
                s.tMessage.lSenderUid = e,
                s.tMessage.sSenderNick = t,
                s.tMessage.sContent = r,
                i.sendWup2("mobileui", "reportMessage", s, o)
            } else TTP.isFlash && (i.callAs("ReportMessageReq", {
                uid: e,
                nick: t,
                msg: r,
                liveId: TTR.liveId
            }), TTP.one("ReportMessageRsp",
            function(e) {
                o(e.param)
            }))
        })
    }
    var tools = require("tools"),
    TTR = require("TTR"),
    TTA = require("TTA"),
    TTP = require("TTP"),
    Dialog = require("widget/dialog").dialog,
    Toast = require("widget/dialog").toast,
    cssText = ".reportMsg-dlg{padding:12px 12px 0;width:270px}.reportMsg-dlg .reportMsg-bd{background:#252525;padding:.5em .8em;border:1px solid #444}.reportMsg-dlg .reportMsg-nick{color:#5db6fe}.reportMsg-dlg .reportMsg-msg{color:#fff;margin-top:2px}",
    tpl = function(obj) {
        {
            var __t, __p = "";
            Array.prototype.join
        }
        with(obj || {}) __p += '<div class="reportMsg-dlg">\n    <div class="reportMsg-bd">\n        <div class="reportMsg-nick">' + (null == (__t = nick) ? "": _.escape(__t)) + '</div>\n        <div class="reportMsg-msg">' + (null == (__t = msg) ? "": _.escape(__t)) + "</div>\n    </div>\n</div>";
        return __p
    },
    importStyle = tools.once(function() {
        tools.importStyle(cssText, "J_reportMsgDlgStyle")
    });
    module.exports = reportMsg
});