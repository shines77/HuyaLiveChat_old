require.resourceMap({
    "res": {
        "widget/createLayer": {
            "url": "https://a.msstatic.com/huya/main/widget/createLayer/createLayer_15f21b9.js"
        },
        "widget/dialog": {
            "url": "https://a.msstatic.com/huya/main/widget/dialog/dialog_bb9fd5d.js"
        },
        "widget/treasureChest": {
            "url": "https://a.msstatic.com/huya/main/widget/treasureChest/treasureChest_05a961b.js"
        },
        "BlockWords": {
            "url": "https://a.msstatic.com/huya/main/modules/BlockWords/BlockWords_b1ecf7c.js"
        },
        "roomBlockWords": {
            "deps": ["BlockWords"],
            "url": "https://a.msstatic.com/huya/main/modules/roomBlockWords/roomBlockWords_cd1e925.js"
        },
        "auditor": {
            "url": "https://a.msstatic.com/huya/main/modules/auditor/auditor_f72b753.js"
        },
        "widget/roomMute": {
            "deps": ["auditor", "widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/widget/roomMute/roomMute_8b654c6.js"
        },
        "widget/roomBan": {
            "deps": ["auditor", "widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/widget/roomBan/roomBan_c3449c5.js"
        },
        "widget/ucard/Menu.js": {
            "deps": ["widget/dialog", "widget/roomMute", "widget/roomBan"],
            "url": "https://a.msstatic.com/huya/main/widget/ucard/Menu_cc462e3.js"
        },
        "roomShield": {
            "url": "https://a.msstatic.com/huya/main/modules/roomShield/roomShield_5854870.js"
        },
        "noble": {
            "deps": ["roomShield"],
            "url": "https://a.msstatic.com/huya/main/modules/noble/noble_ab6483a.js"
        },
        "widget/ucard": {
            "deps": ["auditor", "widget/ucard/Menu.js", "noble"],
            "url": "https://a.msstatic.com/huya/main/widget/ucard/ucard_7f02277.js"
        },
        "jScrollPane": {
            "url": "https://a.msstatic.com/huya/main/modules/jScrollPane/jScrollPane_426a5ac.js"
        },
        "week_rank": {
            "deps": ["jScrollPane"],
            "url": "https://a.msstatic.com/huya/main/modules/week_rank/week_rank_077d54f.js"
        },
        "fans_rank": {
            "deps": ["widget/ucard", "jScrollPane"],
            "url": "https://a.msstatic.com/huya/main/modules/fans_rank/fans_rank_a122273.js"
        },
        "roomVip": {
            "url": "https://a.msstatic.com/huya/main/modules/roomVip/roomVip_643c17b.js"
        },
        "widget/vipSeat": {
            "deps": ["widget/ucard", "jScrollPane", "roomVip"],
            "url": "https://a.msstatic.com/huya/main/widget/vipSeat/vipSeat_18b22b4.js"
        },
        "follow": {
            "url": "https://a.msstatic.com/huya/main/modules/follow/follow_a06b43d.js"
        },
        "followList": {
            "deps": ["follow"],
            "url": "https://a.msstatic.com/huya/main/modules/followList/followList_e2b49be.js"
        },
        "roomFollow": {
            "deps": ["followList"],
            "url": "https://a.msstatic.com/huya/main/modules/roomFollow/roomFollow_b65fe6e.js"
        },
        "roomFans": {
            "url": "https://a.msstatic.com/huya/main/modules/roomFans/roomFans_4cfd36a.js"
        },
        "widget/roomFollow": {
            "deps": ["roomFollow"],
            "url": "https://a.msstatic.com/huya/main/widget/roomFollow/roomFollow_2f57633.js"
        },
        "widget/roomAnchorLv/lvStatus.js": {
            "url": "https://a.msstatic.com/huya/main/widget/roomAnchorLv/lvStatus_13bd81f.js"
        },
        "widget/roomAnchorLv/lvNum.js": {
            "url": "https://a.msstatic.com/huya/main/widget/roomAnchorLv/lvNum_246a467.js"
        },
        "widget/roomAnchorLv/LvInfoClass.js": {
            "url": "https://a.msstatic.com/huya/main/widget/roomAnchorLv/LvInfoClass_5ba03f5.js"
        },
        "widget/roomAnchorLv": {
            "deps": ["widget/roomAnchorLv/lvStatus.js", "widget/roomAnchorLv/lvNum.js", "widget/roomAnchorLv/LvInfoClass.js"],
            "url": "https://a.msstatic.com/huya/main/widget/roomAnchorLv/roomAnchorLv_4197922.js"
        },
        "input_ob": {
            "url": "https://a.msstatic.com/huya/main/modules/input_ob/input_ob_3abf439.js"
        },
        "widget/fansbadge": {
            "deps": ["jScrollPane"],
            "url": "https://a.msstatic.com/huya/main/widget/fansbadge/fansbadge_2f873d3.js"
        },
        "TextAreaHelper": {
            "url": "https://a.msstatic.com/huya/main/modules/TextAreaHelper/TextAreaHelper_f4e4813.js"
        },
        "widget/roomEmot": {
            "deps": ["TextAreaHelper", "input_ob"],
            "url": "https://a.msstatic.com/huya/main/widget/roomEmot/roomEmot_a2b9672.js"
        },
        "chatRoom/ScrollbarModel": {
            "pkg": "p1"
        },
        "chatRoom/ScrollbarView": {
            "pkg": "p1"
        },
        "chatRoom/scrollbar": {
            "deps": ["chatRoom/ScrollbarModel", "chatRoom/ScrollbarView"],
            "pkg": "p1"
        },
        "chatPanelFilter": {
            "url": "https://a.msstatic.com/huya/main/modules/chatPanelFilter/chatPanelFilter_c784b82.js"
        },
        "chatRoom/useUcard": {
            "deps": ["chatRoom/scrollbar", "widget/dialog", "widget/ucard"],
            "pkg": "p1"
        },
        "widget/fansBadgeAnchor": {
            "url": "https://a.msstatic.com/huya/main/widget/fansBadgeAnchor/fansBadgeAnchor_6177cd8.js"
        },
        "chatRoom": {
            "deps": ["chatRoom/scrollbar", "chatPanelFilter", "chatRoom/useUcard", "widget/fansBadgeAnchor"],
            "pkg": "p1"
        },
        "widget/roomBlockWords": {
            "deps": ["roomBlockWords"],
            "url": "https://a.msstatic.com/huya/main/widget/roomBlockWords/roomBlockWords_d4b1dce.js"
        },
        "roomInform": {
            "deps": ["auditor"],
            "url": "https://a.msstatic.com/huya/main/modules/roomInform/roomInform_5329c67.js"
        },
        "widget/room-inform": {
            "deps": ["roomInform"],
            "url": "https://a.msstatic.com/huya/main/widget/room-inform/room-inform_b8bb2b3.js"
        },
        "chat/broadcaster": {
            "pkg": "p2"
        },
        "DefineValue": {
            "url": "https://a.msstatic.com/huya/main/modules/DefineValue/DefineValue_8bbba4d.js"
        },
        "chat/barrageColor": {
            "deps": ["DefineValue", "chat/broadcaster"],
            "pkg": "p2"
        },
        "chat/barrageColorFans": {
            "deps": ["DefineValue", "chat/barrageColor", "widget/fansbadge"],
            "pkg": "p2"
        },
        "userLevel": {
            "deps": ["DefineValue"],
            "url": "https://a.msstatic.com/huya/main/modules/userLevel/userLevel_8a50d0f.js"
        },
        "chat/barrageSize": {
            "deps": ["DefineValue", "userLevel"],
            "pkg": "p2"
        },
        "chat/barragePosition": {
            "deps": ["DefineValue", "userLevel"],
            "pkg": "p2"
        },
        "chat/receive": {
            "pkg": "p2"
        },
        "widget/bindPhone": {
            "deps": ["widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/widget/bindPhone/bindPhone_d7cfca2.js"
        },
        "widget/bindPhoneByHuya": {
            "deps": ["widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/widget/bindPhoneByHuya/bindPhoneByHuya_58f1267.js"
        },
        "bindPhone": {
            "deps": ["widget/bindPhone", "widget/bindPhoneByHuya"],
            "url": "https://a.msstatic.com/huya/main/modules/bindPhone/bindPhone_1e6f149.js"
        },
        "widget/speakTipsBindPhone": {
            "url": "https://a.msstatic.com/huya/main/widget/speakTipsBindPhone/speakTipsBindPhone_5732f46.js"
        },
        "chat/speakLimit": {
            "pkg": "p2"
        },
        "chat/speak": {
            "deps": ["input_ob", "auditor", "chat/barrageColor", "chat/barrageSize", "chat/barragePosition", "bindPhone", "widget/speakTipsBindPhone", "chat/speakLimit"],
            "pkg": "p2"
        },
        "chat/mySelfSpeakCfg": {
            "deps": ["chat/broadcaster", "chat/barrageColor", "chat/barrageSize", "chat/barragePosition", "widget/fansbadge"],
            "pkg": "p2"
        },
        "chat/historyRecord": {
            "pkg": "p2"
        },
        "chat": {
            "deps": ["chat/broadcaster", "chat/barrageColor", "chat/barrageColorFans", "chat/barrageSize", "chat/barragePosition", "chat/receive", "chat/speak", "chat/mySelfSpeakCfg", "roomShield", "roomBlockWords", "auditor", "chat/historyRecord"],
            "pkg": "p2"
        },
        "ProcessorClass": {
            "url": "https://a.msstatic.com/huya/main/modules/ProcessorClass/ProcessorClass_992553c.js"
        },
        "widget/roomMsgOfKing": {
            "deps": ["ProcessorClass", "chat", "chat/mySelfSpeakCfg", "roomBlockWords", "chatPanelFilter"],
            "url": "https://a.msstatic.com/huya/main/widget/roomMsgOfKing/roomMsgOfKing_6960e56.js"
        },
        "qrcode": {
            "url": "https://a.msstatic.com/huya/main/modules/qrcode/qrcode_1f5d95b.js"
        },
        "widget/roomShare": {
            "deps": ["widget/dialog", "qrcode"],
            "url": "https://a.msstatic.com/huya/main/widget/roomShare/roomShare_67a5e2c.js"
        },
        "widget/illegalReport": {
            "deps": ["widget/dialog", "bindPhone"],
            "url": "https://a.msstatic.com/huya/main/widget/illegalReport/illegalReport_b02fe21.js"
        },
        "widget/roomAppGuide": {
            "deps": ["qrcode"],
            "url": "https://a.msstatic.com/huya/main/widget/roomAppGuide/roomAppGuide_1268cf8.js"
        },
        "widget/game_promote": {
            "url": "https://a.msstatic.com/huya/main/widget/game_promote/game_promote_265fbb1.js"
        },
        "widget/game_buy": {
            "deps": ["widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/widget/game_buy/game_buy_b796863.js"
        },
        "videoScale": {
            "url": "https://a.msstatic.com/huya/main/modules/videoScale/videoScale_0a9d766.js"
        },
        "gift/animation": {
            "pkg": "p3"
        },
        "gift/bigStreamerProcessor": {
            "deps": ["ProcessorClass"],
            "pkg": "p3"
        },
        "gift/bigStreamerAnimation": {
            "pkg": "p3"
        },
        "gift/bigStreamer": {
            "deps": ["gift/bigStreamerProcessor", "gift/bigStreamerAnimation"],
            "pkg": "p3"
        },
        "gift/pk": {
            "pkg": "p3"
        },
        "gift/msg": {
            "deps": ["roomShield", "gift/pk"],
            "pkg": "p3"
        },
        "gift/malimalihong": {
            "deps": ["gift/bigStreamerProcessor", "gift/bigStreamerAnimation"],
            "pkg": "p3"
        },
        "gift": {
            "deps": ["roomShield", "ProcessorClass", "gift/animation", "gift/bigStreamer", "gift/pk", "gift/msg", "gift/malimalihong"],
            "pkg": "p3"
        },
        "widget/userUpgradeTips": {
            "url": "https://a.msstatic.com/huya/main/widget/userUpgradeTips/userUpgradeTips_e028610.js"
        },
        "widget/weekStar": {
            "deps": ["widget/dialog", "jScrollPane"],
            "url": "https://a.msstatic.com/huya/main/widget/weekStar/weekStar_bc3dd3f.js"
        },
        "guessReport": {
            "url": "https://a.msstatic.com/huya/main/modules/guessReport/guessReport_b5630d7.js"
        },
        "roomMovie": {
            "url": "https://a.msstatic.com/huya/main/modules/roomMovie/roomMovie_3d1703f.js"
        },
        "widget/room-movie": {
            "deps": ["roomMovie"],
            "url": "https://a.msstatic.com/huya/main/widget/room-movie/room-movie_81f7e98.js"
        },
        "onTVLottery": {
            "deps": ["widget/fansbadge"],
            "url": "https://a.msstatic.com/huya/main/modules/onTVLottery/onTVLottery_1124350.js"
        },
        "postMessageEvent": {
            "url": "https://a.msstatic.com/huya/main/modules/postMessageEvent/postMessageEvent_d179ef0.js"
        },
        "topUp": {
            "deps": ["widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/modules/topUp/topUp_7ad7600.js"
        },
        "widget/recharge": {
            "deps": ["postMessageEvent", "topUp"],
            "url": "https://a.msstatic.com/huya/main/widget/recharge/recharge_064b4c8.js"
        },
        "widget/room-onTVLottery": {
            "deps": ["roomBlockWords", "onTVLottery", "jScrollPane", "widget/recharge"],
            "url": "https://a.msstatic.com/huya/main/widget/room-onTVLottery/room-onTVLottery_20b245e.js"
        },
        "widget/blackPearl/Caribbean.js": {
            "url": "https://a.msstatic.com/huya/main/widget/blackPearl/Caribbean_31ea211.js"
        },
        "widget/blackPearl": {
            "deps": ["widget/dialog", "widget/blackPearl/Caribbean.js", "jScrollPane"],
            "url": "https://a.msstatic.com/huya/main/widget/blackPearl/blackPearl_4bc4250.js"
        },
        "widget/roomGetPrize": {
            "deps": ["widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/widget/roomGetPrize/roomGetPrize_bafbdc4.js"
        },
        "widget/getBadgeFaith": {
            "deps": ["widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/widget/getBadgeFaith/getBadgeFaith_b148e92.js"
        },
        "topUpPayment": {
            "deps": ["postMessageEvent"],
            "url": "https://a.msstatic.com/huya/main/modules/topUpPayment/topUpPayment_eb66a56.js"
        },
        "firstRecharge": {
            "deps": ["postMessageEvent"],
            "url": "https://a.msstatic.com/huya/main/modules/firstRecharge/firstRecharge_2d7d472.js"
        },
        "widget/roomReportMsg": {
            "deps": ["widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/widget/roomReportMsg/roomReportMsg_4e6e9eb.js"
        },
        "watchHistory": {
            "url": "https://a.msstatic.com/huya/main/modules/watchHistory/watchHistory_e1f965f.js"
        },
        "drag": {
            "url": "https://a.msstatic.com/huya/main/modules/drag/drag_0b6caa6.js"
        },
        "widget/roomMiniPlayer": {
            "deps": ["drag"],
            "url": "https://a.msstatic.com/huya/main/widget/roomMiniPlayer/roomMiniPlayer_13a1fd2.js"
        },
        "widget/missAsiaPk": {
            "deps": ["widget/dialog"],
            "url": "https://a.msstatic.com/huya/main/widget/missAsiaPk/missAsiaPk_5b2e129.js"
        },
        "sidebar": {
            "url": "https://a.msstatic.com/huya/main/modules/sidebar/sidebar_914356e.js"
        },
        "widget/sidebar": {
            "deps": ["jScrollPane", "sidebar"],
            "url": "https://a.msstatic.com/huya/main/widget/sidebar/sidebar_7f9292b.js"
        },
        "widget/room-chat-notice": {
            "deps": ["chatPanelFilter"],
            "url": "https://a.msstatic.com/huya/main/widget/room-chat-notice/room-chat-notice_cced5b5.js"
        },
        "roomActivity": {
            "url": "https://a.msstatic.com/huya/main/modules/roomActivity/roomActivity_fdd663c.js"
        },
        "subscribeByTaf": {
            "url": "https://a.msstatic.com/huya/main/modules/subscribeByTaf/subscribeByTaf_7f918be.js"
        },
        "guideBill": {
            "deps": ["subscribeByTaf"],
            "url": "https://a.msstatic.com/huya/main/modules/guideBill/guideBill_54857c6.js"
        },
        "widget/room-guide-playbill": {
            "deps": ["subscribeByTaf", "guideBill"],
            "url": "https://a.msstatic.com/huya/main/widget/room-guide-playbill/room-guide-playbill_963d6b6.js"
        },
        "guide": {
            "deps": ["subscribeByTaf"],
            "url": "https://a.msstatic.com/huya/main/modules/guide/guide_1e36223.js"
        },
        "widget/room-guide": {
            "deps": ["guide", "subscribeByTaf", "chatPanelFilter"],
            "url": "https://a.msstatic.com/huya/main/widget/room-guide/room-guide_a9368f0.js"
        },
        "widget/room-backToTop": {
            "url": "https://a.msstatic.com/huya/main/widget/room-backToTop/room-backToTop_49c7f80.js"
        },
        "MomentFavor": {
            "url": "https://a.msstatic.com/huya/main/modules/MomentFavor/MomentFavor_9a530e7.js"
        },
        "MomentItem": {
            "deps": ["followList", "MomentFavor"],
            "url": "https://a.msstatic.com/huya/main/modules/MomentItem/MomentItem_eb6c95f.js"
        },
        "Moments/HotMoments": {
            "deps": ["MomentItem"],
            "url": "https://a.msstatic.com/huya/main/modules/Moments/HotMoments_4b4418e.js"
        },
        "momentVideoList": {
            "url": "https://a.msstatic.com/huya/main/modules/momentVideoList/momentVideoList_9db9cd2.js"
        },
        "widget/MomentVideo/CtrlPlay.js": {
            "pkg": "p4"
        },
        "widget/MomentVideo/CtrlProgress.js": {
            "pkg": "p4"
        },
        "widget/MomentVideo/CtrlVolume.js": {
            "pkg": "p4"
        },
        "widget/MomentVideo": {
            "deps": ["widget/MomentVideo/CtrlPlay.js", "widget/MomentVideo/CtrlProgress.js", "widget/MomentVideo/CtrlVolume.js"],
            "pkg": "p4"
        },
        "widget/momentShare": {
            "deps": ["widget/dialog", "qrcode"],
            "url": "https://a.msstatic.com/huya/main/widget/momentShare/momentShare_833f496.js"
        },
        "widget/room-moments-item": {
            "deps": ["momentVideoList", "widget/MomentVideo", "widget/momentShare"],
            "url": "https://a.msstatic.com/huya/main/widget/room-moments-item/room-moments-item_6366ba6.js"
        },
        "widget/room-moments-hot": {
            "deps": ["Moments/HotMoments", "widget/room-moments-item"],
            "url": "https://a.msstatic.com/huya/main/widget/room-moments-hot/room-moments-hot_d1a0cab.js"
        },
        "Moments/AnchorMoments": {
            "deps": ["MomentItem"],
            "url": "https://a.msstatic.com/huya/main/modules/Moments/AnchorMoments_775a1d5.js"
        },
        "widget/room-moments-anchor": {
            "deps": ["Moments/AnchorMoments", "widget/room-moments-item"],
            "url": "https://a.msstatic.com/huya/main/widget/room-moments-anchor/room-moments-anchor_9822891.js"
        },
        "widget/room-moments": {
            "deps": ["widget/room-moments-hot", "widget/room-moments-anchor", "momentVideoList"],
            "url": "https://a.msstatic.com/huya/main/widget/room-moments/room-moments_84407a1.js"
        },
        "imgLazyLoad": {
            "url": "https://a.msstatic.com/huya/main/modules/imgLazyLoad/imgLazyLoad_4c69a0d.js"
        },
        "respondCard": {
            "url": "https://a.msstatic.com/huya/main/modules/respondCard/respondCard_be1390c.js"
        },
        "roomRespondCard": {
            "deps": ["respondCard"],
            "url": "https://a.msstatic.com/huya/main/modules/roomRespondCard/roomRespondCard_5cada29.js"
        },
        "widget/room-youlike": {
            "deps": ["imgLazyLoad", "roomRespondCard"],
            "url": "https://a.msstatic.com/huya/main/widget/room-youlike/room-youlike_0ff8d31.js"
        },
        "widget/roomGg0": {
            "url": "https://a.msstatic.com/huya/main/widget/roomGg0/roomGg0_80c61fb.js"
        },
        "widget/roomGg1": {
            "url": "https://a.msstatic.com/huya/main/widget/roomGg1/roomGg1_362a678.js"
        },
        "widget/roomGg2": {
            "url": "https://a.msstatic.com/huya/main/widget/roomGg2/roomGg2_1f76523.js"
        },
        "livePerfReport": {
            "url": "https://a.msstatic.com/huya/main/modules/livePerfReport/livePerfReport_ec87edf.js"
        },
        "h5playeOnloadReport": {
            "url": "https://a.msstatic.com/huya/main/modules/h5playeOnloadReport/h5playeOnloadReport_2c99cb7.js"
        },
        "liveRoomDurationReport": {
            "url": "https://a.msstatic.com/huya/main/modules/liveRoomDurationReport/liveRoomDurationReport_b642335.js"
        },
        "liveRoomPvReport": {
            "url": "https://a.msstatic.com/huya/main/modules/liveRoomPvReport/liveRoomPvReport_f71bfc0.js"
        },
        "roomReport": {
            "deps": ["livePerfReport", "h5playeOnloadReport", "liveRoomDurationReport", "liveRoomPvReport"],
            "url": "https://a.msstatic.com/huya/main/modules/roomReport/roomReport_9f925bd.js"
        },
        "roomStatistics": {
            "url": "https://a.msstatic.com/huya/main/modules/roomStatistics/roomStatistics_20e5187.js"
        }
    },
    "pkg": {
        "p1": {
            "url": "https://a.msstatic.com/huya/main/pkg/room_gb_bbfa55f.js",
            "deps": ["widget/dialog", "widget/ucard", "chatPanelFilter", "widget/fansBadgeAnchor"]
        },
        "p2": {
            "url": "https://a.msstatic.com/huya/main/pkg/room_chat_d644335.js",
            "deps": ["DefineValue", "widget/fansbadge", "userLevel", "input_ob", "auditor", "bindPhone", "widget/speakTipsBindPhone", "roomShield", "roomBlockWords"]
        },
        "p3": {
            "url": "https://a.msstatic.com/huya/main/pkg/room_gift_d32556b.js",
            "deps": ["ProcessorClass", "roomShield"]
        },
        "p4": {
            "url": "https://a.msstatic.com/huya/main/pkg/MomentVideo_d978135.js"
        }
    }
});