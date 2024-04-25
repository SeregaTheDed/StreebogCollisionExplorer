using StreebogCollisionExplorer.ExploreCollision;
using StreebogCollisionExplorer.Streebog;
using System.Collections;
using System.Security.Policy;

namespace StreebogCollisionExplorer
{
    internal static class ImageMeaningfulCollisionsExplorer
    {
        private static Bitmap WriteMessageToBitmap(Bitmap bitmap, byte[] msg)
        {
            Bitmap result = (Bitmap) bitmap.Clone();
            BitArray bitArrayMsg = new BitArray(msg);

            IEnumerator enumerator = bitArrayMsg.GetEnumerator();
            enumerator.MoveNext();//Пропускаем тип сообщения
            for (int i = 0; i < result.Height; i++)
            {
                for (int j = 0; j < result.Width; j++)
                {
                    bool bit = (bool) enumerator.Current;
                    enumerator.MoveNext();
                    Color pixel = result.GetPixel(j, i);
                    byte redByte = pixel.R;
                    if (bit && redByte % 2 != 1) 
                    { 
                        redByte++; 
                    }
                    else if (!bit && redByte % 2 != 0) 
                    { 
                        redByte--; 
                    }

                    result.SetPixel(j, i, Color.FromArgb(redByte, pixel.G, pixel.B));
                }
            }
            return result;
        }

        private static BitArray GetBytesByBitmap(Bitmap bitmap, bool type)
        {
            if (bitmap.Width * bitmap.Height % 8 != 7)
            {
                throw new ArgumentException("Ширина * Высоту картинки по модулю 8 должна быть == 7");
            }
            BitArray result = new BitArray(bitmap.Width * bitmap.Height + 1); //В начале у нас будет тип изображения
            result[0] = type;
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    int resultIndex = bitmap.Width * i + j + 1;
                    bool pixelRedSmallByte = bitmap.GetPixel(j, i).R % 2 == 1;
                    result[resultIndex] = pixelRedSmallByte;
                }
            }
            for (int i = 1; i < result.Length; i+=8)
            {
                /*for (int j = i; j < i + 10; j++)
                {
                    Console.Write(result[j] ? 1 : 0);
                }
                Console.WriteLine();*/
                int right = Math.Min(i + 8, result.Length);
                for (int j = 0; j < 4; j++)
                {
                    int index1 = j + i;
                    int index2 = right-j-1;
                    var temp = result[index1];
                    result[index1] = result[index2];
                    result[index2] = temp;
                }
                /*for (int j = i; j < i+10; j++)
                {
                    Console.Write(result[j] ? 1:0);
                }
                Console.WriteLine();
                Console.WriteLine("---");*/

            }
            return result;
        }
        public static byte[] GetMessageByBitmap(Bitmap bitmap, bool type)
        {
            BitArray bitArray = GetBytesByBitmap(bitmap, type);
            byte[] result = bitArray.ToBytes().ToArray();
            //BitArray bitArray2 = new BitArray(result);
            //byte[] result2 = bitArray2.ToBytes().ToArray();
            return result;
        }

        public static void Explore(string imagePath1, string imagePath2)
        {
            Bitmap bitmap1 = new Bitmap(Image.FromFile(imagePath1));
            Bitmap bitmap2 = new Bitmap(Image.FromFile(imagePath2));

            byte[] msg1 = GetMessageByBitmap(bitmap1, false);
            byte[] msg2 = GetMessageByBitmap(bitmap2, true);
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

            int randomMessageSize = msg1.Length;
            CollisionFinderResult collisionFinderResult = 
                new StandartCollisionFinder().FindCollisions(1, randomMessageSize);
            while(collisionFinderResult.Messages.First().First() >> 7 == 1
                ||
                collisionFinderResult.Messages.Last().First() >> 7 == 0) 
            {
                // Находим такую коллизию, чтобы у нас различались типы сообщений (первый бит)
                collisionFinderResult = new StandartCollisionFinder().FindCollisions(1, randomMessageSize);
            }



            //Записываем сообщения в картинки и сохраняем
            string collizedPrefix = "collized_";
            WriteMessageToBitmap(bitmap1, collisionFinderResult.Messages.First())
                .Save(collizedPrefix + "image1.png");
            WriteMessageToBitmap(bitmap2, collisionFinderResult.Messages.Last())
                .Save(collizedPrefix + "image2.png");
            Console.WriteLine("Хеш найденной коллизии: " + collisionFinderResult.HashString);

            //Загружаем картинки, считаем в них хеши
            string image1_collized_path = collizedPrefix + "image1.png";
            string image2_collized_path = collizedPrefix + "image2.png";
            Bitmap bitmap1_collized = new Bitmap(Image.FromFile(image1_collized_path));
            Bitmap bitmap2_collized = new Bitmap(Image.FromFile(image2_collized_path));

            byte[] msg1_collized = GetMessageByBitmap(bitmap1_collized, false);
            byte[] msg2_collized = GetMessageByBitmap(bitmap2_collized, true);

            Console.WriteLine($"Хеш сообщения картинки {image1_collized_path} = " +
                $"{Convert.ToHexString(new StreebogAlgorithm().GetHash(msg1_collized, 1))}" +
                $"");
            Console.WriteLine($"Хеш сообщения картинки {image2_collized_path} = " +
                $"{Convert.ToHexString(new StreebogAlgorithm().GetHash(msg2_collized, 1))}" +
                $"");



        }
    }
}
