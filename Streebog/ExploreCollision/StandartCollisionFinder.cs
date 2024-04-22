using StreebogCollisionExplorer.Streebog;
using System.Diagnostics;

namespace StreebogCollisionExplorer.ExploreCollision
{
    public class StandartCollisionFinder : BaseCollisionFinder
    {
        public override CollisionFinderResult FindCollisions(int hashSize)
        {
            DateTime startFindingDateTime = DateTime.Now;
            byte[] randomMsg = new byte[messageLength];
            random.NextBytes(randomMsg);

            byte[] hash = streebogAlgorithm.GetHash(randomMsg, hashSize);

            Dictionary<string, byte[]> hashDictionary = new();
            do
            {
                hashDictionary.Add(Convert.ToHexString(hash), randomMsg);
                random.NextBytes(randomMsg);
                hash = streebogAlgorithm.GetHash(randomMsg, hashSize);
            } while (!hashDictionary.ContainsKey(Convert.ToHexString(hash)));

            return new CollisionFinderResult(
                hashDictionary.Count,
                (DateTime.Now - startFindingDateTime).Milliseconds,
                new List<byte[]> { randomMsg, hashDictionary[Convert.ToHexString(hash)] },
                hash
            );
        }
    }
}
