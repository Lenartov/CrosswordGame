using System;
using System.Collections.Generic;
using UnityEngine;

public class Crossword : MonoBehaviour
{
    [SerializeField] private CrosswordTile crosswordTilePrefab;
    [SerializeField] private float margine = 130f;
    [SerializeField] private float outlineOffset = 2.5f;


    public event Action OnAllWordsFinded;

    private RectTransform rectTransform;

    private int initialSpawnPoint;
    private CrosswordSquere[,] grid;
    private List<CorrectWord> wordsInUse;
    private List<CrosswordTile> crosswordTiles;

    private int findedWordCount = 0;

    private void Awake()
    {
        crosswordTiles = new List<CrosswordTile>();
        wordsInUse = new List<CorrectWord>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void Instantiate(string[] words)
    {
        string[] sortedWords = Utility.SortByLengthDesc(words);
        List<CorrectWord> correctWords = new List<CorrectWord>();

        initialSpawnPoint = sortedWords[0].Length;

        for (int i = 0; i < words.Length; i++)
        {
            correctWords.Add(new CorrectWord(words[i], false, false, false, new IntPoint(-1, -1)));
        }

        int gridSize = CalcOptimalGridSize(correctWords);
        grid = new CrosswordSquere[gridSize, gridSize];

        CreateField(correctWords);
        OprimizeFieldSize();
        DebugGrid();
        ShowVisual();
    }

    public void GuessCorrectWord(string word)
    {
        foreach(CorrectWord correctWord in wordsInUse)
        {
            if (correctWord.Finded)
                continue;

            if(word.ToLower().Contains(correctWord.Word.ToLower()))
            {
                findedWordCount++;
                correctWord.Finded = true;
                ShowFindedWord(correctWord);
                break;
            }
        }
        if (findedWordCount >= wordsInUse.Count)
            OnAllWordsFinded?.Invoke();

    }

    public void ShowFindedWord(CorrectWord correctWord)
    {
        IntPoint wordIndexRange;

        if (correctWord.IsHorizontal)
            wordIndexRange = new IntPoint(correctWord.PosOnGrid.X, correctWord.PosOnGrid.X + correctWord.Word.Length);
        else
            wordIndexRange = new IntPoint(correctWord.PosOnGrid.Y, correctWord.PosOnGrid.Y + correctWord.Word.Length);


        IntPoint posOnGrid = new IntPoint(correctWord.PosOnGrid.X, correctWord.PosOnGrid.Y);
        foreach(CrosswordTile tile in crosswordTiles)
        {
            if (correctWord.IsHorizontal)
            {
                if(tile.PosOnGrid.Y == correctWord.PosOnGrid.Y)
                {
                    if(wordIndexRange.X <= tile.PosOnGrid.X && tile.PosOnGrid.X < wordIndexRange.Y)
                    {
                        tile.ActiveText(true);
                    }
                }
            }
            else
            {
                if (tile.PosOnGrid.X == correctWord.PosOnGrid.X)
                {
                    if (wordIndexRange.X <= tile.PosOnGrid.Y && tile.PosOnGrid.Y < wordIndexRange.Y)
                    {
                        tile.ActiveText(true);
                    }
                }
            }

        }
    }

    private void ShowVisual()
    {
        Vector2 tileSize2D = Utility.GetTileSize(new Vector2(rectTransform.rect.width - margine, rectTransform.rect.height - margine), new IntPoint(grid.GetLength(0), grid.GetLength(1)));

        float tileSize;
        float centralizOffset;
        bool isHor;

        if (tileSize2D.x > tileSize2D.y)
        {
            tileSize = tileSize2D.y;
            isHor = false;
            centralizOffset = -(tileSize2D.y - tileSize2D.x) * grid.GetLength(0) * 0.5f;
        }
        else
        {
            tileSize = tileSize2D.x;
            isHor = true;
            centralizOffset = (tileSize2D.x - tileSize2D.y) * grid.GetLength(1) * 0.5f;
        }

        float startPosOffset = tileSize * 0.5f;
        float heightOffset = (rectTransform.rect.height - margine) * 0.5f;
        float widthOffset = (rectTransform.rect.width - margine) * 0.5f;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {

                if (grid[x, y] != null && grid[x, y].Letter != '\0')
                {
                    CrosswordTile tile = Instantiate(crosswordTilePrefab, Vector3.zero, Quaternion.identity, transform);
                    Vector2 tilePos = new Vector2(x * (tileSize + outlineOffset) - widthOffset + startPosOffset, y * -(tileSize + outlineOffset) + heightOffset - startPosOffset);

                    if(isHor)
                        tilePos = new Vector2(tilePos.x, tilePos.y + centralizOffset);
                    else
                        tilePos = new Vector2(tilePos.x + centralizOffset, tilePos.y);

                    tile.RectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                    tile.RectTransform.anchoredPosition = tilePos;
                    tile.TextField.text = grid[x, y].Letter.ToString();
                    tile.PosOnGrid = new IntPoint(x, y);
                    tile.ActiveText(false);
                    crosswordTiles.Add(tile);
                }
                //print enpty tile
                /*else
                {
                    CrosswordTile tile = Instantiate(crosswordTilePrefab, Vector3.zero, Quaternion.identity, transform);
                    Vector2 tilePos = new Vector2(x * (tileSize + tileSizeOffset) - widthOffset + startPosOffset, y * -(tileSize + tileSizeOffset) + heightOffset - startPosOffset);

                    if(isHor)
                        tilePos = new Vector2(tilePos.x, tilePos.y + centralizOffset);
                    else
                        tilePos = new Vector2(tilePos.x + centralizOffset, tilePos.y);

                    tile.RectTransform.sizeDelta = new Vector2(tileSize, tileSize);
                    tile.RectTransform.anchoredPosition = tilePos;
                    tile.PosOnGrid = new IntPoint(x, y);
                    tile.TextField.text = "";
                    tile.ActiveText(false);
                    crosswordTiles.Add(tile);
                }*/
            }
        }
    }

    private void DebugGrid()
    {
        string str = "";
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            str += "[ ";
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] != null && grid[x, y].Letter != '\0')
                {
                    str += "" + grid[x, y].Letter + "";
                }
                else if (grid[x, y] != null && grid[x, y].Letter == '\0' && grid[x, y].Restricted)
                    str += "+";
                else
                    str += "*";

            }
            str += "]\n";
        }
        Debug.Log(str);

    }

    private void CreateField(List<CorrectWord> correctWords)
    {
        if (correctWords.Count < 0)
            return;

        List<CorrectWord> notPlacedWords = new List<CorrectWord>();
        notPlacedWords.AddRange(correctWords);


        CorrectWord word = notPlacedWords[0];
        InsertWord(new IntPoint(initialSpawnPoint, initialSpawnPoint), true, word);
        notPlacedWords.RemoveAt(0);

        bool isHorizontal = false;
        int wordPlacedCount = 1;
        int currentWordIndex = 0;
        while (currentWordIndex < notPlacedWords.Count)
        {
            word = notPlacedWords[currentWordIndex];

            IntPoint index = TryFindPlaceForWord(word, isHorizontal);
            if (index == null) 
            {
                isHorizontal = !isHorizontal;
                index = TryFindPlaceForWord(word, isHorizontal);
                if (index == null)
                {
                    currentWordIndex++;
                    continue;
                }
            }

            InsertWord(index, isHorizontal, word);

            notPlacedWords.RemoveAt(currentWordIndex);

            wordPlacedCount++;
            isHorizontal = !isHorizontal;
            currentWordIndex = 0;
        }
        if (notPlacedWords.Count > 0)
        {
            string warningMassage = "Not all words were used: ";
            foreach(var w in notPlacedWords)
            {
                warningMassage += w.Word + ", ";
            }
            Debug.LogWarning(warningMassage);
        }
    }

    private void OprimizeFieldSize()
    {
        int topIndex = 999999;
        int bottomIndex = 0;
        int leftIndex = 999999;
        int rightIndex = 0;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                if (grid[x, y] != null && grid[x, y].Letter != '\0')
                {
                    if (y < topIndex)
                        topIndex = y;

                    if (y > bottomIndex)
                        bottomIndex = y;

                    if (x < leftIndex)
                        leftIndex = x;

                    if (x > rightIndex)
                        rightIndex = x;
                }
            }
        }

        CrosswordSquere[,] tempGrid = new CrosswordSquere[rightIndex - leftIndex + 1, bottomIndex - topIndex + 1];
        for (int x = leftIndex, xTemp = 0; x < rightIndex + 1; x++, xTemp++)
        {
            for (int y = topIndex, yTemp = 0; y < bottomIndex + 1; y++, yTemp++)
            {
                tempGrid[xTemp, yTemp] = grid[x, y];
            }
        }
        int lengthDiffX = grid.GetLength(0) - tempGrid.GetLength(0);
        int lengthDiffY = grid.GetLength(1) - tempGrid.GetLength(1);
        grid = tempGrid;

        ////////
        foreach (CorrectWord word in wordsInUse)
        {
            word.PosOnGrid.X = word.PosOnGrid.X - leftIndex;
            word.PosOnGrid.Y = word.PosOnGrid.Y - topIndex;
        }
    }

    private IntPoint TryFindPlaceForWord(CorrectWord word, bool isHorizontal)
    {
        if (isHorizontal)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] == null)
                        continue;

                    if (grid[x, y].Restricted || grid[x, y].HorizontalRestricted)
                        continue;

                    for (int k = 0; k < word.Word.Length; k++)
                    {
                        if (grid[x, y].Letter == word.Word[k] && CheckIsWordFit(new IntPoint(x, y), k, isHorizontal, word.Word))
                            return new IntPoint(x - k, y);                            
                    }
                }
            }
            return null;
        }
        // vertical
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                if (grid[x, y] == null)
                    continue;

                if (grid[x, y].Restricted || grid[x, y].VerticalRestricted)
                    continue;

               // if(grid[x, y].Letter == '\0')
               //     continue;

                for (int k = 0; k < word.Word.Length; k++)
                {
                    if (grid[x, y].Letter == word.Word[k] && CheckIsWordFit(new IntPoint(x, y), k, isHorizontal, word.Word))
                        return new IntPoint(x, y - k);
                }
            }
        }
        return null;
    }

    private bool CheckIsWordFit(IntPoint index, int letterIntersectIndex, bool isHorizontal, string word)
    {
        int currLetterIndex = 0;
        if (isHorizontal) 
        {

            for (int x = index.X - letterIntersectIndex; x < index.X + word.Length - letterIntersectIndex; x++, currLetterIndex++)
            {
                if (grid[x, index.Y] == null)
                    continue;

                if(x + 1 >= index.X + word.Length - letterIntersectIndex)//if last
                {
                    if(x + 1 < grid.GetLength(0))
                    {
                        if(grid[x + 1, index.Y] != null)
                            return false;
                    }
                }

                if(grid[x, index.Y].Restricted || grid[x, index.Y].HorizontalRestricted)
                    return false;
                

                if (grid[x, index.Y].Letter == '\0')
                    continue;

                if (grid[x, index.Y].Letter != word[currLetterIndex])
                    return false;

            }
            return true;
        }
        //vertical
        for (int y = index.Y - letterIntersectIndex; y < index.Y + word.Length - letterIntersectIndex; y++, currLetterIndex++)
        {
            if (grid[index.X, y] == null)
                continue;

            if (y + 1 >= index.Y + word.Length - letterIntersectIndex)//if last
            {
                if (y + 1 < grid.GetLength(1))
                {
                    if (grid[index.X, y + 1] != null && grid[index.X, y + 1].Letter != '\0')
                        return false;
                }
            }

            if (grid[index.X, y].Restricted || grid[index.X, y].VerticalRestricted)
                return false;
            
            if (grid[index.X, y].Letter == '\0')
                continue;

            if (grid[index.X, y].Letter != word[currLetterIndex])
                return false;
        }
        return true;
    }

    private void InsertWord(IntPoint index, bool isHorizontalDir, CorrectWord word)
    {
        word.PosOnGrid = index;
        word.IsHorizontal = isHorizontalDir;
        word.Inserted = true;
        wordsInUse.Add(word);

        int letterIndex = 0;
        if (isHorizontalDir)
        {
            InsertGridElement(new IntPoint(index.X - 1, index.Y), Restrictions.REST);
            InsertGridElement(new IntPoint(index.X + word.Word.Length, index.Y), Restrictions.REST);

            for (int i = index.X; i < index.X + word.Word.Length; i++, letterIndex++) 
            {
                InsertGridElement(new IntPoint(i, index.Y), Restrictions.HOR, word.Word[letterIndex]);

                InsertGridElement(new IntPoint(i, index.Y - 1), Restrictions.HOR);
                InsertGridElement(new IntPoint(i, index.Y + 1), Restrictions.HOR);
            }

            return;
        }
        InsertGridElement(new IntPoint(index.X, index.Y - 1), Restrictions.REST);
        InsertGridElement(new IntPoint(index.X, index.Y + word.Word.Length), Restrictions.REST);

        for (int i = index.Y; i < index.Y + word.Word.Length; i++, letterIndex++)
        {
            InsertGridElement(new IntPoint(index.X, i), Restrictions.VERT, word.Word[letterIndex]);

            InsertGridElement(new IntPoint(index.X - 1, i), Restrictions.VERT);
            InsertGridElement(new IntPoint(index.X + 1, i), Restrictions.VERT);

        }
    }

    private void InsertGridElement(IntPoint index, Restrictions rest, char letter = '\0')
    {
        if (index.X < 0 || index.Y < 0)
            return;

        if (grid[index.X, index.Y] == null)
        {
            grid[index.X, index.Y] = new CrosswordSquere();
            grid[index.X, index.Y].Letter = letter;
        }

        if (grid[index.X, index.Y].Letter == '\0')
            grid[index.X, index.Y].Letter = letter;

        if(rest == Restrictions.HOR)
            grid[index.X, index.Y].HorizontalRestricted = true;
        if (rest == Restrictions.VERT)
            grid[index.X, index.Y].VerticalRestricted = true;
        if (rest == Restrictions.REST)
            grid[index.X, index.Y].Restricted = true;
    }

    private int CalcOptimalGridSize(List<CorrectWord> correctWords)
    {
        int arrSize = 2;

        int wordsHalfCount = Mathf.CeilToInt(correctWords.Count * 0.5f);

        for (int i = 0; i < wordsHalfCount; i++) 
        {
            arrSize += correctWords[i].Word.Length;
        }
        arrSize += initialSpawnPoint;
        return arrSize;
    }
}
