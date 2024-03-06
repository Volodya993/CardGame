namespace CardLib
{
    public enum DeckFlags
    {   
        None = 0,
        BigSuit = 0x04,       
        Small = 0x13,
        Medium = 0x23,     
        Large = 0x33,      
        AceHigh =  0x40,
        UseTrump = 0x80,
        TrumpSuit = 0xFF
    }
}