using System;
using System.Collections.Generic;
using System.Linq;

public class Segment
{
    public int Start { get; set; }
    public int End { get; set; }

    public Segment(int start, int end)
    {
        Start = start;
        End = end;
    }
}

public class SegmentIntersection
{
    public static bool AreSegmentsIntersecting(List<Segment> segments)
    {
        // Создаем список событий
        var events = new List<(int point, bool isStart, Segment segment)>();

        // Заполняем список событий
        foreach (var seg in segments)
        {
            events.Add((seg.Start, true, seg));  // Начало отрезка
            events.Add((seg.End, false, seg));   // Конец отрезка
        }

        // Сортируем события: сначала по точке, затем по типу (сначала начало, затем конец)
        events.Sort((e1, e2) =>
        {
            if (e1.point == e2.point) return e1.isStart ? -1 : 1;
            return e1.point.CompareTo(e2.point);
        });

        // Структура для отслеживания активных отрезков (отсортированы по конец отрезка)
        var activeSegments = new SortedSet<Segment>(Comparer<Segment>.Create((s1, s2) =>
        {
            // Сортируем отрезки по их концам
            return s1.End.CompareTo(s2.End);
        }));

        // Обрабатываем события
        foreach (var e in events)
        {
            if (e.isStart)
            {
                // Если отрезок начинается, проверяем пересечения с активными отрезками
                foreach (var active in activeSegments)
                {
                    if (IsIntersecting(e.segment, active))
                    {
                        return true;  // Нашли пересечение
                    }
                }

                // Добавляем отрезок в активные
                activeSegments.Add(e.segment);
            }
            else
            {
                // Если отрезок заканчивается, удаляем его из активных
                activeSegments.Remove(e.segment);
            }
        }

        return false;  // Пересечений не найдено
    }

    // Метод для проверки пересечения двух отрезков
    public static bool IsIntersecting(Segment s1, Segment s2)
    {
        // Отрезки пересекаются, если они не находятся друг от друга
        return !(s1.End < s2.Start || s2.End < s1.Start);
    }

    public static void Main()
    {
        // Пример 1: Отрезки пересекаются
        var segments1 = new List<Segment>
        {
            new Segment(1, 5),
            new Segment(4, 6)
        };
        Console.WriteLine(AreSegmentsIntersecting(segments1) ? "Пересечение найдено" : "Пересечений нет");

        // Пример 2: Отрезки не пересекаются
        var segments2 = new List<Segment>
        {
            new Segment(1, 5),
            new Segment(6, 7)
        };
        Console.WriteLine(AreSegmentsIntersecting(segments2) ? "Пересечение найдено" : "Пересечений нет");

        // Пример 3: Отрезки соприкасаются (и считаются пересекающимися)
        var segments3 = new List<Segment>
        {
            new Segment(1, 5),
            new Segment(5, 6)
        };
        Console.WriteLine(AreSegmentsIntersecting(segments3) ? "Пересечение найдено" : "Пересечений нет");
    }
}
