namespace Typed.Storage;

public interface IVisitor
{
    void Visit<T>(T value) where T : notnull;
}
