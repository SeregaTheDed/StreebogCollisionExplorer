using StreebogCollisionExplorer.ExploreCollision;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace StreebogCollisionExplorer
{
    internal class ConsoleApplication
    {
        private const string SkipLine = "-----------------------------------------------------------------------------------------------------------";

        public void printChartTESTTTTT()
        {
            //populate dataset with some demo data..
            DataSet dataSet = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Counter", typeof(int));
            DataRow r1 = dt.NewRow();
            r1[0] = "Demo";
            r1[1] = 8;
            dt.Rows.Add(r1);
            DataRow r2 = dt.NewRow();
            r2[0] = "Second";
            r2[1] = 15;
            dt.Rows.Add(r2);
            dataSet.Tables.Add(dt);


            //prepare chart control...
            Chart chart = new Chart();
            chart.DataSource = dataSet.Tables[0];
            chart.Width = 600;
            chart.Height = 350;
            //create serie...
            Series serie1 = new Series();
            serie1.Name = "Serie1";
            serie1.Color = Color.FromArgb(112, 255, 200);
            serie1.BorderColor = Color.FromArgb(164, 164, 164);
            serie1.ChartType = SeriesChartType.Column;
            serie1.BorderDashStyle = ChartDashStyle.Solid;
            serie1.BorderWidth = 1;
            serie1.ShadowColor = Color.FromArgb(128, 128, 128);
            serie1.ShadowOffset = 1;
            serie1.IsValueShownAsLabel = true;
            serie1.XValueMember = "Name";
            serie1.YValueMembers = "Counter";
            serie1.Font = new Font("Tahoma", 8.0f);
            serie1.BackSecondaryColor = Color.FromArgb(0, 102, 153);
            serie1.LabelForeColor = Color.FromArgb(100, 100, 100);
            chart.Series.Add(serie1);
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
            chart.SaveImage(@".\myChart.png", ChartImageFormat.Png);
        }
        public void Start()
        {
            //printChartTESTTTTT();
            while (true)
            {
                Console.WriteLine("Выберите пункт меню:");
                Console.WriteLine("1. Построить коллизию используя базовый алгоритм");
                Console.WriteLine("2. Построить коллизию используя итеративный алгоритм");
                Console.WriteLine("3. Выход");

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
            Console.Write("Введите размер хэша: ");
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
