using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	public class DataAccess : IDisposable
	{


		/// <summary>
		/// Returns data from specified stored procedure with defined parameters in given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="procName">Name of the proc.</param>
		/// <param name="param">The parameter list</param>
		/// <returns>Given Type</returns>
		public List<T> Read<T>(string procName, List<SqlParameter> param) where T : new()
		{
			DataAccessLayer.DbCommand command = new DataAccessLayer.DbCommand(procName);
			foreach (var item in param) { command._AddParameter(item); }
			List<T> genericlist = new List<T>();
			SqlDataReader read = null;
			try
			{
				read = command.IsletDataReader();
				while (read.Read()) { genericlist.Add(DataLoad<T>(read)); }
			}
			catch (Exception)
			{
				//LOG Logic
			}
			finally
			{
				//Memory allocation için command ve read nesnelerini yokediyoruz.
				read.Dispose();
				read.Close();
				command.Temizle();
			}
			
			return genericlist;

		}
		/// <summary>
		/// Returns data with specified sql query with defined parameters in given type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query">The sql query.</param>
		/// <param name="param">The parameter list</param>
		/// <returns>Given Type</returns>
		public List<T> ReadManuel<T>(string query, List<SqlParameter> param) where T : new()
		{
			DataAccessLayer.DbCommand command = new DataAccessLayer.DbCommand();
			foreach (var item in param) { command._AddParameter(item); }
			List<T> genericlist = new List<T>();
			SqlDataReader read = null;
			try
			{
				read = command.IsletManuelReader(query);
				while (read.Read()) { genericlist.Add(DataLoad<T>(read)); }
			}
			catch (Exception)
			{
				//LOG Logic
			}
			finally
			{
				//Memory allocation için command ve read nesnelerini yokediyoruz.
				read.Dispose();
				read.Close();
				command.Temizle();
			}
			return genericlist;
		}
		/// <summary>
		/// Create, Update or Deletes with using specified stored proc.
		/// </summary>
		/// <param name="procName">Name of the procedure</param>
		/// <param name="param">The parameter list</param>
		/// <returns>Booelan</returns>
		public bool CUD(string procName, List<SqlParameter> param)
		{
			DataAccessLayer.DbCommand command = new DataAccessLayer.DbCommand(procName);
			foreach (var item in param) { command._AddParameter(item); }
			int x = 0;
			try { x = command.Islet(); }
			catch (Exception)
			{
				//LOG Logic
			}
			finally { command.Temizle(); }
			return x > 0;
		}
		/// <summary>
		/// Create, Update or Deletes with using specified sql query.
		/// </summary>
		/// <param name="procName">Name of the procedure</param>
		/// <param name="param">The parameter list</param>
		/// <returns>Boolean</returns>
		public bool CUDManuel(string query, List<SqlParameter> param)
		{
			DataAccessLayer.DbCommand command = new DataAccessLayer.DbCommand();
			foreach (var item in param) { command._AddParameter(item); }
			int x = 0;
			try { x = command.IsletManuelNonReturn(query); }
			catch (Exception)
			{
				//LOG Logic
			}
			finally { command.Temizle(); }
			return x > 0;
		}
		/// <summary>
		/// Loads the data to properties of given core class which represents database table
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="read">The read.</param>
		/// <returns>SqlDataReader</returns>
		private T DataLoad<T>(SqlDataReader read) where T : new()
		{
			T t = new T(); //DB'ye göre yaratılmış Core nesnesi
			try
			{
				var prop = t.GetType();
				var proplist = prop.GetProperties();
				foreach (var item in proplist)
				{
					var ilktip = DataMatch(read, item, item.PropertyType.Name);
					var sontip = Convert.ChangeType(DataMatch(read, item, item.PropertyType.Name), item.PropertyType);
					item.SetValue(t, sontip, null);
				}
			}
			catch (Exception)
			{
				//LOG Logic
			}
			return t;
		}
		/// <summary>
		/// Matchs properties with specified data type and pulls data from database
		/// </summary>		
		private object DataMatch(SqlDataReader read, PropertyInfo item, string typename)
		{
			switch (typename)
			{
				case "DateTime":
					return DataAccessLayer.DbCommand.TarihGetir(read, item.Name);
				case "String":
					return DataAccessLayer.DbCommand.StringGetir(read, item.Name);
				case "Boolean":
					return DataAccessLayer.DbCommand.BoolGetir(read, item.Name);
				case "Double":
					return DataAccessLayer.DbCommand.DoubleGetir(read, item.Name);
				case "Int32":
					return DataAccessLayer.DbCommand.Int32Getir(read, item.Name);
				case "Int16":
					return DataAccessLayer.DbCommand.ShortGetir(read, item.Name);
				case "Int64":
					return DataAccessLayer.DbCommand.LongGetir(read, item.Name);
				case "Byte":
					return DataAccessLayer.DbCommand.ByteGetir(read, item.Name);
				case "SByte":
					return DataAccessLayer.DbCommand.SByteGetir(read, item.Name);
				case "Byte[]":
					return DataAccessLayer.DbCommand.ByteArrayGetir(read, item.Name);
				case "Decimal":
					return DataAccessLayer.DbCommand.DecimalGetir(read, item.Name);
				case "Guid":
					return DataAccessLayer.DbCommand.GuidGetir(read, item.Name);
				default:
					return DataAccessLayer.DbCommand.StringGetir(read, item.Name);
			}
		}
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
	}
}
