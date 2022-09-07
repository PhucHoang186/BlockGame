using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthDisplay : MonoBehaviour
{
    [SerializeField] Image healthImg;
    [SerializeField] Transform healthContainer;
    private List<Image> healthList;

    void Awake()
    {
        healthList = new List<Image>();
        GameEvents.ON_HEALTH_CHANGED += UpdateHealthUI;
    }

    void OnDestroy()
    {
        GameEvents.ON_HEALTH_CHANGED -= UpdateHealthUI;
    }

    public void UpdateHealthUI(int _amount)
    {
        // var currentHealth = healthList.Count - _amount;
        var currentHealth =  _amount - healthList.Count;
        Debug.LogError("amount " + _amount);
        Debug.LogError("healthList " + healthList.Count);
        Debug.LogError("currentHealth " + currentHealth);
        if (currentHealth > 0)
        {
            for (int i = 0; i < currentHealth; i++)
            {
                healthList.Add(Instantiate(healthImg, healthContainer));
            }
        }
        else if(currentHealth < 0)
        {
            currentHealth = Mathf.Abs(currentHealth);
            for (int i = 0; i < currentHealth; i++)
            {
                Destroy(healthList.LastOrDefault());
            }
        }
    }
}
