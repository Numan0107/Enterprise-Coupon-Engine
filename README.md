# 🚀 Enterprise Campaign & Coupon Engine

Bu proje, e-ticaret sitelerinin (Hepsiburada, Trendyol vb.) büyük indirim dönemlerinde yaşadığı **anlık yoğun trafiği ve kupon çakışmalarını** yönetmek için geliştirilmiş yüksek performanslı bir **Backend API Motorudur**.

## 🎯 Proje Ne Yapıyor?
* **RAM Üzerinden Sorgu:** Gelen kupon isteklerini yavaş çalışan SQL veritabanına göndermez; .NET `IDistributedCache` kullanarak verileri doğrudan RAM bellekten milisaniyeler içinde okur ve onaylar.
* **Kupon Kilit Mekanizması (Lock):** Aynı kupon koduna aynı saniyede 10.000 kişi tıkladığında, kupon sınırının eksiye düşmesini (veri tutarsızlığını) engelleyen kurumsal bir kilitleme mimarisi barındırır.
* **Modern Minimal Tasarım:** Hantal Controller yapıları yerine .NET 9.0/10.0 dünyasının en hızlı API şablonu olan **Minimal APIs** ile çalışır.

## 🛠️ Teknik Altyapı
* **Dil & Framework:** C# / .NET 9.0 (Minimal APIs)
* **Bellek Yönetimi:** IDistributedCache (In-Memory Caching)
* **Veri Standardı:** RESTful API Standartları (JSON)
