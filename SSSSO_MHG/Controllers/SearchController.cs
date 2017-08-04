using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Data;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;


namespace SSSSO_MHG.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }
        MySql.Data.MySqlClient.MySqlConnection conn;
        string myConnectionString;


        string server = "127.0.0.1";//; "219.91.228.170";
        string database = "sssso";
        string uid = "root";// "SAURABH";
        string password = "";// "saibaba123456";
        //string myConnectionString;

        #region

        List<string> StopWordList = new List<string>()
            {
"a",
"able",
"about",
"above",
"according",
"accordingly",
"across",
"actually",
"after",
"afterwards",
"again",
"against",
"ain't",
"all",
"allow",
"allows",
"almost",
"alone",
"along",
"already",
"also",
"although",
"always",
"am",
"among",
"amongst",
"an",
"and",
"another",
"any",
"anybody",
"anyhow",
"anyone",
"anything",
"anyway",
"anyways",
"anywhere",
"apart",
"appear",
"appreciate",
"appropriate",
"are",
"aren't",
"around",
"as",
"a's",
"aside",
"ask",
"asking",
"associated",
"at",
"available",
"away",
"awfully",
"be",
"became",
"because",
"become",
"becomes",
"becoming",
"been",
"before",
"beforehand",
"behind",
"being",
"believe",
"below",
"beside",
"besides",
"best",
"better",
"between",
"beyond",
"both",
"brief",
"but",
"by",
"came",
"can",
"cannot",
"cant",
"can't",
"cause",
"causes",
"certain",
"certainly",
"changes",
"clearly",
"c'mon",
"co",
"com",
"come",
"comes",
"concerning",
"consequently",
"consider",
"considering",
"contain",
"containing",
"contains",
"corresponding",
"could",
"couldn't",
"course",
"c's",
"currently",
"definitely",
"described",
"despite",
"did",
"didn't",
"different",
"do",
"does",
"doesn't",
"doing",
"done",
"don't",
"down",
"downwards",
"during",
"each",
"edu",
"eg",
"eight",
"either",
"else",
"elsewhere",
"enough",
"entirely",
"especially",
"et",
"etc",
"even",
"ever",
"every",
"everybody",
"everyone",
"everything",
"everywhere",
"ex",
"exactly",
"example",
"except",
"far",
"few",
"fifth",
"first",
"five",
"followed",
"following",
"follows",
"for",
"former",
"formerly",
"forth",
"four",
"from",
"further",
"furthermore",
"get",
"gets",
"getting",
"given",
"gives",
"go",
"goes",
"going",
"gone",
"got",
"gotten",
"greetings",
"had",
"hadn't",
"happens",
"hardly",
"has",
"hasn't",
"have",
"haven't",
"having",
"he",
"he'd",
"he'll",
"hello",
"help",
"hence",
"her",
"here",
"hereafter",
"hereby",
"herein",
"here's",
"hereupon",
"hers",
"herself",
"he's",
"hi",
"him",
"himself",
"his",
"hither",
"hopefully",
"how",
"howbeit",
"however",
"how's",
"i",
"i'd",
"ie",
"if",
"ignored",
"i'll",
"i'm",
"immediate",
"in",
"inasmuch",
"inc",
"indeed",
"indicate",
"indicated",
"indicates",
"inner",
"insofar",
"instead",
"into",
"inward",
"is",
"isn't",
"it",
"it'd",
"it'll",
"its",
"it's",
"itself",
"i've",
"just",
"keep",
"keeps",
"kept",
"know",
"known",
"knows",
"last",
"lately",
"later",
"latter",
"latterly",
"least",
"less",
"lest",
"let",
"let's",
"like",
"liked",
"likely",
"little",
"look",
"looking",
"looks",
"ltd",
"mainly",
"many",
"may",
"maybe",
"me",
"mean",
"meanwhile",
"merely",
"might",
"more",
"moreover",
"most",
"mostly",
"much",
"must",
"mustn't",
"my",
"myself",
"name",
"namely",
"nd",
"near",
"nearly",
"necessary",
"need",
"needs",
"neither",
"never",
"nevertheless",
"new",
"next",
"nine",
"no",
"nobody",
"non",
"none",
"noone",
"nor",
"normally",
"not",
"nothing",
"novel",
"now",
"nowhere",
"obviously",
"of",
"off",
"often",
"oh",
"ok",
"okay",
"old",
"on",
"once",
"one",
"ones",
"only",
"onto",
"or",
"other",
"others",
"otherwise",
"ought",
"our",
"ours",
"ourselves",
"out",
"outside",
"over",
"overall",
"own",
"particular",
"particularly",
"per",
"perhaps",
"placed",
"please",
"plus",
"possible",
"presumably",
"probably",
"provides",
"que",
"quite",
"qv",
"rather",
"rd",
"re",
"really",
"reasonably",
"regarding",
"regardless",
"regards",
"relatively",
"respectively",
"right",
"said",
"same",
"saw",
"say",
"saying",
"says",
"second",
"secondly",
"see",
"seeing",
"seem",
"seemed",
"seeming",
"seems",
"seen",
"self",
"selves",
"sensible",
"sent",
"serious",
"seriously",
"seven",
"several",
"shall",
"shan't",
"she",
"she'd",
"she'll",
"she's",
"should",
"shouldn't",
"since",
"six",
"so",
"some",
"somebody",
"somehow",
"someone",
"something",
"sometime",
"sometimes",
"somewhat",
"somewhere",
"soon",
"sorry",
"specified",
"specify",
"specifying",
"still",
"sub",
"such",
"sup",
"sure",
"take",
"taken",
"tell",
"tends",
"th",
"than",
"thank",
"thanks",
"thanx",
"that",
"thats",
"that's",
"the",
"their",
"theirs",
"them",
"themselves",
"then",
"thence",
"there",
"thereafter",
"thereby",
"therefore",
"therein",
"theres",
"there's",
"thereupon",
"these",
"they",
"they'd",
"they'll",
"they're",
"they've",
"think",
"third",
"this",
"thorough",
"thoroughly",
"those",
"though",
"three",
"through",
"throughout",
"thru",
"thus",
"to",
"together",
"too",
"took",
"toward",
"towards",
"tried",
"tries",
"truly",
"try",
"trying",
"t's",
"twice",
"two",
"un",
"under",
"unfortunately",
"unless",
"unlikely",
"until",
"unto",
"up",
"upon",
"us",
"use",
"used",
"useful",
"uses",
"using",
"usually",
"value",
"various",
"very",
"via",
"viz",
"vs",
"want",
"wants",
"was",
"wasn't",
"way",
"we",
"we'd",
"welcome",
"well",
"we'll",
"went",
"were",
"we're",
"weren't",
"we've",
"what",
"whatever",
"what's",
"when",
"whence",
"whenever",
"when's",
"where",
"whereafter",
"whereas",
"whereby",
"wherein",
"where's",
"whereupon",
"wherever",
"whether",
"which",
"while",
"whither",
"who",
"whoever",
"whole",
"whom",
"who's",
"whose",
"why",
"why's",
"will",
"willing",
"wish",
"with",
"within",
"without",
"wonder",
"won't",
"would",
"wouldn't",
"yes",
"yet",
"you",
"you'd",
"you'll",
"your",
"you're",
"yours",
"yourself",
"yourselves",
"you've",
"zero"
            };
        #endregion



        public void openConnection()
        {
            //myConnectionString = "Host=" + server + ";" + "DataBase=" +
            //database + ";" + "Protocol=TCP;" + "Port=3306;" + "User id=" + uid + ";" + "Password=" + password + ";Convert Zero Datetime=True;";


            //server = "219.91.228.170";
            //database = "sssso";
            //uid = "ravi";
            //password = "saibaba123456";

            //server = "localhost";
            //uid = "root";
            //password = "root";
            //string connectionString;
            //connectionString = "Host=" + server + ";" + "DataBase=" +
            //database + ";" + "Protocol=TCP;" + "Port=3306;" + "User id=" + uid + ";" + "Password=" + password + ";";
            string connectionString = ConfigurationManager.ConnectionStrings["saiserver"].ToString();

            //myConnectionString = "Host=219.91.228.170;uid=SAURABH;" + "Port=99;"  + "Protocol=TCP;"+
            //    "pwd=saibaba123456;database=sssso;";

            try
            {
                conn = new MySql.Data.MySqlClient.MySqlConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }
        public ActionResult Search(FormCollection fc)
        {
            string keyword = fc["keyword"];
            List<List<SSSSO_MHG.Models.SearchClass>> finalList = new List<List<Models.SearchClass>>()
            {
               SearchResult(keyword),SearchEvents(keyword)
            };
            return View(finalList);
        }

        public List<SSSSO_MHG.Models.SearchClass> SearchResult(string SearchTextBox)
        {

            try
            {
                string keyWord = SearchTextBox;

                List<Models.SearchClass> sl = new List<Models.SearchClass>() { };
                if (!string.IsNullOrEmpty(keyWord))
                {


                    MySql.Data.MySqlClient.MySqlCommand selectCommand = new MySql.Data.MySqlClient.MySqlCommand();
                    MySql.Data.MySqlClient.MySqlDataAdapter sda = new MySql.Data.MySqlClient.MySqlDataAdapter();
                    DataSet ds = new DataSet();
                    DataTable dt = new DataTable();


                    var wordPattern = new Regex(@"\w+");
                    MatchCollection wordsInString = wordPattern.Matches(keyWord);
                    List<string> wordsList = wordsInString.Cast<Match>().ToList().Select(f => f.Value).ToList();

                    //remove all stope words from the InputType string
                    wordsList.RemoveAll(item => StopWordList.Contains(item));



                    foreach (string word in wordsList)
                    {
                        //BootstrapIntroduction.Models.DBConnect dbconn = new BootstrapIntroduction.Models.DBConnect();
                        openConnection();
                        selectCommand = new MySql.Data.MySqlClient.MySqlCommand(@"SELECT * FROM sssso.search_table where Keyword='" + word + "';", conn);

                        sda.SelectCommand = selectCommand;

                        sda.Fill(ds);

                        //var x=dbconn.executeQuery("SELECT * FROM sssso.search_table where Keyword='" + word + "';");

                        dt = ds.Tables[0];

                        //var res = (from d in dt.AsEnumerable()
                        //           where d.Field<string>("Keyword").ToLower().Contains(word.ToLower().Trim())
                        //           select d);




                        foreach (DataRow item1 in dt.Rows)
                        {
                            int idFromTable = item1.Field<int>("ID");
                            string keywordFromTable = item1.Field<string>("Keyword");
                            List<string> descFromTable = item1.Field<string>("Description").Split(';').ToList();
                            List<string> urlFromTable = item1.Field<string>("Url").Split(';').ToList();
                            List<string> titleFromTable = item1.Field<string>("Title").Split(';').ToList();
                            for (int i = 0; i < urlFromTable.Count; i++)
                            {
                                if (!sl.Select(ob => ob.URL).ToList<string>().Contains(urlFromTable[i]))
                                {
                                    sl.Add(new Models.SearchClass() { ID = idFromTable, Keyword = keywordFromTable, Count = item1.Field<int>("Count"), Description = descFromTable[i], URL = urlFromTable[i], Title = titleFromTable[i] });
                                }
                            }
                        }
                    }
                }
                return sl;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<SSSSO_MHG.Models.SearchClass> SearchEvents(string keyword)
        {
            try
            {

                List<Models.SearchClass> sl = new List<Models.SearchClass>() { };
                if (!string.IsNullOrEmpty(keyword))
                {
                    //searching the old events table as we dont have any data in the new table now.
                    //will have to write a stored procedure instead of selecting all the data and processing it in the code.

                    openConnection();


                    //DataTable dt = ReadExcelFile(@"F:\dkit\EventsSample.csv");


                    var wordPattern = new Regex(@"\w+");
                    MatchCollection wordsInString = wordPattern.Matches(keyword);
                    List<string> wordsList = wordsInString.Cast<Match>().ToList().Select(f => f.Value).ToList();

                    //remove all stope words from the InputType string
                    wordsList.RemoveAll(item => StopWordList.Contains(item));


                    foreach (string word in wordsList)
                    {
                        MySql.Data.MySqlClient.MySqlCommand selectCommand = new MySql.Data.MySqlClient.MySqlCommand(@"call  searchEventTable('" + word + "')", conn);
                        //MySql.Data.MySqlClient.MySqlCommand selectCommand = new MySql.Data.MySqlClient.MySqlCommand(@"SELECT * FROM sssso.sssso_events ", conn);

                        MySql.Data.MySqlClient.MySqlDataAdapter sda = new MySql.Data.MySqlClient.MySqlDataAdapter();
                        sda.SelectCommand = selectCommand;
                        DataSet ds = new DataSet();
                        sda.Fill(ds);

                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];

                        foreach (DataRow item1 in dt.Rows)
                        {
                            var desc = item1.Field<string>("event_desc");
                            desc = Regex.Replace(desc, @"<[^>]+>|&nbsp;", "").Trim();
                            desc = Regex.Replace(desc, @"\s{2,}", " ");
                            sl.Add(new Models.SearchClass() { ID = Convert.ToInt32(item1.Field<double>("event_id")), Description = desc, Title = item1.Field<string>("event_name"), URL = item1.Field<string>("address_line1").ToString(), StartDate = item1.Field<DateTime>("event_start_date") });
                        }


                        //var res = from d in dt.AsEnumerable()
                        //          where d.ItemArray.Aggregate((x, y) => x + " " + y).ToString().ToLower().Contains(word.ToLower().Trim())
                        //          select d;


                        //foreach (var item1 in res)
                        //{
                        //    //                        sl.Add(new Models.SearchClass() { Description = item1.Field<string>("Description") + " at " + item1.Field<string>("Address1"), Title = item1.Field<string>("EventName"), URL = item1.Field<string>("StartDate") + " - " + item1.Field<string>("EndDate") });
                        //    if (!sl.Select(ob => ob.Title).ToList<string>().Contains(item1.Field<string>("set_nm")))
                        //    {
                        //        sl.Add(new Models.SearchClass() { Description = item1.Field<string>("set_desc"), Title = item1.Field<string>("set_nm"), URL = item1.Field<DateTime>("set_start_dt").ToString() + " - " + item1.Field<DateTime>("set_end_dt").ToString() });
                        //    }
                        //}
                    }
                    conn.Close();

                }
                return sl;
            }
            catch (Exception)
            {
                return null;
            }

        }

        private DataTable ReadExcelFile(string path)
        {

            string CSVFilePathName = path;
            string[] Lines = System.IO.File.ReadAllLines(CSVFilePathName);
            string[] Fields;
            Fields = Lines[0].Split(new char[] { ',' });
            int Cols = Fields.GetLength(0);
            DataTable dt = new DataTable();
            //1st row must be column names; force lower case to ensure matching later on.
            for (int i = 0; i < Cols; i++)
                dt.Columns.Add(Fields[i].ToLower(), typeof(string));
            DataRow Row;
            for (int i = 1; i < Lines.GetLength(0); i++)
            {
                Fields = Lines[i].Split(new char[] { ',' });
                Row = dt.NewRow();
                for (int f = 0; f < Cols; f++)
                    Row[f] = Fields[f];
                dt.Rows.Add(Row);
            }


            return dt;
        }

    }
    
}