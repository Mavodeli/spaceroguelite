using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public abstract class Ultimate : ScriptableObject
{
    public float cooldown;
    public bool isActive;
    public Transform player;

    public abstract void Use();
}
