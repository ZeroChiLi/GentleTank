# GentleTank

　　《Gentle Tank》是一款ARPG（Action Role Playing Game，动作角色扮演类）单机游戏。游戏使用Unity引擎，C#语言实现，音乐美术资源均有网上免费下载或自己设计建模完成，同时使用各种免费插件配合完成。

---
## 主菜单
![](http://oz99dhvw8.bkt.clouddn.com/10.png)
 - 由3D场景直接组成，字体由TextMeshPro插件配合完成。

---
## 装备菜单
![](http://oz99dhvw8.bkt.clouddn.com/11.png)
 - 自定义坦克列表同样在3D场景中，而UI部分则是在画布中实现。
 - 玩家可以装备自己的坦克库，最后选择其中一个进行游戏。

![](http://oz99dhvw8.bkt.clouddn.com/15.gif)
 - 镜头的切换是由Cinemachine插件配合完成。
 - 装备系统自己编写完成。
 - 不同装备有不同的属性，如不同的头部，会有不同的攻击方式。

---
## 设置菜单（未完成）
![](http://oz99dhvw8.bkt.clouddn.com/16.gif)
 - 背景虚化特效使用PostProcessing插件配合完成。

---
## 技能系统
![](http://oz99dhvw8.bkt.clouddn.com/10.gif)
 - 包括对特定对象的技能效果，和范围技能效果。
 - 描边效果通过自行编写shader完成。

---
## 道具系统
![](http://oz99dhvw8.bkt.clouddn.com/11.gif)
 - 降落伞和道具方块均有自己建模完成。 :-D
 - 道具包括：补血、加速、加攻击等，属性相关的道具都是只会持续一段时间。

---
## 游戏玩法
 - 目前暂定为回合制。

![](http://oz99dhvw8.bkt.clouddn.com/12.gif)
 
![](http://oz99dhvw8.bkt.clouddn.com/13.gif)
> 在主角死后，会切换到全部角色的镜头。
> 角色死去，会变成残骸，一段时间后消融后消失。
> 角色死去，屏幕下方会显示信息：“谋杀者 X 被杀者”。如上图，因为是主角自己的炮弹炸死自己，所以谋杀者和被杀者都是主角自己。

### 随机地图系统
 - 由代码生成的随机地图，但固定出生点。原理可见： [Unity 生成随机房间、洞穴（2D、3D地图）总结](http://blog.csdn.net/l773575310/article/details/72803191)

![](http://oz99dhvw8.bkt.clouddn.com/14.gif)

---
## AI系统
 - 使用有限状态机模式实现，更多可见：[Unity 有限状态机（Finite State Machine）的理解 与 实现简单的可插拔（Pluggable）AI脚本对象](http://blog.csdn.net/l773575310/article/details/73008669)
- 动态导航使用RuntimeNavMesh插件实现。

---
## 小地图系统
 -  [Unity 制作小地图（Minimap）系统 两种方法](http://blog.csdn.net/l773575310/article/details/73100522)

---
## 自制工具
### 出生点、巡逻点工具
 - [Unity 自制工具：Point。方便标记出生点或巡逻点等功能](http://blog.csdn.net/l773575310/article/details/78158924)

### 计时器工具
 - [Unity 封装 倒计时（计时器，CountDownTimer），实现周期更新、技能冷却等功能](http://blog.csdn.net/l773575310/article/details/77571916)

### AI坦克的预设、团队、控制权等调试工具
![](http://oz99dhvw8.bkt.clouddn.com/15.png)

---
## 更多游戏画面

![](http://oz99dhvw8.bkt.clouddn.com/12.png)
> 在AI坦克被击中时，释放信号，告诉周围的队友自己被攻击了。

![](http://oz99dhvw8.bkt.clouddn.com/13.png)
> 使用技能恢复队友的血量。
