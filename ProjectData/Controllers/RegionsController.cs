using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectData.Context;
using ProjectData.Models;

namespace ProjectData.Controllers
{
    public class RegionsController : Controller
    {
        private readonly RegionContext _context;

        public RegionsController()
        {
            _context = new RegionContext();
        }

        // GET: Regions
        public IActionResult Index(bool isReadable, SortState sortOrder = SortState.NameAsc, string searchString = "")
        {
            IQueryable<Region> regions = _context.region.ToList().AsQueryable();

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["CurrentFilter"] = searchString;
            ViewData["isReadable"] = false;

            if (!string.IsNullOrEmpty(searchString))
            {
                regions = regions.Where(s => s.name.StartsWith(searchString));
            }

            regions = sortOrder switch
            {
                SortState.NameDesc => regions.OrderByDescending(s => s.name),
                _ => regions.OrderBy(s => s.name),
            };

            if(isReadable == true)
            {
                ViewData["isReadable"] = true;
                CountryContext countryContext = new CountryContext();
                var query = regions.Join(
                    countryContext.country,
                    regions => regions.country_id,
                    country => country.country_id,
                    (regions, country) => new FullRegion
                    {
                        region_id = regions.region_id,
                        Country = country.name,
                        Region = regions.name
                    });
                ViewData["Res"] = query.AsNoTracking().ToList();
                return View();
            }

            return View(regions.AsNoTracking().ToList());
        }

        // GET: Regions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Regions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("region_id,country_id,name")] Region region)
        {
            if (ModelState.IsValid)
            {
                _context.Add(region);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(region);
        }

        // GET: Regions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.region.FindAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            return View(region);
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("region_id,country_id,name")] Region region)
        {
            if (id != region.region_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(region);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegionExists(region.region_id))
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
            return View(region);
        }

        // GET: Regions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var region = await _context.region
                .FirstOrDefaultAsync(m => m.region_id == id);
            if (region == null)
            {
                return NotFound();
            }

            return View(region);
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var region = await _context.region.FindAsync(id);
            _context.region.Remove(region);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegionExists(int id)
        {
            return _context.region.Any(e => e.region_id == id);
        }
    }
}
