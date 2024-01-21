namespace Typed.Storage;

internal static class InternalTypedStorage
{
    private static int nextID = 0;
    private static readonly Queue<int> freedSlots = new();

    internal static event Action<int>? OnClear;

    internal static int Reserve()
    {
        if (freedSlots.TryDequeue(out int slot))
        {
            return slot;
        }

        return Interlocked.Increment(ref nextID);
    }

    internal static void Release(int id)
    {
        Clear(id);
        freedSlots.Enqueue(id);
    }

    internal static void Clear(int id)
    {
        OnClear?.Invoke(id);
    }
}

internal static class InternalTypedStorage<T>
{
    static InternalTypedStorage()
    {
        InternalTypedStorage.OnClear += static id =>
        {
            if (values.Length > id)
            {
                values[id] = default;
            }
        };
    }

    private static T?[] values = [];

    internal static T? Get(int id)
    {
        return values.ElementAtOrDefault(id);
    }

    internal static void Set(int id, T? value)
    {
        EnsureCapacity(id);
        values[id] = value;
    }

    private static void EnsureCapacity(int id)
    {
        if (values.Length <= id)
        {
            Grow();
        }
    }

    private static void Grow()
    {
        int newLength = values.Length == 0 ? 1 : values.Length * 2;
        Array.Resize(ref values, newLength);
    }
}
