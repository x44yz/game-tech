以 pixel-dungeon 为主，他简化太多属性只保留一个 str 属性，回蓝是通过 recharge 表示次数（省略智力属性，因为是 turn 机制，所以按照次数没啥问题），闪避是护甲给的（省略敏捷属性）
攻防核心在 char.attack
先判断是否命中，然后 dmg - dr(伤害减免)，然后其他职业，buff 修正伤害，最后实际伤害到敌人身上（敌人还有 buff，抗性）

shattered-pixel-dungeon
git@github.com:00-Evan/shattered-pixel-dungeon.git

pixel-dungeon
git@github.com:watabou/pixel-dungeon.git