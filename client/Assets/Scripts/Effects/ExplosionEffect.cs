using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Explosion effect class.
/// </summary>
public class ExplosionEffect : Effect
{
    private int _explosionRange = 1;
    public ExplosionEffect(Position2D position2D, int level) : base(position2D)
    {
        SetLevel(level);
        _explosionRange = CalculateExplosionRange();
    }

    private int CalculateExplosionRange()
    {
        return EffectLevel * 1;
    }

    public override void ActivateEffect(Gameboard gameboard)
    {
        List<Position2D> elementsToCollect = new();
        
        for (int i = _parentGameboardPosition.X - _explosionRange; i <= _parentGameboardPosition.X + _explosionRange; i++)
        {
            for (int j = _parentGameboardPosition.Y - _explosionRange; j <= _parentGameboardPosition.Y + _explosionRange; j++)
            {
                if (i < 0 || j < 0 || i >= gameboard.boardWidth || j >= gameboard.boardHeigth)
                    continue;

                if (_parentGameboardPosition.X == i && _parentGameboardPosition.Y == j)
                    continue;

                Position2D position2D = new Position2D(i, j);
                Element e = gameboard.GetElementFromCell(position2D);

                if (e == null)
                    continue;

                elementsToCollect.Add(position2D);
            }
        }

        List<CollectedElementsInformation> collectedElementsInformationLists = gameboard.CollectElements(elementsToCollect, false);
        GameController.Instance.CollectMixedElements(collectedElementsInformationLists);
    }
}
