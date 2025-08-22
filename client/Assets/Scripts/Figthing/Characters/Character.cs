using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public GameObject CharacterPrefab;
    public Image HealthBar;

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

    public virtual bool Attack(Character target)
    {
        bool isHit = target.TakeDamage(_currentDamage, _characterType);
        return isHit;
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
