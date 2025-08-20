using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for collected elements effects.
/// </summary>
public abstract class Effect
{
    protected int _effectLevel = 1;
    protected Position2D _parentGameboardPosition;
    protected string _description = "effect description";

    public int EffectLevel { get => _effectLevel; protected set => _effectLevel = value; }
    public string Description { get => _description; protected set => _description = value; }

    public Effect(Position2D position2D) { _parentGameboardPosition = position2D; }

    /// <summary>
    /// Sets position of effect.
    /// </summary>
    /// <param name="position2D"></param>
    public void SetPosition(Position2D position2D) { _parentGameboardPosition = position2D; }

    /// <summary>
    /// Set power of the effect.
    /// </summary>
    /// <param name="level"></param>
    public void SetLevel(int level) { EffectLevel = level; }

    /// <summary>
    /// Activates effect. Abstract method. 
    /// </summary>
    /// <param name="gameboard"></param>
    public abstract void ActivateEffect(Gameboard gameboard);

}
