using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Museu_DB_v02c.Models;

namespace Museu_DB_v02c.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly Museu_DB_v02cContext _context;

        public VisitorsController(Museu_DB_v02cContext context)
        {
            _context = context;
        }

        // GET: Visitors
        [Authorize]
        public async Task<IActionResult> Index(string visitorGender, string searchString)
        {
            // Use LINQ to get list of genders.
            IQueryable<string> genderQuery = from v in _context.Visitor
                orderby v.Gender
                select v.Gender;


            var visitors = from v in _context.Visitor
                select v;

          

            if (!string.IsNullOrEmpty(searchString))
            {
                visitors = visitors.Where(s => s.Nationality.Contains(searchString)); // Lambda Expression
            }

            if (!string.IsNullOrEmpty(visitorGender))
            {
                visitors = visitors.Where(x => x.Gender == visitorGender); 
            }


            //for gender search box
            var visitorGenderVm = new VisitorStatsViewModel();
            visitorGenderVm.GendersList = new SelectList(await genderQuery.Distinct().ToListAsync());
            visitorGenderVm.Visitors = await visitors.ToListAsync();
            

            return View(visitorGenderVm);

        }

        // GET: Visitors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitor
                .SingleOrDefaultAsync(m => m.ID == id);
            if (visitor == null)
            {
                return NotFound();
            }

            return View(visitor);
        }

        // GET: Visitors/Create
        public async  Task<IActionResult> Create()
        {
            
            var visitors = from v in _context.Visitor
                select v;

            // Use LINQ to get list of Nationalities.
            IQueryable<string> nationalityQuery = from v in _context.Visitor
                orderby v.Nationality
                select v.Nationality;
           List<string> NationalityList = new List<string>(await nationalityQuery.Distinct().ToListAsync());

            int index = 0;
            int[] myNationalityInts = new int[NationalityList.Count];
            index = 0;
            foreach (var nationality in NationalityList)
            {

                int count_nationality = 0;
                foreach (var v_nationality in visitors)
                {
                    //search for an option to replace this index for the index of g
                    if (v_nationality.Nationality == nationality)
                    {
                        count_nationality++;
                        myNationalityInts[index] = count_nationality;
                    }

                }
                index++;
            }

            ViewBag.NationalityStringList = new List<string>() { "", "", "", "", "", "", "", "" };
            int nationality_count = 0;
            while (myNationalityInts.Max() != 0 && nationality_count <8)
            {
                int max = myNationalityInts.Max();
                int max_index = myNationalityInts.ToList().IndexOf(max);                
                ViewBag.NationalityStringList[nationality_count] = NationalityList[max_index];
                myNationalityInts[max_index] = 0;
                nationality_count++;
            }


            return View();
        }

        // POST: Visitors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID, Age_Group,Nationality,Gender")] Visitor visitor)
        {
            if (ModelState.IsValid)
            {
                visitor.Date = DateTime.Today;                
                _context.Add(visitor);
                await _context.SaveChangesAsync();
                await Task.Delay(4000);
                return RedirectToAction(nameof(Create));
            }
            return View(visitor);
        }

        // GET: Visitors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitor.SingleOrDefaultAsync(m => m.ID == id);
            if (visitor == null)
            {
                return NotFound();
            }
            return View(visitor);
        }

        // POST: Visitors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Date, Age_Group,Nationality,Gender")] Visitor visitor)
        {
            if (id != visitor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visitor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitorExists(visitor.ID))
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
            return View(visitor);
        }

        // GET: Visitors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitor = await _context.Visitor
                .SingleOrDefaultAsync(m => m.ID == id);
            if (visitor == null)
            {
                return NotFound();
            }

            return View(visitor);
        }

        // POST: Visitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var visitor = await _context.Visitor.SingleOrDefaultAsync(m => m.ID == id);
            _context.Visitor.Remove(visitor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitorExists(int id)
        {
            return _context.Visitor.Any(e => e.ID == id);
        }
    }
}
