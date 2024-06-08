using System;
using System.Linq;

public class ObjectPool<TObject>
{
    /// <summary>
    /// The entire pool of <see cref="object"/>s.
    /// </summary>
    public TObject[] Pool { get; private set; }

    /// <summary>
    /// Defines the criteria for an <see cref="object"/> in the pool to be considered active or in use.
    /// </summary>
    public Func<TObject, bool> IsActive { get; set; } = x => x != null;

    /// <summary>
    /// All objects in the pool that are active.
    /// </summary>
    public TObject[] ActiveObjects => Pool.Where(IsActive).ToArray();
    public TObject NextActive => ActiveObjects.FirstOrDefault();

    /// <summary>
    /// All objects in the pool that are not active.
    /// </summary>
    public TObject[] InactiveObjects => Pool.Where(x => !IsActive(x)).ToArray();
    public TObject NextInactive => InactiveObjects.FirstOrDefault();

    /// <summary>
    /// Creates an <see cref="object"/> pool of length <paramref name="size"/>.
    /// </summary>
    public ObjectPool(int size) => Pool = new TObject[size];

    /// <summary>
    /// Creates an <see cref="object"/> pool of length <paramref name="size"/>, then will <paramref name="initialize"/> each <see cref="object"/>.
    /// </summary>
    /// <param name="initialize">The action performed to initialize the <see cref="object"/></param>
    public ObjectPool(int size, Action<TObject> initialize) : this(size)
    {
        for (int i = 0; i < size; i++)
        {
            initialize(Pool[i]);
        }
    }

    /// <summary>
    /// Creates an <see cref="object"/> pool of length <paramref name="size"/>, then will <paramref name="initialize"/> each <see cref="object"/>.
    /// </summary>
    /// <param name="initialize">The action performed to initialize the <see cref="object"/></param>
    public ObjectPool(int size, Action<TObject> initialize, Func<TObject, bool> activeCriteria) : this(size, initialize) => IsActive = activeCriteria;

    /// <summary>
    /// Creates an <see cref="object"/> pool of length <paramref name="size"/>, then will <paramref name="initialize"/> each <see cref="object"/>.
    /// </summary>
    /// <param name="initialize">The action performed to initialize the <see cref="object"/>, with the <see cref="int"/> parameter being the <see cref="object"/>'s index.</param>
    public ObjectPool(int size, Action<TObject, int> initialize) : this(size)
    {
        for (int i = 0; i < size; i++)
        {
            initialize(Pool[i], i);
        }
    }

    /// <summary>
    /// Creates an <see cref="object"/> pool of length <paramref name="size"/>, then will <paramref name="initialize"/> each <see cref="object"/>.
    /// </summary>
    /// <param name="initialize">The action performed to initialize the <see cref="object"/>, with the <see cref="int"/> parameter being the <see cref="object"/>'s index.</param>
    public ObjectPool(int size, Action<TObject, int> initialize, Func<TObject, bool> activeCriteria) : this(size, initialize) => IsActive = activeCriteria;

    /// <summary>
    /// <paramref name="activate"/>s the <see cref="object"/> returned by <see cref="NextInactive"/>.
    /// </summary>
    public void ActivateNext(Action<TObject> activate, out TObject obj)
    {
        obj = NextInactive != null ? NextInactive : throw new Exception("No inactive objects were found.");
        activate(obj);
    }
}
