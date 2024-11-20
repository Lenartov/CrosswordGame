using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LettersCircle : MonoBehaviour
{
    [SerializeField] private LetterButton letterButtonPrefab;

    [SerializeField] private TextMeshProUGUI textField;

    private RectTransform rectTransform;
    private LineRenderer lineRenderer;
    private LetterButton[] letterButtons;

    private bool isSelectionStarted;
    private HashSet<LetterButton> guessWord = new HashSet<LetterButton>();

    public event Action OnSelectionStart;
    public event Action<string> OnWordSelected;

    private Camera mainCamera;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;

        mainCamera = Camera.main;
        textField.text = "";
    }

    private void Update()
    {
        if (isSelectionStarted)
        {
            Vector2 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
        }

        if(Input.GetMouseButtonUp(0))
        {
            WordSelectionEnd();
        }
    }

    public void InstantiateLetters(char[] letters)
    {
        letterButtons = new LetterButton[letters.Length];

        float margin = 20f;
        float radius = CalcRadius() - margin;

        Vector3[] poses = Utility.GetCirclePositions(rectTransform.position, letters.Length, radius);
        for (int i = 0; i < letters.Length; i++)
        {
            LetterButton letterButton = Instantiate(letterButtonPrefab, Vector3.zero, Quaternion.identity, rectTransform);
            letterButton.RectTransform.anchoredPosition = poses[i];
            letterButton.SetLetter(letters[i]);

            letterButton.OnClick += WordSelectionStart;
            letterButton.OnHover += LetterAdd;
            OnSelectionStart += () => { letterButton.IsSelectionStarted = true; };
            OnWordSelected += (word) => { letterButton.IsSelectionStarted = false; letterButton.DeselectButton(); };

            letterButtons[i] = letterButton;
        }
    }

    private float CalcRadius()
    {
        if(rectTransform.rect.width > rectTransform.rect.height) 
        {
            return rectTransform.rect.height * 0.5f;
        }
        return rectTransform.rect.width * 0.5f;
    }

    private void WordSelectionStart(LetterButton letterButton)
    {
        isSelectionStarted = true;
        guessWord.Clear();
        LetterAdd(letterButton);
        OnSelectionStart?.Invoke();
    }

    private void LetterAdd(LetterButton letterButton)
    {
        textField.text += letterButton.Letter;
        letterButton.IsAdded = true;
        guessWord.Add(letterButton);

        lineRenderer.positionCount = guessWord.Count + 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 2, letterButton.transform.position);
    }

    private void WordSelectionEnd()
    {
        textField.text = "";
        isSelectionStarted = false;
        string word = "";
        foreach (LetterButton letter in guessWord)
        {
            word += letter.Letter;
            letter.IsAdded = false;
        }
        OnWordSelected?.Invoke(word);

        guessWord.Clear();
        lineRenderer.positionCount = 0;
    }
}
