using System.Collections.Generic;
using UnityEngine;

public static class SnapRegistry
{
    private static readonly List<SnapPoint> points = new List<SnapPoint>();

    public static IReadOnlyList<SnapPoint> Points => points;

    public static void Register(SnapPoint p)
    {
        if (p != null && !points.Contains(p)) points.Add(p);
    }

    public static void Unregister(SnapPoint p)
    {
        if (p != null) points.Remove(p);
    }
}