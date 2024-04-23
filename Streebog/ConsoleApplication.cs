using StreebogCollisionExplorer.ExploreCollision;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace StreebogCollisionExplorer
{
    internal class ConsoleApplication
    {
        private const string SkipLine = "-----------------------------------------------------------------------------------------------------------";

        public void printChart()
        {
            Console.WriteLine("Вычисление может занять долгое время! До 10 минут!");
            Console.WriteLine("Создается график...");

            //prepare chart control...
            Chart chart = new Chart();
            chart.Width = 1920;
            chart.Height = 1080;
            //create serie...
            Console.WriteLine("Вычисляем для стандартного точки [1, 4]");
            int iters = 3;
            Series serie1 = new Series();
            for (int i = 1; i < 5; i++)
            {
                long totalTime = 0;
                for (int j = 0; j < iters; j++)
                {
                    CollisionFinderResult collisionFinderResult = new StandartCollisionFinder().FindCollisions(i);
                    totalTime += collisionFinderResult.MillisecondsTotal;
                    Console.WriteLine($"p{i}.y = {collisionFinderResult.MillisecondsTotal} (попытка {j + 1})");
                }
                Console.WriteLine($"p{i}.y = {totalTime / iters} (сред)");
                serie1.Points.AddXY(i, totalTime / iters);
            }
            chart.Series.Add(serie1);


            Console.WriteLine("Вычисляем для итеративного точки [1, 2]");
            Series serie2 = new Series();
            for (int i = 1; i < 3; i++)
            {
                long totalTime = 0;
                for (int j = 0; j < iters; j++)
                {
                    CollisionFinderResult collisionFinderResult = new IterateCollisionFinder().FindCollisions(i);
                    totalTime += collisionFinderResult.MillisecondsTotal;
                    Console.WriteLine($"p{i}.y = {collisionFinderResult.MillisecondsTotal} (попытка {j + 1})");
                }
                Console.WriteLine($"p{i}.y = {totalTime / iters} (сред)");
                serie2.Points.AddXY(i, totalTime / iters);
            }
            chart.Series.Add(serie2);



            Console.WriteLine("Рисуем, биндим...");

            //create chartareas...
            ChartArea ca = new ChartArea();
            ca.Name = "ChartArea1";
            ca.BackColor = Color.White;
            ca.BorderColor = Color.FromArgb(26, 59, 105);
            ca.BorderWidth = 0;
            ca.BorderDashStyle = ChartDashStyle.Solid;
            ca.AxisX = new Axis();
            ca.AxisY = new Axis();
            chart.ChartAreas.Add(ca);

            //databind...
            chart.DataBind();

            //save result...
            string path = @".\myChart.png";
            chart.SaveImage(@".\myChart.png", ChartImageFormat.Png);
            Console.WriteLine("График сохранен по пути: " + path);
        }
        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Выберите пункт меню:");
                Console.WriteLine("1. Построить коллизию используя базовый алгоритм");
                Console.WriteLine("2. Построить коллизию используя итеративный алгоритм");
                Console.WriteLine("3. Построить график");
                Console.WriteLine("4. Выход");

                Console.Write("Значение: ");
                switch (Console.ReadLine().Trim())
                {
                    case "1":
                        FindCollisions(new StandartCollisionFinder());
                        break;
                    case "2":
                        FindCollisions(new IterateCollisionFinder());
                        break;
                    case "3":
                        printChart();
                        break;
                    case "4":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Выберите пункт меню!");
                        break;

                }
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine(SkipLine);
                }
            }
        }



        private static void FindCollisions(BaseCollisionFinder collisionFinder)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ограничение на ввод не стоит, но если вводить большие значения: \n" +
                ">4 для стандратного и >2 для итеративного, то будет работать ОЧЕНЬ долго");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Введите размер хэша(в байтах): ");
            int hashSize = int.Parse(Console.ReadLine());

            CollisionFinderResult collisionFinderResult = collisionFinder.FindCollisions(hashSize);

            Console.WriteLine("Время: " + collisionFinderResult.MillisecondsTotal + "ms");
            Console.WriteLine("Попыток: " + collisionFinderResult.AttemptsCount);
            foreach (var item in collisionFinderResult.MessagesStrings)
            {
                Console.WriteLine(SkipLine);
                Console.WriteLine("Сообщение i: " + item);
                Console.WriteLine("Хеш сообщения i: " + collisionFinderResult.HashString);
            }

        }
    }
}
