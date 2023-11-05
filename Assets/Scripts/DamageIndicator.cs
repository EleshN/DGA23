using System.Collections;
using UnityEngine;

// This class is responsible for showing damage indication
public class DamageIndicator : MonoBehaviour
{
  public Material damageMaterial; // Assign this in the inspector
  private Material originalMaterial;
  private Renderer objectRenderer;

  protected virtual void Start()
  {
    objectRenderer = GetComponent<Renderer>();
    originalMaterial = objectRenderer.material;
  }

  public void IndicateDamage()
  {
    StartCoroutine(ChangeMaterialTemporarily());
  }

  private IEnumerator ChangeMaterialTemporarily()
  {
    objectRenderer.material = damageMaterial;
    yield return new WaitForSeconds(0.150f);
    objectRenderer.material = originalMaterial;
  }
}