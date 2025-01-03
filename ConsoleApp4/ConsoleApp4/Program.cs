using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApp4;

class Program
{
    static void Main(string[] args)
    {
        /*Booking booking = new Booking();
        booking.BookingSolution();*/
        

        Matching matching = new Matching();
        matching.InputData();
        matching.DisplayResult(matching.Solve());

    }
}
