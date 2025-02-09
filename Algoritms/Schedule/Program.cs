using System;
using System.Collections.Generic;

class ExamScheduler
{
    public static int MinimizePenalty(int n, int m, int[] deadlines, int[] penalties)
    {
        List<(int deadline, int penalty, int index)> subjects = new();
        for (int i = 0; i < n; i++)
            subjects.Add((deadlines[i], penalties[i], i));

        // Сортируем предметы по дедлайну (по возрастанию)
        subjects.Sort((a, b) => a.deadline.CompareTo(b.deadline));

        // Куча для предметов, сортировка по убыванию штрафа
        SortedSet<(int penalty, int index)> heap = new SortedSet<(int, int)>(
            Comparer<(int, int)>.Create((x, y) =>
            {
                int cmp = y.penalty.CompareTo(x.penalty);
                return cmp != 0 ? cmp : x.index.CompareTo(y.index);
            })
        );

        int totalPenalty = 0;
        int subjectIndex = 0;

        // Проходим по дням
        for (int day = 1; day <= m; day++)
        {
            // Добавляем в кучу все предметы, чей дедлайн >= текущего дня
            while (subjectIndex < n && subjects[subjectIndex].deadline >= day)
            {
                heap.Add((subjects[subjectIndex].penalty, subjects[subjectIndex].index));
                subjectIndex++;
            }

            // В этот день выбираем 1 предмет с наибольшим штрафом и сдаем его
            if (heap.Count > 0)
            {
                var subjectToTake = heap.Max;
                heap.Remove(subjectToTake);
            }
        }

        // Все предметы, оставшиеся в куче, считаются несданными – штраф начисляется
        foreach (var subject in heap)
            totalPenalty += subject.penalty;

        return totalPenalty;
    }

    public static void Main()
    {
        int n = 3, m = 4;
        int[] deadlines = { 1, 1, 4 };
        int[] penalties = { 200, 300, 100 };

        int result = MinimizePenalty(n, m, deadlines, penalties);
        Console.WriteLine($"Минимальный штраф: {result}");
    }
}