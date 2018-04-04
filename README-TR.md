# adonet-lib

adonet-libV2.0 bit ADO.NET uygulama projesidir. ADO.NET ile bağlantı işlemlerinin yapılacağı herhangi bir projeye kolaylıkla entegre edilebilir.

SAMPLE projesi bu bağlantı tipini uygulayan bir ASP.NET MVC projesidir.

ADO.NET ile ilişkili dosyalar DataLayer kütüphanesinde yer alır.

### Veritabanı bağlantısı:

  *Proje veritabanı bilgilerini entegre edildiği .NET Projesinin config dosyasından çeker.DataLayer sınıf kütüphanesindeki Helper sınıfı config dosyasından bağlantı cümlesi olusturma işlemini yapar. Config dosyası kullanılmak istenmezse bağlantı cümlesi ayarları DataLayer/DB/DbConnection sınıfındadır. 

  *DBConnection sınıfı veritabanı bağlantısı açma kapama işlemlerini üstlenir.

  ```
  public static readonly string connectionString = DB.Helper.connectionstring();//config dosyasından okuma yapar
  ```
  ---

  Proje 2 sınıf kütüphanesinden meydana gelmektedir.
  
  # Core
  
  Core kütüphanesi veritabanı nesneleri ile eşleşecek sınıfları barındırır. Veritabanından dönen veriler bu sınıflara yüklenir.Özellik isimleri veritabanı kolon isimleri ile uyuşmalıdır. 
  ```
  //Örnek core sınıfı
   public class SampleCore
   {
   	public int ID { get; set; }
   	public string Val1 { get; set; }
   	public string Val2 { get; set; }
   }
  ```
  # DataLayer
  
  Bütün veritabanı işlemlerinin yapıldığı katmandır. DbCommand sınıfı veritabanı komut işlemlerinin, DbConnection sınıfı ise bağlantı ayarlarının düzenlendiği sınıftır. 
  
  ####DataLayer metodları
  
  ##### Read metodu
  Generic bir metod olan Read metodu bir SqlParameter listesi ve kullanılacak stored procedure adını parametre olarak alır. Select işlemini yapan Stored Procedure veritabanında mevcut bulunmalıdır. Generic tip olarak çekilecek olan veritabanı tablosuyla örtüşen Core sınıfı kullanılır. Geriye seçilen Core sınıfından bir liste döner.
  
  Dipnot:Read,RaedManuel ve CUD metodları aynı parametre listesini kullanmaktadır.
  
  Örnek
  
   ```
   List<SqlParameter> paramlist = new List<SqlParameter>() {
		new SqlParameter("id","42"),
		
   };//parametre listesinin yaratılması
          
    //Stored Procedure ile data çekilmesi
    List<Core.SampleCore> samplelist = db.Read<Core.SampleCore>("sp_TestProc", paramlist);//veritabanından select işlemi id'si 42 olan    satır ya da satırlar
        
  ```
  
  
  ##### ReadManuel metodu
  Read metodunun benzeri olan bu metodun tek farkı stored procedure yerine string olarak kendisine verilen sorguyu kullanmasıdır. Sql injection'dan kaçınmak için parametreler bir SqlParameter listesi olarak işlenir.
  
  
  ```
     
   //Raw Query ile data çekilmesi
   string query = "Select * from SampleTable where ID=@id";				
   samplelist = db.ReadManuel<Core.SampleCore>(query, paramlist);
  ```
  
  
  ##### CUD metodu (Create Update Delete)
  
  İnsert,update ve delete işlemlerini yapan bu metod geriye boolean tipinden bir değer döner. Doğal olarak true değeri işlemin başarıyla sonuçlandığını ifade eder. Parametre olarak bir SqlParameter listesi ve CUD işlemini yapacak olan stored procedure adını alır
  
  ```
   //Geriye data dönmeyen işlemler(Create,Update,Delete)
   //CUD metodu geriye true ya da false döner. Buradan işlemin başarılı olup olmadığı takip edilebilir.
     bool validate = db.CUD("sp_SatirSil", paramlist);//42 idli satiri sil
				
  ```
  
  
 ##### CUDManuel metodu (Create Update Delete)
 
 Tıpkı ReadManuel işleminde olduğu gibi CUDManuel metodu da stored procedure yerine kendisine verilen query'i kullanarak CUD işlemlerini gerçekleştirir.
  
  ```
   //Raw query ile işlemler
      paramlist.Clear();//parametre listesinin temizlenmesi
      string insertquery="Insert into TB_Sample values @val1,@val2)";
      paramlist.Add(new SqlParameter("42", "So Long, and Thanks for All the Fish "));
      validate = db.CUDManuel(insertquery, paramlist);//ilk satırin inserti
      paramlist.Clear();//parametre listesinin temizlenmesi
//CUDManuel metodu tıpkı CUD gibi geriye true ya da false döner. Tek farkı sorgunun elle girilmiş olmasıdır.
      paramlist.Add(new SqlParameter("8", "GNU Terry Pratchett"));//2. satir inserti
      validate = db.CUDManuel(insertquery, paramlist);//ikinci satir insert		
				
  ```
  
  Bu kütüphane temel veri tipleriyle çalışmak için tasarlanmıştır. Datalayer/DataAccess sınıfındaki DataMatch metodu nesne tipine göre sınıflandırma yapmaktadır. 
  
  #### Desteklenen veri tipleri
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
  
  
  
  

