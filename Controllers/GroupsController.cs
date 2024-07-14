using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dw23787.Data;
using Dw23787.Models;
using System.Security.Claims;

namespace Dw23787.Controllers
{
    public class GroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Users user = _context.UsersApp.FirstOrDefault(u => u.UserID == userId);

            if (user.isAdmin == false)
            {
                // Retrieve all groups where the user is an admin from table GroupAdmins
                var adminGroups = await _context.GroupAdmins
                    .Where(ug => ug.UserFK == user.Id && ug.UserAdmin)
                    .Select(ug => ug.GroupFK)
                    .ToListAsync();

                // Query to get groups with the count of users in each group
                var groups = await _context.Groups
                    .Where(g => adminGroups.Contains(g.GroupId))
                    .ToListAsync();

                return View(groups);
            }

            return View(await _context.Groups.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupId,Name")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groups);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groups);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups.FindAsync(id);
            if (groups == null)
            {
                return NotFound();
            }
            return View(groups);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // First check if groupId is passed 
        /// <summary>
        /// Edit function
        /// 1. check if group exist with given id
        ///     2. If not exist return not found.
        /// 3. If exists:
        ///     4. Update group.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("GroupId,Name")] Groups group)
        {
            // check if groupId is passed
            if (id != group.GroupId)
            {
                return NotFound();
            }

                try
                {    
                    _context.Entry(group).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupsExists(group.GroupId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            
            return View(group);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .FirstOrDefaultAsync(m => m.GroupId == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // POST: Groups/Delete/5
        // 1.Find Group if null return not found
        // 2.Try to find trip with the group id passed
        //  3.case find return model state error
        //  4.case not found remove group from DB.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            Console.WriteLine("IM POST");

            var groups = await _context.Groups.FindAsync(id);
            if (groups == null)
            {
                return NotFound();
            }

            var tripExists = await _context.Trips
                                           .Where(t => t.GroupId == groups.GroupId)
                                           .FirstOrDefaultAsync();

            if (tripExists != null)
            {
                ModelState.AddModelError(string.Empty, "To delete the group, first delete the Trip.");
                return View(groups);
            }

            _context.Groups.Remove(groups);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool GroupsExists(string id)
        {
            return _context.Groups.Any(e => e.GroupId == id);
        }
    }
}
