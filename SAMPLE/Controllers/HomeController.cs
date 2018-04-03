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
				//parametre listesinin yaratılması-Creating parameter lists
				List<SqlParameter> paramlist = new List<SqlParameter>() {
				new SqlParameter("id","42"),				
				};
				List<Core.SampleCore> samplelist = new List<Core.SampleCore>();				
				//Stored Procedure ile data çekilmesi-Using Stored procedures				
				samplelist = db.Read<Core.SampleCore>("sp_TestProc", paramlist);//42 idli satıra select
				
				//Raw Query ile data çekilmesi-Pull data with raw query
				string query = "Select * from SampleTable where ID=@id";				
				samplelist = db.ReadManuel<Core.SampleCore>(query, paramlist);

				//Geriye data dönmeyen işlemler(Create,Update,Delete)-Database transactions with no data returning 
				//CUD metodu geriye true ya da false döner. Buradan işlemin başarılı olup olmadığı takip edilebilir. CUD method returns true or false depending upon succession of transaction
				bool validate = db.CUD("sp_SatirSil", paramlist);//42 idli satiri sil

				//Raw query ile işlemler. Transactions with raw query
				paramlist.Clear();
				string insertquery="Insert into TB_Sample values @val1,@val2)";
				paramlist.Add(new SqlParameter("42", "So Long, and Thanks for All the Fish "));
				validate = db.CUDManuel(insertquery, paramlist);//ilk satır insert
				paramlist.Clear();
				//CUDManuel metodu tıpkı CUD gibi geriye true ya da false döner. Tek farkı sorgunun elle girilmiş olmasıdır.CUDmanuel method returns true or false depending upon succession of transaction CUD method. Only difference is raw query.
				paramlist.Add(new SqlParameter("13", "GNU Terry Pratchett"));
				validate = db.CUDManuel(insertquery, paramlist);//ikinci satir insert				
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