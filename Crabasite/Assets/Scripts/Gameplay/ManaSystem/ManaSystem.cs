using System;
using UnityEngine;


public class ManaSystem {

    // Events
    public event EventHandler OnManaChanged;

    // Fields
    int _currentMana;
    int _currentMaxMana;

    // Properties
    public int Mana{
        get{
            return _currentMana;
        }
        set{
            _currentMana = value;
        }
    }

    public int MaxMana{
        get{
            return _currentMaxMana;
        }
        set{
            _currentMaxMana = value;
        }
    }

    // Constructor
    public ManaSystem(int mana, int maxMana){
        _currentMana = mana;
        _currentMaxMana = maxMana;
    }

    // Methods
    public float GetManaPercent(){
        return (float)_currentMana / _currentMaxMana; 
    }

    public void ManaUsage(int manaUsageAmount){
        if(_currentMana > 0) _currentMana -= manaUsageAmount;
        if(OnManaChanged != null) OnManaChanged(this, EventArgs.Empty); //trigger Event
    }

    public void ManaRefill(int manaRefillAmount){
        if(_currentMana < _currentMaxMana) _currentMana += manaRefillAmount;
        if(_currentMana > _currentMaxMana) _currentMana = _currentMaxMana;
        if(OnManaChanged != null) OnManaChanged(this, EventArgs.Empty); //trigger Event
    }

    //returns a ManaBar attached to the given GameObject with the given offset in y direction
    public ManaBar attachManaBar(GameObject _parent, float offsetY){
        GameObject pfManaBar = Resources.Load<GameObject>("Prefabs/ManaSystem/pfManaBar");
        Transform manaBarTransform = Transform.Instantiate(pfManaBar.transform, Vector3.zero, Quaternion.identity); 
        ManaBar manaBar = manaBarTransform.GetComponent<ManaBar>();
        manaBar.Setup(this);
        manaBar.transform.parent = _parent.transform;
        manaBar.name = _parent.name+"'s manaBar";
        manaBar.transform.localPosition = new Vector3(0, offsetY, 0);
        return manaBar;
    }
}

