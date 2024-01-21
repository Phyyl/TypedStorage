namespace Typed.Storage;

internal static class InternalTypedStorage
{
    private static int size = 0;

    internal static int Size => size;

    internal static event Action<int>? OnRelease;

    internal static int Reserve()
    {
        return Interlocked.Increment(ref size);
    }

    internal static void Release(int id)
    {
        //TODO: Add generations to reuse storage instead of infinitely resizing
        OnRelease?.Invoke(id);
    }
}

internal static class InternalTypedStorage<T>
    where T : class
{
    static InternalTypedStorage()
    {
        InternalTypedStorage.OnRelease += Release;
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

    private static void Release(int id)
    {
        if (values.Length > id)
        {
            values[id] = null;
        }
    }
}
