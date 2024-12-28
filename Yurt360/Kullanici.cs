using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Yurt360
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Net.Mail;
    using System.IO;
    using System.Data.SqlClient;

    namespace Yurt360
    {
        internal class Kullanici
        {

            public int Id { get; set; }
            public string Adi { get; set; }
            public string Soyadi { get; set; }
            public string TCKimlik { get; set; }
            public string TelNo { get; set; }
            public string Email { get; set; }
            public string YurtIsmi { get; set; }
            public string OdaNo { get; set; }
            public string YatakNo { get; set; }
            public string Sifre { get; set; }
            public string sifretekrar { get; set; }
            public YetkiTipi YetkiTipi { get; set; }
            public static string HashleSifre(string sifre)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(sifre));
                    return Convert.ToBase64String(hashBytes); // Şifreyi hashliyoruz
                }
            }

            // Telefon numarasının formatını kontrol etme (11 haneli ve doğru formatta)
            public static bool TelefonNumarasiDogruMu(string telNo)
            {
                bool result = Regex.IsMatch(telNo, @"^\d{11}$");
                Logger logger = new Logger();
                logger.Log(result ? "Info" : "Error", $"Telefon numarası doğrulama: {telNo} işlemi {(result ? "Başarılı" : "Hatalı")}");
                return result;
            }
            // tc kimlik 9(pasaport numarası) veya 11 haneli olması sağlandı
            public static bool TCKimlikDogruMu(string tcKimlik)
            {
                bool result = Regex.IsMatch(tcKimlik, @"^\d{9}$|^\d{11}$");
                Logger logger = new Logger();
                logger.Log(result ? "Info" : "Error", $"TCKimlik doğrulama: {tcKimlik} işlemi {(result ? "Başarılı" : "Hatalı")}");
                return result;
            }

            // E-posta formatının geçerli olup olmadığını kontrol etme
            public static bool IsValidEmail(string email)
            {
                Logger logger = new Logger();
                try
                {
                    var addr = new MailAddress(email);
                    bool result = addr.Address == email;
                    logger.Log(result ? "Info" : "Error", $"E-posta doğrulama: {email} işlemi {(result ? "Başarılı" : "Hatalı")}");
                    return result;
                }
                catch
                {
                    logger.Log("Error", $"E-posta doğrulama: {email} işlemi Hatalı");
                    return false;
                }
            }

            // Şifrelerin eşleşip eşleşmediğini kontrol etme
            public static bool SifreleriKontrolEt(string sifre, string sifreTekrar, string tcKimlik)
            {
                string hashSifre = HashleSifre(sifre);
                string hashSifreTekrar = HashleSifre(sifreTekrar);
                bool result = hashSifre == hashSifreTekrar;

                Logger logger = new Logger();
                if (!result)
                {
                    logger.Log("Error", $"Şifreler uyuşmuyor: {tcKimlik}");
                }
                else
                {
                    logger.Log("Info", $"Şifreler eşleşiyor: {tcKimlik}");
                }
                return result;
            }

            // Girişte şifrenin doğruluğunu kontrol etme (hashlenmiş şifreyi karşılaştırma)
            public static bool SifreDogruMu(string sifre, string sifreHash, string tcKimlik)
            {
                string hashSifre = HashleSifre(sifre);
                bool result = hashSifre == sifreHash;

                Logger logger = new Logger();
                if (!result)
                {
                    logger.Log("Error", $"Şifre doğrulama başarısız: {tcKimlik}");
                }
                else
                {
                    logger.Log("Info", $"Şifre doğrulama başarılı: {tcKimlik}");
                }
                return result;
            }

            
            private static void LogIslem(string islemAdi, string deger, bool basariliMi)
            {
                string sonuc = basariliMi ? "Başarılı" : "Hatalı";
                string mesaj = $"{islemAdi}: {deger} işlemi {sonuc}";

                try
                {
                    Logger logger = new Logger();
                    logger.Log(basariliMi ? "Info" : "Error", mesaj);
                }
                catch (Exception ex)
                {
                    // Loglama başarısız olursa
                    File.AppendAllText("hata.log", $"{DateTime.Now}: Loglama hatası: {ex.Message}\n");

                }
            }
        }

            public enum YetkiTipi
            {
                Admin = 1,
                Memur = 2,
                Ogrenci = 3
            }
    }

}
