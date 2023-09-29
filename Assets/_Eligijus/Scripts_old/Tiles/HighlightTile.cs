using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HighlightTile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ColorGridTile;
    [SerializeField] private SpriteRenderer FogOfWarTile;
    [SerializeField] private SpriteRenderer ArrowTile;
    [SerializeField] private SpriteRenderer DangerUI;
    [SerializeField] private SpriteRenderer PlayerSelect;
    [SerializeField] private SpriteRenderer Preview;
    
    [SerializeField] private Sprite rightStartArrow;
    [SerializeField] private Sprite leftStartArrow;
    [SerializeField] private Sprite downStartArrow;
    [SerializeField] private Sprite upStartArrow;

    [SerializeField] private Sprite rightEndArrow;
    [SerializeField] private Sprite leftEndArrow;
    [SerializeField] private Sprite downEndArrow;
    [SerializeField] private Sprite upEndArrow;

    [SerializeField] private Sprite verticalIntermediateArrow;
    [SerializeField] private Sprite horizontalIntermediateArrow;

    [SerializeField] private Sprite topLeftCornerArrow;
    [SerializeField] private Sprite bottomLeftCornerArrow;
    [SerializeField] private Sprite topRightCornerArrow;
    [SerializeField] private Sprite bottomRightCornerArrow;

    [SerializeField] private SpriteRenderer skullSprite;
    [SerializeField] private MeshRenderer damageTextRenderer;
    [SerializeField] private TextMeshPro damageTextMeshPro;
    
    [HideInInspector] public bool fogOfWar = true;
    public bool isHighlighted = false;
    public string activeState;
    public void ActivateDeathSkull(bool value)
    {
        skullSprite.enabled = value;
    }
    public void SetDamageText(string text)
    {
        damageTextRenderer.enabled = true;
        damageTextMeshPro.text = text;
    }
    public void DisableDamageText()
    {
        damageTextRenderer.enabled = false;
    }
    public void ActivateColorGridTile(bool value)
    {
        ColorGridTile.enabled = value;
        isHighlighted = value;
    }
    public void SetPreviewSprite(Sprite sprite)
    {
        Preview.sprite = sprite;
    }
    public void TogglePreviewSprite(bool value)
    {
        Preview.enabled = value;
    }
    public void SetArrowSprite(int arrowType)
    {
        Sprite arrowSprite = null;

        switch (arrowType)
        {
            case 1:
                arrowSprite = rightStartArrow;
                break;
            case 2:
                arrowSprite = leftStartArrow;
                break;
            case 3:
                arrowSprite = downStartArrow;
                break;
            case 4:
                arrowSprite = upStartArrow;
                break;
            case 5:
                arrowSprite = rightEndArrow;
                break;
            case 6:
                arrowSprite = leftEndArrow;
                break;
            case 7:
                arrowSprite = downEndArrow;
                break;
            case 8:
                arrowSprite = upEndArrow;
                break;
            case 9:
                arrowSprite = verticalIntermediateArrow;
                break;
            case 10:
                arrowSprite = horizontalIntermediateArrow;
                break;
            case 11:
                arrowSprite = topLeftCornerArrow;
                break;
            case 12:
                arrowSprite = topRightCornerArrow;
                break;
            case 13:
                arrowSprite = bottomLeftCornerArrow;
                break;
            case 14:
                arrowSprite = bottomRightCornerArrow;
                break;
            default:
                return;
        }
        ArrowTile.sprite = arrowSprite;
        ArrowTile.enabled = true;
    }
    public void DeactivateArrowTile()
    {
        ArrowTile.enabled = false;
    }
    public void ActivatePlayerTile(bool value)
    {
        PlayerSelect.enabled = value;
    }
    public void SetHighlightColor(Color color)
    {
        ColorGridTile.color = color;
    }
    public void EnableDisableFogOfWar(bool value)
    {
        FogOfWarTile.enabled = value;
    }
    public bool FogOfWarIsEnabled()
    {
        return FogOfWarTile.enabled;
    }
    bool IsSkillAvailableInFOW()
    {
        return (!FogOfWarTile.enabled || activeState == "CreateEye" || activeState == "CreatePortal");
    }
}
