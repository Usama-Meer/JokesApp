using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesApp.Data;
using JokesApp.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace JokesApp.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Jokes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jokes.ToListAsync());
        }

        [Authorize]
        // GET: JokesSearchForm
        public async Task<IActionResult> JokesSearch()
        {
            return View();
        }

        [Authorize]
        // GET: Jokes
        public async Task<IActionResult> JokesSearchResult(String SearchPhrase)
        {
            return View("Index",await _context.Jokes.Where(j=>j.JokeQuestion.Contains(SearchPhrase)).ToListAsync());
        }


        [Authorize]
        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jokes = await _context.Jokes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jokes == null)
            {
                return NotFound();
            }

            return View(jokes);
        }

        [Authorize]
        // GET: Jokes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JokeQuestion,JokeAnswer")] Jokes jokes)
        {
            if (ModelState.IsValid)
            {
                jokes.JokeCreator = User.Identity?.Name;
                _context.Add(jokes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jokes);
        }


        [Authorize]
        // GET: Jokes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jokes = await _context.Jokes.FindAsync(id);
            if (jokes == null)
            {
                return NotFound();
            }
            return View(jokes);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JokeQuestion,JokeAnswer,JokeCreator")] Jokes jokes)
        {
            if (id != jokes.Id && User.Identity?.Name == jokes.JokeCreator)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jokes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokesExists(jokes.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jokes);
        }

        [Authorize]
        // GET: Jokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jokes = await _context.Jokes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jokes == null)
            {
                return NotFound();
            }

            return View(jokes);
        }

        [Authorize]
        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jokes = await _context.Jokes.FindAsync(id);
            if (jokes != null)
            {
                _context.Jokes.Remove(jokes);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokesExists(int id)
        {
            return _context.Jokes.Any(e => e.Id == id);
        }
    }
}
