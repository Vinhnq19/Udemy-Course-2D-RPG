using System.Collections;
using UnityEngine;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [Header("On Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float onDamageVFXDuration = 0.2f;
    private Material originalMaterial;
    private Coroutine onDamageVFXCoroutine;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = sr.material;
    }
    public void PlayOnDamageVFX()
    {
        if (onDamageVFXCoroutine != null)
            StopCoroutine(onDamageVFXCoroutine);
        StartCoroutine(OnDamageVFXCo());
    }
    private IEnumerator OnDamageVFXCo()
    {
        sr.material = onDamageMaterial;
        yield return new WaitForSeconds(onDamageVFXDuration);
        sr.material = originalMaterial;
    }
}
