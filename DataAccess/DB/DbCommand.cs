using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
	public class DbCommand
	{
		private SqlConnection _con = null;
		private SqlCommand _com = null;
		private SqlTransaction _trans;
		//Netsisten veya benzer eski tip db kullanan programlardan kaynaklı olan türkçe karakter sorununun çözüm metodu
		public bool TestConnection()
		{
			try
			{
				using (SqlConnection conn = DataAccessLayer.DbConnection.ConnectionGet())
				{
					conn.Open();
					if (conn.State == System.Data.ConnectionState.Open)
					{
						return true;
					}
					conn.Close();
					return false;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
		static string MSTurkce(string windowsTurkce)
		{			
			return Encoding.Default.GetString(Encoding.GetEncoding(1252).GetBytes(windowsTurkce));
		}
		//Yapıcı metodlar
		internal DbCommand()
		{			
			_con = DbConnection.ConnectionGet();
			_com = _con.CreateCommand();		
		}
		internal DbCommand(string spName)
		{
			//stored procedure olusumu
			_con = DbConnection.ConnectionGet();
			KomutOlustur(spName);
		}
		internal DbCommand(string spName, SqlConnection con)
		{
			//stored procedure olusumu(farklı baglantılar ile)
			this._con = con;
			KomutOlustur(spName);
		}
		/// <summary>
		/// procedure için komut olusutma işlemi 
		/// </summary>
		/// <param name="spName"></param>
		private void KomutOlustur(string spName)
		{
			try
			{
				_com = _con.CreateCommand();
				_com.CommandText = spName;
				_com.CommandType = CommandType.StoredProcedure;
			}
			catch (SqlException ex)
			{

				//log yazıldıktan sonra eklencek ellemeyin lütfen.....
			}
		}
		/// <summary>
		/// verilen değerlere yeni bir parametere oluşturur....
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns>yeni oluşan parametre dönecek....</returns>
		internal SqlParameter AddParameter(string name, object value)
		{
			SqlParameter param = new SqlParameter(name, value);
			_com.Parameters.Add(param);
			return param;
		}
		/// <summary>
		/// parametre oluşturma işlemleri
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		/// <returns>yeni oluşan parametre dönecek....</returns>
		public SqlParameter _AddParameter(SqlParameter par)
		{
			_com.Parameters.Add(par);
			return par;
		}
		/// <summary>
		/// çıkış parametresini elde edecegiz...
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="lenght"></param>
		/// <returns>çıkış parametresini elde edecegiz....</returns>
		internal SqlParameter AddParameterOut(string name, SqlDbType type, int lenght)
		{
			SqlParameter param = new SqlParameter();
			param.ParameterName = name;

			param.SqlDbType = type;
			if (lenght > 0)
			{
				param.Size = lenght;
			}
			param.Direction = ParameterDirection.Output;
			_com.Parameters.Add(param);
			return param;
		}		
		private object IsletSkalar()
		{
			_con.Open();
			_trans = _con.BeginTransaction();
			object sonuc = null;
			try
			{
				_com.Transaction = _trans;
				_trans.Commit();
				return sonuc;
			}
			catch (SqlException ex)
			{
				//daha sonrasında bu alanda log da eklenecektir...
				_trans.Rollback();

			}
			finally
			{
				_con.Close();
			}
			return sonuc;
		}	
		internal bool IsletBool()
		{
			object sonuc = IsletSkalar();
			if (sonuc == null)
			{
				return false;
			}
			bool deger = (bool)sonuc;
			return deger;
		}		
		/// <summary>
		/// sql connection kapatma işlemi....
		/// </summary>
		internal void Temizle()
		{
			DbConnection.ConnectionClose(_con);
			_con.Dispose();
		}
		internal int Int32Islet()
		{
			object sonuc = IsletSkalar();
			if (sonuc == null)
			{
				return 0;
			}
			int deger = (int)sonuc;
			return deger;
		}		
		internal SqlDataReader IsletDataReader()
		{
			_con.Open();
			SqlDataReader read = null;
			_trans = _con.BeginTransaction();
			try
			{
				_com.Transaction = _trans;
				_trans.Commit();
				read = _com.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (SqlException e)
			{
				// loglar eklenecek ellemeyin...
				Exception y = e;
				_trans.Rollback();

			}

			return read;
		}
		/// <summary>
		/// Komut alıp geriye data dönen elle girilen okumalar
		/// </summary>
		/// <returns></returns>
		internal SqlDataReader IsletManuelReader(string komut)
		{
			SqlDataReader rdr = null;
			try
			{
				_con.Open();
				_com.CommandText = komut;
				rdr = _com.ExecuteReader();
			}
			catch(Exception ex)
			{
				//hata bloğu
			}
			return rdr;
		}
		/// <summary>
		/// Komut alıp geriye data dönmeyen elle girilen okumalar
		/// </summary>
		/// <returns></returns>
		internal int IsletManuelNonReturn(string komut)
		{
			int etkilenenSatir = 0;
			try
			{

				_con.Open();
				_com.CommandText = komut;

				etkilenenSatir = _com.ExecuteNonQuery();
			}
			catch
			{
				//hata bloğu
			}
			return etkilenenSatir;
		}		
		internal int Islet()
		{
			_con.Open();
			_trans = _con.BeginTransaction();
			int etkilenenSatir = 0;
			try
			{
				_com.Transaction = _trans;
				etkilenenSatir = _com.ExecuteNonQuery();
				_trans.Commit();
				//return etkilenenSatir;
			}
			catch (SqlException se)
			{
				

				throw new Exception("Hatayı oku", se);
			}
			finally
			{
				_con.Close();
			}
			return etkilenenSatir;
		}
		/// <summary>
		/// parametre çıkıs degerini elde etmek bool/bit olarak
		/// </summary>
		/// <param name="parametreName"></param>
		/// <returns></returns>
		internal bool OutParametreDegeriBool(string parametreName)
		{
			try
			{
				object deger = _com.Parameters[parametreName].Value;
				if (deger != null)
				{
					return Convert.ToBoolean(deger);
				}
				else
					return false;
			}
			catch (Exception ex)
			{

				return false;
			}

		}
		/// <summary>
		/// parametre çıkıs degerini elde etmek int olarak
		/// </summary>
		/// <param name="parametreName"></param>
		/// <returns></returns>
		internal int OutParametreDegeriInt(string parametreName)
		{
			try
			{
				object deger = _com.Parameters[parametreName].Value;
				if (deger != null)
				{
					return (int)deger;
				}
				else
					return 0;
			}
			catch (Exception ex)
			{

				return 0;
			}

		}
		/// <summary>
		/// parametre çıkış değerini elde etmek string olarak...
		/// </summary>
		/// <param name="parametreName"></param>
		/// <returns></returns>
		internal string OutParametreDegeriString(string parametreName)
		{
			try
			{
				object deger = _com.Parameters[parametreName].Value;
				if (deger != null)
				{
					return MSTurkce(deger.ToString());
				}
				else
					return "";
			}
			catch (Exception ex) { return ""; }

		}
		/// <summary>
		/// parametre çıkış değerini elde etmek short olarak...
		/// </summary>
		/// <param name="parametreName"></param>
		/// <returns></returns>
		internal short OutParametreDegeriShort(string parametreName)
		{
			try
			{
				object deger = _com.Parameters[parametreName].Value;
				if (deger != null)
				{
					return (short)deger;
				}
				else
					return 0;
			}
			catch (Exception ex) { return 0; }
		}
		/// <summary>
		/// parametre çıkış değerini elde etmek long olarak
		/// </summary>
		/// <param name="parametreName"></param>
		/// <returns></returns>
		internal long OutParametreDegeriLong(string parametreName)
		{
			try
			{
				object deger = _com.Parameters[parametreName].Value;
				if (deger != null)
					return (long)deger;

				else
					return 0;
			}
			catch (Exception ex) { return 0; }
		}
		/// <summary>
		/// parametre çıkış değerini elde etmek float olarak
		/// </summary>
		/// <param name="parametreName"></param>
		/// <returns></returns>
		internal float OutParametreDegeriFloat(string parametreName)
		{
			try
			{
				//int idFromString = int.Parse(outputIdParam.Value.ToString());
				object deger = _com.Parameters[parametreName].Value;
				if (deger != null)
					return Convert.ToSingle(deger);

				else
					return 0;
			}
			catch (Exception ex) { return 0; }

		}
		/// <summary>
		/// parametre çıkış değerini elde etmek float olarak
		/// </summary>
		/// <param name="parametreName"></param>
		/// <returns></returns>
		internal DateTime OutParametreDegeriDateTime(string parametreName)
		{
			try
			{
				//int idFromString = int.Parse(outputIdParam.Value.ToString());
				object deger = _com.Parameters[parametreName].Value;
				if (deger != null)
					return Convert.ToDateTime(deger);

				else
					return new DateTime();
			}
			catch (Exception ex) { return new DateTime(); }

		}
		/// <summary>
		/// db den gelen değeri string olarak elde etme....
		/// </summary>
		/// <param name="read">sqldataread</param>
		/// <param name="alan">tablodaki alan</param>
		/// <returns>string</returns>
		internal static string StringGetir(SqlDataReader read, string alan)
		{
			string veri = string.Empty;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = read.GetString(indeks);
			}
			return MSTurkce(veri);
		}
		/// <summary>
		/// db den gelen değeri short olarak elde etmek....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static short ShortGetir(SqlDataReader read, string alan)
		{

			short veri = -1;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = read.GetInt16(indeks);
			}
			return veri;
		}
		/// <summary>
		/// db den dönen değeri int olarak elde etmek.....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static int Int32Getir(SqlDataReader read, string alan)
		{
			int veri = -1;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = read.GetInt32(indeks);
			}
			return veri;
		}
		/// <summary>
		/// db dönen degeri long olarak elde etmek....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static long LongGetir(SqlDataReader read, string alan)
		{
			long veri = -1;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = read.GetInt64(indeks);
			}
			return veri;
		}
		/// <summary>
		/// db den gelen veriyi datetime olarak elde etmek....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static DateTime TarihGetir(SqlDataReader read, string alan)
		{
			try
			{
				DateTime veri = DateTime.MinValue;
				int indeks = read.GetOrdinal(alan);
				if (!read.IsDBNull(indeks))
				{
					veri = read.GetDateTime(indeks);
				}
				return veri;
			}
			catch (Exception ex)
			{
				
				throw;
			}
		}
		/// <summary>
		/// dbden dönen değeri bool olarak elde etmek....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static bool BoolGetir(SqlDataReader read, string alan)
		{
			bool veri = false;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = read.GetBoolean(indeks);
			}
			return veri;
		}
		/// <summary>
		/// db den byte dönen degeri byte elde etmek....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static byte ByteGetir(SqlDataReader read, string alan)
		{
			byte veri = 0;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = read.GetByte(indeks);
			}
			return veri;
		}
		/// <summary>
		/// db den sbyte değeri sbyte elde etmek.....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static sbyte SByteGetir(SqlDataReader read, string alan)
		{
			sbyte veri = -1;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = (sbyte)read[indeks];
			}
			return veri;
		}
		/// <summary>
		/// db den dönen değeri decimal olarak elde etmek....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static decimal DecimalGetir(SqlDataReader read, string alan)
		{
			decimal veri = -1;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = read.GetDecimal(indeks);
			}
			return veri;
		}
		/// <summary>
		/// db den dönen degeri double elde etmek....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static char CharGetir(SqlDataReader read, string alan)
		{
			char veri = '\0';
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{

				string veris = read.GetValue(indeks).ToString();

				veri = veris.ToCharArray()[0];
			}
			return veri;
		}
		internal static byte[] ByteArrayGetir(SqlDataReader read, string alan)
		{
			byte[] veri = null;
			
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = (byte[])read.GetValue(indeks);
			}
			return veri;
		}
		internal static double DoubleGetir(SqlDataReader read, string alan)
		{
			double veri = -1;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = read.GetDouble(indeks);
			}
			return veri;
		}
		/// <summary>
		/// db den dönen float değeri elde etmek....
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static float FloatGetir(SqlDataReader read, string alan)
		{
			float veri = -1;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = Convert.ToSingle(read[indeks]);
			}
			return veri;
		}
		/// <summary>
		/// db den değeri guid elde etmek ......
		/// </summary>
		/// <param name="read"></param>
		/// <param name="alan"></param>
		/// <returns></returns>
		internal static Guid GuidGetir(SqlDataReader read, string alan)
		{
			Guid veri = System.Guid.Empty;
			int indeks = read.GetOrdinal(alan);
			if (!read.IsDBNull(indeks))
			{
				veri = read.GetGuid(indeks);
			}
			return veri;
		}
	}
}
