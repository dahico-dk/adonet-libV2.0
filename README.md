# adonet-lib

ADO.NET-LIB farklı sınıf kütüphanelerine bölünmüş bir ADO.NET uygulama projesidir. Projesinde ADO.NET kullanmak isteyenler bu projenin mantığını referans alarak kendi projelerine entegre edebilirler.

SAMPLE projesi bu bağlantı tipini uygulayan bir MVC projesidir.

ADO.NET ile ilişkili dosyalar DataAccessLayer,Facade ve Core sınıf kütüphanelerinde yer alır.Kullanıcı talep doğrultusunda bu sınıf kütüphanelerini tek bir sınıf kütüphanesi altında birleştirerek klasörlerle birbirinden ayırma yoluna gidebilir.

### Veritabanı bağlantısı:

  *Proje veritabanı bağlantılarını entegre edildiği .NET Projesinin config dosyasından çeker. Kullanıcı config dosyasını kullanmak istemez ise bağlantı cümleciği ayarları DataAccessLayer/DB/DbConnection sınıfındadır. 

  *DataAccessLayer sınıf kütüphanesindeki Helper sınıfı config dosyasından bağlantı cümleciği olusturma işlemini yapar.

  *DBConnection sınıfı veritabanı bağlantısını açar.

  ```
  public static readonly string connectionString = DB.Helper.connectionstring();
  ```
  ---

  Proje genel olarak 2 sınıf kütüphanesinden meydana gelmektedir.
  
  #Core
  
  Core kütüphanesi veritabanı nesneleri ile eşlenecek sınıfları barındırır. Veritabanından dönen veriler bu sınıflara yüklenir. ç içe sınıflar ya da yapıcı metodlar yardımı ile farklı şekillerde bu nesneler tek dosyada yaratılabilir. Ben ayrı sınıflarda tutmayı tercih ediyorum.
  ```
   public class SampleCore
	{
		public int ID { get; set; }
		public string Val1 { get; set; }
		public string Val2 { get; set; }
	}
  ```
  #DataLayer
  
  Bütün veritabanı işlemlerinin yapıldığı katman bu katmandır. DbCommand veritabanı komut işlemlerinin DbConnection ise bağlantı ayarlarının düzenlendiği sınıftır. Helper sınıfı bağlantı cümleciği oluşturma işlemlerine yardımcı olur.
  
  Data Access sınıfı ise Generic olarak düzenlenmiş dbconnection ile bağlanılan veritabanı üzerinde CRUD işlemlerini yapmaya yarayan sınıftır.
  
  #Proje (Örnek Kullanım)
  
  Bütün metodlar değişken olarak bir parametre listesi alırlar. Çünkü bütün sorgular paremetrik şekilde çalışmalıdır. 
  Generic Read Metodu hedef veritabanında ki bir stored procedure'u çalıştırıp geriye değerleri döner. Bu metodlar çalıştırılmadan önce   Core sınıf kütüphanesi altında gerekli class ve özellikleri yaratılmalıdır. Özellik isimleri ve veritabanı kolonlarının isimleri aynı   olmalıdır. Çünkü metod reflection ile özellikleri okumakta ve okuduğu isimlere göre dataları eşleştirmektedir.
  
  CUD metodu ise geriye değer dönmeyen procedureler için kullanılmaktadır. Insert,Delete ve Update işlemleri için kullanılır.
  
  ReadManuel ve CUDManuel metodları procedure yerine raw query kullanır. Örnek sorgu cümlecikleri aşağıda ki kod bloğunda mevcuttur.
  
  Clear metodları sadece aynı listeyi tekrar tekrar kullanabilmek için kullanılmıştır.
  
  
  ```
  using (var db=new DataLayer.DataAccess())//IDisposable
   {
	List<SqlParameter> paramlist = new List<SqlParameter>();
	List<Core.SampleCore> samplelist = new List<Core.SampleCore>();
	//parametre listesinin yaratılması-Creating parameter lists
	paramlist.Add(new SqlParameter("name","value"));//1st parameter (sample)
	paramlist.Add(new SqlParameter("name", "value"));//2nd parameter (sampl
	//Stored Procedure ile data çekilmesi-Using Stored procedures
	samplelist = db.Read<Core.SampleCore>("sp_TestProc", paramlist
	//refresh lists
	paramlist.Clear(); samplelist.Clear();//listeleri boşaltalım-Clearing lis
	//Raw Query ile data çekilmesi-Pull data with raw query
	string query = "Select * from SampleTable where ID=@id";
	paramlist.Add(new SqlParameter("@id", 42));//1st parameter(sample)
	samplelist = db.ReadManuel<Core.SampleCore>(query, paramlist
	//Geriye data dönmeyen işlemler(Create,Update,Delete)-Database transactions with no data returning 
	paramlist.Clear();//listeleri boşaltalım-Clearing lis
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
  
  ```
  
  
  
  
  

