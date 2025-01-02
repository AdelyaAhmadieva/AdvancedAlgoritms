/* Дан массив пар чисел - заявок на съем квартиры, состоящих из времени въезда 
    и выезда Квартплата фиксирована и не зависит от длительности съема квартиры 
Задача - отобрать заявки так, чтобы прибыль была максимальна */
namespace ConsoleApp4;
public class Booking
{
    public int Start { get; set; }
    public int End { get; set; }
    
    public void BookingSolution()
    {
        // Исходный массив заявок
        List<Booking> bookings = new List<Booking>
        {
            new Booking { Start = 1, End = 3 },
            new Booking { Start = 2, End = 5 },
            new Booking { Start = 4, End = 6 },
            new Booking { Start = 6, End = 7 },
            new Booking { Start = 5, End = 8 },
            new Booking { Start = 7, End = 9 }
        };

        // Сортируем заявки по времени выезда
        bookings = bookings.OrderBy(b => b.End).ToList();

        // Храним выбранные заявки
        List<Booking> selectedBookings = new List<Booking>();

        // Инициализируем переменную для отслеживания конца последней выбранной заявки
        int lastEndTime = 0;

        foreach (var booking in bookings)
        {
            // Если заявка не пересекается с последней выбранной, добавляем ее
            if (booking.Start >= lastEndTime)
            {
                selectedBookings.Add(booking);
                lastEndTime = booking.End;
            }
        }

        // Вывод результата
        Console.WriteLine("Выбранные заявки:");
        foreach (var booking in selectedBookings)
        {
            Console.WriteLine($"[{booking.Start}, {booking.End}]");
        }

        Console.WriteLine($"Максимальное количество заявок: {selectedBookings.Count}");
    }
    
}