using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuBrain.Helpers;
using SudokuBrain.Models;
using SudokuBrain.Services;

namespace SudokuBrain.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Show menu and button for create new
            var log = new MessageLog();
            var loader = new LoadPuzzle(log);
            var model = new PageModel();
            model.Menu = loader.ListPuzzleFiles("samples");

            return View(model);
        }

        [Route("[action]")]
        public IActionResult Data(string data = null)
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

        [HttpGet()]
        [Route("[action]/{id}")]
        public IActionResult Sample(string id = "input")
        {
            id = id.FormatId();

            Puzzle puzzle;
            var log = new MessageLog();
            var loader = new LoadPuzzle(log);
            puzzle = loader.LoadFromFile($"samples/{id}.txt");

            return BuildSolutionModel(puzzle, log);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Sample()
        {
            string puzzle = Request.Form["puzzle"];
            return RedirectToAction("Sample", new { id = puzzle.FormatId() });
        }

        private IActionResult BuildSolutionModel(Puzzle puzzle, MessageLog log = null)
        {
            var model = new PageModel(log);
            model.InitialState = puzzle.Copy();
            model.Puzzle = puzzle;
            model.PageTitle = "Sudoku brain";                        

            var solver = new PuzzleSolver(model.Log);

            int previousNumberFilledIn = puzzle.CountFilledInCells;
            while (solver.ApplyAllStrats(puzzle))
            {
                int currentNumberFilledIn = puzzle.CountFilledInCells;
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
            solver.EndSolutionStats(puzzle);

            return View("Index", model);
        }
    }
}
