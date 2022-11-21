# 动态避障
Unity NavmeshObstacle 虽然可以使用解决，但是需要提前烘焙 Navmesh，还要在 Agent 和 Obstacle 之间切换。  
需要找到的是一个更通用的解决方法。  

# VO
基本思想就是预测一个会产生碰撞的扇形，然后将扇形偏移（针对移动的 Vb）

https://www.researchgate.net/publication/2672667_Motion_Planning_in_Dynamic_Environments_Using_the_Relative_Velocity_Paradigm
https://blog.csdn.net/zhiai315/article/details/113931422?spm=1001.2014.3001.5506

# RVO2
RVO 是为了解决 VO 速度变化太大带来的抖动。

https://gamma.cs.unc.edu/RVO2

# 目标点不重叠
