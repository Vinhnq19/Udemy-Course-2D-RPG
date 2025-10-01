using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]

public class Buff
{
    public StatType type;
    public float value;
}

public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer sr;
    private Entity_Stats statsToModify;
    [Header("Buff Settings")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName = "Buff Name";
    [SerializeField] private float buffDuration = 4f;
    [SerializeField] private bool canbeUsed = true;
    [Header("floaty movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPosition;
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        startPosition = transform.position;
    }
    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canbeUsed == false)
            return;

        statsToModify = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffCo(buffDuration));
    }
    private IEnumerator BuffCo(float duration)
    {
        canbeUsed = false;
        sr.color = Color.clear;
        foreach (var buff in buffs)
            statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
        yield return new WaitForSeconds(duration);
        foreach (var buff in buffs)
            statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        Destroy(gameObject);
    }

    private void ApplyBuff(bool aply)
    {
        foreach (var buff in buffs)
        {
            if (aply)
                statsToModify.GetStatByType(buff.type).AddModifier(buff.value, buffName);
            else
                statsToModify.GetStatByType(buff.type).RemoveModifier(buffName);
        }
    }
}
