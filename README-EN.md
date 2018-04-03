# adonet-lib

adonet-libV2.0 is an ADO.NET project divided into different class libraries. It can easily be integrated into any type of project.

The SAMPLE project is an ASP.NET MVC project that uses this database connection type.

The files associated with ADO.NET are located in the DataLayer library.

### Database Connection:

  *Project pulls credential information for database connections from the config file of the .NET project in which it is integrated. If the user does not want to pull the informayion from the config file, the connection string settings are in the DataLayer / DB / DbConnection class.

  *The Helper class in the DataLayer class library creates the connection string from the config file.

  *The DBConnection class handles database connection and disconnection operations.

  ```
  public static readonly string connectionString = DB.Helper.connectionstring();//This method reads from config file
  ```
  ---

  The project is comprised of 2 class libraries.
  
  # Core
  
  The Core library contains classes that match database objects.The data returned from the database is loaded into these classes. These objects can be created in one file in different ways with the help of nested classes or constructor methods. Property names must match database column names.
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
  
<<<<<<< HEAD
 This is the layer where all database operations are made. DbCommand contains methods for configuring command operations.DbConnection class organizes connection operations. Helper class creates connections string from config file.
  ####DataLayer metodları
  
  ##### Read method
  The Read method which is a generic method takes a SqlParameter list and a stored procedure name as string. The Stored Procedure which makes the select operation must be available in the database. The Core class that overlaps with the database table must be used as generic type. Returns a list of the selected Core class.
  
  Footnoe :Read, RaedManuel and CUD methods use the same parameter list in this example.
  
  Example
=======
 This layer is the layer where all database operations are made. DbCommand class is the class  DbCommand veritabanı komut işlemlerinin DbConnection ise bağlantı ayarlarının düzenlendiği sınıftır. Helper sınıfı bağlantı cümleciği oluşturma işlemlerine yardımcı olur.
  ####DataLayer metodları
  
  ##### Read metodu
  Generic bir metod olan Read metodu bir SqlParameter listesi ve string tipinden stored procedure adını parametre olarak alır. Select yapan Stored Procedure veritabanında mevcut bulunmalıdır. Generic tip olarak çekilecek olan veritabanı tablosuyla örtüşen Core sınıfı kullanılır. Geriye seçilen Core sınıfından bir liste döner.
  Dipnot:Read,RaedManuel ve CUD metodları aynı parametre listesini kullanmaktadır.
  
  Örnek
>>>>>>> 01ad4705a8ff75424d6e03a414c02683fdd6902c
  
   ```
   List<SqlParameter> paramlist = new List<SqlParameter>() {
		new SqlParameter("id","42"),
		
<<<<<<< HEAD
   };//creating parameter list
          
    //Pulling data using Stored procedures
    List<Core.SampleCore> samplelist = db.Read<Core.SampleCore>("sp_TestProc", paramlist);//Select operation. returns row with id of 42
=======
   };//parametre listesinin yaratılması
          
    //Stored Procedure ile data çekilmesi-Using Stored procedures
    List<Core.SampleCore> samplelist = db.Read<Core.SampleCore>("sp_TestProc", paramlist);//veritabanından select işlemi id'si 42 olan    satır ya da satırlar
>>>>>>> 01ad4705a8ff75424d6e03a414c02683fdd6902c
        
  ```
  
  
<<<<<<< HEAD
  ##### ReadManuel method
  The only difference between this method and the Read method is that it uses the given query instead of the stored procedure. To avoid sql injection, the parameters are handled as a SqlParameter list.
=======
  ##### ReadManuel metodu
  Read metodunun benzeri olan bu metodun tek farkı stored procedure yerine string olarak kendisine verilen sorguyu kullanmasıdır. Sql injection'dan kaçınmak için parametreler bir SqlParameter listesi olarak işlenir.
>>>>>>> 01ad4705a8ff75424d6e03a414c02683fdd6902c
  
  
  ```
     
<<<<<<< HEAD
   //Pulling data with raw query
=======
   //Raw Query ile data çekilmesi-Pull data with raw query
>>>>>>> 01ad4705a8ff75424d6e03a414c02683fdd6902c
   string query = "Select * from SampleTable where ID=@id";				
   samplelist = db.ReadManuel<Core.SampleCore>(query, paramlist);
  ```
  
  
<<<<<<< HEAD
  ##### CUD method (Create Update Delete)
  This method which operates insert, update and delete operations returns a boolean value. Naturally, a true value indicates successful processing. It takes a SqlParameter list and the name of the stored procedure which will perform the CUD operation as parameter.
  
  ```
   //Database transactions which returns no data
   // CUD method returns true or false depending upon succession of transaction
     bool validate = db.CUD("sp_SatirSil", paramlist);//Delete row which has the id of 42
=======
  ##### CUD metodu (Create Update Delete)
  İnsert,update ve delete işlemlerini yapan bu metod geriye boolean tipinden bir değer döner. Doğal olarak true değeri işlemin başarıyla sonuçlandığını ifade eder. Parametre olarak bir SqlParameter listesi ve CUD işlemini yapacak stored procedure adını alır
  
  ```
   //Geriye data dönmeyen işlemler(Create,Update,Delete)-Database transactions with no data returning 
   //CUD metodu geriye true ya da false döner. Buradan işlemin başarılı olup olmadığı takip edilebilir. CUD method returns true or false depending upon succession of transaction
     bool validate = db.CUD("sp_SatirSil", paramlist);//42 idli satiri sil
>>>>>>> 01ad4705a8ff75424d6e03a414c02683fdd6902c
				
  ```
  
  
<<<<<<< HEAD
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
      paramlist.Add(new SqlParameter("13", "GNU Terry Pratchett"));//2nd row
      validate = db.CUDManuel(insertquery, paramlist);//insert 2nd row		
				
  ```
  
  This library is designed to work with basic data types. The DataMatch method in the Datalayer / DataAccess class categorizes properties according the object type.
  
  #### Supported Data Types
=======
 ##### CUDManuel metodu (Create Update Delete)
 Tıpkı ReadManuel işleminde olduğu gibi CUDManuel metodu da stored procedure yerine kendisine verilen query'i kullanarak CUD işlemlerini gerçekleştirir.
  
  ```
   //Raw query ile işlemler. Transactions with raw query
      paramlist.Clear();//parametre listesinin temizlenmesi
      string insertquery="Insert into TB_Sample values @val1,@val2)";
      paramlist.Add(new SqlParameter("42", "So Long, and Thanks for All the Fish "));
      validate = db.CUDManuel(insertquery, paramlist);//ilk satırin inserti
      paramlist.Clear();//parametre listesinin temizlenmesi
//CUDManuel metodu tıpkı CUD gibi geriye true ya da false döner. Tek farkı sorgunun elle girilmiş olmasıdır.CUDmanuel method returns true or false depending upon succession of transaction CUD method. Only difference is raw query.
      paramlist.Add(new SqlParameter("13", "GNU Terry Pratchett"));//2. satir inserti
      validate = db.CUDManuel(insertquery, paramlist);//ikinci satir insert		
				
  ```
  
  Bu kütüphane temel veri tipleriyle çalışmak için tasarlanmıştır. Datalayer/DataAccess sınıfındaki DataMatch metodu nesne tipine göre sınıflandırma yapmaktadır. 
  #### Desteklenen veri tipleri
>>>>>>> 01ad4705a8ff75424d6e03a414c02683fdd6902c
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
  
  
  
  

