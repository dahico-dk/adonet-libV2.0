using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DB
{
	public class Helper
	{

		//Web.Config ya da App.Config'den DB bilgilerini ya da başka bilgileri çekmek için. Eğer istenirse hata log kodlarıda bu classtan goturulebılır.

		private static string _Provider;
		private static string _ConnectionString;
		private readonly static string _SiteName;
		private static string _datasource;
		private static string _katalog;
		private static string _userid;
		private static string _pass;

		static Helper()
		{
			_Provider = ConfigurationManager.AppSettings["Provider"];
			_ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

		}
		public static string connectionstring()
		{

			_datasource = ConfigurationManager.AppSettings["DataSource"];
			_katalog = ConfigurationManager.AppSettings["Katalog"];
			_userid = ConfigurationManager.AppSettings["userid"];
			_pass = ConfigurationManager.AppSettings["pass"];
			return @"Data Source=" + _datasource + ";Initial Catalog=" + _katalog + ";User ID=" + _userid + ";Password=" + _pass + ";";
		}
	}
}
