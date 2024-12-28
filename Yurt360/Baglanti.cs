using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yurt360.Yurt360;

namespace Yurt360
{
    internal class Baglanti

    {
     public static string baglantiDizisi = "server=DESKTOP-57F0A7E\\SQLEXPRESS;database=Yurt360;integrated security=True";

        public static bool TelefonNumarasiKontrol(string telNo)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(baglantiDizisi))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM ogrenciler WHERE TelNo = @TelNo";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@TelNo", telNo);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Veritabanı bağlantısı hatası: " + ex.Message);
            }
        }
        public static bool KullaniciKaydet(Kullanici kullanici)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(baglantiDizisi))
                {
                    connection.Open();
                    string query = "INSERT INTO ogrenciler (adı, soyadı, tckimlik, telno, email, yurtisim, odano, yatakno, sifre, sifretekrar, yetkitipi) " +
                                   "VALUES (@Adi, @Soyadi, @TCKimlik, @TelNo, @Email, @YurtIsmi, @OdaNo, @YatakNo, @Sifre, @SifreTekrar, 3)";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Parameters.AddWithValue("@Adi", kullanici.Adi);
                    cmd.Parameters.AddWithValue("@Soyadi", kullanici.Soyadi);
                    cmd.Parameters.AddWithValue("@TCKimlik", kullanici.TCKimlik);
                    cmd.Parameters.AddWithValue("@TelNo", kullanici.TelNo);
                    cmd.Parameters.AddWithValue("@Email", kullanici.Email);
                    cmd.Parameters.AddWithValue("@YurtIsmi", kullanici.YurtIsmi);
                    cmd.Parameters.AddWithValue("@OdaNo", kullanici.OdaNo);
                    cmd.Parameters.AddWithValue("@YatakNo", kullanici.YatakNo);
                    cmd.Parameters.AddWithValue("@Sifre", kullanici.Sifre);
                    cmd.Parameters.AddWithValue("@SifreTekrar", kullanici.sifretekrar);
                    cmd.Parameters.AddWithValue("@YetkiTipi", (int)kullanici.YetkiTipi);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Logger logger = new Logger();
                logger.Log("Error", ex.Message);
                throw new Exception("Veritabanı işlemi hatası: " + ex.Message);
            }
        }//logiin sayfası veritabanı bağlantısı kodları
        public static Kullanici KullaniciDogrula(string tcKimlik, string sifre)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(baglantiDizisi))
                {
                    connection.Open();

                    // Memur tablosunu kontrol et
                    string queryMemur = "SELECT * FROM memur WHERE tckimlik = @TCKimlik";
                    SqlCommand cmdMemur = new SqlCommand(queryMemur, connection);
                    cmdMemur.Parameters.AddWithValue("@TCKimlik", tcKimlik);

                    SqlDataReader readerMemur = cmdMemur.ExecuteReader();
                    if (readerMemur.Read())
                    {
                        // Şifre hashlenmeden kontrol ediliyor
                        if (readerMemur["sifre"].ToString() == sifre)
                        {
                            return new Kullanici
                            {
                                Id = readerMemur.GetInt32(0),
                                Adi = readerMemur["adı"].ToString(),
                                Soyadi = readerMemur["soyadı"].ToString(),
                                TCKimlik = readerMemur["tckimlik"].ToString(),
                                TelNo = readerMemur["telno"].ToString(),
                                Email = readerMemur["email"].ToString(),
                                YurtIsmi = readerMemur["yurtisim"].ToString(),
                                YetkiTipi = (YetkiTipi)Enum.Parse(typeof(YetkiTipi), readerMemur["yetkitipi"].ToString())
                            };
                        }
                    }
                    readerMemur.Close(); // İlk sorgunun reader'ını kapatıyoruz.
                                         // Admin tablosunu kontrol et
                    string queryAdmin = "SELECT * FROM admin WHERE tckimlik = @TCKimlik";
                    SqlCommand cmdAdmin = new SqlCommand(queryAdmin, connection);
                    cmdAdmin.Parameters.AddWithValue("@TCKimlik", tcKimlik);

                    SqlDataReader readerAdmin = cmdAdmin.ExecuteReader();
                    if (readerAdmin.Read())
                    {
                        // Admin için şifre doğrulaması
                        if (readerAdmin["sifre"].ToString() == sifre)
                        {
                            return new Kullanici
                            {
                                TCKimlik = readerAdmin["tckimlik"].ToString(),
                                YetkiTipi = YetkiTipi.Admin // Admin yetkisi atıyoruz
                            };
                        }
                    }
                    readerAdmin.Close(); // Admin kontrolü bitti, reader'ı kapatıyoruz.

                    // Eğer memur tablosunda bulunmazsa ogrenciler tablosunu kontrol et
                    string queryOgrenci = "SELECT * FROM ogrenciler WHERE tckimlik = @TCKimlik";
                    SqlCommand cmdOgrenci = new SqlCommand(queryOgrenci, connection);
                    cmdOgrenci.Parameters.AddWithValue("@TCKimlik", tcKimlik);

                    SqlDataReader readerOgrenci = cmdOgrenci.ExecuteReader();
                    if (readerOgrenci.Read())
                    {
                        // Şifre hash kontrolü (ogrenciler için)
                        string hashedSifre = Kullanici.HashleSifre(sifre);
                        if (readerOgrenci["sifre"].ToString() == hashedSifre)
                        {
                            return new Kullanici
                            {
                                Id = readerOgrenci.GetInt32(0),
                                Adi = readerOgrenci["adı"].ToString(),
                                Soyadi = readerOgrenci["soyadı"].ToString(),
                                TCKimlik = readerOgrenci["tckimlik"].ToString(),
                                TelNo = readerOgrenci["telno"].ToString(),
                                Email = readerOgrenci["email"].ToString(),
                                YurtIsmi = readerOgrenci["yurtisim"].ToString(),
                                OdaNo = readerOgrenci["odano"].ToString(),
                                YatakNo = readerOgrenci["yatakno"].ToString(),
                                YetkiTipi = (YetkiTipi)Enum.Parse(typeof(YetkiTipi), readerOgrenci["yetkitipi"].ToString())
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Kullanıcı doğrulama hatası: " + ex.Message);
            }

            return null; // Kullanıcı bulunamadı veya şifre yanlış

        }

    }
}
    




































