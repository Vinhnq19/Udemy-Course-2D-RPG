using UnityEngine;
using System;
using System.Text;

[Serializable]

public class Inventory_Item
{
    private string itemId;
    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers { get; private set; }
    public ItemEffectDataSO itemEffect;
    public int buyPrice { get; private set; }
    public float sellPrice { get; private set; }

    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;

        itemEffect = itemData.itemEffect;
        buyPrice = itemData.itemPrice;
        sellPrice = itemData.itemPrice * 0.35f;

        modifiers = EquipmentData()?.modifiers;
        itemId = itemData.itemName + " - " + Guid.NewGuid();
    }
    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var modifier in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.AddModifier(modifier.value, itemId);
        }
    }

    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var modifier in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(modifier.statType);
            statToModify.RemoveModifier(itemId);
        }
    }

    public void AddItemEffect(Player player) => itemEffect?.Subscribe(player);
    public void RemoveItemEffect() => itemEffect?.Unsubscribe();

    private EquipmentDataSO EquipmentData()
    {
        if (itemData is EquipmentDataSO equipmentData)
        {
            return equipmentData;
        }
        return null;
    }

    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;

    public string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();
        if (itemData.itemType == ItemType.Material)
        {
            sb.AppendLine("");
            sb.AppendLine("Use for crafting.");
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }
        if (itemData.itemType == ItemType.Consumable)
        {
            sb.AppendLine("");
            sb.AppendLine(itemEffect.effectDescription);
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }
        sb.AppendLine("");
        foreach (var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentageStat(mod.statType) ? $"{mod.value}%" : mod.value.ToString();
            sb.AppendLine($"{modType}: {modValue}");
        }

        if (itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("UniqueEffect:");
            sb.AppendLine(itemEffect.effectDescription);
        }

        sb.AppendLine("");
        sb.AppendLine("");

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

