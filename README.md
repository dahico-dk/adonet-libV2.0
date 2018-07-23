# adonet-lib

adonet-libV2.0 is an ADO.NET project divided into different class libraries. It can easily be integrated into any type of project.

The SAMPLE project is an ASP.NET MVC project that uses this database connection type.

The files associated with ADO.NET are located in the DataLayer library.

### Database Connection:

   Project pulls credential information for database connections from the config file of the .NET project in which it is integrated. If the user does not want to pull the information from the config file, the connection string settings are in the DataLayer/DB/DbConnection class. The Helper class in the DataLayer class library creates the connection string from the config file. The DbConnection class handles database connection and disconnection operations.

  ```
  public static readonly string connectionString = DB.Helper.connectionstring();//This method reads from config file
  ```
  ---

  The project is comprised of 2 class libraries.
  
  # Core
  
  The Core library contains POCO classes that match database objects.The data returned from the database is loaded into these classes. These objects can be created in one file in different ways with the help of nested classes or constructor methods. Property names must match database column names.
  ```
  //Example core library
   public class SampleCore
   {
   	public int ID { get; set; }
   	public string Val1 { get; set; }
   	public string Val2 { get; set; }
   }
  ```
  # DataLayer
  
 This is the layer where all database operations are made. DbCommand contains methods for configuring command operations.DbConnection class organizes connection operations. Helper class creates connections string from config file.
 
 #### DataLayer methods
  
  ##### Read method
  The Read method which is a generic method takes a SqlParameter list and a stored procedure name as string. The Stored Procedure which makes the select operation must be available in the database. The Core class that overlaps with the database table must be used as generic type. Returns a list of the selected Core class.
  
  Footnote :Read, RaedManuel and CUD methods use the same parameter list in this example.
  
  Example

   ```
   List<SqlParameter> paramlist = new List<SqlParameter>() {
		new SqlParameter("id","42"),
		
   };//craeting parameter list
          
    //Pulling data with stored procedure
    List<Core.SampleCore> samplelist = db.Read<Core.SampleCore>("sp_TestProc", paramlist);//select row which has id of 42
        
  ```
  
  ##### ReadManuel method
  The only difference between this method and the Read method is that it uses the given query instead of the stored procedure. To avoid sql injection, the parameters are handled as a SqlParameter list.
  
  
  ```
        
   //Pull data with raw query
     string query = "Select * from SampleTable where ID=@id";				
   samplelist = db.ReadManuel<Core.SampleCore>(query, paramlist);

  ```
  
  

  ##### CUD method (Create Update Delete)
  This method which operates insert, update and delete operations returns a boolean value. Naturally, a true value indicates successful processing. It takes a SqlParameter list and the name of the stored procedure which will perform the CUD operation as parameter.
  
  ```
   //Database transactions which returns no data
   // CUD method returns true or false depending upon succession of transaction
     bool validate = db.CUD("sp_SatirSil", paramlist);//Delete row which has the id of 42
   ```
    
 ##### CUDManuel method (Create Update Delete)
 Just as in the ReadManuel method, the CUDManuel method performs CUD operations using the query supplied to method rather than a stored procedure.
  
  ```
   // Transactions with raw query
      paramlist.Clear();//Clearing parameter list
      string insertquery="Insert into TB_Sample values @val1,@val2)";
      paramlist.Add(new SqlParameter("42", "So Long, and Thanks for All the Fish "));
      validate = db.CUDManuel(insertquery, paramlist);//First row
      paramlist.Clear();
//.CUDmanuel method returns true or false depending upon succession of transaction CUD method. Only difference is raw query.
      paramlist.Add(new SqlParameter("8", "GNU Terry Pratchett"));//2nd row
      validate = db.CUDManuel(insertquery, paramlist);//insert 2nd row		
				
  ```
  
  This library is designed to work with basic data types. The DataMatch method in the Datalayer / DataAccess class categorizes properties according the object type.
  
  #### Supported Data Types

  * DateTime
  * String
  * Boolean 
  * Double ⋅
  * Short 
  * Long 
  * Int32 
  * Byte 
  * SByte 
  * Decimal 
  * Byte[]
  * Guid
  
  
  
  

