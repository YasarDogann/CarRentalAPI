# 🚗 Car Rental API

Bu proje, araç kiralama işlemlerini yönetmek için geliştirilmiş bir RESTful API'dir. .NET 8 kullanılarak geliştirilmiş ve modern web API geliştirme pratiklerini içermektedir.

## 🏗️ Proje Yapısı

Proje, N-Tier Architecture prensiplerine uygun olarak 3 katmandan oluşmaktadır:

### 📁 CarRentalApi.Data
- Entity Framework Core ile veritabanı işlemleri
- Code First yaklaşımı ile oluşturulmuş entity'ler
- Repository pattern implementasyonu
- UnitOfWork pattern implementasyonu

### 📁 CarRentalApi.Business
- İş mantığı katmanı
- DTO (Data Transfer Object) modelleri
- Service implementasyonları
- Custom exception types

### 📁 CarRentalApi.WebApi
- RESTful API endpoints
- JWT authentication
- Role-based authorization
- Custom filters ve middleware'ler

## 🔑 Temel Özellikler

### 🚙 Araç Yönetimi
- Araç listeleme, ekleme, güncelleme, silme
- Araç özelliklerini yönetme (many-to-many ilişki)
- Stok takibi
- Fiyat güncelleme

### 👥 Kullanıcı Yönetimi
- JWT tabanlı kimlik doğrulama
- Rol bazlı yetkilendirme
- Admin ve müşteri rolleri

### 🛡️ Güvenlik ve Performans
- Global exception handling
- Model validation
- API maintenance mode
- Response caching
- Rate limiting

## 🔧 Teknik Özellikler

### Veritabanı
- Entity Framework Core
- Code First yaklaşımı
- Migration support
- Soft delete implementasyonu

### API Güvenliği
- JWT authentication
- Role-based authorization
- API key validation
- Model validation

### Performans
- Response caching
- API rate limiting
- Maintenance mode support

### Kod Kalitesi
- Dependency Injection
- Repository & UnitOfWork patterns
- Custom exception handling
- Action filters
- Custom middleware

## 🚀 Endpoint'ler

### Cars
```http
GET /api/cars - Tüm araçları listele
GET /api/cars/{id} - Belirli bir aracı getir
POST /api/cars - Yeni araç ekle (Admin)
PUT /api/cars/{id} - Araç bilgilerini güncelle (Admin)
PATCH /api/cars/{id}/PricePerDay - Araç fiyatını güncelle (Admin)
DELETE /api/cars/{id} - Araç sil (Admin)
```

### Features
```http
GET /api/features - Tüm özellikleri listele (Admin)
POST /api/features - Yeni özellik ekle (Admin)
PUT /api/features/{id} - Özellik güncelle (Admin)
DELETE /api/features/{id} - Özellik sil (Admin)
```

## 🛠️ Kurulum

1. Repoyu klonlayın
```bash
git clone https://github.com/yourusername/CarRentalApi.git
```

2. Veritabanını oluşturun
```bash
dotnet ef database update
```

3. appsettings.json dosyasını düzenleyin
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string"
  },
  "JwtSettings": {
    "SecretKey": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience",
    "ExpirationInMinutes": 60
  }
}
```

4. Projeyi çalıştırın
```bash
dotnet run
```

## 📝 Notlar

- API'yi kullanmak için JWT token gereklidir
- Admin işlemleri için Admin rolüne sahip olunmalıdır
- API bakım modundayken bazı endpoint'ler kullanılamaz
- Rate limiting ile API kullanımı sınırlandırılmıştır

## 🤝 Katkıda Bulunma

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request 