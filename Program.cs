namespace FestivalKewirausahaan
{
    public class Stand
    {
        protected string _namaStand;
        protected double _hargaSewaPerHari;
        protected bool _isAvailable;

        public Stand(string namaStand, double hargaSewaPerHari)
        {
            NamaStand = namaStand;
            HargaSewaPerHari = hargaSewaPerHari;
            _isAvailable = true;
        }

        public string NamaStand
        {
            get { return _namaStand; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nama stand tidak boleh kosong.");
                _namaStand = value;
            }
        }

        public double HargaSewaPerHari
        {
            get { return _hargaSewaPerHari; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Harga sewa harus lebih besar dari 0.");
                _hargaSewaPerHari = value;
            }
        }

        public bool IsAvailable => _isAvailable;

        public void UbahStatus()
        {
            _isAvailable = !_isAvailable;
        }

        public virtual double HitungTotal(int jumlahHari)
        {
            return _hargaSewaPerHari * jumlahHari;
        }

        public void DisplayInfo()
        {
            string status = _isAvailable ? "Tersedia" : "Disewa";
            Console.WriteLine($"{_namaStand,-15} | Rp {_hargaSewaPerHari,-15} / hari | {status}");
        }
    }
}

namespace FestivalKewirausahaan
{
    public class StandOutdoor : Stand
    {
        protected double _biayaTenda = 75000;

        public StandOutdoor(string namaStand, double hargaSewaPerHari)
            : base(namaStand, hargaSewaPerHari) { }

        public double BiayaTenda => _biayaTenda;

        public override double HitungTotal(int jumlahHari)
        {
            return (_hargaSewaPerHari * jumlahHari) + (_biayaTenda * jumlahHari);
        }
    }
}

namespace FestivalKewirausahaan
{
    public class StandIndoor : Stand
    {
        protected double _biayaListrik = 100000;

        public StandIndoor(string namaStand, double hargaSewaPerHari)
            : base(namaStand, hargaSewaPerHari) { }

        public double BiayaListrik => _biayaListrik;

        public override double HitungTotal(int jumlahHari)
        {
            return (_hargaSewaPerHari * jumlahHari) + (_biayaListrik * jumlahHari);
        }
    }
}

namespace FestivalKewirausahaan
{
    public class StandPremium : Stand
    {
        protected double _biayaKeamanan = 300000;

        public StandPremium(string namaStand, double hargaSewaPerHari)
            : base(namaStand, hargaSewaPerHari) { }

        public double BiayaKeamanan => _biayaKeamanan;
        public override double HitungTotal(int jumlahHari)
        {
            return (_hargaSewaPerHari * jumlahHari) + _biayaKeamanan;
        }
    }
}

namespace FestivalKewirausahaan
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Stand> daftarStand = new List<Stand>()
            {
                new StandOutdoor("Outdoor-1", 450000),
                new StandOutdoor("Outdoor-2", 500000),
                new StandIndoor("Indoor-1", 600000),
                new StandIndoor("Indoor-2", 700000),
                new StandPremium("Premium-1", 1800000),
                new StandPremium("Premium-2", 2000000)
            };

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== Moklet Expo Management Center ===");
                Console.WriteLine("Daftar Stand Tersedia\n");

                foreach (var stand in daftarStand)
                {
                    stand.DisplayInfo();
                }

                Console.WriteLine("\n1. Sewa Stand");
                Console.WriteLine("2. Akhiri Sewa Stand");
                Console.WriteLine("3. Keluar");
                Console.Write("\nPilih Menu: ");

                string pilihan = Console.ReadLine();
                Console.WriteLine();

                switch (pilihan)
                {
                    case "1":
                        ProsesSewa(daftarStand);
                        break;
                    case "2":
                        ProsesAkhiriSewa(daftarStand);
                        break;
                    case "3":
                        Console.WriteLine("Terima kasih...");
                        running = false;
                        PressEnterToContinue();
                        break;
                    default:
                        Console.WriteLine("Pilihan menu tidak valid.");
                        PressEnterToContinue();
                        break;
                }
            }
        }

        static void ProsesSewa(List<Stand> daftarStand)
        {
            Console.Write("Masukkan nama stand yang ingin disewa: ");
            string namaInput = Console.ReadLine();

            Stand standDitemukan = daftarStand.FirstOrDefault(s => s.NamaStand.Equals(namaInput, StringComparison.OrdinalIgnoreCase));

            if (standDitemukan == null)
            {
                Console.WriteLine("Stand tidak ditemukan.");
            }
            else if (!standDitemukan.IsAvailable)
            {
                Console.WriteLine("Stand sedang tidak tersedia.");
            }
            else
            {
                Console.WriteLine($"Stand ditemukan: {standDitemukan.NamaStand} | Rp {standDitemukan.HargaSewaPerHari} / hari");
                Console.Write("\nMasukkan jumlah hari: ");

                if (int.TryParse(Console.ReadLine(), out int hari) && hari > 0)
                {
                    double totalBiaya = standDitemukan.HitungTotal(hari);

                    Console.WriteLine($"\nTotal Biaya: Rp {totalBiaya}");
                    standDitemukan.UbahStatus();
                    Console.WriteLine($"\nStand {standDitemukan.NamaStand} berhasil disewakan selama {hari} hari");
                }
                else
                {
                    Console.WriteLine("Jumlah hari tidak valid.");
                }
            }
            PressEnterToContinue();
        }

        static void ProsesAkhiriSewa(List<Stand> daftarStand)
        {
            Console.Write("Masukkan nama stand yang ingin diselesaikan sewanya: ");
            string namaInput = Console.ReadLine();

            Stand standDitemukan = daftarStand.FirstOrDefault(s => s.NamaStand.Equals(namaInput, StringComparison.OrdinalIgnoreCase));

            if (standDitemukan == null)
            {
                Console.WriteLine("Stand tidak ditemukan.");
            }
            else if (standDitemukan.IsAvailable)
            {
                Console.WriteLine("Stand memang belum disewa atau sudah tersedia.");
            }
            else
            {
                standDitemukan.UbahStatus();
                Console.WriteLine($"\nStand {standDitemukan.NamaStand} sekarang kembali Tersedia.");
            }
            PressEnterToContinue();
        }

        static void PressEnterToContinue()
        {
            Console.WriteLine("\nTekan ENTER...");
            Console.ReadLine();
        }
    }
}
