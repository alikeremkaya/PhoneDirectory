# 📞 Telefon Rehberi Mikroservis Projesi

Bu proje, **.NET Core** kullanılarak geliştirilen bir **mikroservis mimarisi** ile çalışan **telefon rehberi uygulamasıdır**. Sistemde **RabbitMQ** kullanılarak **asenkron raporlama** yapılmaktadır.

## 📌 **Proje Özeti**
**Telefon Rehberi Mikroservis Projesi**, kullanıcıların kişi ekleyip silebildiği, iletişim bilgilerini yönetebildiği ve konum bazlı rapor alabileceği bir uygulamadır.

## 🎯 **Özellikler**
- ✅ **Rehberde kişi oluşturma, silme, güncelleme**
- ✅ **Kişilere telefon numarası, e-mail veya konum ekleme**
- ✅ **Rehberdeki kişilerin listelenmesi ve detay görüntüleme**
- ✅ **Belirli bir konumdaki kişi ve telefon numarası sayısını içeren rapor oluşturma**
- ✅ **RabbitMQ ile asenkron rapor işlemleri**
- ✅ **Swagger UI ile API dokümantasyonu**

---

## 📂 **Mikroservisler ve Teknolojiler**

| Mikroservis Adı     | Açıklama |
|---------------------|----------|
| 📌 `PhoneDirectory.API` | Kişi ve iletişim bilgilerini yöneten API |
| 📌 `Report.API` | Raporları yöneten ve RabbitMQ'dan mesaj tüketen API |
| 📌 `RabbitMQ` | Mikroservisler arası mesajlaşmayı yönetir |
| 📌 `MSSQL` | Veritabanı yönetimi |

### **🛠 Kullanılan Teknolojiler:**
- **C# / .NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core & MSSQL**
- **RabbitMQ (CloudAMQP ile)**
- **Docker (Opsiyonel)**
- **Swagger UI**

---

## 🚀 **Kurulum**

### **📌 1️⃣ Gerekli Bağımlılıkları Yükleyin**
- [ ] .NET 8 SDK'yı indirin ve yükleyin: https://dotnet.microsoft.com/en-us/download/dotnet/8.0
- [ ] MSSQL Server'ı yükleyin ve çalıştırın.
- [ ] RabbitMQ servisini kurun veya CloudAMQP kullanın.

### **📌 2️⃣ Proje Depolarını Klonlayın**
```bash
git clone https://github.com/kullanici/phone-directory-microservices.git
cd phone-directory-microservices
```

### **📌 3️⃣ Veritabanını Ayarlayın**
📌 **Veritabanı bağlantı ayarlarını `appsettings.json` içine ekleyin:**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=x;Database=x;User Id=x;Password=x;TrustServerCertificate=True"
}
```
📌 **EF Core ile veritabanını oluşturun:**
```bash
dotnet ef database update --project PhoneDirectory.Infrastructure
```

### **📌 4️⃣ RabbitMQ Bağlantısını Güncelleyin**
📌 **RabbitMQ bağlantı ayarlarını `appsettings.json` içinde güncelleyin:**
```json
"RabbitMQ": {
  "Uri": "amqps://your-cloudamqp-uri",  (https://customer.cloudamqp.com/) dan oluşturduğunuz ınstance ile)
  "RequestQueue": "report_requests",
  "ResultQueue": "report_results"
}
```

### **📌 5️⃣ API'leri Çalıştırın**
```bash
dotnet run --project PhoneDirectory.API
```
```bash
dotnet run --project Report.API
```
📌 **API'ler başarılı bir şekilde başladıysa, aşağıdaki adreslerden erişebilirsiniz:**
- 📞 **Kişi API (PhoneDirectory.API)**: http://localhost:5001/swagger
- 📊 **Rapor API (Report.API)**: http://localhost:5002/swagger

---

## 📌 **API Endpointleri**
### 📞 **Kişi Yönetimi (PhoneDirectory.API)**
| Metot | URL | Açıklama |
|-------|-----|----------|
| **GET** | `/api/person` | Tüm kişileri getir |
| **GET** | `/api/person/{id}` | Belirtilen ID'ye sahip kişiyi getir |
| **POST** | `/api/person` | Yeni kişi ekle |
| **PUT** | `/api/person/{id}` | Kişi bilgilerini güncelle |
| **DELETE** | `/api/person/{id}` | Kişiyi sil |

### 📊 **Rapor Yönetimi (Report.API)**
| Metot | URL | Açıklama |
|-------|-----|----------|
| **GET** | `/api/reports` | Tüm raporları listele |
| **GET** | `/api/reports/{id}` | Belirtilen raporu getir |
| **POST** | `/api/reports` | Yeni rapor oluştur |
| **PUT** | `/api/reports/{id}/status` | Rapor durumunu güncelle |

---

## 📌 **RabbitMQ Entegrasyonu**
📌 **Telefon Rehberi API bir kişi oluşturduğunda, RabbitMQ kuyruğuna bir mesaj ekler:**
```json
{
  "PersonId": "guid",
  "Location": "Istanbul",
  "RequestedDate": "2024-02-17T12:00:00Z"
}
```
📌 **Report API, RabbitMQ'dan gelen mesajı tüketerek rapor oluşturur.**










