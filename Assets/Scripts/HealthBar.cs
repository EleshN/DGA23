using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    public Image healthBarFill;
    float maxHealth;

    /// <summary>
    /// Set the max health and update healthbar
    /// </summary>
    public void SetHealthBar(float maxHealth)
    {
        this.maxHealth = maxHealth;
        UpdateHealthBar(maxHealth);
    }

    public void UpdateHealthBar(float health)
    {
        healthBarFill.fillAmount = health / maxHealth;
    }

}
