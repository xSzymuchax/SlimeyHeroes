using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    public Transform _myTeamSpawn;
    public Transform _enemyTeamSpawn;

    private List<Character> _myTeam;
    private List<Character> _enemyTeam;

    // TODO - fix initialization -it is for sure zepsuta
    public void Initialize(List<GameObject> myTeam, List<GameObject> enemyTeam, Transform myteamSpawn, Transform enemyTeamSpawn)
    {
        _myTeamSpawn = myteamSpawn;
        _enemyTeamSpawn = enemyTeamSpawn;
        
        SpawnPrefabs(myTeam, enemyTeam);
        GetCharacterComponents();
        InitializeCharacters();
    }

    private void GetCharacterComponents()
    {
        _myTeam = new();
        _enemyTeam = new();

        foreach (Transform child in _myTeamSpawn.transform)
        {
            _myTeam.Add(child.gameObject.GetComponent<Character>());
        }

        foreach (Transform child in _enemyTeamSpawn.transform)
        {
            _enemyTeam.Add(child.gameObject.GetComponent<Character>());
        }
    }

    private void InitializeCharacters()
    {
        foreach (Character item in _myTeam)
        {
            item.Initialize();
        }

        foreach (var item in _enemyTeam)
        {
            item.Initialize();
        }
    }

    private void SpawnPrefabs(List<GameObject> myTeam, List<GameObject> enemyTeam)
    {
        foreach (var item in myTeam)
        {
            Instantiate(item.gameObject, _myTeamSpawn);
        }

        foreach (var item in enemyTeam)
        {
            Instantiate(item.gameObject, _enemyTeamSpawn);
        }
    }

    private List<Character> FindAttackedTeam(Character attacker)
    {
        List<Character> attackedTeam = null;

        foreach (Character c in _myTeam)
        {
            if (attacker == c)
                attackedTeam = _enemyTeam;
        }

        if (attackedTeam == null)
            attackedTeam = _myTeam;

        return attackedTeam;
    }

    private Character FindWhoAttack(ElementType elementType)
    {
        foreach (Character c in _myTeam)
        {
            if (c._characterType == elementType)
                return c;
        }
        return null;
    }

    private void CharacterPerformsAttack(Character attacker)
    {
        List<Character> attackedTeam = FindAttackedTeam(attacker);
        Character target = attackedTeam[0];
        attacker.Attack(target);

        //ApplyEffectsOverTime();
        //RemoveDeadCharacters();
    }

    public void PerformAction(ElementType elementType)
    {
        Character c = FindWhoAttack(elementType);

        // if no character assigned
        if (c == null)
            return;

        // attack or other ability
        CharacterPerformsAttack(c);
    }
}
