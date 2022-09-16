using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Slider slider;
    private int maxHealth;

    public void SetMaxHealth(int _maxHealth)
    {
        maxHealth = _maxHealth;
        slider.maxValue = _maxHealth;
        slider.value = _maxHealth;
    }

    public void UpdateHealthUI(int _newHealth)
    {
        slider.value = _newHealth;
    }
}
