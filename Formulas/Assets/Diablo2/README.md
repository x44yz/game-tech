TODO:  
[x]level up  
[+]drop  
[x]伤害数字  
[x]装备  
[+]去除 class 区别  
[+]去除 difficult 区别  
[x]monster hit player
[x]player hit player
[+]DamageArmor
[+]DamageWeapon

https://diablo2.diablowiki.net/
http://wiki.d.163.com/index.php?title=%E8%AF%8D%E7%BC%80_(Diablo_I)
https://d2.lc/AB/wiki/index9c3b.html?title=Damage  
https://www.diablo-2.net/skillcalculator  
https://planetdiablo.eu/diablo2/calcs/dmgcalc/dmgcalc.php?patch=110&lang=eng
http://www.baronsbazaar.ca/forum/ppr/damage_calc.html  

> 升级所需的经验值  
![excel](./../../Raw/diablo_%E7%BB%8F%E9%AA%8C%E5%80%BC.xlsx)  
从 excel 中可以看出，是个类似前平后高的曲线，而 2 级之间的差值近似是条直线  

> 玩家 A 攻击玩家 B 流程  
A 广播 destAction=ACTION_ATTACKPLR，每个客户端更新攻击（为了表现一致，A 的攻击动画和 B 的受伤动画），实际触发攻击的关键帧在 A 计算，然后发送 B 受伤的消息到 B，，B 收到消息扣除数值，然后同步数据到其他玩家。

> player 属性

| 属性 | 名字 | 说明 |
|------|-----|------| 
|_pLevel|等级|玩家升级的时候，影响只是 hp, mana 和加点  
|_pBaseDex|基础敏捷度|来自于 DexterityTbl，只与职业相关，不随等级变化，使用物品或加点可以改变该值 ModifyPlrDex
|_pDexterity| 敏捷 | 会影响击中概率和闪避概率，来自于 _pBaseDex + item 加成 + spell 技能加成 |
|_pVitality | 活力 | 增加生命值，除此之外没有其他效果 |
|_pStrength | 力量 | 近战伤害，以及可以携带装备的指标，基本公式 _pDamageMod = _pLevel * _pStrength / 100，不同职业有不同计算 |
|_pMagic | 魔法 | 增加法术伤害，以及法术命中概率 |
|_pIMinDam| 最小伤害 | 由 item 提供，如果无 item 加成，默认=1 |
|_pIMaxDam| 最大伤害 | 由 item 提供，如果无 item 加成，默认=1 |
|_pIBonusDam| 额外伤害比 | 由 item 提供，计算公式 dam += dam * _pIBounsDam / 100
|_pIBonusDamMod | 额外伤害 | 由 item 提供，计算公式在 _pIBounsDam 之后，dam+=_pIBounsDamMod
|_pDamageMod | 伤害 | 由力量提供，参考 _pStrength
|_pIAC | 精准 | =item._iAC，会用于计算护甲 GetArmor，可以理解为精准度越高，越容易躲避伤害  
|_pIBonusAC| 额外精准 | =item._iAC * item._iPLAC / 100
|_pIGetHit| 减免伤害 | 由 item 提供 
|_pHPBase/_pMaxHPBase| 基础血量 | 与Vitality和玩家等级相关，魔法师每级加64，其余职业加128
|_pHitPoints/_pMaxHP | 血量 | 在玩家基础血量之上
|pDamAcFlags|


> player._pClass  

| 职业 | 力量 | 意志（魔法） | 敏捷 | 活力 | 抗性 | 防御 | 精准 | 说明 |
|------|-----|-----|------| -----|------|------|-----|------|
|Warrior战士 | 20 | 10 | 20 | 25 | 0 | 0 | 0 | 近战命中概率+20 |
|Rogue弓箭手 | 20 | 15 | 30 | 20 | 0 | 0 | 0 | 
|Sorcerer魔法师 | 15 | 35 | 15 | 20 | 0 | 0 | 0 |
|Monk武僧 | 25 | 15 | 25 | 20 | 0 | 7 | 62 | 擅长空手和杖类武器，属性平均  
|Bard游吟诗人 | 20 | 20 | 25 | 20 | 0 | 5 | 62 | 属性平衡
|Barbarian野蛮人 | 40 | 0 | 20 | 25 | 0 | 6 | 60 | 魔法为0，完全靠力量取胜，最适合 axe，maces，mauls 武器，可以同时打击三个目标，如果装备了盾牌反而降低伤害力和攻击速度，防御也将下降，同时没有多目标攻击能力

> item 属性

|属性|名字|说明|
|----|----|----|
|_iDurability|耐久|护甲格挡攻击成功会有概率损失耐久，但是用盾牌攻击的时候不会损失耐久，使用武器攻击成功时候也会损失耐久，可以参看 DamageArmor，DamageWeapon [ref](https://diablo2.diablowiki.net/Ethereal#Item_Durability_Loss)|
|_iPLDam|额外伤害比|叠加到玩家 _pIBonusDam |



