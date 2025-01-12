using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    public class Subject
    {
        public int Day { get; set; }   // День сдачи предмета
        public int Fine { get; set; } // Штраф за несдачу
        public int Index { get; set; } // Индекс предмета
    }

    static void Main()
    {
        Random random = new Random();
        int n = random.Next(5, 15); // Количество предметов
        int m = random.Next(3, n); // Доступные дни

        int[] days = new int[n];
        int[] fines = new int[n];

        for (int i = 0; i < n; i++)
        {
            days[i] = random.Next(1, m + 1);  // День сдачи
            fines[i] = random.Next(1, 20);    // Штраф
        }

        Console.WriteLine($"Количество предметов: {n}");
        Console.WriteLine($"Доступные дни: {m}");
        Console.WriteLine("Предметы (день сдачи, штраф):");
        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"Предмет {i + 1}: День сдачи = {days[i]}, Штраф = {fines[i]}");
        }

        // Выполняем оба алгоритма
        int fineQuadratic = MinimizeFineQuadratic(n, days, fines);
        Console.WriteLine($"\nМинимальная сумма штрафов (O(n^2)): {fineQuadratic}");

        int fineEfficient = MinimizeFineEfficient(n, m, days, fines);
        Console.WriteLine($"Минимальная сумма штрафов (O(n log n)): {fineEfficient}");
    }

    // O(n^2) Алгоритм
    static int MinimizeFineQuadratic(int n, int[] days, int[] fines)
    {
        List<Subject> subjects = new List<Subject>();
        for (int i = 0; i < n; i++)
        {
            subjects.Add(new Subject { Day = days[i], Fine = fines[i], Index = i });
        }

        // Сортируем предметы
        subjects = subjects
            .OrderBy(s => s.Day)
            .ThenByDescending(s => s.Fine)
            .ToList();

        bool[] usedDays = new bool[subjects.Max(s => s.Day) + 1];
        int totalFine = 0;

        foreach (var subject in subjects)
        {
            bool scheduled = false;

            // Перебираем дни от дня сдачи до 1
            for (int day = subject.Day; day > 0; day--)
            {
                if (!usedDays[day])
                {
                    usedDays[day] = true; // Занимаем день
                    scheduled = true;
                    break;
                }
            }

            // Если день не найден, добавляем штраф
            if (!scheduled)
            {
                totalFine += subject.Fine;
            }
        }

        return totalFine;
    }

    // O(n log n) Алгоритм
    static int MinimizeFineEfficient(int n, int m, int[] days, int[] fines)
    {
        List<Subject> subjects = new List<Subject>();
        for (int i = 0; i < n; i++)
        {
            subjects.Add(new Subject { Day = days[i], Fine = fines[i], Index = i });
        }

        // Сортируем предметы
        subjects = subjects
            .OrderBy(s => s.Day)
            .ThenByDescending(s => s.Fine)
            .ToList();

        // Используем SortedSet для отслеживания доступных дней
        SortedSet<int> availableDays = new SortedSet<int>();
        for (int i = 1; i <= m; i++)
        {
            availableDays.Add(i);
        }

        int totalFine = 0;

        foreach (var subject in subjects)
        {
            // Найти ближайший доступный день, начиная с subject.Day
            var day = availableDays.GetViewBetween(1, subject.Day).Max;

            if (day > 0)
            {
                availableDays.Remove(day); // Занимаем день
            }
            else
            {
                totalFine += subject.Fine; // Если день не найден, добавляем штраф
            }
        }
        return totalFine;
    }
}
