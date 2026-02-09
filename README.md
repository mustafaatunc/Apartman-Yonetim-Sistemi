# 🏢 Apartman Yönetim Sistemi

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-blue?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)

**Apartman Yönetim Sistemi**, site ve apartman yöneticilerinin aidat, gider, kasa ve sakinleri kolayca yönetmesini sağlayan kapsamlı bir masaüstü otomasyon projesidir.

Bu proje, **güvenli kodlama prensipleri** (SQL Injection koruması), **rol bazlı yetkilendirme** ve **modern veritabanı mimarisi** kullanılarak geliştirilmiştir.

---

## 🚀 Özellikler

### 👤 Yönetici Paneli
- **Daire & Sakin Yönetimi:** Daire ekleme, kişi atama ve düzenleme.
- **Finansal İşlemler:** Gelir (Aidat) ve Gider (Fatura, Bakım) kalemlerinin işlenmesi.
- **Borçlandırma:** Toplu veya bireysel borç/aidat atama.
- **Raporlama:** Kasa durumunu ve geçmiş hareketleri görüntüleme.
- **Log Sistemi:** Yapılan işlemlerin IP ve Kullanıcı bazlı kaydı.

### 🏠 Kullanıcı (Sakin) Paneli
- Kişisel borç sorgulama.
- Ödeme geçmişini görüntüleme.

### 🔒 Teknik Özellikler
- **Güvenlik:** Parametreli sorgular ile %100 SQL Injection koruması.
- **Yetkilendirme:** Admin, Yönetici ve Kullanıcı rolleri.
- **Veritabanı:** İlişkisel SQL Server veritabanı yapısı.

---

## 📸 Ekran Görüntüleri

| Giriş Ekranı | Ana Panel |
| :---: | :---: |
|<img width="1217" height="713" alt="1" src="https://github.com/user-attachments/assets/2b06c6c5-a288-4d7d-a2e5-75a1fa8f64eb" /> | <img width="1915" height="1016" alt="2" src="https://github.com/user-attachments/assets/ca083e3e-24bf-4874-8b57-3d20859d43cb" /> |

| Yönetici Paneli | Apartman İşlemleri |
| :---: | :---: |
|<img width="1640" height="676" alt="3" src="https://github.com/user-attachments/assets/d72ed5f6-7dbc-4245-ac18-4e36af16dc5e" /> | <img width="1251" height="586" alt="4" src="https://github.com/user-attachments/assets/57a50931-061e-4a73-a371-8731539eb9a8" /> |

| Apartman Yönetim | Kategori İşlemleri | Kayıtlar |
| :---: | :---: | :---: |
|<img width="1072" height="697" alt="5" src="https://github.com/user-attachments/assets/a769ef6e-16a3-40fb-b976-5bd72d287310" /> |<img width="1157" height="666" alt="6" src="https://github.com/user-attachments/assets/3b00e4eb-6b25-4b29-9bff-e67d3051e10d" /> | <img width="972" height="567" alt="7" src="https://github.com/user-attachments/assets/029cb5f5-6f61-4647-8b03-928381312ad0" /> |




 




---

## 🛠️ Kurulum ve Çalıştırma

Projeyi kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları izleyin:

### 1. Gereksinimler
- Visual Studio 2019 veya 2022
- SQL Server Express (LocalDB)
- .NET Framework 4.8

### 2. Veritabanı Kurulumu
1. SQL Server Management Studio (SSMS) uygulamasını açın.
2. `Apartman_Yonetim` adında yeni bir veritabanı oluşturun.
3. Proje dosyaları içerisindeki **`Veritabani_Kurulum.sql`** dosyasını açın ve içindeki kodları çalıştırın (Execute).
4. Bu işlem gerekli tabloları ve Admin kullanıcısını oluşturacaktır.

### 3. Bağlantı Ayarı
Eğer SQL Server adınız `.\SQLEXPRESS` değilse, projedeki `sqlbaglantisi.cs` dosyasını açıp kendi sunucu adınızı güncelleyin:
```csharp
public SqlConnection baglan()
{
    // Buradaki adresi kendi sunucunuza göre düzenleyin
    SqlConnection baglanti = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=Apartman_Yonetim;Integrated Security=True");
    baglanti.Open();
    return baglanti;
}
