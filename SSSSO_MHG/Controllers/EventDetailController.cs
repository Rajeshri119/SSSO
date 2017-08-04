using System;
using System.Collections.Generic;
using SSSSO_MHG.Models;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SSSSO_MHG.Controllers
{
    public class EventDetailController : Controller
    {
        // GET: EventDetail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPageLoadDetails(String id)
        {
            return Json(GetEventDetails(id), JsonRequestBehavior.AllowGet);
        }

        public List<EventDetails> GetEventDetails(String id)
        {
            try
            {
                List<EventDetails> eventDetailsList = new List<EventDetails>();
                String str_id = id.Substring(0, id.Length - 8);
                String date = id.Substring(id.Length - 8, 8);

                DBConnect db = new DBConnect();
                List<MySqlParameter> param = new List<MySqlParameter>();
                param.Add(new MySqlParameter("@input_event_id", str_id));
                
                List<List<Object>> list = db.execStoredProcedure("getEventDetails", param);
                String address;

                List<Object> record = list[0];
                String formatted_date = (DateTime.ParseExact(date, "ddMMyyyy", null)).ToString("dd-MM-yyyy");
                String time = (DateTime.ParseExact(record[0].ToString(), "HH:mm:ss", null)).ToString("h:mm tt")
                 + " - " + (DateTime.ParseExact(record[1].ToString(), "HH:mm:ss", null)).ToString("h:mm tt");
                if(record[5]!=null)
                    address = record[4] + "<br>" + record[5] + "<br>" + record[6] ;
                else
                    address = record[4] + "<br>"  + record[6];
                String description = record[3].ToString();
                String name = record[2].ToString();
                String contact = record[7] + " (" + record[8] + " ) <br> email id: " + record[9];
                String img_src =  record[10].ToString();
                eventDetailsList.Add(new EventDetails(formatted_date, time, address,name, description,contact,img_src));

                return eventDetailsList;
            }
            catch(Exception ex)
            {
                //exception message
                return null;
            }
        }
    }

    public class EventDetails
    {
        public String eventDate;
        public String eventTime;
        public String eventLocation;
        public String eventName;
        public String eventDescription;
        public String contact;
        public String img_src;


        public EventDetails(String eventDate, String eventTime, String loc, String name, String desc,String contact,String img)
        {
            this.eventDate = eventDate;
            this.eventTime = eventTime;
            eventLocation = loc;
            eventName = name;
            eventDescription = desc;
            this.contact = contact;
            img_src = img;
        }
    }
}