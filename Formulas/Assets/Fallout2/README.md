基于 https://github.com/alexbatalov/fallout2-re 项目代码分析  

>TODO:  
[x]combat_attack   

>REF:  
https://fallout.fandom.com/wiki/Fallout_2  
https://fallout.fandom.com/wiki/Damage  
https://f3mic.github.io/index.html  
https://www.bilibili.com/read/cv12206741  
https://fallout.fandom.com/wiki/Fallout_2_skills  

> fallout2 设定  
tile 是 hex grid 布局  
武器作为主武器和副武器的最大距离来自不同设定 maxRange1 & maxRange2  
Strength 与投掷物最远距离公式 maxRange = 3 * strength(包含 perk 加成)  
空手状态下攻击距离是 1 格，但是长手脚（CRITTER_LONG_LIMBS）攻击距离是 2 格  
标记技能（taged skill）能够增加  
命中率（accuracy）的上限是 95  

> Perk

| 属性 | 定义 | 等级要求 | 其他要求 |好处Benefit |
|------|-----|----------|---------|----|
|Heave Ho! | PERK_HEAVE_HO |6|ST<9 | 当使用投掷武器时，每级增加 2 点 Strength
|Weapon Handling|PERK_WEAPON_HANDLING|12|ST<7, AG 5 | 武器使用需要的 Strength 检查时 +3 Strength

> Weapon Perk

| 属性 | 定义 | 作用Effect |
|------|-----|----------|
|Weapon Accurate|PERK_WEAPON_ACCURATE | 增加20%命中

> Skill
所有技能最大上限 300%  
武器根据类型有默认的 skill 定义（attack_skill），另外武器的伤害类型或扩展定义（extendedFlags）也会影响 skill 类型。（武器的 skill 是根据表现反推的？）

| 属性 | 定义 | 公式
|----|----|----|
|Small Guns|SKILL_SMALL_GUNS| 5%+(4xAG) 
|Unarmed|SKILL_UNARMED| 30%+2x(ST+AG)
|Melee Weapons|SKILL_MELEE_WEAPONS| 20%+2x(ST+AG)
|Throwing|SKILL_THROWING| 4%xAG
|Energy Weapons|SKILL_ENERGY_WEAPONS|2%xAG
|Big Guns|SKILL_BIG_GUNS|2%xAG

> Trait  
天赋，创建角色时候可以选择 2 个天赋

| 属性 | 定义 | 好处Benefit | 坏处Penalty
|----|----|----|----|
|One Hander|TRAIT_ONE_HANDER| 单手武器增加20%命中概率 | 双手武器减少40%命中概率

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
|_iDurability|耐久|护甲格挡攻击成功会有概率损失耐久，但是用盾牌攻击的时候不会损失耐久，使用武器攻击成功时候也会损失耐久，可以参看 DamageArmor，DamageWeapon [ref](https://diablo2.diablowiki.net/Ethereal#Item_Durability_Loss),护甲每次扣一点，与伤害大小无关|
|_iPLDam|额外伤害比|叠加到玩家 _pIBonusDam |

