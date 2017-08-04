using SSSSO_MHG.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSSSO_MHG.Controllers
{
    public class JoinController : Controller
    {

        

        public ActionResult GetDistrictList()
        {

            List<SelectListItem> items = new List<SelectListItem>();
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

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddJoinUsDetails(String name, String email, String phone, String location, String services)
        {
            List<MySqlParameter> param = new List<MySqlParameter>();


            param.Add(new MySqlParameter("@input_name", name));
            
            param.Add(new MySqlParameter("@input_contact_no", phone));
            param.Add(new MySqlParameter("@input_email_addr", email));
            param.Add(new MySqlParameter("@input_district", location));
            param.Add(new MySqlParameter("@input_interest", services));


            DBConnect db = new DBConnect();


            List<List<Object>> list = db.execStoredProcedure("AddJoinUs", param);
            List<SelectListItem> items = new List<SelectListItem>();
            
            
            return Json(items, JsonRequestBehavior.AllowGet);
        }

    }
}