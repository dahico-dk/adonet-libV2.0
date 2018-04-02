using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DB;

namespace DataAccessLayer
{
	public class DbConnection
	{
		//mvc de sql baglanstısında zaman aşımı hatası alırsan başlat>çalıştır>cliconfg.exe dosyasını çalıştır ....etkinleştirilen iletişim kuralları TCP/IP üste olucak şekilde alttaki iki seçenekte işaret konulmadan 
		// sql connection cümlesi db yeri ip değiştiğinde sadece burda değişiklik yapmak yeterli olacaktır...
		public static string connectionString = "";
		
		/// <summary>
		/// bir bağlantı metni olusturularak yollanılıyor....
		/// </summary>
		/// <returns></returns>
		public static SqlConnection ConnectionGet()
		{
			connectionString = Helper.connectionstring();
			 SqlConnection con = new SqlConnection(connectionString);
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
