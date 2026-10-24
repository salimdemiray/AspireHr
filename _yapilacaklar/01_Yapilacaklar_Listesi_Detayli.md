# AspireHR - Yapılacaklar Listesi (Detaylı)

*Oluşturulma Tarihi: 2025-10-24*  
*Amaç: Projenin adım adım geliştirilmesi için yol haritası*

## 1. Genel Amaç

.NET Aspire + Dapr ile grup bazlı mikroservis mimarisi, Keycloak + JWT ile login/yetkilendirme, YARP API Gateway, MongoDB (öncelik) ve Next.js (App Router) + TypeScript ile frontend olacak bir HR uygulaması geliştirmek.

## 2. Altyapı Kurulumu (Başlangıç)

| ID | Görev | Durum | Açıklama |
|---|---|---|---|
| ALT-01 | .NET Aspire AppHost Projesi Kurulumu | Beklemede | AppHost + ServiceDefaults ile orkestrasyon kurulacak |
| ALT-02 | Dapr Entegrasyonu | Beklemede | Dapr sidecar'lar, service invocation, pub/sub yapılandırması |
| ALT-03 | YARP API Gateway Kurulumu | Beklemede | Tüm servisler Gateway altında toplanacak |
| ALT-04 | Docker/Compose Altyapısı | Beklemede | Keycloak, MongoDB vb. docker-compose ile ayağa kalkacak şekilde ayarlama |
| ALT-05 | Keycloak Kurulumu (Docker) | Beklemede | Realm/client, JWT doğrulaması için hazır hale getirme |
| ALT-06 | Shared Projeler Oluşturma | Beklemede | Shared.Domain, Shared.Auth, Shared.Contracts, Shared.Dapr |

## 3. Ortak Katman (Shared)

| ID | Görev | Durum | Açıklama |
|---|---|---|---|
| SHA-01 | BaseEntity Oluştur | Beklemede | Id(string), CreatedAt(Utc), CreatedBy, UpdatedAt?, UpdatedBy? |
| SHA-02 | IDateItem Oluştur | Beklemede | StartDate, EndDate (boş/null -> 0001-01-01) |
| SHA-03 | ApprovalStatus Enum | Beklemede | Draft, Pending, Approved, Rejected, Returned, Cancelled |
| SHA-04 | BaseApprovableEntity | Beklemede | Onay süreci alanları (CurrentApproverId/RoleId/GroupId, ApprovalStep, SubmittedAt vb.) |
| SHA-05 | ApprovalHistory Modeli | Beklemede | Değiştirilemez onay geçmişi (EntityId, EntityType, From/ToStatus, ActionBy vb.) |
| SHA-06 | EffectiveDating Helper'ları | Beklemede | O tarihte geçerli kayıt filtrelemesi için extension/metotlar |
| SHA-07 | JWT/Policy Yapısı (Shared.Auth) | Beklemede | Keycloak JWT doğrulaması, ekran + webservis bazlı yetkilendirme altyapısı |
| SHA-08 | Dapr Helpers (Shared.Dapr) | Beklemede | Service Invocation ve PubSub yardımcı metodları |
| SHA-09 | Event Contracts (Shared.Contracts) | Beklemede | WorkflowCompletedEvent ve diğer ortak eventler |

## 4. Login/Auth Servisi

| ID | Görev | Durum | Açıklama |
|---|---|---|---|
| AUTH-01 | Servis İskeleti | Beklemede | Auth.Login projesi (Web API), Dapr entegrasyonu |
| AUTH-02 | Keycloak Entegrasyonu | Beklemede | OIDC/JWT doğrulaması |
| AUTH-03 | Grup/Kişi/Yetki Yapısı | Beklemede | Gruplar, Kullanıcılar, Yetkiler, Ekran + Webservis bazlı yetkiler ayrı yönetimi |
| AUTH-04 | Yetki Matrisi Yönetimi | Beklemede | Grup-Kişi-İzin eşleştirmeleri |
| AUTH-05 | Kullanıcı Bilgileri ve Claim'ler | Beklemede | Uygulama seviyesinde yetki üretimi, Gateway ile entegrasyon |

## 5. Onay/İş Akışı Servisi (Approval.Workflow)

| ID | Görev | Durum | Açıklama |
|---|---|---|---|
| WF-01 | Servis İskeleti | Beklemede | Approval.Workflow projesi, MongoDB bağlantısı |
| WF-02 | WorkflowTemplate | Beklemede | Sürükle-bırak şeması, DefinitionJson, RoutingRules, versiyonlama |
| WF-03 | WorkflowRule (Koşullu Yönlendirme) | Beklemede | Modele/içeriğe göre farklı şablon seçimi |
| WF-04 | WorkflowInstance | Beklemede | Çalışan onay süreci, ContextData, durum takibi |
| WF-05 | WorkflowTask (Onay Bekleyenler) | Beklemede | Kişi/Rol/Grup bazlı görev atamaları |
| WF-06 | WorkflowHistory | Beklemede | Değiştirilemez geçmiş kayıtları |
| WF-07 | Workflow Engine (Motor) | Beklemede | Start/Approver/Condition/Parallel/Notify/End node tipleri, dallanma ve ilerleme |
| WF-08 | API Endpoint'leri | Beklemede | StartWorkflow, Approve/Reject/Return, MyPendingTasks, Templates vb. |
| WF-09 | Event Yayınlama (PubSub) | Beklemede | WorkflowCompletedEvent ile kaynak servislere bildirim |
| WF-10 | Şablon Tasarımcısı Altyapısı | Beklemede | React Flow ile sürükle-bırak editörü için API desteği |

## 6. Hr.Ozluk Servisi

| ID | Görev | Durum | Açıklama |
|---|---|---|---|
| OZL-01 | Servis İskeleti | Beklemede | Hr.Ozluk projesi, Dapr + MongoDB |
| OZL-02 | Person (Kişi) Modeli | Beklemede | Temel kimlik bilgileri (BaseEntity) |
| OZL-03 | PersonFamily/PersonAddress/PersonContact | Beklemede | Kişiye bağlı tarihli bilgiler (IDateItem, ayrı koleksiyonlar) |
| OZL-04 | Tanımlar (Master Data) | Beklemede | Company, Workplace, OrganizationUnit, Job, Title, Position |
| OZL-05 | OrganizationUnit Hiyerarşisi | Beklemede | ParentId ile ağaç yapısı, Bölüm Amiri ilişkisi |
| OZL-06 | EmployeeAssignment (Personellik) | Beklemede | İş ilişkisi (BaseEntity + IDateItem), ManagerPersonId, tek aktif atama kuralı |
| OZL-07 | Effective Dating Sorgulamaları | Beklemede | O tarihte geçerli kayıtları getirme mantığı |
| OZL-08 | CRUD API'leri | Beklemede | Tüm özlük modülü için CRUD ve listeleme |

## 7. Diğer Grup Bazlı Servisler

| ID | Görev | Durum | Açıklama |
|---|---|---|---|
| SRV-01 | Hr.IdariIsler Servisi | Beklemede | Evrak, Yazı/Onay, Arşiv, İdari Talepler |
| SRV-02 | Hr.Izin Servisi | Beklemede | İzin türleri, talepler, onay akışı (Approval.Workflow ile entegre) |
| SRV-03 | Hr.Ucret Servisi | Beklemede | Bordro, Ücret kalemleri, Ek ödemeler, Maaş geçmişi |
| SRV-04 | Hr.Performans Servisi | Beklemede | Değerlendirme dönemleri, kriterler, formlar, sonuçlar |
| SRV-05 | Finance.Fis (Muhasebe Fişi Sistemi) | Beklemede | Hesap planı (Hesap Ağacı), Fişler, Fiş Detayları. Fiş No/Tarih/İşyeri önemli. Manuel fiş girişi öncelik |
| SRV-06 | Servisler Arası Entegrasyon | Beklemede | Dapr Service Invocation/PubSub, Shared.Contracts eventleri (Bordro -> Muhasebe Fişi vb.) |

## 8. Frontend (Next.js)

| ID | Görev | Durum | Açıklama |
|---|---|---|---|
| FE-01 | Next.js App Router İskeleti | Beklemede | TypeScript, TailwindCSS, shadcn/ui |
| FE-02 | Monorepo Yapısı | Beklemede | apps/web, packages/ui, packages/auth, packages/api-client, packages/forms, packages/permissions, packages/types |
| FE-03 | Auth.js + Keycloak (BFF) | Beklemede | HttpOnly cookie ile oturum yönetimi |
| FE-04 | Route Yapısı (Modül Bazlı) | Beklemede | (auth)/login, (app)/dashboard, ozluk, idari-isler, izin, ucret, performans, muhasebe-fis, tanimlar |
| FE-05 | DataEntryShell & Generic CRUD | Beklemede | Bilgi girişleri/Master Data için ortak şablon |
| FE-06 | Menü & Yetki Kontrolü | Beklemede | Ekran + buton bazlı yetkilendirme, route guard |
| FE-07 | Onay Bekleyenlerim & Onay Ekranı | Beklemede | Approval.Workflow ile entegre |
| FE-08 | Workflow Şema Tasarımcısı | Beklemede | React Flow ile sürükle-bırak onay şeması tasarımı |
| FE-09 | API Client (Gateway) | Beklemede | TanStack Query ile Gateway üzerinden API çağrıları |

## 9. Entegrasyon, Test & Yayın

| ID | Görev | Durum | Açıklama |
|---|---|---|---|
| INT-01 | Aspire AppHost Entegrasyonu | Beklemede | Tüm servisler, Dapr sidecar'lar, Keycloak, MongoDB entegrasyonu |
| INT-02 | Gateway Rota Tanımları (YARP) | Beklemede | Tüm servisler için route mapping ve yetkilendirme |
| INT-03 | Effective Dating Uygulaması | Beklemede | Tüm ilgili servislerde standart filtreleme kullanımı |
| INT-04 | Approval.Workflow Entegrasyonu | Beklemede | İzin ve diğer onay gerektiren modüllerle entegrasyon |
| INT-05 | Docker Üzerinden Ayağa Kaldırma | Beklemede | docker compose ile lokal ortamda çalıştırılabilir hale getirme |
| INT-06 | İlk Entegrasyon Testleri | Beklemede | Login, yetkilendirme, onay süreci, özlük, muhasebe fişi temel testleri |

## 10. Açık Sorular (Netleştirilmeli)

| ID | Soru | Durum | Öneri |
|---|---|---|---|
| ASK-01 | TC Kimlik No zorunlu mu? | Açık | Netleştirilmeli |
| ASK-02 | Aile/Adres/İletişim geçmiş halleri raporlanacak mı? | Açık | Ayrı tarihlı koleksiyon öneriliyor - geçmiş önemli görünüyor |
| ASK-03 | Pozisyon vs Ünvan vs Görev farkı net mi? | Açık | Netleştirilmeli |
| ASK-04 | Bölüm Amiri ilişkisi nasıl tutulsun? | Açık | ManagerPersonId + OrganizasyonUnit birlikte öneriliyor |
| ASK-05 | Bir kişi aynı anda birden fazla aktif atamada olabilir mi? | Açık | Varsayım: Hayır (tek aktif atama) |
| ASK-06 | Keycloak kurulumu nasıl olacak? | Açık | Docker ile otomatik ayağa kalksın mı? |
| ASK-07 | Yetki yönetimi Keycloak mu, Login modülü mü? | Açık | Tamamen Login modülünde yönetim öneriliyor (daha esnek) |
| ASK-08 | MongoDB yapısı? | Açık | Aynı sunucuda servis başına ayrı veritabanı öneriliyor |
| ASK-09 | Frontend yaklaşımı? | Açık | Next.js SPA (BFF ile HttpOnly cookie) öneriliyor |
| ASK-10 | Bölüm Amiri kapsamı? | Açık | Sadece direkt bağlı mı, yoksa tüm alt zincir mi? |

## Notlar

- Tüm entity'ler `BaseEntity`'den türeyecek
- Zamana bağlı kayıtlar `IDateItem`'tan türeyecek (EndDate boş -> `0001-01-01`)
- Onay gerektiren işlemler ayrı `Approval.Workflow` servisine yönlendirilecek
- Her servis kendi veritabanını/koleksiyonunu kullanacak (veri izolasyonu)
- Webservis ve ekran bazlı yetkiler ayrı değerlendirilecek
- Dapr Pub/Sub ile olay tabanlı entegrasyon yapılacak
- Tüm dış erişim Gateway (YARP) üzerinden sağlanacak
- Frontend'de BFF yaklaşımı ile HttpOnly cookie kullanılacak
- Effective Dating kuralı tüm ilgili alanlarda uygulanacak
- `BaseApprovableEntity` sadece onay süreci olan işlemlerde kullanılacak, master data kayıtlarında kullanılmayacak
