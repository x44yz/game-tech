基于 https://github.com/alexbatalov/fallout2-re 项目代码分析  

> 公式  
```
var accuracy = skill_level // 命中率，来自空手或武器技能等级
accuracy = trait_adjust_accuracy // 天赋调整
accuracy = perk_adjust_accuracy // perk 调整
accuracy = minStrength_adjust_accuracy // 武器要求的力量不足时降低命中率，少1点扣20
accuracy = accuracy - armorClass // 护甲调整
accuracy = hitLocation_adjust_accuracy // 远近武器针对不同部位修正命中率
if (defender.isMultiHex) accuracy += 15 // 如果目标体型占用多格
accuracy = lightIntensity_adjust_accuracy // 光线修正命中率
if (attacker.isBlind) accuracy -= 25 // 如果攻击者处于致盲
if (defender.isKnockOut/isKnockDown) accuracy += 40 // 如果被攻击者击晕或击倒
accuracy = min(accuracy, 95) // 限定上限

if (rand(accuracy) == ROLL_SUCCESS || ROLL_CRITICAL_SUCCESS) // 随机命中率
{
    // 计算伤害，遍历有效子弹
    totalDamage = 0
    foreach (ammo)
    {
        damage = item_w_damage // 获得武器伤害
        damage += damageBouns_perk // perk 武器加成
        damage *= damageMultiplier_perkSlientDeath // 潜行伤害缩放
        if (crit) damage *= damageMultiplier_crit // 暴击伤害缩放
        damage *= combatDifficultyDamageModifier // 不同战斗难度伤害修正
        damage -= damageThreshold // 扣除防护硬值（来自护甲）
        damage -= damage * damageResistance / 100 // 扣除伤害减免（来自护甲）
        totalDamage += damage
    }
    totalDamage = totalDamage_adjust_perk // perk 修正
}
```

>TODO:  
[x]combat_attack   
[x]apply_damage  
[x]register_clear  
[x]STAT_DAMAGE_THRESHOLD
[x]STAT_DAMAGE_RESISTANCE
[x]与伤害有关 STAT_MELEE_DAMAGE  
[x]stat_recalc_derived  
[x]stat_set_base  

>REF:  
https://fallout.fandom.com/wiki/Fallout_2  
https://fallout.fandom.com/wiki/Damage  
https://f3mic.github.io/index.html  
https://www.bilibili.com/read/cv12206741  
https://fallout.fandom.com/wiki/Fallout_2_skills  

> 设定  
tile 是 hex grid 布局  
武器作为主武器和副武器的最大距离来自不同设定 maxRange1 & maxRange2  
Strength 与投掷物最远距离公式 maxRange = 3 * strength(包含 perk 加成)  
空手状态下攻击距离是 1 格，但是长手脚（CRITTER_LONG_LIMBS）攻击距离是 2 格  
标记技能（taged skill）能够增加  
命中率（accuracy）的上限是 95  
减少伤害三种方式 AC（Armor Class），DR（Damage Resistance），DT（Damage Threshold）  

> Stat  

| 属性 | 定义 | Base | 说明 | 改变 |
|------|-----|----- |------|------|
|Strength |STAT_STRENGTH |0| 力量，1 点可携带 25 磅重量，当不满足武器的最低力量要求时每少一点损失 20 的命中率 | HP，近战伤害，携带重量，武器力量要求 
|Max HP|STAT_MAXIMUM_HIT_POINTS | base.ST + base.EN * 2 + 15 |
|Max AP|STAT_MAXIMUM_ACTION_POINTS | AG / 2 + 5 |
|Armor Class|STAT_ARMOR_CLASS | AG |
|Melee Damage |STAT_MELEE_DAMAGE | max(ST - 5, 1)
|Carry Weight |STAT_CARRY_WEIGHT | 25 * ST + 25 |
|Sequence |STAT_SEQUENCE | 2 * PE
|Healing Rate |STAT_HEALING_RATE | max(EN / 3, 1)
|Critical Chance|STAT_CRITICAL_CHANCE | luck
|Better Criticals|STAT_BETTER_CRITICALS | 0 |
|Radiation Resistance|STAT_RADIATION_RESISTANCE | 2 * EN |
|Poison Resistance|STAT_POISON_RESISTANCE | 2 * EN | 
|Damage Threshold |STAT_DAMAGE_THRESHOLD |0|防护硬值，来自 Armor 加成，直接从伤害中扣除
|Damage Resistance |STAT_DAMAGE_RESISTANCE |0|伤害减免，来自 Armor 加成，扣除最后伤害的百分比

> Perk

| 属性 | 定义 | 等级要求 | 其他要求 |好处Benefit |
|------|-----|----------|---------|----|
|Heave Ho! | PERK_HEAVE_HO |6|ST<9 | 当使用投掷武器时，每级增加 2 点 Strength
|Weapon Handling|PERK_WEAPON_HANDLING|12|ST<7, AG 5 | 武器使用需要的 Strength 检查时 +3 Strength
|Slayer |PERK_SLAYER|24|ST 8，AG8，Unarmed skill 80% |所有近战攻击变为暴击
|Silent Death |PERK_SILENT_DEATH|18|AG 6,Sneak 80%, Unarmed 80% | 潜行的时候从后面造成双倍伤害

> Weapon Perk

| 属性 | 定义 | 作用Effect |
|------|-----|----------|
|Weapon Accurate|PERK_WEAPON_ACCURATE | 增加20%命中
|Weapon Night Sight|PERK_WEAPON_NIGHT_SIGHT | 排除环境亮度对命中率的干扰
|Weapon Long Range |PERK_WEAPON_LONG_RANGE | 攻击距离增加100%
|Weapon Scope Range |PERK_WEAPON_SCOPE_RANGE | 攻击距离增加150%

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
|Jinxed |TRAIT_JINXED | 环绕你的暴击失败更高 |自己暴击失败也更高
|Finesse |TRAIT_FINESSE |+10%暴击概率 |-30%总伤害


