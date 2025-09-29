using UnityEngine;

public class Chest : MonoBehaviour, IDamagable // Make sure to implement the IDamagable interface
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator animator => GetComponentInChildren<Animator>();
    [SerializeField] private Entity_VFX vFX => GetComponent<Entity_VFX>();
    public bool TakeDamage(float damage, float elementalDamage, ElementType elementType, Transform damageDealer)
    {
        vFX.PlayOnDamageVFX();
        animator.SetBool("chestOpen", true);
        rb.linearVelocity = new Vector2(0, 3f);
        rb.angularVelocity = Random.Range(-200f, 200f);
        return true;
    }
}
