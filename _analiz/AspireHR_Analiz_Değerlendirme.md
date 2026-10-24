# AspireHR - Analiz ve Değerlendirme Dokümanı

*Oluşturulma Tarihi: 2025-10-24*  
*Durum: Analiz Aşaması*

## 1. Proje Hedefi

.NET Aspire ile tam uyumlu, Dapr kullanarak grup bazlı mikroservis mimarisiyle bir İnsan Kaynakları (HR) uygulaması geliştirmek hedeflenmektedir.

## 2. Temel Gereksinimler

### 2.1 Mimari Gereksinimler
- **Mimari:** .NET Aspire (AppHost + ServiceDefaults) + Dapr
- **API Geçidi:** YARP tabanlı Gateway altında toplanacak
- **Mikroservis Tipi:** Grup bazlı mikroservisler (modül bazlı ayrı ayrı değil, işlevsel gruplara göre)
- **Veri İzolasyonu:** Her servis kendi veritabanını/koleksiyonunu saklayacak
- **Altyapı:** Docker/Compose ile host edilecek (Keycloak, MongoDB vb.)
- **Erişim:** Sistem login olmadan açılmayacak

### 2.2 Kimlik Doğrulama ve Yetkilendirme
- **Kimlik Sağlayıcısı:** Keycloak + JWT
- **Doğrulama/Yetkilendirme Bazlı:** Hem doğrulama hem yetkilendirme bazlı olacak
- **Esnek Yetki Yönetimi:** Login/Auth modülünde Gruplar, Kişiler ve Yetkiler yönetilecek
- **Yetki Ayrımı:** Webservis ve ekran bazlı yetkiler farklı olabilir
- **Roller:** İK Admini, İK Modül Sorumluları, Çalışan, Bölüm Amirleri, Üst Yöneticiler, Finans vb. (zamanla artabilir)

### 2.3 Zaman Bazlı Geçerlilik (Effective Dating)
- **Kural:** `StartDate` / `EndDate` yapısı kullanılacak
- **Sentinel Değeri:** `EndDate` boş/null ise `0001-01-01` olarak saklanacak
- **Geçerlilik:** `EndDate == 0001-01-01` veya `EndDate >= Bugün` ise kayıt **hala geçerli/aktif** kabul edilecek
- **Uygulama Kapsamı:** Tüm değişken/zamana göre değişen kayıtlar için geçerli
- **Interface:** `IDateItem` interface'inden türemeli

## 3. Önerilen Servis Dağılımı

| Servis | Kapsam | Açıklama |
|---|---|---|
| **Login/Auth** | Kimlik, Gruplar, Kullanıcılar, Yetkiler | Keycloak entegrasyonu + uygulama yetki matrisi. Tüm servislerin yetkilendirme kaynağı olacak. Webservis ve ekran bazlı yetkiler ayrı yönetilebilir. |
| **Özlük (HR Core)** | Personel, Sicil, Kadro/Atama, Organizasyon, Aile Bilgileri, Görev/Unvan geçmişi | En çok effective dating kullanılacak çekirdek servis. Diğer servislerin referans verdiği "kim kimdir" bilgisi burada olacak. Bölüm amiri ilişkisi yönetilecek. |
| **İdari İşler** | Evrak, Yazı/Onay, Arşiv, İdari Talepler | İş akışlı, Özlük ile entegre |
| **İzin** | İzin Türleri, Talepler, Onay Akışları, Hakediş/Bakiye | Bölüm amiri + İK sorumluları bazlı onay zinciri |
| **Ücret** | Bordro, Ücret Kalemleri, Ek Ödemeler, Maaş Geçmişi | Hassas veri, geçmiş için effective dating. Muhasebe fişi ile PubSub üzerinden entegre olacak |
| **Performans** | Değerlendirme Dönemleri, Kriterler, Formlar, Sonuçlar | Dönemsel, genellikle amirler/üst yöneticiler + İK |
| **Muhasebe Fişi Sistemi** | Hesap Planı (Hesap Ağacı), Fişler, Fiş Detayları | Manuel fiş girişi, hesap tanımları, Fiş No + Tarih + İşyeri önemli. Bordro/avans gibi durumlarda entegre olacak (event/PubSub). | 

## 4. Özlük Bilgi Yapısı Analizi

### 4.1 Temel Prensipler
- **Kişi (Person) ile Personel/İş İlişkisi (EmployeeAssignment) ayrı tutulmalı**
- **Değişken olan her şey tarihli, değişmeyenler ise ana kayıtta olmalı**
- **Zamana göre değişen tüm kayıtlar `IDateItem`'tan türemeli**
- **Bir kişinin aynı anda sadece 1 adet aktif ataması olmalı** (varsayım)

### 4.2 Kişiye Bağlı Bilgiler

| Alan | Model | Açıklama |
|---|---|---|
| **Temel Bilgiler** | `Person` (ana koleksiyon) | Ad, Soyad, TC Kimlik No, Cinsiyet, Doğum Tarihi, Doğum Yeri, Medeni Durum vb. |
| **Aile Bilgileri** | `PersonFamily` : `IDateItem` (ayrı koleksiyon) | Eş, Çocuk, Yakınlık Derecesi vb. Tarihli tutulmalı |
| **Adres Bilgileri** | `PersonAddress` : `IDateItem` (ayrı koleksiyon) | İkametgah, Yazışma Adresi vb. Tarihli tutulmalı |
| **İletişim Bilgileri** | `PersonContact` : `IDateItem` (ayrı koleksiyon) | Cep Telefonu, Ev Telefonu, E-Posta vb. Tarihli tutulmalı |

### 4.3 Tanımlar (Master Data) - Personellik İçindeki Tanımlar

| Tanım | Açıklama |
|---|---|
| **Firma (`Company`)** | Şirket unvanı, vergi bilgileri vb. |
| **İşyeri (`Workplace`)** | Firmaya bağlı işyeri/merkez/şube tanımları |
| **Organizasyon (`OrganizationUnit`)** | Departman, Müdürlük, Birim, Şube. Hiyerarşik yapı (ParentId). Bölüm Amirliği ilişkisi burada |
| **Görev (`Job`)** | Çalışanın yaptığı görev tanımı |
| **Ünvan (`Title`)** | Kurumsal ünvan tanımı |
| **Pozisyon (`Position`)** | Kadro ile ilişkili yapılandırılmış tanım |

### 4.4 Personellik Yapısı (EmployeeAssignment)

`EmployeeAssignment` koleksiyonu asıl iş ilişkisini tutacak ve `IDateItem`'tan türemeli.

**Temel Alanlar:**
- `PersonId` (Persons koleksiyonuna referans)
- `CompanyId`, `WorkplaceId`, `OrganizationUnitId`, `JobId`, `TitleId`, `PositionId`
- `ManagerPersonId` (amir ilişkisi)
- `SicilNo`, `PersonelNo`
- `StartDate`, `EndDate` (0001-01-01 = aktif)
- `Aciklama`, `CreatedAt`, `CreatedBy`

**Kurallar:**
- Terfi/nakil/atama değişikliği → eski kayıt `EndDate` ile kapatılır, yeni kayıt `StartDate` ile açılır
- Bir kişi aynı anda sadece 1 adet aktif atamaya sahip olmalı
- Hiyerarşi OrganizasyonUnit üzerinden, bölüm amiri ilişkisi ile belirlenebilecek

### 4.5 MongoDB Koleksiyon Önerisi (Hr.Ozluk)

| Koleksiyon | Tür | Açıklama |
|---|---|---|
| `persons` | Ana Kayıt | Temel kimlik bilgileri |
| `person_families` | Kişiye bağlı (tarihli) | Aile bilgileri |
| `person_addresses` | Kişiye bağlı (tarihli) | Adres bilgileri |
| `person_contacts` | Kişiye bağlı (tarihli) | İletişim bilgileri |
| `definitions_companies` | Tanım | Firma tanımları |
| `definitions_workplaces` | Tanım | İşyeri tanımları |
| `definitions_organization_units` | Tanım (Hiyerarşik) | Organizasyon birimleri + ParentId |
| `definitions_jobs` | Tanım | Görev tanımları |
| `definitions_titles` | Tanım | Ünvan tanımları |
| `definitions_positions` | Tanım | Pozisyon tanımları |
| `employee_assignments` | Personellik (tarihli) | Asıl iş ilişkisi + atamalar |

## 5. Frontend Önerisi

**Kabul Edilen Yaklaşım:** Next.js (App Router) + TypeScript

### 5.1 Teknoloji Stack'i
- **Framework:** Next.js (App Router)
- **Dil:** TypeScript
- **Styling:** TailwindCSS
- **UI Bileşenleri:** shadcn/ui
- **Form Yönetimi:** React Hook Form + Zod
- **Data Fetching:** TanStack Query
- **Kimlik Doğrulama:** Auth.js (next-auth) + Keycloak Provider (BFF yaklaşımı ile HttpOnly cookie)
- **API İletişimi:** Gateway (YARP) üzerinden

### 5.2 Monorepo Yapısı Önerisi

```text
apps/
├── web/                # Next.js App Router (ana arayüz)

packages/
├── ui/                 # shadcn/ui tabanlı ortak bileşenler
├── auth/               # Yetki/rol kontrolü, route guard helpers
├── api-client/         # Gateway üzerinden çağrılar için client
├── forms/              # Ortak bilgi giriş form şablonları (DataEntryShell)
├── permissions/        # Ekran + buton bazlı yetki tanımları
└── types/              # Backend DTO tipleri
```

### 5.3 Route Yapısı
```text
app/
├── (auth)/login        # Giriş sayfası
├── (app)/
│   ├── layout.tsx      # Ortak menü + yetki kontrolü
│   ├── dashboard/
│   ├── ozluk/
│   ├── idari-isler/
│   ├── izin/
│   ├── ucret/
│   ├── performans/
│   ├── muhasebe-fis/
│   └── tanimlar/       # Bilgi Giriş / Master Data Modülü
```

### 5.4 "Bilgi Girişleri" Modülü
- **DataEntryShell:** Tüm tanım sayfaları için ortak kabuk
- **Generic CRUD:** Benzer yapılı master data için şablon
- **Ekran + Buton Bazlı Yetki:** Login modülünden gelen yetkilerle kontrol
- **Effective Dating Destekli Tablolar:** Gerektiğinde tarihli görünüm

## 6. Dapr Kullanımı

| Bileşen | Amaç |
|---|---|
| **Service Invocation** | Servisler arası senkron çağrılar |
| **Pub/Sub** | Olay tabanlı iletişim (örn. PersonelAtandi, MaasHesaplandi, IzinOnaylandi, MuhasebeFisiOlustur) |
| **Dapr Secrets/Configuration** | İleride hassas ayarlar için değerlendirilebilir |

## 7. Muhasebe Fişi Sistemi - MVP Kapsamı

- **Temel Kapsam:** Hesap planı (hesap ağacı) + fiş tanımları/girişi
- **Önemli Alanlar:** Fiş No, Tarih, İşyeri
- **İlk Aşama:** Manuel fiş girişi yeterli
- **Entegrasyon:** Bordro/avans ödemeleri gibi durumlarda entegre olması gerekli (PubSub ile)
- **İleride:** Otomatik fiş üretimi düşünülebilir

## 8. Açık Sorular ve Netleştirilecek Noktalar

| No | Soru | Durum |
|---|---|---|
| 1 | TC Kimlik No `Person` içinde zorunlu mu? | Açık |
| 2 | Aile/Adres/İletişim geçmiş halleri raporlanacak mı? (ayrı tarihlı önerildi) | Açık - geçmiş önemli görünüyor |
| 3 | Pozisyon vs Ünvan vs Görev farkı netleştirilmeli | Açık |
| 4 | `ManagerPersonId` mi yoksa sadece OrganizasyonUnit üzerinden mi amirlik? | Açık - ikisi birlikte daha güvenli görünüyor |
| 5 | Bir kişi aynı anda birden fazla aktif iş ilişkisinde olabilir mi? | Varsayım: Hayır (tek aktif atama) |
| 6 | Keycloak realm/client Docker ile otomatik mi kurulacak? | Açık |
| 7 | Keycloak grup/rol yapısı mı, yoksa tamamen Login modülünde mi yönetilecek? | Açık - esnek yapı için Login modülünde yönetim öneriliyor |
| 8 | MongoDB - aynı sunucuda ayrı DB mi, yoksa ayrı instance mı? | Açık - ayrı DB öneriliyor |
| 9 | Frontend SPA mı olacak? | Next.js SPA şeklinde düşünülüyor |
| 10 | Bölüm Amiri kapsamı - sadece direkt bağlı mı, yoksa tüm alt zincir mi? | Açık |

## 9. Önerilen Uygulama Sırası

1. **İskelet** - Aspire AppHost + ServiceDefaults + YARP Gateway + Dapr + Docker Compose (Keycloak, MongoDB)
2. **Shared** - Shared.Auth (JWT/Policy), Shared.Contracts (eventler), Shared.Dapr, EffectiveDating helper'ları, IDateItem
3. **Login/Auth** - Keycloak entegrasyonu + Grup/Kişi/Yetki matrisi, yetki kontrolü (webservis+ekran ayrı)
4. **Özlük** - Domain modelleri (Person, PersonFamily/Address/Contact, Tanımlar, EmployeeAssignment), effective dating logic
5. **Diğer Servisler** - İdari İşler, İzin, Ücret, Performans
6. **Muhasebe Fişi** - Hesap ağacı + fiş girişi, PubSub ile olay dinleme
7. **Frontend** - Next.js iskeleti, Auth.js + Keycloak, modül bazlı route yapısı
8. **Entegrasyon & Test** - Dapr entegrasyonu, yetkilendirme, end-to-end test

## 10. Teknoloji Özeti

| Katman | Teknoloji | Açıklama |
|---|---|---|
| **Orkestrasyon** | .NET Aspire | AppHost, ServiceDefaults |
| **Mikroservis Komünikasyonu** | Dapr | Service Invocation, Pub/Sub |
| **API Gateway** | YARP | Tüm istekler gateway üzerinden |
| **Kimlik** | Keycloak | Identity Provider |
| **Auth Token** | JWT | Bearer token |
| **Backend** | ASP.NET Core (Web API) | Mikroservisler |
| **Veritabanı** | MongoDB (öncelik) | Her servise ayrı DB |
| **Frontend** | Next.js 15 (App Router), TypeScript | Web arayüzü |
| **UI** | TailwindCSS + shadcn/ui | Bileşen kütüphanesi |
| **Container** | Docker/Compose | Lokal geliştirme ortamı |
