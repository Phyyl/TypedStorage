namespace TypedStorage;

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

    public T? Get<T>()
        where T : class
    {
        return InternalTypedStorage<T>.Get(id);
    }

    public void Set<T>(T value)
        where T : class
    {
        InternalTypedStorage<T>.Set(id, value);
    }
}
