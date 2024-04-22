using StreebogCollisionExplorer.Streebog;

namespace StreebogCollisionExplorer.ExploreCollision
{
    public abstract class BaseCollisionFinder
    {
        protected Random random = new Random();
        protected StreebogAlgorithm streebogAlgorithm = new StreebogAlgorithm();
        protected readonly int messageLength = 64;
        public abstract CollisionFinderResult FindCollisions(int hashSize);
    }
}
