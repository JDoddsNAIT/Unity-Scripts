using System;
using System.Collections.Generic;

public class Quadtree<T>
{
    private const int MAX_OBJECTS = 4;
    private const int MAX_LEVELS = 10;

    public int Level { get; private set; }
    public List<T> Objects { get; private set; }
    public Rectangle Bounds { get; private set; }
    public Quadtree<T>[] Nodes { get; private set; }

    private readonly Func<T, Rectangle> _accessObjectBounds;

    public Quadtree(int level, Rectangle bounds, Func<T, Rectangle> accessObjectBounds)
    {
        Level = level;
        Bounds = bounds;
        Nodes = new Quadtree<T>[4];
        _accessObjectBounds = accessObjectBounds;
    }

    public void Clear()
    {
        Objects.Clear();

        for (int i = 0; i < Nodes.Length; i++)
        {
            if (Nodes[i] != null)
            {
                Nodes[i].Clear();
                Nodes[i] = null;
            }
        }
    }

    private void Split()
    {
        int subWidth = Bounds.Width / 2;
        int subHeight = Bounds.Height / 2;
        int x = Bounds.X;
        int y = Bounds.Y;

        Nodes[0] = new Quadtree<T>(Level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight), _accessObjectBounds);
        Nodes[1] = new Quadtree<T>(Level + 1, new Rectangle(x, y, subWidth, subHeight), _accessObjectBounds);
        Nodes[2] = new Quadtree<T>(Level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight), _accessObjectBounds);
        Nodes[3] = new Quadtree<T>(Level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight), _accessObjectBounds);
    }

    private int GetIndex(Rectangle bounds)
    {
        int index = -1;
        double verticalMidpoint = Bounds.X + Bounds.Width / 2;
        double horizontalMidpoint = Bounds.Y + Bounds.Height / 2;

        bool topQuad = bounds.Y < horizontalMidpoint && bounds.Y + Bounds.Height < horizontalMidpoint;
        bool bottomQuad = bounds.Y > horizontalMidpoint;

        if (bounds.X < verticalMidpoint && bounds.X + bounds.Width < verticalMidpoint)
        {
            if (topQuad)
            {
                index = 1;
            }
            else if (bottomQuad)
            {
                index = 2;
            }
        }
        else if (bounds.X > verticalMidpoint)
        {
            if (topQuad)
            {
                index = 0;
            }
            else if (bottomQuad)
            {
                index = 3;
            }
        }
        return index;
    }

    public void Insert(T item)
    {
        if (Nodes[0] != null)
        {
            int index = GetIndex(_accessObjectBounds(item));
            if (index != -1)
            {
                Nodes[index].Insert(item);
            }
        }
        Objects.Add(item);

        if (Objects.Count > MAX_OBJECTS && Level < MAX_LEVELS && Nodes[0] == null)
        {
            Split();
            int i = 0;
            while (i < Objects.Count)
            {
                int index = GetIndex(_accessObjectBounds(Objects[i]));
                if (index != -1)
                {
                    Nodes[index].Insert(item);
                    Objects.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }

    public List<T> Retrieve(List<T> returnObjects, T item)
    {
        int index = GetIndex(_accessObjectBounds(item));
        if (index != -1 && Nodes[0] != null)
        {
            Nodes[index].Retrieve(returnObjects, item);
        }
        foreach (T obj in Objects)
        {
            returnObjects.Add(obj);
        }
        return returnObjects;
    }
}

public struct Rectangle
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle(int x, int y, int width, int height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
}