using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum WeaponType
{
    Stake = 0,
    Pistol = 1,
    Rifle = 3,
    Shotgun = 2,
    DiceGun = 4,

    Count
}



public class UIDice : MonoBehaviour
{
    [SerializeField]
    private DiceAndWeaponData m_data;

    [SerializeField]
    private Image m_diceImage;
    [SerializeField]
    private Animation m_diceAnimation;
    [SerializeField]
    private Image m_weaponImage;

    private Dictionary<WeaponType,DiceAndWeaponData.Definition> m_dataMap = null;

    private void Start()
    {
        Debug.Assert(m_data != null);
        Debug.Assert(m_diceImage != null);
        Debug.Assert(m_diceAnimation != null);
        Debug.Assert(m_weaponImage != null);

        m_dataMap = new Dictionary<WeaponType, DiceAndWeaponData.Definition>(m_data.Definitions.Length);

        foreach (var d in m_data.Definitions)
        {
            m_dataMap.Add(d.weaponType, d);
        }
    }

    WeaponType m_currentWeaponType = WeaponType.Pistol;

    [ContextMenu("TestRefreshDice")]
    public void TestRefreshDice()
    {
        UpdateDiceWeapon((WeaponType)Random.Range(0, (int)(WeaponType.DiceGun) + 1));
    }

    public void UpdateDiceWeapon(WeaponType type)
    {
        m_currentWeaponType = type;
        m_diceAnimation.Play("A_UIDiceRoll");

        m_weaponImage.enabled = false;
    }

    public void DiceTextureSwap()
    {
        m_diceImage.sprite = m_dataMap[m_currentWeaponType].diceSprite;
        m_weaponImage.sprite = m_dataMap[m_currentWeaponType].weaponSprite;
    }

    public void DiceAnimationEnd()
    {
        m_weaponImage.enabled = true;
    }
}
