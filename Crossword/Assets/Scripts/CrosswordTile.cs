using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CrosswordTile : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI TextField;
    [HideInInspector] public Vector2 Size => new Vector2(RectTransform.rect.width, RectTransform.rect.height);
    
    
    [HideInInspector] public RectTransform RectTransform;
    [HideInInspector] public IntPoint PosOnGrid;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        RectTransform = GetComponent<RectTransform>();
    }

    public void ActiveText(bool isActive)
    {
        TextField.enabled = isActive;
    }
}
