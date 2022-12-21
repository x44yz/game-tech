TODO:  
[x]level up  
[+]drop  
[x]伤害数字  
[+]装备  
[+]去除 class 区别  
[+]去除 difficult 区别  

http://wiki.d.163.com/index.php?title=%E8%AF%8D%E7%BC%80_(Diablo_I)
https://d2.lc/AB/wiki/index9c3b.html?title=Damage  
https://www.diablo-2.net/skillcalculator  
https://planetdiablo.eu/diablo2/calcs/dmgcalc/dmgcalc.php?patch=110&lang=eng
http://www.baronsbazaar.ca/forum/ppr/damage_calc.html  

> 升级所需的经验值  
![excel](./../../Raw/diablo_%E7%BB%8F%E9%AA%8C%E5%80%BC.xlsx)  
从 excel 中可以看出，是个类似前平后高的曲线，而 2 级之间的差值近似是条直线  

> item 的 _iPLDam 属性只有 unique item 才有

> item._iDurability  
武器耐久，战斗时候受伤会损失耐久，耐久归 0 之后无法继续使用，只能在 smith 铁匠出修理，拥有特殊 ItemPower 的 item 可以改变耐久
(源代码中没找到对应变化公式)

> player._pIAC  
accuracy 精准度，来自于 item._iAC，会用于计算护甲 GetArmor，可以理解为精准度越高，越容易躲避伤害  

> player._pIBonusAC  
额外奖励的 accuracy，来自于 item._iAC * item._iPLAC / 100

> player._pBaseDex  
基础敏捷度 dexterity，来自于 DexterityTbl，只与职业相关，不随等级变化，但是使用物品可以改变该值 ModifyPlrDex  

> player._pDexterity  
实际的 dexterity，来自于 _pBaseDex + item 加成 + spell 技能加成  

> player._pLevel  
玩家等级，玩家升级的时候，影响只是 hp, mana 和加点  





