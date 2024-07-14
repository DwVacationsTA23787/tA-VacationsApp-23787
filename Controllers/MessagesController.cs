using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dw23787.Data;
using Dw23787.Models;
using Microsoft.AspNetCore.Authorization;

namespace Dw23787.Controllers
{
    // Authorize to acces only if user is logged in.
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Messages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Messages.Include(m => m.Group).Include(m => m.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _context.Messages
                .Include(m => m.Group)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (messages == null)
            {
                return NotFound();
            }

            return View(messages);
        }

        // GET: Messages/Create
        public IActionResult Create()
        {
            ViewData["GroupFK"] = new SelectList(_context.Groups, "GroupId", "GroupId");
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MessageId,MessageTitle,Description,Photo,GroupFK,UserFK")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(messages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupFK"] = new SelectList(_context.Groups, "GroupId", "GroupId", messages.GroupFK);
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id", messages.UserFK);
            return View(messages);
        }

        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _context.Messages.FindAsync(id);
            if (messages == null)
            {
                return NotFound();
            }
            ViewData["GroupFK"] = new SelectList(_context.Groups, "GroupId", "GroupId", messages.GroupFK);
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id", messages.UserFK);
            return View(messages);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageId,MessageTitle,Description,Photo,GroupFK,UserFK")] Messages messages)
        {
            if (id != messages.MessageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(messages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessagesExists(messages.MessageId))
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
            ViewData["GroupFK"] = new SelectList(_context.Groups, "GroupId", "GroupId", messages.GroupFK);
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id", messages.UserFK);
            return View(messages);
        }

        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _context.Messages
                .Include(m => m.Group)
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.MessageId == id);
            if (messages == null)
            {
                return NotFound();
            }

            return View(messages);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var messages = await _context.Messages.FindAsync(id);
            if (messages != null)
            {
                _context.Messages.Remove(messages);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessagesExists(int id)
        {
            return _context.Messages.Any(e => e.MessageId == id);
        }
    }
}
