public class CrosswordSquere
{
    public char Letter { get; set; }
    public bool HorizontalRestricted { get; set; } = false;
    public bool VerticalRestricted { get; set; } = false;

    public bool Restricted
    {
        get
        {
            return HorizontalRestricted && VerticalRestricted;
        }
        set
        {
            HorizontalRestricted = value; 
            VerticalRestricted = value;
        }
    }

    public CrosswordSquere(char letter, bool restricted = false)
    {
        Letter = letter;
        Restricted = restricted;
    }

    public CrosswordSquere(bool restricted = false)
    {
        Restricted = restricted;
    }
}
