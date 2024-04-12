namespace Typed.Storage;

internal sealed class ObjectVisitor(Action<object> visit) : IVisitor
{
    public void Visit<T>(T value)
    {
        if (value is T t)
        {
            visit.Invoke(t);
        }
    }
}