using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core;

namespace adonet_libV2._0.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			using (var db=new DataLayer.DataAccess())//IDisposable
			{
				List<SqlParameter> paramlist = new List<SqlParameter>();
				List<Core.SampleCore> samplelist = new List<Core.SampleCore>();
				//parametre listesinin yaratılması-Creating parameter lists
				paramlist.Add(new SqlParameter("name","value"));//1st parameter (sample)
				paramlist.Add(new SqlParameter("name", "value"));//2nd parameter (sample)


				//Stored Procedure ile data çekilmesi-Using Stored procedures
				samplelist = db.Read<Core.SampleCore>("sp_TestProc", paramlist);

				//refresh lists
				paramlist.Clear(); samplelist.Clear();//listeleri boşaltalım-Clearing lists

				//Raw Query ile data çekilmesi-Pull data with raw query
				string query = "Select * from SampleTable where ID=@id";
				paramlist.Add(new SqlParameter("@id", 42));//1st parameter(sample)
				samplelist = db.ReadManuel<Core.SampleCore>(query, paramlist);

				//Geriye data dönmeyen işlemler(Create,Update,Delete)-Database transactions with no data returning 
				paramlist.Clear();//listeleri boşaltalım-Clearing lists

				paramlist.Add(new SqlParameter("@id", 42));//42 idli satırı silelim mesela-Deleting row which have 42 as id

				//CUD metodu geriye true ya da false döner. Buradan işlemin başarılı olup olmadığı takip edilebilir. CUD method returns true or false depending upon succession of transaction
				bool validate = db.CUD("sp_SatirSil", paramlist);
				paramlist.Clear();
				
				
				//Raw query ile işlemler. Transactions with raw query
				string insertquery="Insert into TB_Sample values @val1,@val2)";
				paramlist.Add(new SqlParameter("42", "So Long, and Thanks for All the Fish "));//1st parameter (sample)
				paramlist.Add(new SqlParameter("13", "GNU Terry Pratchett"));//2nd parameter (sample)

				//CUDManuel metodu tıpkı CUD gibi geriye true ya da false döner. Tek farkı sorgunun elle girilmiş olmasıdır.CUDmanuel method returns true or false depending upon succession of transaction CUD method. Only difference is raw query.
				validate = db.CUDManuel(insertquery, paramlist);
			}
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}