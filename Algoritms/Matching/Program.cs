using System;
using System.Collections.Generic;

class Matchmaking
{
    private int n, m; // Количество невест (n) и женихов (m)
    private List<int>[] brides; // brides[i] - список возможных женихов для невесты i
    private int[] dowries; // dowries[i] - приданое невесты i
    private int[] brideMatch, groomMatch; // brideMatch[i] = жених невесты i, groomMatch[j] = невеста жениха j
    private bool[] visited; // Для поиска увеличивающей цепи

    public Matchmaking(int n, int m)
    {
        this.n = n;
        this.m = m;
        brides = new List<int>[n];
        dowries = new int[n];
        for (int i = 0; i < n; i++)
        {
            brides[i] = new List<int>();
        }
        brideMatch = new int[n];
        groomMatch = new int[m];
        Array.Fill(brideMatch, -1);
        Array.Fill(groomMatch, -1);
    }

    // Добавление возможного жениха и установление приданого
    public void AddBride(int bride, int dowry, params int[] grooms)
    {
        dowries[bride] = dowry;
        foreach (int groom in grooms)
        {
            brides[bride].Add(groom);
        }
    }

    // Поиск увеличивающей цепи
    private bool FindAugmentingPath(int bride)
    {
        foreach (var groom in brides[bride])
        {
            if (visited[groom]) continue; // Пропускаем уже проверенного жениха
            visited[groom] = true; // Отмечаем жениха как посещённого

            // Если жених свободен или можно пересватать его невесту
            if (groomMatch[groom] == -1 || FindAugmentingPath(groomMatch[groom]))
            {
                brideMatch[bride] = groom;
                groomMatch[groom] = bride;
                return true;
            }
        }
        return false;
    }

    // Поиск максимального суммарного приданого
    public int FindMaximumDowry()
    {
        // Сортируем невест по убыванию приданого
        List<int> bridesOrder = new List<int>();
        for (int i = 0; i < n; i++) bridesOrder.Add(i);
        bridesOrder.Sort((a, b) => dowries[b].CompareTo(dowries[a]));

        int maxDowry = 0;

        foreach (int bride in bridesOrder)
        {
            visited = new bool[m]; // Обнуляем посещённые вершины перед поиском
            if (FindAugmentingPath(bride))
            {
                maxDowry += dowries[bride];
            }
        }

        return maxDowry;
    }

    // Вывод пар
    public void PrintMatches()
    {
        Console.WriteLine("\nСватовство:");
        for (int i = 0; i < n; i++)
        {
            if (brideMatch[i] != -1)
                Console.WriteLine($"Невеста {i} → Жених {brideMatch[i]} (Приданое: {dowries[i]})");
        }
    }
}

// Генератор случайных данных
class RandomTest
{
    static void Main()
    {
        Random rand = new Random();

        int n = rand.Next(3, 6); // Количество невест (от 3 до 5)
        int m = rand.Next(3, 6); // Количество женихов (от 3 до 5)
        Matchmaking matchmaking = new Matchmaking(n, m);

        Console.WriteLine($"Генерируем {n} невест и {m} женихов...\n");

        for (int i = 0; i < n; i++)
        {
            int dowry = rand.Next(10, 200); // Приданое невесты (от 10 до 200)
            int numPartners = rand.Next(1, m + 1); // Количество доступных женихов
            HashSet<int> chosenGrooms = new HashSet<int>();

            for (int j = 0; j < numPartners; j++)
            {
                int groom;
                do
                {
                    groom = rand.Next(0, m); // Выбираем случайного жениха
                } while (chosenGrooms.Contains(groom));
                chosenGrooms.Add(groom);
            }

            matchmaking.AddBride(i, dowry, chosenGrooms.ToArray());

            Console.WriteLine($"Невеста {i} → Женихи {string.Join(", ", chosenGrooms)} (Приданое: {dowry})");
        }

        int maxDowry = matchmaking.FindMaximumDowry();
        matchmaking.PrintMatches();

        Console.WriteLine($"\nМаксимальное суммарное приданое: {maxDowry}");
    }
}
