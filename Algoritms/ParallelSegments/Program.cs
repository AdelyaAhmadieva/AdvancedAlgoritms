using System;
using System.Collections.Generic;

public class Segment3D
{
    public int X1, Y1, Z1, X2, Y2, Z2;
    
    public Segment3D(int x1, int y1, int z1, int x2, int y2, int z2)
    {
        X1 = Math.Min(x1, x2);
        Y1 = Math.Min(y1, y2);
        Z1 = Math.Min(z1, z2);
        X2 = Math.Max(x1, x2);
        Y2 = Math.Max(y1, y2);
        Z2 = Math.Max(z1, z2);
    }
}

public class SegmentIntersection3D
{
    public static bool AreSegmentsIntersecting(List<Segment3D> segments)
    {
        var events = new List<(int x, bool isStart, Segment3D segment)>();

        // 1. Разделяем на "события" (начало / конец отрезка)
        foreach (var seg in segments)
        {
            events.Add((seg.X1, true, seg));  // Начало
            events.Add((seg.X2, false, seg)); // Конец
        }

        // 2. Сортируем события: сначала по X, затем по началу/концу
        events.Sort((a, b) => a.x != b.x ? a.x.CompareTo(b.x) : a.isStart ? -1 : 1);

        // 3. Активные отрезки храним в отсортированном виде по Y и Z
        var activeSegments = new SortedSet<Segment3D>(Comparer<Segment3D>.Create((s1, s2) =>
        {
            if (s1.Y1 != s2.Y1) return s1.Y1.CompareTo(s2.Y1);
            return s1.Z1.CompareTo(s2.Z1);
        }));

        // 4. Обрабатываем события (sweep-line алгоритм)
        foreach (var e in events)
        {
            if (e.isStart)
            {
                // Проверяем пересечения с активными отрезками
                foreach (var active in activeSegments)
                {
                    if (IsIntersecting3D(e.segment, active))
                    {
                        return true;
                    }
                }
                activeSegments.Add(e.segment);
            }
            else
            {
                activeSegments.Remove(e.segment);
            }
        }
        return false;
    }

    private static bool IsIntersecting3D(Segment3D s1, Segment3D s2)
    {
        return Overlaps(s1.Y1, s1.Y2, s2.Y1, s2.Y2) && 
               Overlaps(s1.Z1, s1.Z2, s2.Z1, s2.Z2);
    }

    private static bool Overlaps(int a1, int a2, int b1, int b2)
    {
        return Math.Max(a1, b1) <= Math.Min(a2, b2);
    }

    public static void Main()
    {
        var segments = new List<Segment3D>
        {
            new Segment3D(1, 4, 16, 12, 77, 1),
            new Segment3D(5, 10, 3, 8, 50, 4),
            new Segment3D(6, 6, 6, 15, 80, 5)
        };

        Console.WriteLine(AreSegmentsIntersecting(segments) ? "Пересечение найдено" : "Пересечений нет");
    }
}
