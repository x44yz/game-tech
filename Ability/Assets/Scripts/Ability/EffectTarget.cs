using System;

public interface EffectTarget
{
    void RemoveEffect(Effect effect);
    bool IsValidEffectTarget();
}

