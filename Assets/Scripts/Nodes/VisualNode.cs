using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VisualNodeType
{
    Highlight,
    Hover,
    Attack,
    All,
}
public class VisualNode : MonoBehaviour
{

    [SerializeField] SpriteRenderer highlightSprite;
    [SerializeField] SpriteRenderer hoverSprite;
    [SerializeField] SpriteRenderer attackSprite;

    public void ToggleNodeVisual(bool _isActive, VisualNodeType _type)
    {
        switch (_type)
        {
            case VisualNodeType.Highlight:
                highlightSprite.gameObject.SetActive(_isActive);
                break;
            case VisualNodeType.Hover:
                hoverSprite.gameObject.SetActive(_isActive);
                break;
            case VisualNodeType.Attack:
                attackSprite.gameObject.SetActive(_isActive);
                break;
            case VisualNodeType.All:
                highlightSprite.gameObject.SetActive(_isActive);
                hoverSprite.gameObject.SetActive(_isActive);
                attackSprite.gameObject.SetActive(_isActive);
                break;
            default:
                break;
        }
    }

    void OnMouseEnter()
    {
        ToggleNodeVisual(true, VisualNodeType.Hover);
    }

    void OnMouseExit()
    {
        ToggleNodeVisual(false, VisualNodeType.Hover);
    }

}
