using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorSplashEffect : Effect
{
    public int _colorSplashRange = 1;
    private ElementType _myType;

    public ColorSplashEffect(Position2D position2D, int level, ElementType elementType) : base(position2D)
    {
        SetLevel(level);
        _myType = elementType;
        _colorSplashRange = CalculateSplashRange();
    }

    private int CalculateSplashRange()
    {
        return 1 * EffectLevel;
    }

    public override void ActivateEffect(Gameboard gameboard)
    {
        //ElementType myType = gameboard.GetElementFromCell(_parentGameboardPosition).elementType;

        for (int i = _parentGameboardPosition.X - _colorSplashRange; i <= _parentGameboardPosition.X + _colorSplashRange; i++)
        {
            for (int j = _parentGameboardPosition.Y - _colorSplashRange; j <= _parentGameboardPosition.Y + _colorSplashRange; j++)
            {
                if (i < 0 || j < 0 || i >= gameboard.boardWidth || j >= gameboard.boardHeigth)
                    continue;

                if (_parentGameboardPosition.X == i && _parentGameboardPosition.Y == j)
                    continue;

                Position2D position2D = new Position2D(i,j);
                Element e = gameboard.GetElementFromCell(position2D);
                
                if (e == null)
                    continue;

                gameboard.ChangeElementType(position2D, _myType);
            }
        }
    }

}
