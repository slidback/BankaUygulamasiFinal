using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace BankaUygulamasıSQLDeneme
{
    public class Kisi
    {
        public string strbaglanti = "server=127.0.0.1;port=3306;user=root;password=Yigit2009.;database=bankauygulama;";
        string kartnumara;
        string ad;
        string bankaNo;
        double bakiye;
        double euroBakiye;
        double dolarBakiye;
        string tcNo;
        string sifre;
        public double dolar = 39.87;
        public double Euro = 46.57;

        public string Ad
        {
            get { return ad; }
            set { ad = value; }
        }
        public string BankaNo
        {
            get { return bankaNo; }
            set { bankaNo = value; }
        }
        public double Bakiye
        {
            get { return bakiye; }
            set { bakiye = value; }
        }
        public string Sifre
        {
            get { return sifre; }
            set { sifre = value; }
        }
        public double EuroBakiye
        {
            get { return euroBakiye; }
            set { euroBakiye = value; }
        }
        public double DolarBakiye
        {
            get { return dolarBakiye; }
            set { dolarBakiye = value; }
        }
        public string TcNo
        {
            get { return tcNo; }
            set { tcNo = value; }
        }
        public string KartNumara
        {
            get { return kartnumara; }
            set { kartnumara = value; }
        }

        public string SifreGir()
        {
            while (true)
            {
                string sifre = Console.ReadLine();
                bool sayiVarMi = sifre.Any(c => char.IsDigit(c));
                bool harfVarMi = sifre.Any(c => char.IsLetter(c));

                if ((sifre.Length == 8 || sifre.Length > 8) && sayiVarMi && harfVarMi)
                {
                    Console.WriteLine("Şifre geçerli.");
                    return sifre;
                }
                else
                {
                    Console.WriteLine("Şifre en az 8 karakter uzunluğunda olmalı ve en az bir rakam ile bir harf içermelidir.");
                }
            }
        }
        public void SifreDegistir()
        {
            Console.WriteLine("Lütfen eski şifrenizi giriniz:");
            string eskiSifre = Console.ReadLine();
            if (this.Sifre == eskiSifre)
            {
                Console.WriteLine("Lütfen yeni şifrenizi giriniz:");
                string yeniSifre = Console.ReadLine();
                bool sayiVarMi = yeniSifre.Any(c => char.IsDigit(c));
                bool harfVarMi = yeniSifre.Any(c => char.IsLetter(c));

                if ((yeniSifre.Length == 8 || yeniSifre.Length > 8) && sayiVarMi && harfVarMi)
                {
                    this.Sifre = yeniSifre;
                    Console.WriteLine("Şifreniz başarıyla değiştirildi.");
                }
                else
                {
                    Console.WriteLine("Yeni şifre en az 8 karakter uzunluğunda olmalı ve en az bir rakam ile bir harf içermelidir.");
                }
            }
            else
            {
                Console.WriteLine("Eski şifre yanlış.");
            }
            using (MySqlConnection baglanti = new MySqlConnection(strbaglanti))
            {
                baglanti.Open();
                string strUpdate = "UPDATE bankauygulama.musteri SET sifre = @sifre WHERE kartNo = @kartNo";
                using (MySqlCommand cmd2 = new MySqlCommand(strUpdate, baglanti))
                {
                    cmd2.Parameters.AddWithValue("@sifre", Sifre);
                    cmd2.Parameters.AddWithValue("@kartNo", kartnumara);

                    int affectedRows = cmd2.ExecuteNonQuery();
                    if (affectedRows > 0)
                        Console.WriteLine("Şifre veritabanında başarıyla güncellendi!");
                    else
                        Console.WriteLine("Şifre güncelleme işlemi başarısız oldu.");
                }
            }
        }
        public double EuroYap()
        {
            Euro = 46.57;
            while (true)
            {
                Console.WriteLine("Lütfen çevirmek istediğiniz TL miktarını giriniz:");
                string giris = Console.ReadLine();
                double miktar;

                if (double.TryParse(giris, out miktar) && miktar > 0)
                {
                    if (miktar <= bakiye)
                    {
                        double cevrilenEuro = miktar / Euro;
                        EuroBakiye += cevrilenEuro;
                        bakiye -= miktar;

                        Console.WriteLine($"{miktar} TL başarıyla {cevrilenEuro} Euro'ya çevrildi.");
                        Console.WriteLine($"Güncel TL bakiyeniz: {bakiye} - Euro bakiyeniz: {EuroBakiye}");

                        using (MySqlConnection baglanti = new MySqlConnection(strbaglanti))
                        {
                            baglanti.Open();
                            string strUpdate = "UPDATE bankauygulama.musteri SET bakiye = @bakiye, Ebakiye = @Ebakiye WHERE kartno = @kartNo";
                            using (MySqlCommand cmd2 = new MySqlCommand(strUpdate, baglanti))
                            {
                                cmd2.Parameters.AddWithValue("@bakiye", bakiye);
                                cmd2.Parameters.AddWithValue("@Ebakiye", EuroBakiye);
                                cmd2.Parameters.AddWithValue("@kartNo", kartnumara);

                                int affectedRows = cmd2.ExecuteNonQuery();
                                if (affectedRows > 0)
                                    Console.WriteLine("Veritabanı başarıyla güncellendi!");
                                else
                                    Console.WriteLine("Veritabanı güncellenirken bir hata oluştu!");
                            }
                        }

                        return EuroBakiye;
                    }
                    else
                    {
                        Console.WriteLine("Bakiyenizden fazla TL çeviremezsiniz.");
                    }
                }
                else
                {
                    Console.WriteLine("Lütfen geçerli ve pozitif bir sayı giriniz.");
                }
            }
        }

        public double DolarYap()
        {
            dolar = 39.87;
            Console.WriteLine("Lütfen çevirmek istediğiniz TL miktarını giriniz:");

            string istenenBakiye = Console.ReadLine();
            double IstenenBakiye = Convert.ToDouble(istenenBakiye);
            if (IstenenBakiye < Bakiye && IstenenBakiye > 0)
            {
                dolarBakiye += IstenenBakiye / dolar;
                bakiye -= IstenenBakiye;
                Console.WriteLine($"Güncel TL bakiyeniz: {bakiye} - Toplam Dolar bakiyeniz: {DolarBakiye}");
            }
            else
            {
                Console.WriteLine("Lütfen bakiyeniz kadar veya daha az tutarda ve 0'dan büyük bir değer giriniz.");
            }
            using (MySqlConnection baglanti = new MySqlConnection(strbaglanti))
            {
                baglanti.Open();
                string strUpdate = "UPDATE bankauygulama.musteri SET bakiye = @bakiye, Dbakiye = @dolarBakiye WHERE kartNo = @kartNo";
                using (MySqlCommand cmd2 = new MySqlCommand(strUpdate, baglanti))
                {
                    cmd2.Parameters.AddWithValue("@bakiye", bakiye);
                    cmd2.Parameters.AddWithValue("@kartNo", kartnumara);
                    cmd2.Parameters.AddWithValue("@dolarBakiye", dolarBakiye);

                    int affectedRows = cmd2.ExecuteNonQuery();
                    if (affectedRows > 0)
                        Console.WriteLine("Veritabanı başarıyla güncellendi!");
                    else
                        Console.WriteLine("Veritabanı güncellenirken bir hata oluştu!");
                }
            }
            return dolarBakiye;
        }
        public double ParaYatir()
        {
            Console.WriteLine("Lütfen yatırmak istediğiniz tutarı giriniz:");
            string giris = Console.ReadLine();
            double miktar;
            if (double.TryParse(giris, out miktar) && miktar > 0)
            {
                bakiye += miktar;
                Console.WriteLine($"{miktar} TL başarıyla yatırıldı. Güncel bakiyeniz: {bakiye}");
            }
            else
            {
                Console.WriteLine("Lütfen pozitif bir sayı giriniz.");
            }

            using (MySqlConnection baglanti = new MySqlConnection(strbaglanti))
            {
                baglanti.Open();
                string strUpdate = "UPDATE bankauygulama.musteri SET bakiye = @bakiye WHERE kartno = @kartNo";
                using (MySqlCommand cmd2 = new MySqlCommand(strUpdate, baglanti))
                {
                    cmd2.Parameters.AddWithValue("@bakiye", bakiye);
                    cmd2.Parameters.AddWithValue("@kartNo", kartnumara);

                    int affectedRows = cmd2.ExecuteNonQuery();
                    if (affectedRows > 0)
                        Console.WriteLine("Veritabanı başarıyla güncellendi!");
                    else
                        Console.WriteLine("Veritabanına ekleme başarısız oldu.");
                }
            }
            return bakiye;
        }
        public double ParaCek()
        {
            Console.WriteLine("Lütfen çekmek istediğiniz tutarı giriniz:");
            string deger = Console.ReadLine();
            double miktar;
            if (double.TryParse(deger, out miktar) && miktar > 0)
            {
                if (miktar <= bakiye)
                {
                    bakiye -= miktar;
                    Console.WriteLine($"{miktar} TL başarıyla çekildi. Güncel bakiyeniz: {bakiye}");
                }
                else
                {
                    Console.WriteLine("Bakiyenizden fazla para çekemezsiniz.");
                }
            }
            else
            {
                Console.WriteLine("Lütfen geçerli bir sayı giriniz.");
            }
            using (MySqlConnection baglanti = new MySqlConnection(strbaglanti))
            {
                baglanti.Open();
                string strUpdate = "UPDATE bankauygulama.musteri SET bakiye = @bakiye WHERE kartno = @kartNo";
                using (MySqlCommand cmd2 = new MySqlCommand(strUpdate, baglanti))
                {
                    cmd2.Parameters.AddWithValue("@bakiye", bakiye);
                    cmd2.Parameters.AddWithValue("@kartNo", kartnumara);

                    int affectedRows = cmd2.ExecuteNonQuery();
                    if (affectedRows > 0)
                        Console.WriteLine("Veritabanı başarıyla güncellendi!");
                    else
                        Console.WriteLine("Veritabanına ekleme başarısız oldu.");
                }
            }
            return bakiye;
        }
        static string LuthMetodu()
        {
            int sayi = 0;
            Random rnd = new Random();
            char[] sayilar = new char[16];
            sayilar[0] = '4';
            for (int i = 1; i < 15; i++)
            {
                sayilar[i] = (char)('0' + rnd.Next(0, 10));
            }

            for (int i = 14, j = 0; i >= 0; i--, j++)
            {
                int rakam = sayilar[i] - '0';
                if (j % 2 == 0)
                {
                    int ikikati = rakam * 2;
                    if (ikikati < 10)
                    {
                        sayi += ikikati;
                    }
                    else
                    {
                        sayi += ikikati - 9;
                    }
                }
                else
                {
                    sayi += rakam;
                }

            }

            if (sayi % 10 == 0)
            {
                sayilar[15] = '0';
            }
            else
            {
                int kontrolBasamagi = (10 - (sayi % 10)) % 10;
                sayilar[15] = (char)(kontrolBasamagi + '0');
            }
            return new string(sayilar);
        }

        internal class Program
        {
            static void Main(string[] args)
            {
                string strbaglanti = "server=127.0.0.1;port=3306;user=root;password=Yigit2009.;database=bankauygulama;";

                List<Kisi> kullanicilar = new List<Kisi>();
                using (MySqlConnection baglanti = new MySqlConnection(strbaglanti))
                {
                    baglanti.Open();
                    string sql = "SELECT ad, sifre, bakiye, Ebakiye, Dbakiye, kartno FROM bankauygulama.musteri";
                    using (MySqlCommand cmd = new MySqlCommand(sql, baglanti))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Kisi kisi = new Kisi();
                            kisi.Ad = reader["ad"].ToString();
                            kisi.Sifre = reader["sifre"].ToString();
                            kisi.Bakiye = Convert.ToDouble(reader["bakiye"]);
                            kisi.EuroBakiye = Convert.ToDouble(reader["Ebakiye"]);
                            kisi.DolarBakiye = Convert.ToDouble(reader["Dbakiye"]);
                            kisi.KartNumara = reader["kartno"].ToString();
                            kullanicilar.Add(kisi);
                        }
                    }
                }

                while (true)
                {
                    
                    Console.WriteLine("Lütfen yapmak istediğiniz işlemi seçiniz:");
                    Console.WriteLine("Var olan hesaba giriş için ==> 1");
                    Console.WriteLine("Yeni hesap açmak için ==> 2");
                    Console.WriteLine("Programdan çıkmak için ==> 3");
                    string giris = Console.ReadLine();

                    if (giris == "1")
                    {
                        Console.Clear();
                        Console.WriteLine("Giriş yapmak istediğiniz hesabın kullanıcı adı ve şifresini giriniz...");
                        Console.Write("Kullanıcı Adı: ");
                        string girilenIsim = Console.ReadLine();

                        Console.Write("Şifre: ");
                        string girilenSifre = Console.ReadLine();

                        Kisi bulunanKullanici = kullanicilar.FirstOrDefault(k => k.Ad == girilenIsim && k.Sifre == girilenSifre);

                        if (bulunanKullanici != null)
                        {
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("Hoşgeldiniz sayın {0}, mevcut bakiyeniz: {1} TL", bulunanKullanici.Ad, bulunanKullanici.Bakiye);
                                Console.WriteLine("Dolar bakiyeniz: {0}", bulunanKullanici.DolarBakiye);
                                Console.WriteLine("Euro bakiyeniz: {0}", bulunanKullanici.EuroBakiye);
                                Console.WriteLine("Para çekmek için (çek), para yatırmak için (yatır), şifre değiştirmek için (şifre) yazınız.");
                                Console.WriteLine("Bakiyenizi Euro'ya çevirmek için (euro), dolara çevirmek için (dolar) yazınız.");
                                Console.WriteLine("Para transferi yapmak için (transfer) yazınız.");
                                Console.WriteLine("Çıkmak için (çıkış) yazınız.");
                                string cevap = Console.ReadLine().ToLower();

                                if (cevap == "yatır" || cevap == "yatir")
                                    bulunanKullanici.ParaYatir();
                                else if (cevap == "çek" || cevap == "cek")
                                    bulunanKullanici.ParaCek();
                                else if (cevap == "şifre" || cevap == "sifre")
                                    bulunanKullanici.SifreDegistir();
                                else if (cevap == "euro")
                                    bulunanKullanici.EuroYap();
                                else if (cevap == "dolar")
                                    bulunanKullanici.DolarYap();
                                else if (cevap == "transfer")
                                {
                                    Console.WriteLine("Para yatırmak istediğiniz hesabın kart numarasını giriniz:");
                                    string Hedefkartnumarasi = Console.ReadLine();

                                    using (MySqlConnection baglanti1 = new MySqlConnection(strbaglanti))
                                    {
                                        baglanti1.Open();
                                        string strSelect = "SELECT COUNT(*) FROM musteri WHERE kartno  = @kartno";
                                        using (MySqlCommand cmd = new MySqlCommand(strSelect, baglanti1))
                                        {
                                            cmd.Parameters.AddWithValue("@kartno", Hedefkartnumarasi);
                                            long kayitSayisi = (long)cmd.ExecuteScalar();

                                            if (kayitSayisi > 0)
                                            {
                                                Console.WriteLine("yatirmak istediğiniz para türünü giriniz");

                                                string cev = Console.ReadLine().ToLower();

                                                if(cev == "tl")
                                                {
                                                    Console.WriteLine("yatırmak istediğiniz ücreti giriniz");
                                                    double hedefbakiye = 0;
                                                    while (true)
                                                    {
                                                        Console.WriteLine("Yatırmak istediğiniz ücreti giriniz:");
                                                        string giri = Console.ReadLine();

                                                        try
                                                        {
                                                            hedefbakiye = Convert.ToDouble(giri);

                                                            if (hedefbakiye > 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Lütfen 0'dan büyük bir değer giriniz.");
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            Console.WriteLine("Lütfen geçerli bir sayı giriniz!");
                                                        }
                                                    }
                                                    if (hedefbakiye >0)
                                                    {
                                                       if(hedefbakiye <= bulunanKullanici.bakiye)
                                                        {
                                                            bulunanKullanici.bakiye -= hedefbakiye;
                                                            string updateHedef = "UPDATE musteri SET bakiye = bakiye + @hedefbakiye WHERE kartno = @kartno";
                                                            using (MySqlCommand updateCmd = new MySqlCommand(updateHedef, baglanti1))
                                                            {
                                                                updateCmd.Parameters.AddWithValue("@hedefbakiye", hedefbakiye);
                                                                updateCmd.Parameters.AddWithValue("@kartno", Hedefkartnumarasi);
                                                                updateCmd.ExecuteNonQuery();

                                                            }
                                                            string updateGonderen = "UPDATE musteri SET bakiye = @bakiye WHERE kartno = @kartno";
                                                            using (MySqlCommand updateCmd2 = new MySqlCommand(updateGonderen, baglanti1))
                                                            {
                                                                updateCmd2.Parameters.AddWithValue("@bakiye", bulunanKullanici.Bakiye);
                                                                updateCmd2.Parameters.AddWithValue("@kartno", bulunanKullanici.KartNumara);
                                                                updateCmd2.ExecuteNonQuery();
                                                                Console.WriteLine("bakiye transferi başarılı");
                                                                Console.WriteLine("devam etmek için entera basınız");
                                                                Console.ReadLine();
                                                                Console.Clear();
                                                            }
                                                        }
                                                       else
                                                       {
                                                            Console.WriteLine("lütfen geçerli bir değer girin");
                                                       }

                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Lütfen geçerli bir değer girniz");
                                                    }
                                                }
                                                else if (cev == "euro")
                                                {
                                                    Console.WriteLine("yatırmak istediğiniz ücreti giriniz");
                                                    double hedefbakiye = 0;
                                                    while (true)
                                                    {
                                                        Console.WriteLine("Yatırmak istediğiniz ücreti giriniz:");
                                                        string giri = Console.ReadLine();

                                                        try
                                                        {
                                                            hedefbakiye = Convert.ToDouble(giri);

                                                            if (hedefbakiye > 0)
                                                            {
                                                                break; 
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Lütfen 0'dan büyük bir değer giriniz.");
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            Console.WriteLine("Lütfen geçerli bir sayı giriniz!");
                                                        }
                                                    }


                                                    if (hedefbakiye <= bulunanKullanici.EuroBakiye)
                                                    {
                                                        bulunanKullanici.EuroBakiye -= hedefbakiye;

                                                        string updateHedef = "UPDATE musteri SET Ebakiye = Ebakiye + @miktar WHERE kartno = @kartno";
                                                        using (MySqlCommand updateCmd = new MySqlCommand(updateHedef, baglanti1))
                                                        {
                                                            updateCmd.Parameters.AddWithValue("@miktar", hedefbakiye);
                                                            updateCmd.Parameters.AddWithValue("@kartno", Hedefkartnumarasi);
                                                            updateCmd.ExecuteNonQuery();
                                                        }

                                                        string updateGonderen = "UPDATE musteri SET Ebakiye = @euroBakiye WHERE kartno = @kartno";
                                                        using (MySqlCommand updateCmd2 = new MySqlCommand(updateGonderen, baglanti1))
                                                        {
                                                            updateCmd2.Parameters.AddWithValue("@euroBakiye", bulunanKullanici.EuroBakiye);
                                                            updateCmd2.Parameters.AddWithValue("@kartno", bulunanKullanici.KartNumara);
                                                            updateCmd2.ExecuteNonQuery();
                                                            Console.WriteLine("bakiye transferi başarılı");
                                                            Console.WriteLine("devam etmek için entera basınız");
                                                            Console.ReadLine();
                                                            Console.Clear();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Bakiyeniz yeterli değil.");
                                                    }

                                                }
                                                else if (cev == "dolar")
                                                {
                                                    Console.WriteLine("yatırmak istediğiniz ücreti giriniz");
                                                    double hedefbakiye = 0;
                                                    while (true)
                                                    {
                                                        Console.WriteLine("Yatırmak istediğiniz ücreti giriniz:");
                                                        string giri = Console.ReadLine();

                                                        try
                                                        {
                                                            hedefbakiye = Convert.ToDouble(giri);

                                                            if (hedefbakiye > 0)
                                                            {
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Lütfen 0'dan büyük bir değer giriniz.");
                                                            }
                                                        }
                                                        catch
                                                        {
                                                            Console.WriteLine("Lütfen geçerli bir sayı giriniz!");
                                                        }
                                                    }
                                                    if (hedefbakiye <= bulunanKullanici.DolarBakiye)
                                                    {
                                                        bulunanKullanici.DolarBakiye -= hedefbakiye;

                                                        string updateHedef = "UPDATE musteri SET Dbakiye = Dbakiye + @miktar WHERE kartno = @kartno";
                                                        using (MySqlCommand updateCmd = new MySqlCommand(updateHedef, baglanti1))
                                                        {
                                                            updateCmd.Parameters.AddWithValue("@miktar", hedefbakiye);
                                                            updateCmd.Parameters.AddWithValue("@kartno", Hedefkartnumarasi);
                                                            updateCmd.ExecuteNonQuery();
                                                        }

                                                        string updateGonderen = "UPDATE musteri SET Dbakiye = @DolarBakiye WHERE kartno = @kartno";
                                                        using (MySqlCommand updateCmd2 = new MySqlCommand(updateGonderen, baglanti1))
                                                        {
                                                            updateCmd2.Parameters.AddWithValue("@DolarBakiye", bulunanKullanici.DolarBakiye);
                                                            updateCmd2.Parameters.AddWithValue("@kartno", bulunanKullanici.KartNumara);
                                                            updateCmd2.ExecuteNonQuery();
                                                            Console.WriteLine("bakiye transferi başarılı");
                                                            Console.WriteLine("devam etmek için entera basınız");
                                                            Console.ReadLine();
                                                            Console.Clear();

                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Bakiyeniz yeterli değil.");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("lütfen desteklenen (tl, euro veya dolar) para birminden birini seçiniz");
                                                }
                                                    
                                            }
                                            else
                                            {
                                                Console.WriteLine("Girilen kart numarası bulunamadı.");
                                            }
                                        }
                                    }
                                
                                }
                                else if (cevap == "çıkış" || cevap == "cikis")
                                {
                                    Console.WriteLine("Hesaptan çıkış yapılıyor...");
                                    break;
                                }
                                else
                                {
                                  Console.WriteLine("Geçersiz komut, lütfen tekrar deneyiniz.");
                                  Console.WriteLine("Devam etmek için bir tuşa basınız...");
                                  Console.ReadKey();
                                }
                                    
                            }
                        }
                        else
                        {
                            Console.WriteLine("Kullanıcı adı veya şifre hatalı.");
                        }
                    }
                    else if (giris == "2")
                    {
                        Kisi yeniKisi = new Kisi();
                        Console.Write("Yeni kullanıcının adını giriniz: ");
                        yeniKisi.Ad = Console.ReadLine();
                        using (MySqlConnection baglanti1 = new MySqlConnection(strbaglanti))
                        {
                            baglanti1.Open();
                            string strSelect = "SELECT COUNT(*) FROM musteri WHERE ad = @ad";
                            using (MySqlCommand cmd = new MySqlCommand(strSelect, baglanti1))
                            {
                                cmd.Parameters.AddWithValue("@ad", yeniKisi.ad);
                                long kayitSayisi = (long)cmd.ExecuteScalar();

                                if (kayitSayisi > 0)
                                {
                                    Console.WriteLine("Bu isim zaten alınmış, lütfen başka bir isim giriniz.");
                                    Console.WriteLine("devam etmek için hrhangi bir tuşa basın");
                                    Console.ReadLine();
                                    Console.Clear();
                                }
                                else
                                {

                                    Console.WriteLine("Şifre oluşturunuz (en az 8 karakter, içinde rakam ve harf olmalı):");
                                    yeniKisi.Sifre = yeniKisi.SifreGir();
                                    yeniKisi.Bakiye = 0;
                                    yeniKisi.DolarBakiye = 0;
                                    yeniKisi.EuroBakiye = 0;

                                    yeniKisi.KartNumara = Kisi.LuthMetodu();


                                    using (MySqlConnection baglanti2 = new MySqlConnection(strbaglanti))
                                    {
                                        baglanti2.Open();
                                        string strSelect1 = "SELECT COUNT(*) FROM musteri WHERE kartno = @KartNumara";
                                        using (MySqlCommand cmd1 = new MySqlCommand(strSelect1, baglanti2))
                                        {
                                            cmd1.Parameters.AddWithValue("@KartNumara", yeniKisi.KartNumara);
                                            long kayitSayisi1 = (long)cmd1.ExecuteScalar();

                                            while (kayitSayisi > 0)
                                            {
                                                yeniKisi.KartNumara = Kisi.LuthMetodu();
                                                cmd1.Parameters["@KartNumara"].Value = yeniKisi.KartNumara;
                                                kayitSayisi = (long)cmd1.ExecuteScalar();
                                            }
                                        }
                                    }

                                    kullanicilar.Add(yeniKisi);
                                    Console.Clear();

                                    // Veritabanına ekleme
                                    using (MySqlConnection baglanti = new MySqlConnection(strbaglanti))
                                    {
                                        baglanti.Open();
                                        string strInsert = "INSERT INTO bankauygulama.musteri(ad, sifre, bakiye, Ebakiye, Dbakiye, kartno) VALUES(@ad, @sifre, @bakiye, @Ebakiye, @Dbakiye, @kartno)";
                                        using (MySqlCommand cmd1 = new MySqlCommand(strInsert, baglanti))
                                        {
                                            cmd1.Parameters.AddWithValue("@ad", yeniKisi.Ad);
                                            cmd1.Parameters.AddWithValue("@sifre", yeniKisi.Sifre);
                                            cmd1.Parameters.AddWithValue("@bakiye", yeniKisi.Bakiye);
                                            cmd1.Parameters.AddWithValue("@Dbakiye", yeniKisi.DolarBakiye);
                                            cmd1.Parameters.AddWithValue("@Ebakiye", yeniKisi.EuroBakiye);
                                            cmd1.Parameters.AddWithValue("@kartno", yeniKisi.KartNumara);

                                            int affectedRows = cmd1.ExecuteNonQuery();
                                            if (affectedRows > 0)
                                                Console.WriteLine("Kullanıcı veritabanına başarıyla eklendi!");
                                            else
                                                Console.WriteLine("Veritabanına ekleme başarısız oldu.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (giris == "3")
                    {
                        Console.WriteLine("Programdan çıkılıyor...");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz giriş, lütfen tekrar deneyiniz.");
                    }
                    }
                }
            }
        }
    }

