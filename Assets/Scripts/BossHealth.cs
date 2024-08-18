using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 250;
    public int currentHealth;

    public Slider healthSlider;

    internal bool isBossAlive;

    StartManager startManager;

    void Start()
    {
        isBossAlive = true;
        healthSlider.interactable = false;
        startManager = GameObject.Find("StartManager").GetComponent<StartManager>();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    internal void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (startManager.infiniteMode == true)
        {
            return;
        }
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isBossAlive = false;
        gameObject.SetActive(false);
    }
}
