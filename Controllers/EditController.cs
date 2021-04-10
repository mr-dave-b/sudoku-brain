using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuBrain.Helpers;
using SudokuBrain.Models;
using SudokuBrain.Services;

namespace SudokuBrain.Controllers
{
    public class EditController : Controller
    {
        [HttpGet()]
        public IActionResult Index(string data = null)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return RedirectToAction("Index");
            }

            var log = new MessageLog();
            var loader = new LoadPuzzle(log);
            Puzzle puzzle = loader.LoadFromString(data);

            return BuildSolutionModel(puzzle, log);
        }

        [HttpPost()]
        public IActionResult Index()
        {
            throw new NotImplementedException();
        }

        private IActionResult BuildSolutionModel(Puzzle puzzle, MessageLog log = null)
        {
            var model = new PageModel(log);
            //model.InitialState = puzzle;
            model.Puzzle = puzzle;
            model.PageTitle = "Sudoku brain - edit puzzle data";                        

            return View("Index", model);
        }
    }
}