using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSplashEffect : Effect
{
    public int _colorSplashRange = 1;

    public ColorSplashEffect(int level)
    {
        EffectLevel = level;
        _colorSplashRange = CalculateSplashRange();
    }

    private int CalculateSplashRange()
    {
        return 1 * EffectLevel;
    }

    public override void ActivateEffect()
    {
        throw new System.NotImplementedException();
    }

}
