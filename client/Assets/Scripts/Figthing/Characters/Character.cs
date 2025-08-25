using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public GameObject CharacterPrefab;
    public Image HealthBar;
    public Image UltimateBar;

    public ElementType _characterType;

    protected float _baseHealth = 50f;
    public float _currentHealth;
    protected float _finalHealth;

    protected float _baseDamage = 4f;
    protected float _currentDamage;
    protected float _finalDamage;

    protected float _dodgeRate = 5f;
    protected float _blockRate = 5f;

    public bool isAlive = true;

    protected int _ultimateCooldown = 5;
    protected int _currentUltimateCooldown = 0;

    public void Initialize()
    {
        _finalDamage = CalculateDamage();
        _currentDamage = _finalDamage;

        _finalHealth = CalculateHealth();
        _currentHealth = _finalHealth;



        UpdateHealthBar();
    }

    protected void UpdateHealthBar()
    {
        HealthBar.fillAmount = _currentHealth / _finalHealth;
    }

    protected void UpdateUltBar()
    {
        UltimateBar.fillAmount = (float)_currentUltimateCooldown / _ultimateCooldown;
    }

    public void HealUp(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _finalHealth)
            _currentHealth = _finalHealth;
        UpdateHealthBar();
    }

    public bool TakeDamage(float amount, ElementType damageType)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0)
            isAlive = false;

        UpdateHealthBar();
        return true;
    }

    // this method should describe how the corresponding character attack
    public virtual List<bool> Attack(List<Character> targets)
    {
        List<bool> hits = new List<bool>();

        foreach (var target in targets)
        {
            hits.Add(target.TakeDamage(_currentDamage, _characterType));
        }

        _currentUltimateCooldown++;

        if (_currentUltimateCooldown == _ultimateCooldown)
            CastUltimate(targets);

        UpdateUltBar();
        return hits;
    }

    protected void CastUltimate(List<Character> targets)
    {
        _currentUltimateCooldown = 0;
        UpdateUltBar();
        return;
    }

    protected float CalculateDamage()
    {
        return _baseDamage;
    }

    protected float CalculateHealth()
    {
        return _baseHealth;
    }
}
