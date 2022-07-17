using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DiceAndWeaponData : ScriptableObject
{
    public Definition[] Definitions;
    [System.Serializable]
    public class Definition
    {
        public WeaponType weaponType;
        public Sprite weaponSprite;
        public Sprite diceSprite;
    }
}