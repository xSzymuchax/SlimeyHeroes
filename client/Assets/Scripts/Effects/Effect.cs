using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect
{
    protected int _effectLevel = 1;
    protected Position2D _parentGameboardPosition;
    protected string _description = "effect description";

    public int EffectLevel { get => _effectLevel; protected set => _effectLevel = value; }
    public string Description { get => _description; protected set => _description = value; }

    public Effect(Position2D position2D) { _parentGameboardPosition = position2D; }

    public void SetPosition(Position2D position2D) { _parentGameboardPosition = position2D; }
    public void SetLevel(int level) { EffectLevel = level; }
    public abstract void ActivateEffect(Gameboard gameboard);

}
