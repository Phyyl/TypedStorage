namespace Typed.Storage;

internal static class InternalTypedStorage<T>
    where T : notnull
{
    static InternalTypedStorage()
    {
        TypedStorage.OnClear += static id =>
        {
            if (values.Length > id)
            {
                values[id] = default;
            }
        };

        TypedStorage.OnQuery += static (id, visitor) =>
        {
            if (values.Length > id && values[id] is T t)
            {
                visitor.Visit(t);
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
