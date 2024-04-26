using StreebogCollisionExplorer.ExploreCollision;
using StreebogCollisionExplorer.Streebog;
using System.Collections;
using System.Drawing;
using System.Security.Policy;

namespace StreebogCollisionExplorer
{
    internal static class ImageMeaningfulCollisionsExplorer
    {
        static Random random = new Random();
        private static void WriteRandomToImageAndSave(string path, string savePath)
        {
            Bitmap bitmap = new Bitmap(Image.FromFile(path));
            int x = random.Next(bitmap.Width);
            int y = random.Next(bitmap.Height);

            bitmap.SetPixel(x, y, Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)));

            bitmap.Save(savePath);
        }
        public static byte[] GetHashByImage(string path)
        {
            return new StreebogAlgorithm().GetHash(File.ReadAllBytes(path), 1);
        }

        public static void Explore(string imagePath1, string imagePath2)
        {
            string tempPath = "Temp";
            Directory.CreateDirectory(tempPath);

            Dictionary<string, string> hashDictionary1 = new();
            Dictionary<string, string> hashDictionary2 = new();
            int i = 0;
            while (true)
            {
                string newPath1 = tempPath + "/image1_" + i + ".png";
                WriteRandomToImageAndSave(imagePath1, newPath1);
                byte[] hash1 = GetHashByImage(newPath1);
                hashDictionary1.TryAdd(Convert.ToHexString(hash1), newPath1);

                if (hashDictionary2.ContainsKey(Convert.ToHexString(hash1)))
                {
                    string file1 = newPath1;
                    string file2 = hashDictionary2[Convert.ToHexString(hash1)];
                    Console.WriteLine($"Файл:{file1}, хеш:{Convert.ToHexString(GetHashByImage(file1))}");
                    Console.WriteLine($"Файл:{file2}, хеш:{Convert.ToHexString(GetHashByImage(file2))}");
                    break;
                }




                string newPath2 = tempPath + "/image2_" + i + ".png";
                WriteRandomToImageAndSave(imagePath2, newPath2);
                byte[] hash2 = GetHashByImage(newPath2);
                hashDictionary2.TryAdd(Convert.ToHexString(hash2), newPath2);

                if (hashDictionary1.ContainsKey(Convert.ToHexString(hash2)))
                {
                    string file1 = newPath2;
                    string file2 = hashDictionary1[Convert.ToHexString(hash2)];
                    Console.WriteLine($"Файл:{file1}, хеш:{Convert.ToHexString(GetHashByImage(file1))}");
                    Console.WriteLine($"Файл:{file2}, хеш:{Convert.ToHexString(GetHashByImage(file2))}");
                    break;
                }
                

                i++;
            }
        }
    }
}
