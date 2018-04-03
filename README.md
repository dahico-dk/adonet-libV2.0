# adonet-lib

ADO.NET-LIB farklı sınıf kütüphanelerine bölünmüş bir ADO.NET uygulama projesidir. ADO.NET kullanılmak istenen herhangi bir projeye kolaylıkla entegre edilebilir.

SAMPLE projesi bu bağlantı tipini uygulayan bir MVC projesidir.

ADO.NET ile ilişkili dosyalar DataLayer kütüphanesinde yer alır.

### Veritabanı bağlantısı:

  *Proje veritabanı bağlantılarının bilgilerini entegre edildiği .NET Projesinin config dosyasından çeker. Kullanıcı config dosyasını kullanmak istemez ise bağlantı cümleciği ayarları DataLayer/DB/DbConnection sınıfındadır. 

  *DataLayer sınıf kütüphanesindeki Helper sınıfı config dosyasından bağlantı cümleciği olusturma işlemini yapar.

  *DBConnection sınıfı veritabanı bağlantısı açma kapama işlemlerini üstlenir.

  ```
  public static readonly string connectionString = DB.Helper.connectionstring();//config dosyasından okuma yapar
  ```
  ---

  Proje genel olarak 2 sınıf kütüphanesinden meydana gelmektedir.
  
  #Core
  
  Core kütüphanesi veritabanı nesneleri ile eşlenecek sınıfları barındırır. Veritabanından dönen veriler bu sınıflara yüklenir. iç içe sınıflar ya da yapıcı metodlar yardımı ile farklı şekillerde bu nesneler tek dosyada yaratılabilir. Ben ayrı sınıflarda tutmayı tercih ediyorum. Özellik isimleri veritabanı kolon isimleri ile uyuşmalıdır. 
  ```
  //Örnek core sınıfı
   public class SampleCore
	{
		public int ID { get; set; }
		public string Val1 { get; set; }
		public string Val2 { get; set; }
	}
  ```
  #DataLayer
  
  Bütün veritabanı işlemlerinin yapıldığı katman bu katmandır. DbCommand veritabanı komut işlemlerinin DbConnection ise bağlantı ayarlarının düzenlendiği sınıftır. Helper sınıfı bağlantı cümleciği oluşturma işlemlerine yardımcı olur.
  ####DataLayer metodları
  
  #####Read metodu
  Generic bir metod olan Read metodu bir SqlParameter listesi ve string tipinden stored procedure adını parametre olarak alır. Select yapan Stored Procedure veritabanında mevcut bulunmalıdır. Generic tip olarak çekilecek olan veritabanı tablosuyla örtüşen Core sınıfı kullanılır. Geriye seçilen Core sınıfından bir liste döner.
  
  Örnek
   ```
    List<SqlParameter> paramlist = new List<SqlParameter>(); //parametre listesinin yaratılması-Creating parameter lists   
    paramlist.Add(new SqlParameter("name","value"));//1st parameter (sample)
    paramlist.Add(new SqlParameter("name", "value"));//2nd parameter (sample)   
    //Stored Procedure ile data çekilmesi-Using Stored procedures
    List<Core.SampleCore> samplelist = db.Read<Core.SampleCore>("sp_TestProc", paramlist);//veritabanından select işlemi
    
  ```
  
  #####ReadManuel metodu
  Read metodunun benzeri olan bu metodun tek farkı stored procedure yerine string olarak kendisine verilen sorguyu kullanmasıdır. Sql injection'dan kaçınmak için parametreler bir SqlParameter listesi olarak işlenir.
  
  ```
     List<SqlParameter> paramlist = new List<SqlParameter>(); //parametre listesinin yaratılması-Creating parameter lists
     string query = "Select * from SampleTable where ID=@id";
     paramlist.Add(new SqlParameter("@id", 42));//1st parameter(sample)
     List<Core.SampleCore> samplelist = db.ReadManuel<Core.SampleCore>(query, paramlist);
  ```
  
  
  #####CUD metodu (Create Update Delete)
  İnsert,update ve delete işlemlerini yapan bu metod geriye boolean tipinden bir değer döner. Doğal olarak true değeri işlemin başarıyla sonuçlandığını ifade eder. Parametre olarak bir SqlParameter listesi ve CUD işlemini yapacak stored procedure adını alır
  
  ```
  List<SqlParameter> paramlist = new List<SqlParameter>();
  paramlist.Add(new SqlParameter("@id", 42));//42 idli satırı silelim mesela
  //CUD metodu geriye true ya da false döner. Buradan işlemin başarılı olup olmadığı takip edilebilir. 
  bool validate = db.CUD("sp_SatirSil", paramlist);
				
  ```
  
  
 #####CUDManuel metodu (Create Update Delete)
 Tıpkı ReadManuel işleminde olduğu gibi CUDManuel metodu da stored procedure yerine kendisine verilen query'i kullanarak CUD işlemlerini gerçekleştirir.
  
  ```
   List<SqlParameter> paramlist = new List<SqlParameter>();
  //Raw query ile işlemler. Transactions with raw query
  string insertquery="Insert into TB_Sample values @val1,@val2)";
  paramlist.Add(new SqlParameter("42", "So Long, and Thanks for All the Fish "));//1st parameter (sample)
  paramlist.Add(new SqlParameter("13", "GNU Terry Pratchett"));//2nd parameter (sample)
  //CUDManuel metodu tıpkı CUD gibi geriye true ya da false döner. Tek farkı sorgunun elle girilmiş olmasıdır.
   bool validate = db.CUDManuel(insertquery, paramlist);			
				
  ```
  
  Bu kütüphane temel veri tipleriyle çalışmak için tasarlanmıştır. Datalayer/DataAccess sınıfındaki DataMatch metodu nesne tipine göre sınıflandırma yapmaktadır. 
  ####Desteklenen veri tipleri
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
  
  
  
  

