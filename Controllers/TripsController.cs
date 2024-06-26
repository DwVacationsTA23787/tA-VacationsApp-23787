﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dw23787.Data;
using Dw23787.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Dw23787.Controllers
{

    [Authorize]
    public class TripsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TripsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Trips.Include(t => t.Group).Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Trips/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trips = await _context.Trips
                .Include(t => t.Group)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trips == null)
            {
                return NotFound();
            }

            return View(trips);
        }

        // GET: Trips/Create
        public IActionResult Create()
        {
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId");
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id");
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TripName,Description,Category,Transport,InicialBudget,FinalBudget,Banner,Closed,UserFK")] Trips trips)
        {
            try
            {
                var userIdLogged = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Get the user from UsersApp table based on UserID
                Users user = await _context.UsersApp.FirstOrDefaultAsync(u => u.UserID == userIdLogged);

                if (user == null)
                {
                    ModelState.AddModelError("", "User not found. Please log in again.");
                    return View(trips);
                }

                // Assign the UserFK based on the found user's Id
                trips.UserFK = user.Id;

                // Create a new Group associated with the Trip
                var newGroup = new Groups
                {
                    GroupId = Guid.NewGuid().ToString(), // Generate a new GUID for GroupId
                    Name = trips.TripName // Set the group name (adjust as needed)
                };

                // Associate the new Group with the Trip
                trips.Group = newGroup;

                // Generate a GUID for the trip
                trips.Id = Guid.NewGuid().ToString();

                // Add the trip and the associated group to the context
                _context.Trips.Add(trips);

                // Save changes to the database to ensure trips.Id is generated
                await _context.SaveChangesAsync();

                // Redirect to the index page upon successful save
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                // Log the exception or handle specific database update errors
                ModelState.AddModelError("", $"Database update error occurred: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                ModelState.AddModelError("", $"An error occurred while creating the trip: {ex.Message}");
            }

            // If there are validation errors or exceptions, reload necessary data for the view
            return View(trips);
        }
    

    // GET: Trips/Edit/5
    public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trips = await _context.Trips.FindAsync(id);
            if (trips == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", trips.GroupId);
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id", trips.UserFK);
            return View(trips);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TripName,Description,Category,Transport,InicialBudget,FinalBudget,Banner,Closed,GroupId,UserFK")] Trips trips)
        {
            if (id != trips.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trips);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripsExists(trips.Id))
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
            ViewData["GroupId"] = new SelectList(_context.Groups, "GroupId", "GroupId", trips.GroupId);
            ViewData["UserFK"] = new SelectList(_context.UsersApp, "Id", "Id", trips.UserFK);
            return View(trips);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trips = await _context.Trips
                .Include(t => t.Group)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trips == null)
            {
                return NotFound();
            }

            return View(trips);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var trips = await _context.Trips.FindAsync(id);
            if (trips != null)
            {
                _context.Trips.Remove(trips);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripsExists(string id)
        {
            return _context.Trips.Any(e => e.Id == id);
        }
    }
}
