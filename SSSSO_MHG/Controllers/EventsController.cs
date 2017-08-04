using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using SSSSO_MHG.Models;
using MySql.Data.MySqlClient;

using Ical.Net.DataTypes;
using Ical.Net;
using Ical.Net.Interfaces.DataTypes;

namespace SSSSO_MHG.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events+
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEvents(string district, string samithi, string category, string from_date, string to_date, string dist_name, string sam_name, string cat_name, int draw, int start)
        {
            List<EventSearchDetails> details;
            if (HttpContext.Session["srch_criteria"] == null)
            {
                details = GetEventList(district, samithi, category, from_date, to_date,dist_name,sam_name,cat_name);
                HttpContext.Session["srch_results"] = details;
            }
            else
            {
                String[] search_filters = (String[])HttpContext.Session["srch_criteria"];
                if ( district.Equals(search_filters[5]) &&  samithi.Equals(search_filters[6]) &&  category.Equals(search_filters[7]) && ((String.IsNullOrEmpty(from_date) && String.IsNullOrEmpty(search_filters[3])) || from_date.Equals(search_filters[3])) && ((String.IsNullOrEmpty(to_date) && String.IsNullOrEmpty(search_filters[4])) || to_date.Equals(search_filters[4])))
                    details = (List<EventSearchDetails>)HttpContext.Session["srch_results"];
                else
                {
                    details = GetEventList(district, samithi, category, from_date, to_date, dist_name, sam_name, cat_name);
                    HttpContext.Session["srch_results"] = details;
                }


            }

            EventSearchDetails[] details_10;
            if (start >= details.Count - 10)
            {
                details_10 = new EventSearchDetails[details.Count - start];
                details.CopyTo(start, details_10, 0, details.Count - start);
            }
            else
            {
                details_10 = new EventSearchDetails[10];
                details.CopyTo(start, details_10, 0, 10);
            }

            SearchResultForAjax ajaxParams = new SearchResultForAjax(draw, details.Count, details.Count, details_10);
            return Json(ajaxParams, JsonRequestBehavior.AllowGet);
        }

        public List<EventSearchDetails> GetEventList(string district, string samithi, string category, string from_date, string to_date, string dist_name , string sam_name , string cat_name )
        {
            

            HttpContext.Session["is_evntresult"] = true;
            //Current.Session["evnt_result"] = true;



            DateTime tDate;
            DateTime fDate;
            String fd="";
            String td="";
            List<EventSearchDetails> items = new List<EventSearchDetails>();
            List<MySqlParameter> param = new List<MySqlParameter>();
            




            if (district.Equals("999"))
            {
                dist_name = "All Districts";
                param.Add(new MySqlParameter("@i_district_id", DBNull.Value));
            }
            else
                param.Add(new MySqlParameter("@i_district_id", district));

            if (samithi.Equals("999"))
                param.Add(new MySqlParameter("@i_samithi_id", DBNull.Value));
            else
            {
                sam_name = "All Samithies";
                param.Add(new MySqlParameter("@i_samithi_id", samithi));
            }
            if (category.Equals("999"))
            {
                cat_name = "All Categories";
                param.Add(new MySqlParameter("@i_event_type_id", DBNull.Value));
            }
            else
                param.Add(new MySqlParameter("@i_event_type_id", category));

            //if (category == null || category == "")
            //    category = "";
            if (from_date == null || from_date == "")
            {
                param.Add(new MySqlParameter("@i_start_date", DBNull.Value));
                fd = DateTime.Now.ToString("yyyy-MM-dd");   //for recurring pattern dates
            }
            else
            {
                fDate = DateTime.ParseExact(from_date, "dd-MM-yyyy", null);
                fd = fDate.ToString("yyyy-MM-dd");
                param.Add(new MySqlParameter("@i_start_date", fd));
            }

            if (to_date == null || to_date == "")
            {
                td = DateTime.Now.AddYears(2).ToString("yyyy-MM-dd");   //for recurring pattern dates
                param.Add(new MySqlParameter("@i_end_date", td)); 
            }
            else
            {
                tDate = DateTime.ParseExact(to_date, "dd-MM-yyyy", null);
                td = tDate.ToString("yyyy-MM-dd");
                param.Add(new MySqlParameter("@i_end_date", td));
            }


            
            DBConnect db = new DBConnect();
            
            
            
            
            String[] sc = new String[] { dist_name, sam_name, cat_name, from_date, to_date, district, samithi , category };
            HttpContext.Session["srch_criteria"] = sc;

            



            List<List<Object>> list = db.execStoredProcedure("R_SearchEvents", param);

            
            String i_event_id;
            String i_event_name;
            String i_event_start_date;
            String i_event_end_date;
            String i_recursive_yn; //0 for NO & 1 for YES
            String i_recurring_type; //0 for Daily. 1 for Weekly. 2 for Monthly. 3 for Yearly / Annually.
            String i_day_of_week;
            String i_day_of_month;
            String i_week_of_month;
            String i_month_of_year;
            String i_separation_count;
            String i_max_num_of_occurences;

            #region for loop
            foreach (List<Object> record in list)
            {
                
                i_event_id = record[0].ToString();
                i_event_name = record[2].ToString();
                i_event_start_date= record[3].ToString();
                i_event_end_date = record[4].ToString();
                i_recursive_yn = record[7].ToString(); //0 for NO & 1 for YES
                i_recurring_type = record[8].ToString(); //0 for Daily. 1 for Weekly. 2 for Monthly. 3 for Yearly / Annually.
                i_day_of_week = record[9].ToString();
                i_day_of_month = record[10].ToString();
                i_week_of_month = record[11].ToString();
                i_month_of_year = record[12].ToString();
                i_separation_count = record[13].ToString();
                i_max_num_of_occurences = record[14].ToString();



                if (i_recursive_yn.Equals("Y"))
                {
                    
                    List<String> rec_dates = getRecurringDates(i_recurring_type, i_event_start_date, i_event_end_date, fd, td, i_day_of_week, i_day_of_month, i_week_of_month, i_month_of_year, i_separation_count, i_max_num_of_occurences);
                    foreach (String rd in rec_dates)
                    {
                        items.Add(new EventSearchDetails(rd, i_event_name, i_event_id));
                    }
                }
                else
                {
                    items.Add(new EventSearchDetails(i_event_start_date, i_event_name, i_event_id));
                }
            }
            #endregion for loop

            items.Sort((i1, i2) => DateTime.Compare(DateTime.Parse(i1.eventDate), DateTime.Parse(i2.eventDate)));
            
            return items;
        }


        public List<String> getRecurringDates(String i_recurring_type, String i_event_start_date, String i_event_end_date, String o_start, String o_end, String i_day_of_week, String i_day_of_month, String i_week_of_month, String i_month_of_year, String i_separation_count, String i_max_num_of_occurences)
        {
            List<String> dates = new List<String>();
            try
            {
                

                if (String.IsNullOrEmpty(i_separation_count))
                {
                    i_separation_count = "1";
                }

                RecurrencePattern recurrenceRule = new RecurrencePattern();
                if (i_recurring_type.Equals("D"))         /*daily recurrance*/
                {
                    recurrenceRule.Frequency = FrequencyType.Daily;
                    recurrenceRule.Interval = Convert.ToInt16(i_separation_count);
                }

                else       /*weekly, monthly, yearly recurrance*/
                {
                    if (!String.IsNullOrEmpty(i_day_of_week))   // Get the Day(s) of the week
                    {
                        List<IWeekDay> list_weekdays = new List<IWeekDay>();
                        string[] days;
                        if (i_day_of_week.Contains(","))    //multiple days
                        {
                            i_day_of_week = i_day_of_week.Substring(0, i_day_of_week.Length - 1);
                            days = i_day_of_week.Split(new char[] { ',' }); //1 for monday .. 7 for sunday
                        }
                        else//single day
                            days = new string[] { i_day_of_week };

                        foreach (string day in days)
                        {
                            switch (day)
                            {
                                case "Mon":
                                    list_weekdays.Add(new WeekDay(DayOfWeek.Monday));
                                    break;
                                case "Tue":
                                    list_weekdays.Add(new WeekDay(DayOfWeek.Tuesday));
                                    break;
                                case "Wed":
                                    list_weekdays.Add(new WeekDay(DayOfWeek.Wednesday));
                                    break;
                                case "Thu":
                                    list_weekdays.Add(new WeekDay(DayOfWeek.Thursday));
                                    break;
                                case "Fri":
                                    list_weekdays.Add(new WeekDay(DayOfWeek.Friday));
                                    break;
                                case "Sat":
                                    list_weekdays.Add(new WeekDay(DayOfWeek.Saturday));
                                    break;
                                case "Sun":
                                    list_weekdays.Add(new WeekDay(DayOfWeek.Sunday));
                                    break;
                            }
                        }
                        recurrenceRule.ByDay = list_weekdays;
                    }

                    if (i_recurring_type.Equals("W"))
                    {
                        recurrenceRule.Frequency = FrequencyType.Weekly;
                        recurrenceRule.Interval = Convert.ToInt16(i_separation_count);
                    }

                    else        /*monthly, yearly recurrance*/
                    {
                        if (!String.IsNullOrEmpty(i_week_of_month) && !i_week_of_month.Equals("0")) // 0 means null
                        {
                            List<int> weeks = new List<int>();
                            if (i_week_of_month.Contains(","))    //multiple weeks
                                foreach (string week in i_week_of_month.Split(new char[] { ',' }))
                                {
                                    weeks.Add(Convert.ToInt16(week));
                                }
                            else//single week
                                weeks.Add(Convert.ToInt16(i_week_of_month));

                            recurrenceRule.BySetPosition = weeks;
                        }

                        if (!String.IsNullOrEmpty(i_day_of_month) && !i_day_of_month.Equals("0"))
                        {
                            List<int> day_m = new List<int>();
                            if (i_day_of_month.Contains(","))    //multiple weeks
                                foreach (string day in i_day_of_month.Split(new char[] { ',' }))
                                {
                                    day_m.Add(Convert.ToInt16(day));
                                }
                            else//single week
                                day_m.Add(Convert.ToInt16(i_day_of_month));

                            recurrenceRule.ByMonthDay = day_m;
                        }

                        if (i_recurring_type.Equals("M"))
                        {
                            recurrenceRule.Frequency = FrequencyType.Monthly;
                            recurrenceRule.Interval = Convert.ToInt16(i_separation_count);
                        }

                        else if (i_recurring_type.Equals("Y"))         /*yearly recurrance*/
                        {
                            recurrenceRule.Frequency = FrequencyType.Yearly;
                            recurrenceRule.Interval = Convert.ToInt16(1);

                            if (!String.IsNullOrEmpty(i_month_of_year))
                            {
                                List<int> months = new List<int>();
                                if (i_month_of_year.Contains(","))    //multiple months
                                    foreach (string month in i_month_of_year.Split(new char[] { ',' }))
                                    {
                                        months.Add(Convert.ToInt16(month));
                                    }
                                else//single month
                                    months.Add(Convert.ToInt16(i_month_of_year));

                                recurrenceRule.ByMonthDay = months;
                            }
                        }

                    }
                }

                if (!String.IsNullOrEmpty(i_event_end_date))     //If END daTE IS SPecifiED
                {
                    recurrenceRule.Until = DateTime.Parse(i_event_end_date);
                }

                Event e = new Event
                {
                    DtStart = new CalDateTime(DateTime.Parse(i_event_start_date)),
                    DtEnd = new CalDateTime(DateTime.Parse(i_event_start_date)),
                };

                e.RecurrenceRules = new List<IRecurrencePattern> { recurrenceRule };
                HashSet<Occurrence> hs = e.GetOccurrences(DateTime.Parse(o_start).AddDays(-1), DateTime.Parse(o_end).AddDays(1));

                foreach (Occurrence o in hs)
                {
                    dates.Add(o.Period.StartTime.Date.ToShortDateString());

                }

                return dates;
            }
            catch(Exception ex)
            {
                DBConnect db = new DBConnect();
                db.handleException(ex);
                return dates;
            }
        }

        // GET: Samithi list
        private List<SelectListItem> GetSamiti(String selection)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            Console.WriteLine("selection value" + selection);
            
            DBConnect db = new DBConnect();
            string query = "select s.ssm_samithi_id,s.ssm_samithi_nm from sssso_samithi s, sssso_sm_dt_zn_ct_st_cn conn where conn.s6t_samithi_id = s.ssm_samithi_id and conn.s6t_district_id = '" + selection + "' ";
            List<List<Object>> list = db.executeQuery(query);
            SelectListItem sli;

            foreach (List<Object> record in list)
            {
                sli = new SelectListItem();
                sli.Text = record[1].ToString();
                sli.Value = record[0].ToString();
                items.Add(sli);
            }



            return items;
        }
        [HttpPost]
        public ActionResult GetSamithiList(string selectedValue)
        {
            return Json(GetSamiti(selectedValue), JsonRequestBehavior.AllowGet);
        }

        // GET: District list
        public ActionResult GetDistrictList()
        {
            return Json(GetDistricts(), JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> GetDistricts()
        {
            
            List<SelectListItem> items = new List<SelectListItem>();
            
            items.Add(new SelectListItem() { Text = "State Level Events", Value = "0" });

            DBConnect db = new DBConnect();
            string query = "SELECT sdt_district_id,sdt_district_nm FROM sssso_district where sdt_district_id<>0";
            List<List<Object>> list = db.executeQuery(query);
            SelectListItem sli;

            foreach (List<Object> record in list)
            {
                sli = new SelectListItem();
                sli.Text = record[1].ToString();
                sli.Value = record[0].ToString();
                items.Add(sli);
            }

            return items;

        }

        // GET: Category list
        public ActionResult GetCategoryList(String d_id, String s_id)
        {
            return Json(GetCategories(d_id, s_id), JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> GetCategories(String d_id, String s_id)
        {

            DBConnect db = new DBConnect();
            List<SelectListItem> items = new List<SelectListItem>();
            String query = "";
            if (d_id.Equals("999"))  //All Districts and Samithies
                query = "SELECT distinct(event_type_id),event_type_name FROM sssso.event_type_dist_samithi";
            else if (d_id.Equals("0"))  //District and Samithi level events
                query = "SELECT distinct(event_type_id),event_type_name FROM sssso.event_type_dist_samithi where district_id=0 and samithi_id=0";
            else    //district is selected
            {
                if(s_id.Equals("999"))
                    query = "SELECT distinct(event_type_id),event_type_name FROM sssso.event_type_dist_samithi where (district_id=999 and samithi_id=999) or  (district_id="+d_id+" and samithi_id=999)";
                else if(s_id.Equals("0"))
                    query = "SELECT distinct(event_type_id),event_type_name FROM sssso.event_type_dist_samithi where (district_id="+d_id+ " and samithi_id=0) or (district_id=999 and samithi_id=999)";
                else
                    query = "SELECT distinct(event_type_id),event_type_name FROM sssso.event_type_dist_samithi where (district_id=999 and samithi_id=999) or (district_id="+d_id+" and samithi_id=999) or (district_id="+d_id+" and samithi_id="+s_id+")";
            }
            //items.Add(new SelectListItem() { Text = "Balvikas", Value = "1" });
            //items.Add(new SelectListItem() { Text = "Vidya Vahini", Value = "2" });
            //items.Add(new SelectListItem() { Text = "Blood Donation", Value = "3" });
            //items.Add(new SelectListItem() { Text = "Samarpan", Value = "4" });

            List<List<Object>> list = db.executeQuery(query);
            SelectListItem sli;

            foreach (List<Object> record in list)
            {
                sli = new SelectListItem();
                sli.Text = record[1].ToString();
                sli.Value = record[0].ToString();
                items.Add(sli);
            }



            return items;

            

        }

        // GET: Festive dates list
        private List<FestiveDates> GetFestivalDates()
        {
            //the webmthod to read
            List<FestiveDates> items = new List<FestiveDates>();
            items.Add(new FestiveDates() { date = "11/13/2016", text = "Poornima" });
            items.Add(new FestiveDates() { date = "10/30/2016", text = "Diwali" });
            items.Add(new FestiveDates() { date = "11/19/2016", text = "Ladies' Day" });
            items.Add(new FestiveDates() { date = "11/23/2016", text = "Swami's Bday" });

            return items;

        }

        public ActionResult GetFestivalList()
        {
            return Json(GetFestivalDates(), JsonRequestBehavior.AllowGet);
        }

        //GetUpcomingEvents
        private List<UpcomingEvents> GetUpcomingEventList(String district_id)
        {
            List<UpcomingEvents> items = new List<UpcomingEvents>();

            //uncomment this section once the getSpecial events stored proc is ready
            List<MySqlParameter> param = new List<MySqlParameter>();

            if(district_id.Equals("999"))
                param.Add(new MySqlParameter("@i_district_id", DBNull.Value));
            else
                param.Add(new MySqlParameter("@i_district_id", district_id));
            DBConnect db = new DBConnect();

            UpcomingEvents esd;
            List<List<Object>> list = db.execStoredProcedure("getSpecialEvents", param);
            foreach (List<Object> record in list)
            {

                esd = new UpcomingEvents(record[1].ToString(), record[2].ToString(), record[0].ToString());
                items.Add(esd);
            }

            //items.Add(new UpcomingEvents("Swami's Birthday", "2016-11-23", "1"));
            //items.Add(new UpcomingEvents( "Balvikas", "2016-11-23", "1"));
            //items.Add(new UpcomingEvents( "Vidya Vahini", "2016-11-23", "1"));
            //items.Add(new UpcomingEvents( "Blood Donation", "2016-11-23", "1"));
            //items.Add(new UpcomingEvents( "Samarpan", "2016-11-23", "1"));
            //items.Add(new UpcomingEvents("Blood Donation", "2016-11-23", "1"));
            //items.Add(new UpcomingEvents("Samarpan", "2016-11-23", "1"));

            return items;

        }

        public ActionResult GetUpcomingEvents(String district_id)
        {
            return Json(GetUpcomingEventList(district_id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEventRec()
        {
            List<String> items = new List<String>();
            items.Add("FREQ=DAILY;COUNT=10");
            items.Add("FREQ=WEEKLY;BYDAY=TH");
            items.Add("FREQ=MONTHLY;BYSETPOS=2;BYDAY=SU;INTERVAL=1");

            return Json(items, JsonRequestBehavior.AllowGet);
        }


    }

    public class UpcomingEvents
    {
        public String eventname;
        public String year;
        public String link;
        public String day;
        public String month;


        public UpcomingEvents(String eventname, String date, String eventid)
        {
            DateTime eDate = DateTime.Parse(date);
            this.eventname = eventname;
            this.day = eDate.Day + "";
            this.month = eDate.ToString("MMM");
            this.year = eDate.Year + "";
            this.link = "/Home/EventDetail/" + eventid + eDate.ToString("ddMMyyyy");
        }

    }

    public class EventSearchDetails
    {
        public String eventDate;
        public String eventDesc;
        public String eventid;
        public String eventMonthYear;

        public EventSearchDetails(String eventDate, String eventDesc, String eventid)
        {
            try
            {
                DateTime eDate = DateTime.Parse(eventDate);
                //DateTime eDate = Convert.ToDateTime(eventDate);
                this.eventDate = eDate.ToString("yyyy-MM-dd");
                this.eventDesc = eventDesc;
                this.eventid = eventid;
                this.eventMonthYear = String.Format("{0} {1}", eDate.ToString("MMM"), eDate.Year);
                //this.eventMonthYear = "dsfsdfsdf";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        //public EventSearchDetails(DateTime eventDate, String eventDesc, int eventid)
        //{
        //    try
        //    {
        //        //DateTime eDate = DateTime.ParseExact(eventDate, "yyyy-MM-dd", null);
        //        //DateTime eDate = Convert.ToDateTime(eventDate);
        //        this.eventDate = eventDate.ToString("yyyy-MM-dd");
        //        this.eventDesc = eventDesc;
        //        this.eventid = eventid;
        //        //this.eventMonthYear = String.Format("{0} {1}", eventDate.ToString("MMM"), eventDate.Year);
        //        //this.eventMonthYear = "dsfsdfsdf";
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //}

        public EventSearchDetails(DateTime eventDate, String eventDesc, String eventid)
        {
            try
            {
                //DateTime eDate = DateTime.ParseExact(eventDate, "yyyy-MM-dd", null);
                //DateTime eDate = Convert.ToDateTime(eventDate);
                this.eventDate = eventDate.ToString("yyyy-MM-dd");
                this.eventDesc = eventDesc;
                this.eventid = eventid;
                this.eventMonthYear = String.Format("{0} {1}", eventDate.ToString("MMM"), eventDate.Year);
                //this.eventMonthYear = "dsfsdfsdf";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

    }

    public class FestiveDates
    {
        public String date;
        public String text;
    }

    public class SearchResultForAjax
    {
        public int draw;
        public int recordsTotal;
        public int recordsFiltered;
        public EventSearchDetails[] data;

        public SearchResultForAjax(int d, int rt, int rf, EventSearchDetails[] data)
        {
            draw = d;
            recordsFiltered = rf;
            recordsTotal = rt;
            this.data = data;

        }
    }
}