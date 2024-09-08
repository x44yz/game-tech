// TODO
[+]通过状态机生成代码
[+]查看当前状态机状态
[+]添加参数可以在编辑器里面实现简单的状态切换
编辑器可以参考 Unity 动画状态机

http://www.gameaipro.com/GameAIPro3/GameAIPro3_Chapter12_A_Reusable_Light-Weight_Finite-State_Machine.pdf

// 经验
尽量将转换条件写在 Translation 里面，而不是直接调用 SetState
只在强制转换的时候使用 SetState
2 者兼用效果更好