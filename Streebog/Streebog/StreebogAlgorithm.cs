namespace StreebogCollisionExplorer.Streebog
{
    public partial class StreebogAlgorithm
    {
        public byte[] GetHash(byte[] data)
        {
            var hash = new byte[blockSize];
            var cnt = new byte[blockSize];
            var sigma = new byte[blockSize];

            FillPreparedArrays(data, ref hash, ref cnt, ref sigma, out int index);
            var temp = new byte[blockSize];
            int lengthAddition = blockSize - index;
            Array.Copy(data, 0, temp, lengthAddition, index);
            temp[lengthAddition - 1] = 1;
            GenerateHash(ref hash, ref cnt, ref sigma, index, ref temp);
            return hash;
        }

        public byte[] GetHash(byte[] data, int length)
        {
            return GetHash(data)[..length];
        }

        private void GenerateHash(ref byte[] hash, ref byte[] cnt, ref byte[] sigma, int index, ref byte[] tempBlock)
        {
            ExtendArraysByte(ref sigma, tempBlock);
            TransformG(ref hash, ref tempBlock, cnt);
            int lengthRemainsInBit = index * minBloсkSize;
            byte[] temp = { 
                (byte) ((lengthRemainsInBit & 0b100000000) >> minBloсkSize), 
                (byte) (lengthRemainsInBit ^ 0b100000000) 
            };
            ExtendArraysByte(ref cnt, temp);
            TransformG(ref hash, ref cnt);
            TransformG(ref hash, ref sigma);
        }

        private void FillPreparedArrays(byte[] data, ref byte[] hash, ref byte[] cnt, ref byte[] sigma, out int index)
        {
            index = data.Length;
            for (; index >= blockSize; index -= blockSize)
            {
                byte[] tempBlock = data[(index - blockSize)..index];
                ExtendArraysByte(ref sigma, tempBlock);
                TransformG(ref hash, ref tempBlock, cnt);
                ExtendArraysByte(ref cnt, new byte[] { 2, 0 });
            }
        }
    }
}
