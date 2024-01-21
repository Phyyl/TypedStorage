namespace Typed.Storage;

public class TypedStorage : IDisposable
{
    private readonly int id;

    public TypedStorage()
    {
        id = InternalTypedStorage.Reserve();
    }

    public void Dispose()
    {
        InternalTypedStorage.Release(id);
    }

    public void Clear()
    {
        InternalTypedStorage.Clear(id);
    }

    public T? Get<T>()
    {
        return InternalTypedStorage<T>.Get(id);
    }

    public void Set<T>(T value)
    {
        InternalTypedStorage<T>.Set(id, value);
    }
}
