using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Museu_DB_v02c.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Authorization;

namespace Museu_DB_v02c.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly Museu_DB_v02cContext _context;
        private int myMaleNum;
        private int myFemaleNum;


        public StatsController(Museu_DB_v02cContext context)
        {
            _context = context;
        }

        // GET: Visitors
        public async Task<IActionResult> Index(string visitorGender, string searchString)
        {



            // Use LINQ to get list of genders.
            IQueryable<string> genderQuery = from v in _context.Visitor
                                             orderby v.Gender
                                             select v.Gender;

            // Use LINQ to get list of Nationalities.
            IQueryable<string> nationalityQuery = from v in _context.Visitor
                                                  orderby v.Nationality
                                                  select v.Nationality;

            // Use LINQ to get list of Ages.
            IQueryable<int> AgeQuery = from v in _context.Visitor
                                       orderby v.Age_Group
                                       select v.Age_Group;

            // Use LINQ to get list of Dates.
            IQueryable<DateTime> DateQuery = from v in _context.Visitor
                                             orderby v.Date
                                             select v.Date;

            var visitors = from v in _context.Visitor
                           select v;

            var visitorViewModel = new VisitorStatsViewModel();
            visitorViewModel.Visitors = await visitors.ToListAsync();
            //for gender search box
            visitorViewModel.GendersList = new SelectList(await genderQuery.Distinct().ToListAsync());
            int sizeGender_list = visitorViewModel.GendersList.Count();
            int[] myGenderInts = new int[sizeGender_list];
            int index = 0;
            foreach (var g in visitorViewModel.GendersList)
            {

                int count = 0;
                foreach (var v in visitors)
                {
                    //search for an option to replace this index for the index of g
                    if (v.Gender == g.Text)
                    {
                        count++;
                        myGenderInts[index] = count;
                    }
                }
                index++;
            }
            //for Nationality                  
            visitorViewModel.NationalityList = new SelectList(await nationalityQuery.Distinct().ToListAsync());
            int sizeNationality_list = visitorViewModel.NationalityList.Count();
            int[] myNationalityInts = new int[sizeNationality_list];
            index = 0;
            foreach (var nationality in visitorViewModel.NationalityList)
            {

                int count_nationality = 0;
                foreach (var v_nationality in visitors)
                {
                    //search for an option to replace this index for the index of g
                    if (v_nationality.Nationality == nationality.Text)
                    {
                        count_nationality++;
                        myNationalityInts[index] = count_nationality;
                    }

                }
                index++;
            }
            //for age search box
            visitorViewModel.AgeList = new SelectList(await AgeQuery.Distinct().ToListAsync());
            int sizeAge_list = visitorViewModel.AgeList.Count();
            int[] myAgeInts = new int[sizeAge_list];
            index = 0;
            foreach (var age in visitorViewModel.AgeList)
            {

                int age_count = 0;
                foreach (var v_age in visitors)
                {
                    //search for an option to replace this index for the index of g
                    if (v_age.Age_Group == int.Parse(age.Text))
                    {
                        age_count++;
                        myAgeInts[index] = age_count;
                    }
                }
                index++;
            }
            //for Date search 
            visitorViewModel.DateList = new SelectList(await DateQuery.Distinct().ToListAsync());
            int sizeDate_list = visitorViewModel.DateList.Count();
            int[] myDateInts = new int[sizeDate_list];
            index = 0;
            foreach (var date in visitorViewModel.DateList)
            {

                int date_count = 0;
                foreach (var v_date in visitors)
                {
                    string visitordate = v_date.Date.ToString();
                    string dateslistitem = date.Text;
                    //search for an option to replace this index for the index of g
                    if (visitordate == dateslistitem)
                    {
                        date_count++;
                        myDateInts[index] = date_count;
                    }
                }
                index++;
            }

            //end of Date search

            if (!string.IsNullOrEmpty(searchString))
            {
                visitors = visitors.Where(s => s.Nationality.Contains(searchString)); // Lambda Expression
            }

            if (!string.IsNullOrEmpty(visitorGender))
            {
                visitors = visitors.Where(x => x.Gender == visitorGender);

            }

            //gender
            visitorViewModel.GenderInts = myGenderInts;
            visitorViewModel.MaleNum = myMaleNum;
            visitorViewModel.FemaleNum = myFemaleNum;
            //nationality
            visitorViewModel.NationalityInts = myNationalityInts;
            //Age
            visitorViewModel.AgeInts = myAgeInts;
            //date
            visitorViewModel.DateInts = myDateInts;


            ///////////////////////////////new chart.js for Date


            List<string> DataList_labels = new List<string>();
            IList<double> DataList_values = new List<double>();

            foreach (SelectListItem item in visitorViewModel.DateList)
            {
                DataList_labels.Add(item.Text);
            }

            foreach (Int32 item in visitorViewModel.DateInts)
            {
                double value = item;
                DataList_values.Add(value);
            }

            createNewChart(DataList_labels, DataList_values, "line", "Visits per Date", "DateChart");

            ///////////////////////////////new chart.js for Nationality
            List<string> NationalityList_labels = new List<string>();
            IList<double> NationalityList_values = new List<double>();

            foreach (SelectListItem item in visitorViewModel.NationalityList)
            {
                NationalityList_labels.Add(item.Text);
            }

            foreach (Int32 item in visitorViewModel.NationalityInts)
            {
                double value = item;
                NationalityList_values.Add(value);
            }

            createNewChart(NationalityList_labels, NationalityList_values, "bar", "Visits per Nationality", "NationalityChart");
            ///////////////////////////////new chart.js for Age
            List<string> AgeList_labels = new List<string>();
            IList<double> AgeList_values = new List<double>();

            foreach (SelectListItem item in visitorViewModel.AgeList)
            {
                AgeList_labels.Add(item.Text);
            }

            foreach (Int32 item in visitorViewModel.AgeInts)
            {
                double value = item;
                AgeList_values.Add(value);
            }

            createNewChart(AgeList_labels, AgeList_values, "line", "Visits per Age", "AgeChart");

            ///////////////////////////////new chart.js for Gender
            List<string> GenderList_labels = new List<string>();
            IList<double> GenderList_values = new List<double>();

            foreach (SelectListItem item in visitorViewModel.GendersList)
            {
                GenderList_labels.Add(item.Text);
            }

            foreach (Int32 item in visitorViewModel.GenderInts)
            {
                double value = item;
                GenderList_values.Add(value);
            }

            createNewChart(GenderList_labels, GenderList_values, "pie", "Visits per Gender", "GenderChart");
            /// end new chart.js



            return View(visitorViewModel);


        }

        public ActionResult CharterColumn()
        {

            ArrayList xValue = new ArrayList(new[] { 2, 3, 2, 3, 2, 2, 3, 4 });
            ArrayList yValue = new ArrayList(new[] { 2, 3, 2, 3, 2, 2, 3, 4 });

            /*var results = from v in _context.Visitor select v;
            results.ToList().ForEach(rs => xValue.Add(rs.Age_Group));
            results.ToList().ForEach(rs => yValue.Add(rs.Age_Group));*/

            return null;
        }

        

        public void createNewChart(List<string> ChartLabels, IList<double> ChartValues, string chartType, string ChartTitle, string ChartID)
        {
            Chart DateChart = new Chart();
            DateChart.Type = chartType;
            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();
            data.Labels = ChartLabels;

            Options options = new Options()
            {
                Scales = new Scales()
            };

            Scales scales = new Scales()
            {
                YAxes = new List<Object>()
                {
                    new CartesianScale()
                    {
                        Type = "linear",
                        Position = "bottom",
                        Ticks = new CartesianLinearTick()
                        {
                            BeginAtZero = true
                        }
                    }
                }
            };



            if (chartType != "pie")
            {
                options.Scales = scales;

                DateChart.Options = options;

                LineDataset dataset = new LineDataset()
                {
                    Label = ChartTitle,
                    Data = ChartValues,
                    Fill = false,
                    LineTension = 0.1,
                    BackgroundColor = "rgba(75, 192, 192, 0.4)",
                    BorderColor = "rgba(75,192,192,1)",
                    BorderCapStyle = "butt",
                    BorderDash = new List<int> { },
                    BorderDashOffset = 0.0,
                    BorderJoinStyle = "miter",
                    PointBorderColor = new List<string>() { "rgba(75,192,192,1)" },
                    PointBackgroundColor = new List<string>() { "#fff" },
                    PointBorderWidth = new List<int> { 1 },
                    PointHoverRadius = new List<int> { 5 },
                    PointHoverBackgroundColor = new List<string>() { "rgba(75,192,192,1)" },
                    PointHoverBorderColor = new List<string>() { "rgba(220,220,220,1)" },
                    PointHoverBorderWidth = new List<int> { 2 },
                    PointRadius = new List<int> { 1 },
                    PointHitRadius = new List<int> { 10 },
                    SpanGaps = false
                };

                data.Datasets = new List<Dataset>();
                data.Datasets.Add(dataset);


            }
            else
            {

                PieDataset pie_dataset = new PieDataset()
                {
                    Label = "My dataset",
                    BackgroundColor = new List<string>() { "#FF6384", "#36A2EB", "#FFCE56" },
                    HoverBackgroundColor = new List<string>() { "#FF6384", "#36A2EB", "#FFCE56" },
                    Data = ChartValues
                };


                data.Datasets = new List<Dataset>();
                data.Datasets.Add(pie_dataset);


            }

            DateChart.Data = data;

            ViewData[ChartID] = DateChart;


        }
    }
}