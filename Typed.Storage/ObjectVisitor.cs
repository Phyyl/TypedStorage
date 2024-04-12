namespace Typed.Storage;

internal sealed class ObjectVisitor(Action<object> visit) : IVisitor
{
    public void Visit<T>(T value)
        where T : notnull
    {
        visit.Invoke(value);
    }
}