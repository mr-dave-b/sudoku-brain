using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuBrain.Models;
using SudokuBrain.Services;

namespace SudokuBrain.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var model = new PageModel();
            model.PageTitle = "Sudoku brain test";

            var loader = new LoadPuzzle(model.Log);

            var puzzle = loader.LoadFromInputTxt("input.txt"); // args[0]

            puzzle.WriteGridAsText(model.Log);

            int previousNumberFilledIn = puzzle.NumbersFilledIn;
            while (puzzle.ApplyAllStrats())
            {
                int currentNumberFilledIn = puzzle.NumbersFilledIn;
                if (currentNumberFilledIn > previousNumberFilledIn)
                {
                    puzzle.WriteGridAsText(model.Log);
                }
                if (currentNumberFilledIn == 81)
                {
                    break;
                }
                previousNumberFilledIn = currentNumberFilledIn;
            }
            puzzle.EndSolutionStats(model.Log);

            // This will be the home screen where you select a sudoku or create one
            return View(model);
        }
    }
}