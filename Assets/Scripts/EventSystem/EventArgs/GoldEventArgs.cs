using System;

public class GoldEventArgs : EventArgs
{
    public int Gold { get; set; }

    public GoldEventArgs(int gold)
    {
        Gold = gold;
    }
}
