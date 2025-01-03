namespace ConsoleApp4;
public class Matching
{
        private int n;
    private List<List<int>> preferencesMen;
    private List<List<int>> preferencesWomen;

    public Matching()
    {
        preferencesMen = new List<List<int>>();
        preferencesWomen = new List<List<int>>();
    }

    public void InputData()
    {
        Console.Write("Введите номер парней и девушек: ");
        while (!int.TryParse(Console.ReadLine(), out n) || n <= 0)
        {
            Console.WriteLine("Ошибка ввода. Введите положительное число");
            Console.Write("Введите номер парней и девушек: ");
        }

        Console.WriteLine("Введите предпочтения для парней:");
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Парень {i}: ");
            preferencesMen.Add(ReadPreferences());
        }

        Console.WriteLine("Введите предпочтения для девушек:");
        for (int i = 0; i < n; i++)
        {
            Console.Write($"Девушка {i}: ");
            preferencesWomen.Add(ReadPreferences());
        }
    }

    private List<int> ReadPreferences()
    {
        while (true)
        {
            var input = Console.ReadLine();
            var preferences = input.Split().Select(s => int.TryParse(s, out var num) ? num - 1 : -1).ToList(); // Convert to zero-based indexing

            if (preferences.Count == n && preferences.All(p => p >= 0 && p < n) && preferences.Distinct().Count() == n)
            {
                return preferences;
            }
            Console.WriteLine($"Ошибка ввода. Пожалуйста введите {n} уникальных значений между 1 и {n}, разделенных пробелами.");
            Console.Write("Попробуйте заново для того же человека: ");
        }
    }

    public Dictionary<int, int> Solve()
    {
        // Отслеживаем свободных парней
        var freeMen = new Queue<int>(Enumerable.Range(0, n));

        // Список текущих пар: парень -> девушка
        var currentMatches = new Dictionary<int, int>();

        // Список предложений: сколько предложений сделал каждый парень
        var proposals = new int[n];

        // Обратный индекс предпочтений девушек: девушка -> (парень -> ранг)
        var rankWomen = new List<Dictionary<int, int>>(n);
        for (int i = 0; i < n; i++)
        {
            rankWomen.Add(new Dictionary<int, int>());
            for (int j = 0; j < n; j++)
            {
                rankWomen[i][preferencesWomen[i][j]] = j;
            }
        }

        while (freeMen.Count > 0)
        {
            int man = freeMen.Dequeue();
            int woman = preferencesMen[man][proposals[man]];
            proposals[man]++;

            if (!currentMatches.ContainsValue(woman))
            {
                // Если девушка свободна, связываем её с парнем
                currentMatches[man] = woman;
            }
            else
            {
                // Найти текущего партнера девушки
                int currentMan = currentMatches.First(x => x.Value == woman).Key;

                // Проверить, предпочитает ли девушка нового парня старому
                if (rankWomen[woman][man] < rankWomen[woman][currentMan])
                {
                    // Она предпочитает нового парня, заменяем пару
                    currentMatches.Remove(currentMan);
                    currentMatches[man] = woman;
                    freeMen.Enqueue(currentMan);
                }
                else
                {
                    // Она остаётся со старым партнером
                    freeMen.Enqueue(man);
                }
            }
        }

        return currentMatches;
    }

    public void DisplayResult(Dictionary<int, int> result)
    {
        Console.WriteLine("Результат подбора:");
        foreach (var pair in result)
        {
            Console.WriteLine($"Парень {pair.Key + 1} сведен с девушкой {pair.Value + 1}"); // Convert back to one-based indexing
        }
    }
}
