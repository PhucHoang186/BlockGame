using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Image healthImg;
    [SerializeField] Transform healthContainer;
    private List<Image> healthList = new List<Image>();

    void Awake()
    {
        GameEvents.ON_HEALTH_CHANGED += UpdateHealthUI;
    }

    void OnDestroy()
    {
        GameEvents.ON_HEALTH_CHANGED -= UpdateHealthUI;
    }

    public void UpdateHealthUI(int _newHealth)
    {
        var currentHealth = _newHealth - healthList.Count;
        if (currentHealth > 0)
        {
            for (int i = 0; i < currentHealth; i++)
            {
                healthList.Add(Instantiate(healthImg, healthContainer));
            }
        }
        else if (currentHealth < 0)
        {
            for (int i = 0; i < -currentHealth; i++)
            {
                Destroy(healthList.LastOrDefault().gameObject);
                if (healthList.Count > 0)
                {
                    healthList.RemoveAt(healthList.Count - 1);
                }
            }
        }
    }
}
