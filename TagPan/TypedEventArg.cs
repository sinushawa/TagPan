using System;

/// <summary>
/// Encapsulates a single object as a parameter for simple event argument passing
/// <remarks>Now you can declare a simple event args derived class in one wrap</remarks>
/// <code>public void delegate MyCustomeEventHandler(TypedEventArg&lt;MyObject&gt; theSingleObjectParameter)</code>
/// </summary>
public class TypedEventArg<T> : EventArgs
{
    private T _Value;

    public TypedEventArg(T value)
    {
        _Value = value;
    }

    public T Value
    {
        get { return _Value; }
        set { _Value = value; }
    }
}
