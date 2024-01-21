namespace Typed.Storage;

public class TypedStorage
{
    private static int nextID = 0;
    private static readonly Queue<int> freedIDs = new();
    internal static event Action<int>? OnClear;

    private readonly int id;

    public TypedStorage()
    {
        if (freedIDs.TryDequeue(out int id))
        {
            this.id = id;
        }
        else
        {
            this.id = Interlocked.Increment(ref nextID);
        }
    }

    ~TypedStorage()
    {
        Clear(id);
        freedIDs.Enqueue(id);
    }

    public void Clear()
    {
        Clear(id);
    }

    public T? Get<T>()
    {
        return InternalTypedStorage<T>.Get(id);
    }

    public void Set<T>(T value)
    {
        InternalTypedStorage<T>.Set(id, value);
    }

    internal static void Clear(int id)
    {
        OnClear?.Invoke(id);
    }
}
