using System;

public class TypoEventArgs : EventArgs
{
    public float Time { get; set; }

    public TypoEventArgs(float time)
    {
        Time = time;
    }
}
