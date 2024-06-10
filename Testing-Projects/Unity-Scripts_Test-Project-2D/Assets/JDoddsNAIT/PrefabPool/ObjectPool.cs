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
    public Func<TObject, bool> IsActive { get; set; }

    public Action<TObject> Activate { get; set; }
    public Action<TObject> Deactivate { get; set; }

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
    /// Creates an object pool of length <paramref name="size"/>, then will <paramref name="initialize"/> each <see cref="object"/>.
    /// </summary>
    /// <param name="initialize">The action performed to initialize the <see cref="object"/>.</param>
    /// <param name="activeCriteria">Defines the criteria for an <see cref="object"/> in the pool to be considered active or in use.</param>
    public ObjectPool(int size, Func<TObject, TObject> initialize, Func<TObject, bool> activeCriteria)
    {
        Pool = new TObject[size];
        IsActive = activeCriteria;
        for (int i = 0; i < size; i++)
        {
            Pool[i] = initialize(Pool[i]);
        }
    }

    /// <summary>
    /// Creates an object pool of length <paramref name="size"/>, then will <paramref name="initialize"/> each <see cref="object"/>.
    /// </summary>
    /// <param name="initialize">The action performed to initialize the <see cref="object"/>, with the <see cref="int"/> parameter being the <see cref="object"/>'s index.</param>
    /// <param name="activeCriteria">Defines the criteria for an <see cref="object"/> in the pool to be considered active or in use.</param>
    public ObjectPool(int size, Func<TObject, int, TObject> initialize, Func<TObject, bool> activeCriteria)
    {
        Pool = new TObject[size];
        IsActive = activeCriteria;
        for (int i = 0; i < size; i++)
        {
            Pool[i] = initialize(Pool[i], i);
        }
    }

    /// <summary>
    /// A variation of <see cref="NextActive"/> that returns the next active object without allocating new memory.
    /// </summary>
    public TObject GetNextActive()
    {
        TObject nextActive = default;
        bool found = false;
        for (int i = 0; i < Pool.Length & !found; i++)
        {
            if (IsActive(Pool[i]))
            {
                found = true;
                nextActive = Pool[i];
            }
        }
        return nextActive;
    }

    /// <summary>
    /// A variation of <see cref="NextInactive"/> that returns the next inactive object without allocating new memory.
    /// </summary>
    public TObject GetNextInactive()
    {
        TObject nextInactive = default;
        bool found = false;
        for (int i = 0; i < Pool.Length & !found; i++)
        {
            if (!IsActive(Pool[i]))
            {
                found = true;
                nextInactive = Pool[i];
            }
        }
        return nextInactive;
    }

    public void ActivateObject(TObject obj) => Activate(obj);
    public void ActivateObject(TObject obj, Action<TObject> activate) => activate(obj);

    public void DeactivateObject(TObject obj) => Deactivate(obj);
    public void DeactivateObject(TObject obj, Action<TObject> deactivate) => deactivate(obj);
}