using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Yurt360.Yurt360;

namespace Yurt360
{
    internal class Kullaniciislem
    {
        public static bool KayitYap(Kullanici kullanici)
        {
            try
            {
                if (!Kullanici.TCKimlikDogruMu(kullanici.TCKimlik))
                {
                    MessageBox.Show("Geçerli bir TC Kimlik Numarası (9 veya 11 haneli) giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger logger = new Logger();
                    logger.Log("Error", $"Hatalı TC Kimlik girildi: {kullanici.TCKimlik}");
                    return false; // Eğer TC Kimlik yanlışsa false döndür
                }

                if (!Kullanici.SifreleriKontrolEt(kullanici.Sifre, kullanici.sifretekrar, kullanici.TCKimlik))
                {
                    MessageBox.Show("Şifreler uyuşmuyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger logger = new Logger();
                    logger.Log("Error", $"Şifreler uyuşmuyor: {kullanici.TCKimlik}");
                    return false;
                }

                if (!Kullanici.TelefonNumarasiDogruMu(kullanici.TelNo))
                {
                    MessageBox.Show("Telefon numarası geçerli değil.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger logger = new Logger();
                    logger.Log("Error", $"Geçersiz telefon numarası: {kullanici.TelNo}");
                    return false;
                }

                if (!Kullanici.IsValidEmail(kullanici.Email))
                {
                    MessageBox.Show("Geçerli bir e-posta adresi giriniz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger logger = new Logger();
                    logger.Log("Error", $"Geçersiz e-posta adresi: {kullanici.Email}");
                    return false;
                }

                if (Baglanti.TelefonNumarasiKontrol(kullanici.TelNo))
                {
                    MessageBox.Show("Bu telefon numarası zaten kayıtlı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger logger = new Logger();
                    logger.Log("Error", $"Zaten kayıtlı telefon numarası: {kullanici.TelNo}");
                    return false;
                }

                kullanici.Sifre = Kullanici.HashleSifre(kullanici.Sifre);

                bool kayitBasarili = Baglanti.KullaniciKaydet(kullanici);
                if (kayitBasarili)
                {
                    MessageBox.Show("Kullanıcı kaydı başarılı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Logger logger = new Logger();
                    logger.Log("Info", $"Yeni kullanıcı kaydoldu: Tc Kimlik={kullanici.TCKimlik}, Ad={kullanici.Adi}, Soyad={kullanici.Soyadi}");
                    return true; 
                }
                else
                {
                    MessageBox.Show("Kullanıcı kaydı sırasında hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger logger = new Logger();
                    logger.Log("Error", $"Kullanıcı kaydı başarısız oldu: Tc Kimlik={kullanici.TCKimlik}");
                    return false; 
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger logger = new Logger();
                logger.Log("Error", $"Hata oluştu: {ex.Message}");
                return false; // Hata durumunda false döndür
            }
        }

        public static Kullanici KullaniciDogrulaById(int kullaniciId)
        {
            Kullanici kullanici = null;
            string sorgu = "SELECT * FROM ogrenciler WHERE kullanıcıid = @kullaniciId";

            using (SqlConnection connection = new SqlConnection(Baglanti.baglantiDizisi))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sorgu, connection))
                {
                    cmd.Parameters.AddWithValue("@kullaniciId", kullaniciId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            kullanici = new Kullanici
                            {
                                Id = Convert.ToInt32(reader["kullanıcıid"]),
                                Adi = reader["adı"].ToString(),
                                Soyadi = reader["soyadı"].ToString(),
                                TCKimlik = reader["tckimlik"].ToString(),
                                TelNo = reader["telno"].ToString(),
                                Email = reader["email"].ToString(),
                                YurtIsmi = reader["yurtisim"].ToString(),
                                OdaNo = reader["odano"].ToString(),
                                YatakNo = reader["yatakno"].ToString(),
                                Sifre = reader["sifre"].ToString()
                            };
                        }
                    }
                }
            }
            return kullanici;
        }
        public static bool OgrenciBilgileriniGuncelle(int kullaniciId, string yeniTelNo, string yeniEmail, string yeniSifre)
        {
            string sorgu = "UPDATE ogrenciler SET telno = @telno, email = @email, sifre = @sifre WHERE kullanıcıid = @kullaniciId";

            using (SqlConnection connection = new SqlConnection(Baglanti.baglantiDizisi))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sorgu, connection))
                {
                    cmd.Parameters.AddWithValue("@telno", yeniTelNo);
                    cmd.Parameters.AddWithValue("@email", yeniEmail);
                    cmd.Parameters.AddWithValue("@sifre", yeniSifre);
                    cmd.Parameters.AddWithValue("@kullaniciId", kullaniciId);

                    int etkilesimSayisi = cmd.ExecuteNonQuery();
                    return etkilesimSayisi > 0; // Başarılıysa true döner
                }
            }
        }

        public bool MemurBilgileriniGuncelle(int kullaniciId, string yeniTelNo, string yeniEmail, string yeniSifre)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Baglanti.baglantiDizisi))
                {
                    conn.Open();
                    string query = "UPDATE Kullanicilar SET TelNo = @TelNo, Email = @Email, Sifre = @Sifre WHERE Id = @Id AND YetkiTipi = @YetkiTipi";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@TelNo", yeniTelNo);
                    cmd.Parameters.AddWithValue("@Email", yeniEmail);
                    cmd.Parameters.AddWithValue("@Sifre", yeniSifre);
                    cmd.Parameters.AddWithValue("@Id", kullaniciId);
                    cmd.Parameters.AddWithValue("@YetkiTipi", YetkiTipi.Memur);

                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
                return false;
            }
        }


    }
}

















