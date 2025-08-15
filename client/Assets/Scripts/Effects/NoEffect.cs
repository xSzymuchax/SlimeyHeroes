using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
