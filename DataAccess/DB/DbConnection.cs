using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class DbConnection
	{
		//mvc de sql baglanstısında zaman aşımı hatası alırsan başlat>çalıştır>cliconfg.exe dosyasını çalıştır ....etkinleştirilen iletişim kuralları TCP/IP üste olucak şekilde alttaki iki seçenekte işaret konulmadan 
		// sql connection cümlesi db yeri ip değiştiğinde sadece burda değişiklik yapmak yeterli olacaktır...
		public static readonly string connectionString = "Data Source=MIKROSERVER;Initial Catalog=MikroDB_V15_ERMA_2015;user id=webservice;password=1111x!;MultipleActiveResultSets=True;Integrated Security=False";
		public static readonly string awsString = "Data Source=etemindb.cvnhuujst0xu.eu-west-1.rds.amazonaws.com;Initial Catalog =SSPLogin;user id=eteminsa;password=q2w3e4!!; Integrated Security = False";
		/// <summary>
		/// bir bağlantı metni olusturularak yollanılıyor....
		/// </summary>
		/// <returns></returns>
		public static SqlConnection ConnectionGet()
		{
			SqlConnection con = new SqlConnection(connectionString);
			return con;
		}

		public static SqlConnection ConnectionGet(bool aws)
		{
			SqlConnection con;
			if (aws) { con = new SqlConnection(awsString); }
			else { con = new SqlConnection(connectionString); }
			return con;
		}
		/// <summary>
		/// sql connection baglantısının kapatılması.....
		/// </summary>
		/// <param name="connection"></param>
		public static void ConnectionClose(SqlConnection connection)
		{
			if (connection == null)
			{
				return;
			}
			connection.Close();
			connection.Dispose();
		}
	}
}
