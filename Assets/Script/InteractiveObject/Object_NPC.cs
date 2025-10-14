using UnityEngine;

public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;
    [SerializeField] private Transform npc;
    [SerializeField] private GameObject interactTooltip;
    private bool isFacingRight = true;

    [Header("Tooltip Float")]
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPosition;

    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPosition = interactTooltip.transform.position;
        interactTooltip.SetActive(false);
    }

    protected virtual void Update()
    {
        HanleNpcFlip();
        HandleTooltipFloat();
    }

    private void HandleTooltipFloat()
    {
        if(interactTooltip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactTooltip.transform.position = startPosition + new Vector3(0f, yOffset);
        }
        
    }

    private void HanleNpcFlip()
    {
        if (player == null || npc == null) return;
        
        if(npc.position.x > player.position.x && isFacingRight)
        {
            npc.transform.Rotate(0f, 180f, 0f);
            isFacingRight = false;
        }
        else if(npc.position.x < player.position.x && !isFacingRight)
        {
            npc.transform.Rotate(0f, 180f, 0f);
            isFacingRight = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        player = other.transform;
        interactTooltip.SetActive(true);
    }
    
    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        interactTooltip.SetActive(false);
    }
}
