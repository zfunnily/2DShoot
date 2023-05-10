## 2DShoot
https://www.bilibili.com/video/BV1SB4y1w7VY?p=7&vd_source=3b8bb2d4a2160770e25d2a56b850c4b9
粒子特效入门: https://www.bilibili.com/video/BV1io4y1m7FH/?vd_source=3b8bb2d4a2160770e25d2a56b850c4b9


## 实现过程
1. 资源导入
2. 输入系统/移动
3. add 单例基类
4. add 视口/屏幕: 限制player在视口活动，不能超出屏幕。
5. 设备检索；移动优化； player 翻转.
6. 后处理-子弹
7. 射击功能
8. 对象池
9. 子弹对象池
10. add 敌人 + 随机敌人位置 + 敌人随机开火
11. 生命值系统
12. 物理碰撞系统
13. 能量条

## 触发器的触发条件
1. 两个游戏对象中至少一个拥有动态/运动学刚体组件
2. 两个游戏对象都拥有碰撞体组件
3. 其中一个游戏对象的碰撞体组件被标记为触发器
4. 两个游戏对象所属的层在层碰撞矩阵中设置为可互相产生碰撞

## 产生物理碰撞的条件
1. 两个游戏对象都有碰撞组件
2. 两个游戏对象的碰撞体组件都不被标记为触发器
3. 至少其中一个游戏对象拥有动态刚体组件
4. 两个游戏对象所属的层在层碰撞矩阵中设置为可互相产生碰撞。

## package
- Unity UI
- Universal RP
- Visual Studio Code Editor
- Input System
