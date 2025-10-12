using System;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemTooltip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow)
    {
        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);

        base.ShowToolTip(show, targetRect);
    }
    public string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
        {
            return "Used for crafting other items.";

        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("");
        foreach(var mod in item.modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? $"{mod.value}%" : mod.value.ToString();
            sb.AppendLine($"{modType}: {modValue}");
        }
        return sb.ToString();
    }

    private string GetStatNameByType(StatType statType)
    {
        switch (statType)
        {
            //Max Health
            case StatType.MaxHealth:
                return "Max Health";
            //Health Regeneration
            case StatType.HealthRegen:
                return "Health Regen";
            //Strength
            case StatType.Strength:
                return "Strength";
            //Intelligence
            case StatType.Intelligence:
                return "Intelligence";
            //Agility
            case StatType.Agility:
                return "Agility";
            //Vitality
            case StatType.Vitality:
                return "Vitality";
            //Attack Speed
            case StatType.AttackSpeed:
                return "Attack Speed";
            //Damage
            case StatType.Damage:
                return "Damage";
            //Critical Chance
            case StatType.CritChance:
                return "Crit Chance";
            //Critical Power
            case StatType.CritPower:
                return "Crit Power";
            //Armor Reduction
            case StatType.Armor:
                return "Armor";
            case StatType.ArmorReduction:
                return "Armor Reduction";
            //Fire, Ice, Lightning Damage
            case StatType.FireDamage:
                return "Fire Damage";
            case StatType.IceDamage:
                return "Ice Damage";
            case StatType.LightningDamage:
                return "Lightning Damage";
            //Evasion
            case StatType.Evasion:
                return "Evasion";
            //Resistances
            case StatType.FireResistance:
                return "Fire Resist";
            case StatType.IceResistance:
                return "Ice Resist";
            case StatType.LightningResistance:
                return "Lightning Resist";
            default:
                return "Unknown Stat";
        }
    }
    private bool IsPercentageStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.CritChance:
            case StatType.CritPower:
            case StatType.ArmorReduction:
            case StatType.AttackSpeed:
            case StatType.FireResistance:
            case StatType.IceResistance:
            case StatType.LightningResistance:
            case StatType.Evasion:
                return true;
            default:
                return false;
        }
    }
    
}
