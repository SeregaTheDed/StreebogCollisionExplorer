using StreebogCollisionExplorer.ExploreCollision;
using StreebogCollisionExplorer.Streebog;
using System.Collections;
using System.Drawing;
using System.Security.Policy;

namespace StreebogCollisionExplorer
{
    internal static class ImageMeaningfulCollisionsExplorer
    {
        private static Bitmap WriteMessageToBitmap(Bitmap bitmap, byte[] msg)
        {
            Bitmap result = (Bitmap) bitmap.Clone();

            for (int i = 0; i < result.Height; i++)
            {
                for (int j = 0; j < result.Width; j++)
                {
                    int resultIndex = bitmap.Width * i + j + 1;
                    byte pixelRedByte = msg[resultIndex];
                    Color pixel = result.GetPixel(j, i);
                    result.SetPixel(j, i, Color.FromArgb(pixelRedByte, pixel.G, pixel.B));
                }
            }
            return result;
        }
        public static byte[] GetMessageByBitmap(Bitmap bitmap, byte type)
        {
            byte[] result = new byte[bitmap.Width * bitmap.Height + 1];
            result[0] = type;
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    int resultIndex = bitmap.Width * i + j + 1;
                    byte pixelRedByte = bitmap.GetPixel(j, i).R;
                    result[resultIndex] = pixelRedByte;
                }
            }
            return result;
        }

        public static void Explore(string imagePath1, string imagePath2)
        {
            Bitmap bitmap1 = new Bitmap(Image.FromFile(imagePath1));
            Bitmap bitmap2 = new Bitmap(Image.FromFile(imagePath2));

            int randomMessageSize = bitmap1.Width * bitmap1.Height + 1;
            CollisionFinderResult collisionFinderResult =
                new StandartCollisionFinder().FindCollisions(1, randomMessageSize);

            byte type1 = collisionFinderResult.Messages.First().First();
            byte type2 = collisionFinderResult.Messages.Last().First();
            Console.WriteLine("Тип 1 = " + type1);
            Console.WriteLine("Тип 2 = " + type2);



            byte[] msg1 = GetMessageByBitmap(bitmap1, type1);
            byte[] msg2 = GetMessageByBitmap(bitmap2, type2);
            if (msg1.Length != msg2.Length)
            {
                throw new ArgumentException("Размеры картинок не совпадают");
            }

            //Вывод очень большой! решил не принтовать
            //Console.WriteLine($"Сообщение картинки по пути {imagePath1} = {Convert.ToHexString(msg1)}");
            //Console.WriteLine($"Сообщение картинки по пути {imagePath2} = {Convert.ToHexString(msg2)}");
            Console.WriteLine($"Хеш сообщения картинки по пути {imagePath1} = " +
                $"{Convert.ToHexString(new StreebogAlgorithm().GetHash(msg1, 1))}" +
                $"");
            Console.WriteLine($"Хеш сообщения картинки по пути {imagePath2} = " +
                $"{Convert.ToHexString(new StreebogAlgorithm().GetHash(msg2, 1))}" +
                $"");

            


            //Записываем сообщения в картинки и сохраняем
            string collizedPrefix = "collized_";
            Bitmap bitmap1_tosave = WriteMessageToBitmap(bitmap1, collisionFinderResult.Messages.First());
            bitmap1_tosave.Save(collizedPrefix + "image1.png");
            Bitmap bitmap2_tosave = WriteMessageToBitmap(bitmap2, collisionFinderResult.Messages.Last());
            bitmap2_tosave.Save(collizedPrefix + "image2.png");
            Console.WriteLine("Хеш найденной коллизии: " + collisionFinderResult.HashString);

            //Загружаем картинки, считаем в них хеши
            string image1_collized_path = collizedPrefix + "image1.png";
            string image2_collized_path = collizedPrefix + "image2.png";
            Bitmap bitmap1_collized = new Bitmap(Image.FromFile(image1_collized_path));
            Bitmap bitmap2_collized = new Bitmap(Image.FromFile(image2_collized_path));

            byte[] msg1_collized = GetMessageByBitmap(bitmap1_collized, type1);
            byte[] msg2_collized = GetMessageByBitmap(bitmap2_collized, type2);

            Console.WriteLine($"Хеш сообщения картинки {image1_collized_path} = " +
                $"{Convert.ToHexString(new StreebogAlgorithm().GetHash(msg1_collized, 1))}" +
                $"");
            Console.WriteLine($"Хеш сообщения картинки {image2_collized_path} = " +
                $"{Convert.ToHexString(new StreebogAlgorithm().GetHash(msg2_collized, 1))}" +
                $"");



        }
    }
}
