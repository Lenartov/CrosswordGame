using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LetterButton : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI LetterText;

    //[HideInInspector] public int LevelNumber { get; set; }
    [HideInInspector] public char Letter { get; private set; }
    [HideInInspector] public bool IsAdded { get; set; }
    [HideInInspector] public bool IsSelectionStarted { get; set; }

    public event Action<LetterButton> OnClick;
    public event Action<LetterButton> OnHover;

    public RectTransform RectTransform;
    private Image image;
    private Outline outline;

    private Color regularColor;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        image = GetComponentInChildren<Image>();
        outline = GetComponent<Outline>();

        if (LetterText == null)
            LetterText = GetComponentInChildren<TextMeshProUGUI>();


        regularColor = image.color;
    }

    public void SetLetter(char letter)
    {
        Letter = letter;
        LetterText.text = letter.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsSelectionStarted)
            return;

        SelectButton();
        OnClick?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsSelectionStarted)
            return;

        if (IsAdded)
            return;

        SelectButton();
        OnHover?.Invoke(this);
    }

    public void SelectButton()
    {
        image.color = Color.white;
        outline.enabled = false;
    }

    public void DeselectButton()
    {
        image.color = regularColor;
        outline.enabled = true;
    }
}
