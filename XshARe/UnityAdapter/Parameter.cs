using System;
public class Parameter
{
    private int v;


    public Parameter(int value)
    {
        this.v = value;
    }

    public int Value {
        get
        {
            return v;
        }
        set
        {
            v = value;
        }
    }
}
