using System;

namespace _03.TemplatePattern
{
   public class StartUp
    {
        public static void Main(string[] args)
        {
            var sourdough = new Sourdough();
            var twelveGrain = new TwelveGrain();
            var wholeWheat = new WholeWheat();

            sourdough.Make();
            twelveGrain.Make();
            wholeWheat.Make();
        }
    }
}
