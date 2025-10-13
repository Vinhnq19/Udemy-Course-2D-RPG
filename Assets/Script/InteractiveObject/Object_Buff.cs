using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class Object_Buff : MonoBehaviour
{
    private Player_Stats statsToModify;
    [Header("Buff Settings")]
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private string buffName = "Buff Name";
    [SerializeField] private float buffDuration = 4f;
    [SerializeField] private bool canbeUsed = true;
    [Header("floaty movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPosition;
    private void Awake()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        statsToModify = collision.GetComponent<Player_Stats>();

        if(statsToModify.CanApplyBuffOf(buffName))
        {
            statsToModify.ApplyBuff(buffs, buffDuration, buffName);
            Destroy(gameObject);
        }
    }

}
