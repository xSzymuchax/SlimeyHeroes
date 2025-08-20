using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Color wiping effect class.
/// </summary>
public class ColorWipeEffect : Effect
{
    public int _colorSplashRange = 1;
    private ElementType _myType;

    public ColorWipeEffect(Position2D position2D, int level, ElementType elementType) : base(position2D)
    {
        SetLevel(level);
        _myType = elementType;
        _colorSplashRange = CalculateWipeRange();
    }

    private int CalculateWipeRange()
    {
        return 2 * EffectLevel;
    }

    public override void ActivateEffect(Gameboard gameboard)
    {
        List<Position2D> elementsToCollect = new();

        for (int i = _parentGameboardPosition.X - _colorSplashRange; i <= _parentGameboardPosition.X + _colorSplashRange; i++)
        {
            for (int j = _parentGameboardPosition.Y - _colorSplashRange; j <= _parentGameboardPosition.Y + _colorSplashRange; j++)
            {
                if (i < 0 || j < 0 || i >= gameboard.boardWidth || j >= gameboard.boardHeigth)
                    continue;

                if (_parentGameboardPosition.X == i && _parentGameboardPosition.Y == j)
                    continue;

                Position2D position2D = new Position2D(i, j);
                Element e = gameboard.GetElementFromCell(position2D);

                if (e == null)
                    continue;

                if (e.elementType == _myType)
                    elementsToCollect.Add(position2D);
            }
        }

        List<CollectedElementsInformation> collectedElementsInformationLists = gameboard.CollectElements(elementsToCollect, false);
        GameController.Instance.CollectMixedElements(collectedElementsInformationLists);
    }

}
