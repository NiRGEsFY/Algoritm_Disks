namespace Algoritm_Disks
{
    public class Disk
    {
        public int Lenght { get; set; }
        public int Radius { get; set; }
        public Disk(int lenght, int radius)
        {
            Lenght = lenght;
            Radius = radius;
        }
        public static double FindMinimumBoxLength(List<Disk> disks)
        {
            disks = disks.OrderBy(x => x.Radius).ToList();
            double totalLength = 0;

            foreach (var disk in disks)
            {
                totalLength += 2 * disk.Radius;
            }

            return totalLength;
        }

    }
    public class Program
    {
        
        static void Main(string[] args)
        {
            List<Disk> disks = new List<Disk>();
            Random rand = new Random();
            int lenght = 5;
            int countDisk = rand.Next(0, 100);
            for (int i = 0; i < countDisk;i++)
            {
                disks.Add(new Disk(lenght,rand.Next(0,100)));
            }
            int iteration = 1;
            Console.WriteLine("Список всех дисков: ");
            foreach (var disk in disks)
            {
                Console.WriteLine($"Диск #{iteration++}: Радиус:{disk.Radius}");
            }
            Console.WriteLine($"Минимальная длинна: {Disk.FindMinimumBoxLength(disks)}");
            disks = disks.OrderBy(x => x.Radius).ToList();
            iteration = 1;
            Console.WriteLine("Расположение дисков:");
            foreach (var disk in disks)
            {
                Console.WriteLine($"Диск #{iteration++}: Радиус:{disk.Radius}");
            }
        }
    }
}
