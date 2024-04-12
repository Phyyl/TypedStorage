namespace Typed.Storage;

public class TypedStorage
{
    private static int nextID = 0;
    private static readonly Queue<int> freedIDs = new();
    internal static event Action<int>? OnClear;
    internal static event Action<int, IVisitor>? OnQuery;

    private readonly int id;
    private readonly List<object> all = [];

    public TypedStorage()
    {
        if (freedIDs.TryDequeue(out int id))
        {
            this.id = id;
        }
        else
        {
            this.id = Interlocked.Increment(ref nextID) - 1;
        }
    }

    ~TypedStorage()
    {
        Clear(id);
        freedIDs.Enqueue(id);
    }

    public void Clear()
    {
        all.Clear();
        Clear(id);
    }

    public void QueryAll(IVisitor visitor)
    {
        OnQuery?.Invoke(id, visitor);
    }

    public IEnumerable<object> GetAll()
    {
        List<object> result = [];
        ObjectVisitor visitor = new(result.Add);
        QueryAll(visitor);
        return result;
    }

    public T? Get<T>()
    {
        return InternalTypedStorage<T>.Get(id);
    }

    public void Set<T>(T value)
        where T : notnull
    {
        InternalTypedStorage<T>.Set(id, value);
        all.Add(value);
    }

    internal static void Clear(int id)
    {
        OnClear?.Invoke(id);
    }
}
