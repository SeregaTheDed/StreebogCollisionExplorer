using System.Diagnostics;

namespace StreebogCollisionExplorer.ExploreCollision
{
    internal class IterateCollisionFinder : BaseCollisionFinder
    {
        public override CollisionFinderResult FindCollisions(int hashSize)
        {
            DateTime startFindingDateTime = DateTime.Now;
            int attemptsCount = 1;
            byte[] message = new byte[messageLength];
            random.NextBytes(message);
            byte[] lastHashOne = message;
            byte[] lastHashB = message;

            byte[] hashA = streebogAlgorithm.GetHash(message);
            byte[] hashB = streebogAlgorithm.GetHash(hashA);

            while (!hashA[..hashSize].SequenceEqual(hashB[..hashSize]))
            {
                lastHashOne = hashA;
                hashA = streebogAlgorithm.GetHash(hashA);
                hashB = streebogAlgorithm.GetHash(hashB);
                lastHashB = hashB;
                hashB = streebogAlgorithm.GetHash(hashB);
                attemptsCount++;
            }

            return new CollisionFinderResult(
                attemptsCount,
                (DateTime.Now - startFindingDateTime).Milliseconds,
                new List<byte[]> { lastHashOne, lastHashB },
                hashB
            );
        }


    }
}
