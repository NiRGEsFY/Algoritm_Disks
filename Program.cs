using System.Xml.Linq;

namespace Algoritm_Disks
{
    public interface Item 
    {
        int Height();
        int Width();
        int Lenght();
    }

    public class Disk : Item
    {
        public int _Lenght { get; set; }
        public int Radius { get; set; }
        public Disk(int lenght, int radius)
        {
            _Lenght = lenght;
            Radius = radius;
        }

        public int Height()
        {
            return Radius * 2;
        }
        public int Width()
        {
            return Radius * 2;
        }
        public int Lenght()
        {
            return _Lenght;
        }
    }
    public class Box<T1>
        where T1 : Item
    {
        public int Width { get; set; }
        public int Lenght { get; set; }
        public List<T1> ItemIn { get; set; }
        public int[,]? BoxField { get; set; }
        public Box(int width = 1, int lenght = 1)
        {
            Width = width;
            Lenght = lenght;
        }
        public Box(int width, int lenght, List<T1> itemIn) 
            : this(width, lenght)
        {
            Lenght = itemIn.Sum(x => x.Lenght());
            ItemIn = itemIn.ToList();
        }

        public void PackedItemIn()
        {
            ItemIn = ItemIn.OrderByDescending(x => x.Width()).ToList();
            Width = ItemIn.Max(x => x.Width());
            Lenght = ItemIn.Sum(x => x.Lenght());
            BoxField = new int[Width, Lenght];
            int packedLenght = 0;
            var endingItemIn = ItemIn.ToList();
            int Row = 0;
            foreach(var item in ItemIn)
            {
                if(item.Width() == Width)
                {
                    endingItemIn.Remove(item);
                    var indexElement = ItemIn.IndexOf(item);
                    if (indexElement == 0)
                        indexElement--;
                    for (int i = 0; i < Width; i++)
                    {
                        BoxField[i, Row] = indexElement;
                    }
                    Row++;
                    continue;
                }
                else
                {
                    break;
                }
            }
            endingItemIn = endingItemIn.OrderByDescending(x => x.Width()).ToList();

            bool PackedOneLine(List<T1> elementsInLine, List<T1> freeElements)
            {
                int freeSpace = Width - elementsInLine.Sum(x => x.Width());
                if (freeSpace == 0)
                {
                    int lineWidth = 0;
                    foreach (var element in elementsInLine)
                    {
                        int indexElement = ItemIn.IndexOf(element);
                        endingItemIn.Remove(element);
                        for (int i = lineWidth; i < element.Width() + lineWidth;i++)
                        {
                            BoxField[i, Row] = indexElement;
                        }
                        lineWidth += element.Width();    
                    }
                    Row++;
                    return true;
                }    
                var elementFillingSpace = freeElements.FirstOrDefault(x => x.Width() == freeSpace);
                if (elementFillingSpace is not null)
                {
                    elementsInLine.Add(elementFillingSpace);
                    freeElements.Remove(elementFillingSpace);
                    return PackedOneLine(elementsInLine.ToList(), freeElements.ToList());
                }

                var canAddElements = freeElements.Where(x => x.Width() <= freeSpace).ToList();
                if (canAddElements is null || canAddElements.Count() == 0)
                    return false;
                foreach(var element in canAddElements)
                {
                    var nextElementInLine = elementsInLine.ToList();
                    nextElementInLine.Add(element);
                    var nextFreeElements = freeElements.ToList();
                    nextFreeElements.Remove(element);
                    var result = PackedOneLine(nextElementInLine, nextFreeElements);
                    if (result)
                        return true;
                }
                return false;
            }
            bool MaximasedPackedInOneLine(List<T1> elementsInLine, List<T1> freeElements)
            {
                int freeSpace = Width - elementsInLine.Sum(x => x.Width());
                var canAddElements = freeElements.Where(x => x.Width() <= freeSpace).ToList();
                if (canAddElements is null || canAddElements.Count() == 0)
                {
                    int lineWidth = 0;
                    foreach (var element in elementsInLine)
                    {
                        int indexElement = ItemIn.IndexOf(element);
                        endingItemIn.Remove(element);
                        for (int i = lineWidth; i < element.Width() + lineWidth; i++)
                        {
                            BoxField[i, Row] = indexElement;
                        }
                        lineWidth += element.Width();
                    }
                    Row++;
                    return true;
                }
                else
                {
                    foreach (var element in canAddElements)
                    {
                        var nextElementInLine = elementsInLine.ToList();
                        nextElementInLine.Add(element);
                        var nextFreeElements = freeElements.ToList();
                        nextFreeElements.Remove(element);
                        var result = MaximasedPackedInOneLine(nextElementInLine, nextFreeElements);
                        if (result)
                            return true;
                    }
                }
                return true;
            }
            for (int i = 0; i < endingItemIn.Count() -1;i++)
            {
                var item = endingItemIn[i];
                var freeElements = endingItemIn.ToList();
                freeElements.Remove(item);
                var elementInLine = new List<T1> { item };
                if (PackedOneLine(elementInLine, freeElements))
                {
                    i--;
                };
            }

            for (int i = 0; i < endingItemIn.Count() - 1; i++)
            {
                var item = endingItemIn[i];
                var freeElements = endingItemIn.ToList();
                freeElements.Remove(item);
                var elementInLine = new List<T1> { item };
                if (MaximasedPackedInOneLine(elementInLine, freeElements))
                {
                    i--;
                };
            }

            int[,] minimalBoxField = new int[Width, Row];
            Lenght = Row;
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Lenght; j++)
                {
                    minimalBoxField[i, j] = BoxField[i, j];
                }
            }
            BoxField = minimalBoxField;

        }
        public void ToConsole()
        {
            List<ConsoleColor> colorisedList = new List<ConsoleColor>() 
            {
                ConsoleColor.DarkCyan,
                ConsoleColor.White, 
                ConsoleColor.Blue,
                ConsoleColor.Green,
                ConsoleColor.Yellow,
                ConsoleColor.Red,
                ConsoleColor.Gray,
                ConsoleColor.Cyan,
                ConsoleColor.Magenta,
                ConsoleColor.DarkBlue,
                ConsoleColor.DarkRed,
            };

            int colorIndex = 1;
            int diskValue = BoxField[0, 0];
            int oldColor = BoxField[0, 0] % 10 + 1;
            for (int i = 0; i < Lenght; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (BoxField[j, i] == 0)
                    {
                        Console.ForegroundColor = colorisedList[0];
                        Console.Write('o');
                        continue;
                    }

                    if (BoxField[j,i] != diskValue)
                    {
                        
                        diskValue = BoxField[j, i];
                        colorIndex = BoxField[j, i] % 10 +1;
                        if (oldColor == colorIndex)
                            colorIndex++;
                        oldColor = colorIndex;
                    }
                    Console.ForegroundColor = colorisedList[colorIndex%10+1];
                    Console.Write('*');
                }
                Console.WriteLine();
            }

        }
    }
    public class Program
    {
        
        static void Main(string[] args)
        {
            List<Disk> disks = new List<Disk>();
            Random rand = new Random();
            int lenght = 1;
            int countDisk = rand.Next(500, 1000);
            for (int i = 0; i < countDisk;i++)
            {
                disks.Add(new Disk(lenght,rand.Next(1,30)));
            }
            Box<Disk> box = new Box<Disk>(1,1,disks);
            Console.WriteLine($"Размер коробки до сортировки: {box.Lenght}");
            Console.WriteLine($"Размещение дисков в коробке:");
            box.PackedItemIn();
            box.ToConsole();
            Console.WriteLine($"Размер коробки после сортировки: {box.Lenght}");
        }
    }
}
