# PhoneDirectory

PhoneDirectory, mikroservis mimarisi kullanılarak geliştirilmiş bir telefon rehberi uygulamasıdır. Bu uygulama, kişilerin ve iletişim bilgilerinin yönetimini sağlar. Ayrıca, RabbitMQ kullanarak mikroservisler arasında mesaj iletişimini sağlar.

## İçindekiler

- [Özellikler](#özellikler)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [Mimari](#mimari)
- [Yazarlar](#yazarlar)
- [Lisans](#lisans)

## Özellikler

- Kişi ve iletişim bilgilerini yönetme
- Mikroservis mimarisi
- RabbitMQ ile mesaj iletişimi
- Swagger ile API dokümantasyonu

## Kurulum

### Gereksinimler

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [RabbitMQ](https://www.rabbitmq.com/download.html)
- [Docker](https://www.docker.com/get-started) (isteğe bağlı)

### Adımlar

1. Bu projeyi klonlayın:

    ```bash
    git clone https://github.com/alikeremkaya/PhoneDirectory.git
    cd PhoneDirectory
    ```

2. RabbitMQ'yu başlatın:

    Docker kullanarak:
    ```bash
    docker run -d --hostname my-rabbit --name some-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
    ```

    veya manuel olarak RabbitMQ'yu indirin ve başlatın.

3. Projeyi çalıştırın:

    ```bash
    dotnet run --project PhoneDirectory.API
    dotnet run --project Report.API
    ```

4. Tarayıcınızda Swagger dokümantasyonunu açın:

    - PhoneDirectory.API: `http://localhost:5133/swagger`
    - Report.API: `http://localhost:5191/swagger`

## Kullanım

### API İstekleri

#### Kişi Yönetimi

- Tüm kişileri getir:
    ```http
    GET /api/persons
    ```

- Belirli bir kişiyi getir:
    ```http
    GET /api/persons/{id}
    ```

- Yeni bir kişi oluştur:
    ```http
    POST /api/persons
    Content-Type: application/json

    {
        "firstName": "John",
        "lastName": "Doe"
    }
    ```

- Kişi güncelle:
    ```http
    PUT /api/persons/{id}
    Content-Type: application/json

    {
        "id": "{id}",
        "firstName": "John",
        "lastName": "Doe"
    }
    ```

- Kişi sil:
    ```http
    DELETE /api/persons/{id}
    ```

#### İletişim Bilgisi Yönetimi

- Belirli bir kişiye ait tüm iletişim bilgilerini getir:
    ```http
    GET /api/persons/{personId}/communication-info
    ```

- Belirli bir kişiye ait belirli bir iletişim bilgisini getir:
    ```http
    GET /api/persons/{personId}/communication-info/{communicationInfoId}
    ```

- Yeni bir iletişim bilgisi ekle:
    ```http
    POST /api/persons/{personId}/communication-info
    Content-Type: application/json

    {
        "type": "Phone",
        "value": "+123456789"
    }
    ```

- İletişim bilgisi güncelle:
    ```http
    PUT /api/persons/{personId}/communication-info/{communicationInfoId}
    Content-Type: application/json

    {
        "type": "Phone",
        "value": "+123456789"
    }
    ```

- İletişim bilgisi sil:
    ```http
    DELETE /api/persons/{personId}/communication-info/{communicationInfoId}
    ```

### Rapor Yönetimi

- Tüm raporları getir:
    ```http
    GET /api/reports
    ```

- Belirli bir raporu getir:
    ```http
    GET /api/reports/{id}
    ```

- Yeni bir rapor oluştur:
    ```http
    POST /api/reports
    Content-Type: application/json

    {
        "reportType": "LocationReport",
        "requestDate": "2025-02-09T18:25:37Z"
    }
    ```

- Rapor durumunu güncelle:
    ```http
    PUT /api/reports/{id}/status
    Content-Type: application/json

    {
        "status": "Completed"
    }
    ```

### Sağlık Kontrolü

- PhoneDirectory.API sağlık kontrolü:
    ```http
    GET /api/contacts/health
    ```

- Report.API sağlık kontrolü:
    ```http
    GET /api/reports/health
    ```

## Mimari

- `PhoneDirectory.API`: Kişi ve iletişim bilgilerini yönetir.
- `Report.API`: Raporlama işlemlerini yapar.
- RabbitMQ: Mikroservisler arasında mesaj iletişimini sağlar.

## Yazarlar

- **Ali Kerem Kaya** - [alikeremkaya](https://github.com/alikeremkaya)

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır - daha fazla bilgi için [LICENSE](LICENSE) dosyasına bakın.
