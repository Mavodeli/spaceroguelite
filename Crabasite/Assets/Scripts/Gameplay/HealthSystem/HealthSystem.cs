using System;
using UnityEngine;

public class HealthSystem {

    // Events
    public event EventHandler OnHealthChanged;

    // Fields
    int _currentHealth;
    int _currentMaxHealth;

    // Properties
    public int Health{
        get{
            return _currentHealth;
        }
        set{
            _currentHealth = value;
        }
    }

    public int MaxHealth{
        get{
            return _currentMaxHealth;
        }
        set{
            _currentMaxHealth = value;
        }
    }

    // Constructor
    public HealthSystem(int health, int maxHealth){
        _currentHealth = health;
        _currentMaxHealth = maxHealth;
    }

    // Methods
    public float GetHealthPercent(){
        return (float)_currentHealth / _currentMaxHealth; 
    }

    public void Damage(int damageAmount){
        if(_currentHealth > 0) _currentHealth -= damageAmount;
        if(OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

    public void Heal(int healAmount){
        if(_currentHealth < _currentMaxHealth) _currentHealth += healAmount;
        if(_currentHealth > _currentMaxHealth) _currentHealth = _currentMaxHealth;
        if(OnHealthChanged != null) OnHealthChanged(this, EventArgs.Empty);
    }

}

