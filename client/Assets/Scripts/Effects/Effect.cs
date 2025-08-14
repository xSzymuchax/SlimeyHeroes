using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    private int _effectLevel = 1;
    private string _description = "effect description";

    public int EffectLevel { get => _effectLevel; protected set => _effectLevel = value; }
    public string Description { get => _description; protected set => _description = value; }


    public void SetLevel(int level) { EffectLevel = level; }
    public abstract void ActivateEffect();

}
