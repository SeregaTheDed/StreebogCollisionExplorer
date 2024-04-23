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
            byte[] lastHashA = message;
            byte[] lastHashB = message;

            byte[] hashA = streebogAlgorithm.GetHash(message);
            byte[] hashB = streebogAlgorithm.GetHash(hashA);

            while (!hashA.Take(hashSize).SequenceEqual(hashB.Take(hashSize)))
            {
                lastHashA = hashA;
                hashA = streebogAlgorithm.GetHash(hashA);
                hashB = streebogAlgorithm.GetHash(hashB);
                lastHashB = hashB;
                hashB = streebogAlgorithm.GetHash(hashB);
                attemptsCount++;
            }

            return new CollisionFinderResult(
                attemptsCount,
                (long) (DateTime.Now - startFindingDateTime).TotalMilliseconds,
                new List<byte[]> { lastHashA, lastHashB },
                hashB[..hashSize]
            );
        }


    }
}
