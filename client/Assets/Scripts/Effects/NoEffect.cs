using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// redundant class
/// </summary>
public class NoEffect : Effect
{
    public NoEffect(Position2D position2D) : base(position2D)
    {
    }

    public override void ActivateEffect(Gameboard gameboard)
    {
        return;
    }
}
