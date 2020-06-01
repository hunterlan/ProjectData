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
    public class CitiesController : Controller
    {
        private readonly CityContext _context;

        public CitiesController()
        {
            _context = new CityContext();
        }

        public void HoaraSort(List<City> sortCities, SortState sortOrder, int b, int e)
        {
            int i, j;
            if (sortOrder == SortState.NameAsc)
            {
                i = b;
                j = e;

                string startStr = sortCities.ElementAt((i + j) / 2).name;
                //TO-DO: Fix sorting in DESC mode
                do
                {

                    while (string.Compare(sortCities.ElementAt(i).name, startStr) > 0) i++;
                    while (string.Compare(sortCities.ElementAt(j).name, startStr) < 0) j--;

                    if (i <= j)
                    {
                        City tempCity = sortCities[i];
                        sortCities[i] = sortCities[j];
                        sortCities[j] = tempCity;
                        i++;
                        j--;
                    }
                } while (i <= j);
            }
            else
            {
                i = e;
                j = b;

                string startStr = sortCities.ElementAt((i + j) / 2).name;

                do
                {

                    while (string.Compare(sortCities.ElementAt(i).name, startStr) < 0) i--;
                    while (string.Compare(sortCities.ElementAt(j).name, startStr) > 0) j++;

                    if (i >= j)
                    {
                        City tempCity = sortCities[i];
                        sortCities[i] = sortCities[j];
                        sortCities[j] = tempCity;
                        i++;
                        j--;
                    }
                } while (i >= j);
            }

            if (i < e) HoaraSort(sortCities, sortOrder, i, e);

            if (b < j) HoaraSort(sortCities, sortOrder, b, j);
        }

        // GET: Cities
        public IActionResult Index(SortState sortOrder = SortState.NameAsc)
        {
            List<City> cities = _context.city.ToList();

            HoaraSort(cities, sortOrder, 0, cities.Count - 1);

            return View(cities);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("city_id,country_id,region_id,name")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("city_id,country_id,region_id,name")] City city)
        {
            if (id != city.city_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.city_id))
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
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .FirstOrDefaultAsync(m => m.city_id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.City.FindAsync(id);
            _context.City.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.City.Any(e => e.city_id == id);
        }
    }
}
