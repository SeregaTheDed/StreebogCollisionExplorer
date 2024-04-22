namespace StreebogCollisionExplorer.ExploreCollision
{
    public class CollisionFinderResult
    {
        public CollisionFinderResult(int attemptsCount, long millisecondsTotal, 
            IEnumerable<byte[]> messages, byte[] hash)
        {
            this.AttemptsCount = attemptsCount;
            this.MillisecondsTotal = millisecondsTotal;
            this.Messages = messages;
            this.Hash = hash;
        }

        public int AttemptsCount { get; protected set; }
        public long MillisecondsTotal { get; protected set; }
        public IEnumerable<byte[]> Messages { get; protected set; }
        public byte[] Hash { get; protected set; }


        // маппинг сообщений и хеша к строке для читаемости и переиспользуемости
        public IEnumerable<string> MessagesStrings => Messages.Select(Convert.ToHexString);
        public string HashString => Convert.ToHexString(Hash);

    }
}
