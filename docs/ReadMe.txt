在网上找了些别的语言的帖子，供各位大神参考：

【参考一】node.js版本：安装 npm install huya-danmu --save（https://www.npmjs.com/package/huya-danmu）

【参考二】利用chakra引擎获取（http://bbs.aardio.com/forum.php?mod=viewthread&tid=22218）

【参考三】知乎上的讨论（https://www.zhihu.com/question/49019191）

上面三个例子，我发现都需要引用 http://shangjing.huya.com/pkg/taf_f74d170.js ，然后跟

ws://http://ws.api.huya.com

建立连接，就可以获取数据了，由于我不懂 js 和 websoke tcp 抓包，而自己通过 http 抓包又抓不到赠送礼物的信息。
（在电脑端，是通过 flash 获取弹幕；手机端ie打开 m.huya.com 抓不到弹幕，谷歌设置成手机模式可以抓到弹幕一条条接收，但不显示）
