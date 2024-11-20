public class CorrectWord
{
    public CorrectWord(string word, bool finded, bool placed, bool isHorizontal, IntPoint posOnGrid)
    {
        Word = word;
        Finded = finded;
        Inserted = placed;
        IsHorizontal = isHorizontal;
        PosOnGrid = posOnGrid;
    }

    public string Word { get; set; }
    public bool Finded { get; set; }
    public bool Inserted { get; set; }
    public bool IsHorizontal { get; set; }
    public IntPoint PosOnGrid { get; set; }
}
