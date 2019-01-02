RPG 数值和公式

物理 + 魔法
防御 = 闪避

动作帧数 = 根据等级 + 武器 调整

##术语:  
attack rating: 命中率
defense rating: 防御率
Evade: 闪避
damage Reduction: 减伤


##TODO:  
[+]实现 Diablo 的数值  
[+]动态设置动画 speed  

##Q&A  
Q: 如何设计攻击的时候被hit
A: hit的时候，attack被打断，播放 hit 动画/音效，如果在 hit 过程中再次受到攻击，攻击动画不应该叠加，否则玩家将不会有机会攻击敌人，当然这也会存在 bug，就是 hit 正好形成连续的，那么表现就是被打到无法还手，也就是攻速快，当然这个是相对的，玩家/敌人都可以触发。
<泰坦之旅>中没有普攻受伤动画，或者受伤动画就是站立

Q: 敌人的攻击间隔？
A: 敌人的攻击间隔还是根据动画来决定的，玩家的是手动，敌人的是自动，开始攻击的条件就是在攻击范围之内
如果不在攻击范围之内，就更新 Move

主要参考:
Diablo 

https://www.gameres.com/812566.html
http://web.archive.org/web/20071013030903/http://users.tkk.fi/~tgustafs/tohit.html#SimpleAR
https://diablo.gamepedia.com/Attack_Rating_(Diablo_II)
https://diablo2.diablowiki.net/Guide:Defense_101_v1.11,_by_Alecz
http://web.archive.org/web/20071024204556/http://www.battle.net:80/diablo2exp/monsters/act1-fallen.shtml
http://wiki.d.163.com/index.php?title=%E9%A6%96%E9%A1%B5_%E6%9A%97%E9%BB%912

ART: https://www.spriters-resource.com/pc_computer/diablodiablohellfire/

辅助参考:
Soda Dungeon

https://www.reddit.com/r/sodadungeon/comments/3pe273/how_is_damage_calculated/
https://www.reddit.com/r/sodadungeon/comments/3qcy3f/formulas_for_several_value_in_the_game/