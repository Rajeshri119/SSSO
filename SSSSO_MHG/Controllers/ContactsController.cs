using SSSSO_MHG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SSSSO_MHG.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Contacts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStateDetails()
        {
            return Json(GetStateLevelDetails(), JsonRequestBehavior.AllowGet);
        }

        public List<Coordinators> GetStateLevelDetails()
        {
            List<Coordinators> result = new List<Coordinators>();
            Coordinators c;
            try
            {
                DBConnect db = new DBConnect();
                List<List<Object>> list = db.executeQuery("SELECT `contact_us`.`contact_designation`,`contact_us`.`contact_name`,`contact_us`.`contact_email`,`contact_us`.`contact_mobile` FROM `sssso`.`contact_us` where `contact_us`.`district_id`= 0 and `contact_us`.`samithi_id`= 0 and `contact_us`.`contact_active`= 1");

                //throw new Exception(); //supraja : comment this
                int rank = 0;
                foreach (List<Object> record in list)
                {
                    //MessageBox.Show(record[20].ToString() + " " + record[8].ToString() + " " + Convert.ToInt32(record[0]));

                    rank++;
                    c = new Coordinators(rank,record[0].ToString(),record[1].ToString(),record[2].ToString(), record[3].ToString());
                    result.Add(c);
                    
                }
            }
            catch (Exception ex)
            {
                ////handle
                //result.Add(new Coordinators(1,"State President", "Mr  Ramesh D Sawant", "rameshd.sawant@ssssoindia.org", "9820063909"));
                //result.Add(new Coordinators(2," State Vice President", "Mr P Dharvatkar", "pdharvatker@gmail.com", "9820063909"));
                //result.Add(new Coordinators(3," State Vice Presidentt", "Mr Murali Jaju", "murli.jaju@ssssooindia.org", "9820063909"));
                //result.Add(new Coordinators(4," State Vice President", "Mr  S K Satpute", "shriramk.satpute@ssssoindia.org", "9820063909"));
                //result.Add(new Coordinators(5,"State Youth Cordinator (Gents)", "Mr. Dharmesh Vaidya", "dharmeshvaidya@gmail.com", "9820063909"));

                
                //return result;
            }
            return result;
        }

        public ActionResult GetDistrictDetails(string district_id)
        {
            return Json(GetDistrictLevelDetails(district_id), JsonRequestBehavior.AllowGet);
        }

        public List<Coordinators> GetDistrictLevelDetails(string district_id)
        {
            List<Coordinators> result = new List<Coordinators>();
            Coordinators c;
            try
            {
                DBConnect db = new DBConnect();
                List<List<Object>> list = db.executeQuery("SELECT `contact_us`.`contact_designation`,`contact_us`.`contact_name`,`contact_us`.`contact_email`,`contact_us`.`contact_mobile` FROM `sssso`.`contact_us` where `contact_us`.`district_id`= "+ district_id + " and samithi_id=0 and `contact_us`.`contact_active`= 1");

                //throw new Exception(); //supraja : comment this

                int rank = 0;
                foreach (List<Object> record in list)
                {
                    //MessageBox.Show(record[20].ToString() + " " + record[8].ToString() + " " + Convert.ToInt32(record[0]));

                    rank++;
                    //MessageBox.Show(record[20].ToString() + " " + record[8].ToString() + " " + Convert.ToInt32(record[0]));


                    c = new Coordinators(rank, record[0].ToString(), record[1].ToString(), record[2].ToString(), record[3].ToString());
                    result.Add(c);

                }
            }
            catch (Exception ex)
            {
                //handle
                //result.Add(new Coordinators(1, "State President", "Mr  Ramesh D Sawant", "rameshd.sawant@ssssoindia.org", "9820063909"));
                //result.Add(new Coordinators(2, " State Vice President", "Mr P Dharvatkar", "pdharvatker@gmail.com", "9820063909"));
                //result.Add(new Coordinators(3, " State Vice President", "Mr Murali Jaju", "murli.jaju@ssssooindia.org", "9820063909"));
                //result.Add(new Coordinators(4, " State Vice President", "Mr  S K Satpute", "shriramk.satpute@ssssoindia.org", "9820063909"));
                //result.Add(new Coordinators(5, "State Youth Cordinator (Gents)", "Mr. Dharmesh Vaidya", "dharmeshvaidya@gmail.com", "9820063909"));


                //return result;
            }
            return result;
        }

        public ActionResult GetSamithiDetails(string district_id,string samithi_id)
        {
            return Json(GetSamithiLevelDetails(district_id,samithi_id), JsonRequestBehavior.AllowGet);
        }

        public List<Coordinators> GetSamithiLevelDetails(string district_id,string samithi_id)
        {
            List<Coordinators> result = new List<Coordinators>();
            Coordinators c;
            try
            {
                DBConnect db = new DBConnect();
                string query;
                if (samithi_id == "0")
                {
                    query = "SELECT  `contact_us`.`contact_designation`,`contact_us`.`contact_name`,`contact_us`.`contact_email`,`contact_us`.`contact_mobile` FROM `sssso`.`contact_us` where `contact_us`.`district_id`= " + district_id + "  and  `contact_us`.`samithi_id`<>0 and `contact_us`.`contact_active`= 1";
                }
                else
                    query = "SELECT `contact_us`.`contact_designation`,`contact_us`.`contact_name`,`contact_us`.`contact_email`,`contact_us`.`contact_mobile` FROM `sssso`.`contact_us` where `contact_us`.`district_id`= " + district_id + " and samithi_id = " + samithi_id + " and `contact_us`.`contact_active`= 1";


                List<List<Object>> list = db.executeQuery(query);

                //throw new Exception(); //supraja : comment this

                int rank = 0;
                foreach (List<Object> record in list)
                {
                    //MessageBox.Show(record[20].ToString() + " " + record[8].ToString() + " " + Convert.ToInt32(record[0]));

                    rank++;
                    //MessageBox.Show(record[20].ToString() + " " + record[8].ToString() + " " + Convert.ToInt32(record[0]));


                    c = new Coordinators(rank, record[0].ToString(), record[1].ToString(), record[2].ToString(), record[3].ToString());
                    result.Add(c);

                }
            }
            catch (Exception ex)
            {
                //handle
                //result.Add(new Coordinators(1, "State President", "Mr  Ramesh D Sawant", "rameshd.sawant@ssssoindia.org", "9820063909"));
                //result.Add(new Coordinators(2, " State Vice President", "Mr P Dharvatkar", "pdharvatker@gmail.com", "9820063909"));
                //result.Add(new Coordinators(3, " State Vice President", "Mr Murali Jaju", "murli.jaju@ssssooindia.org", "9820063909"));
                //result.Add(new Coordinators(4, " State Vice President", "Mr  S K Satpute", "shriramk.satpute@ssssoindia.org", "9820063909"));
                //result.Add(new Coordinators(5, "State Youth Cordinator (Gents)", "Mr. Dharmesh Vaidya", "dharmeshvaidya@gmail.com", "9820063909"));


                //return result;
            }
            return result;
        }

    }

    public class Coordinators
    {
        public int index;
        public string designation;
        public string name;
        public string email;
        public string phone;

        public Coordinators(int i,string d, string n, string e, string p)
        {
            index = i;
            designation = d;
            name = n;
            email = e;
            phone = p;
        }

    }
}