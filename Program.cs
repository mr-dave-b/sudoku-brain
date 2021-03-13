using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace sudoku_brain
{
    class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            //.UseIISIntegration()
            .UseStartup<Startup>()
            .Build();

            host.Run();

        }
    }
}
/*
        static void Main(string[] args)
        {
            var loader = new LoadPuzzle();

            var puzzle = loader.LoadFromInputTxt(args[0]);

            puzzle.WriteToConsole();

            int previousNumberFilledIn = puzzle.NumbersFilledIn;
            while (puzzle.ApplyAllStrats())
            {
                int currentNumberFilledIn = puzzle.NumbersFilledIn;
                if (currentNumberFilledIn > previousNumberFilledIn)
                {
                    puzzle.WriteToConsole();
                }
                if (currentNumberFilledIn == 81)
                {
                    puzzle.EndSolutionStats();
                    return;
                }
                previousNumberFilledIn = currentNumberFilledIn;
            }
            puzzle.EndSolutionStats();
        }
    }
}
*/