using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Player_Stats playerStats;
    private RectTransform rect;
    private UI ui;

    [SerializeField] private StatType statSlotType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(statSlotType);
        statName.text = GetStatNameByType(statSlotType);
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        playerStats = FindFirstObjectByType<Player_Stats>();
    }

        public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(true, rect, statSlotType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(false, null);
    }

    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statSlotType);
        if (statToUpdate == null && statSlotType != StatType.ElementalDamage)
        {
            Debug.Log($"You do not have {statSlotType} implemented on the player!");
            return;
        }
        float value = 0;

        switch (statSlotType)
        {
            case StatType.Strength:
                value = playerStats.major.strength.GetValue();
                break;
            case StatType.Intelligence:
                value = playerStats.major.intelligence.GetValue();
                break;
            case StatType.Agility:
                value = playerStats.major.agility.GetValue();
                break;
            case StatType.Vitality:
                value = playerStats.major.vitality.GetValue();
                break;
            //Offensen stats
            case StatType.Damage:
                value = playerStats.GetBaseDamage();
                break;
            case StatType.CritChance:
                value = playerStats.GetCritChance();
                break;
            case StatType.CritPower:
                value = playerStats.GetCritPower();
                break;
            case StatType.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100f; // Convert to percentage
                break;
            case StatType.AttackSpeed:
                value = playerStats.offense.attackSpeed.GetValue() * 100f; // Convert to percentage
                break;
            //Defense stats; Max health, Health regen, Evasion, Amror
            case StatType.MaxHealth:
                value = playerStats.GetMaxHp();
                break;
            case StatType.HealthRegen:
                value = playerStats.resources.healthRegen.GetValue();
                break;
            case StatType.Evasion:
                value = playerStats.GetEvasion();
                break;
            case StatType.Armor:
                value = playerStats.GetBaseArmor();
                break;
            //Elemental Damage
            case StatType.FireDamage:
                value = playerStats.offense.fireDamage.GetValue();
                break;
            case StatType.IceDamage:
                value = playerStats.offense.iceDamage.GetValue();
                break;
            case StatType.LightningDamage:
                value = playerStats.offense.lightningDamage.GetValue();
                break;
            case StatType.ElementalDamage:
                value = playerStats.GetElementalDamage(out ElementType elementType, 1);
                break;
            //Resistances
            case StatType.FireResistance:
                value = playerStats.GetElementalResistance(ElementType.Fire) * 100; // Convert to percentage
                break;
            case StatType.IceResistance:
                value = playerStats.GetElementalResistance(ElementType.Ice) * 100;
                break;
            case StatType.LightningResistance:
                value = playerStats.GetElementalResistance(ElementType.Lightning) * 100;
                break;

        }
        
        statValue.text = IsPercentageStat(statSlotType) ? value + "%" : value.ToString();
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
            case StatType.ElementalDamage:
                return "Elemental Damage";
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
