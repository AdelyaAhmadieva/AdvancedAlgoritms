using System;
using System.Collections.Generic;

class ExamScheduler
{
    public static int MinimizePenalty(int n, int m, int[] deadlines, int[] penalties, int[] durations)
    {
        List<(int deadline, int penalty, int duration)> subjects = new();
        for (int i = 0; i < n; i++)
            subjects.Add((deadlines[i], penalties[i], durations[i]));

        // Сортируем по дедлайну
        subjects.Sort((a, b) => a.deadline.CompareTo(b.deadline));

        // Куча для хранения предметов по штрафу
        SortedSet<(int penalty, int index)> heap = new SortedSet<(int, int)>();
        int totalPenalty = 0;
        int usedDays = 0;

        foreach (var subject in subjects)
        {
            heap.Add((subject.penalty, usedDays));
            usedDays += subject.duration;

            // Если выходим за лимит дней, удаляем самый "дешевый" предмет
            while (usedDays > m && heap.Count > 0)
            {
                var minPenalty = heap.Min;
                heap.Remove(minPenalty);
                totalPenalty += minPenalty.penalty;
                usedDays -= subjects[minPenalty.index].duration;
            }
        }

        return totalPenalty;
    }

    public static void Main()
    {
        int n = 3, m = 3;
        int[] deadlines = { 1, 2, 3 };
        int[] penalties = { 50, 60, 200 };
        int[] durations = { 1, 1, 3 };

        int result = MinimizePenalty(n, m, deadlines, penalties, durations);
        Console.WriteLine($"Минимальный штраф: {result}");
    }
}