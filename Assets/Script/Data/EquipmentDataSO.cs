using UnityEngine;
using System;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Equipment item", fileName = "Equipment data - ")]

public class EquipmentDataSO : ItemDataSO
{
    [Header("Item Modifiers")]
    public ItemModifier[] modifiers;
}
[Serializable]
public class ItemModifier
{
    public StatType statType;
    public float value;
}
