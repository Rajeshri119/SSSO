using MySql.Data.MySqlClient;
using SSSSO_MHG.Controllers;
using SSSSO_MHG.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSSSO_MHG.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Events()
        {
            //ViewBag.district = "";

            ViewBag.isSearched = HttpContext.Session["is_evntresult"].ToString();
            bool flag = (bool)HttpContext.Session["is_evntresult"];
            ViewBag.District = null;
            ViewBag.Samithi = null;
            ViewBag.Category = null;
            ViewBag.frmDate = null;
            ViewBag.toDate = null;
            ViewBag.dist_id = null;
            ViewBag.sam_id = null;
            ViewBag.cat_id = null;

            String[] search_filters = (String[])HttpContext.Session["srch_criteria"];
            if (flag)
            {
                ViewBag.District = search_filters[0];
                ViewBag.Samithi = search_filters[1];
                ViewBag.Category = search_filters[2];
                ViewBag.frmDate = search_filters[3];
                ViewBag.toDate = search_filters[4];
                ViewBag.dist_id = search_filters[5];
                ViewBag.sam_id = search_filters[6];
                ViewBag.cat_id = search_filters[7];
            }

            return View();
        }

        public ActionResult Index()
        {
            String[] s = getQuoteReference();
            ViewBag.Reference = s[0];
            ViewBag.Quote = s[1];

            HttpContext.Session["srch_criteria"] = null;
            HttpContext.Session["srch_results"] = null;
            HttpContext.Session["is_evntresult"] = false;


            return View();
        }

        public ActionResult Trust()
        {

            return View();
        }


        public ActionResult Contact()
        {
            HttpContext.Session["srch_criteria"] = null;
            HttpContext.Session["srch_results"] = null;
            HttpContext.Session["is_evntresult"] = false;

            return View();
        }


        public ActionResult EventDetail(String id)
        {
            ViewBag.eventid = id;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            HttpContext.Session["srch_criteria"] = null;
            HttpContext.Session["srch_results"] = null;
            HttpContext.Session["is_evntresult"] = false;
            return View();
        }


        public ActionResult Join()
        {
            ViewBag.Message = "Your Join Us Page";
            HttpContext.Session["srch_criteria"] = null;
            HttpContext.Session["srch_results"] = null;
            HttpContext.Session["is_evntresult"] = false;
            return View();
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Gallery(string category = "Cat1")
        {
            HttpContext.Session["srch_criteria"] = null;
            HttpContext.Session["srch_results"] = null;
            HttpContext.Session["is_evntresult"] = false;

            List<string> send = new List<string>();

            if (category == "Cat1")
            {
                ViewBag.headingname = "Dharmakshetra";
                String[] filenames = Directory.GetFiles((Server.MapPath("~/Images/Gallery/Dharmakshetra")));

                foreach (String file in filenames)
                {
                    send.Add("/Images/Gallery/Dharmakshetra/" + Path.GetFileName(file));
                }



            }
            else if (category == "Cat2")
            {
                ViewBag.headingname = "Swami Visits Maharashtra";
                String[] filenames = Directory.GetFiles((Server.MapPath("~/Images/Gallery/VisitToMaharashtra")));

                foreach (String file in filenames)
                {
                    send.Add("/Images/Gallery/VisitToMaharashtra/" + Path.GetFileName(file));
                }

            }
            else if (category == "Cat3")
            {
                ViewBag.headingname = "Pictures of Swami";
                String[] filenames = Directory.GetFiles((Server.MapPath("~/Images/Gallery/candid")));

                foreach (String file in filenames)
                {
                    send.Add("/Images/Gallery/candid/" + Path.GetFileName(file));
                }

            }
            else if (category == "Cat4")
            {
                ViewBag.headingname = "Rare Pictures of Swami";
                String[] filenames = Directory.GetFiles((Server.MapPath("~/Images/Gallery/rare")));

                foreach (String file in filenames)
                {
                    send.Add("/Images/Gallery/rare/" + Path.GetFileName(file));
                }

            }


            ViewBag.Images = send;
            return View();
        }
        public ActionResult Projects(string id)
        {
            HttpContext.Session["srch_criteria"] = null;
            HttpContext.Session["srch_results"] = null;
            HttpContext.Session["is_evntresult"] = false;

            ViewBag.SearchedProject = id;

            return View();
        }
        public ActionResult Search(string searchval)
        {
            List<List<SSSSO_MHG.Models.SearchClass>> finalList = new List<List<SSSSO_MHG.Models.SearchClass>>();

            try
            {

                HttpContext.Session["srch_criteria"] = null;
                HttpContext.Session["srch_results"] = null;
                HttpContext.Session["is_evntresult"] = false;
                //string SearchTextBox = fc["keyword"];
                var SearchTextBox = searchval;
                ViewBag.SearchText = SearchTextBox;
                SearchController sc = new SearchController();
                var searchList = sc.SearchResult(SearchTextBox);
                var eventList = sc.SearchEvents(SearchTextBox);
                Session["searchList"] = searchList;
                Session["eventList"] = eventList;

                searchList = searchList.Take(5).ToList();
                eventList = eventList.Take(5).ToList();

                finalList = new List<List<SSSSO_MHG.Models.SearchClass>>()
                {
                    searchList,eventList
                };


            }
            catch (Exception ex)
            {

            }

            return View(finalList);
        }

        public ActionResult searchResultPV(string index)
        {
            int i = Convert.ToInt32(index);
            List<SSSSO_MHG.Models.SearchClass> searchList = Session["searchList"] as List<SSSSO_MHG.Models.SearchClass>;
            if (searchList == null)
            {
                searchList = new List<SSSSO_MHG.Models.SearchClass>();
            }
            else
                searchList = searchList.Take(i).Skip((i - 5)).ToList();

            return PartialView(searchList);
        }
        public ActionResult EventSearchPV(string index)
        {
            int i = Convert.ToInt32(index);
            List<SSSSO_MHG.Models.SearchClass> searchList = Session["eventList"] as List<SSSSO_MHG.Models.SearchClass>;
            if (searchList == null)
            {
                searchList = new List<SSSSO_MHG.Models.SearchClass>();
            }
            else
                searchList = searchList.Take(i).Skip((i - 5)).ToList();

            return PartialView(searchList);
        }
        public ActionResult FourWings(string id)
        {
            HttpContext.Session["srch_criteria"] = null;
            HttpContext.Session["srch_results"] = null;
            HttpContext.Session["is_evntresult"] = false;

            ViewBag.SearchedWing = id;
            return View();
        }

        public String[] getQuoteReference()
        {
            String[] values = new String[2];
            try
            {

                DBConnect db = new DBConnect();
                List<MySqlParameter> param = new List<MySqlParameter>();
                List<List<Object>> result = db.execStoredProcedure("GetCurrentThought", param);
                values[0] = result.ElementAt(0).ElementAt(0).ToString();
                values[1] = result.ElementAt(0).ElementAt(1).ToString();
                return values;


            }
            catch (Exception ex)
            {
                values[0] = "Sri Sathya Sai Baba";
                values[1] = "Love All Serve All";
                return values;
            }
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            //base.OnException(filterContext);
            if (!filterContext.ExceptionHandled)
            {

                DBConnect db = new DBConnect();
                db.handleException(filterContext.Exception);

                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error",
                };
            }
        }


    }
}