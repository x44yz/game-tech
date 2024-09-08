TODO:  
[+]weapon  
[x]Strike 击打  
[+]KillPedWithCar  
[x]FireMelee  
[+]FireShotgun  

https://github.com/td512/re3  
https://gtamods.com/wiki/Weapon.dat_(VC)  


> player 属性

| 属性 | 名字 | 说明 |
|------|-----|------| 
m_fHealth | 血量 | 范围（0, 100）
m_fArmour | 护甲 | 范围 (0, 100)

> 伤害公式  
health - max(damage - armor, 0)  
很好理解的公式，就是护甲先抵消伤害，再扣除血量，所以射击的变化点主要在不同武器的伤害，以及玩家的特殊状态（比如肾上腺素状态伤害加深）
