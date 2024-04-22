namespace StreebogCollisionExplorer.Streebog
{
    public partial class StreebogAlgorithm
    {
        private void XOR(ref byte[] blockA, byte[] blockB)
        {
            foreach (var ind in Enumerable.Range(0, blockSize))
            {
                blockA[ind] ^= blockB[ind];
            }
        }

        private void TransformS(ref byte[] inputBlock)
        {
            foreach (var ind in Enumerable.Range(0, blockSize))
            {
                inputBlock[ind] = Pi[inputBlock[ind]];
            }
        }

        private void TransformP(ref byte[] inputBlock)
        {
            foreach ((byte ind1, byte ind2) in Tau)
            {
                (inputBlock[ind1], inputBlock[ind2]) = (inputBlock[ind2], inputBlock[ind1]);
            }
        }

        private void TransformL(ref byte[] inputBlock)
        {
            for (byte i = 0; i < blockSize; i += minBloсkSize)
            {
                ulong result = 0;
                result = TransformLInt(inputBlock, i, result);
                TransformLUlong(inputBlock, i, result);
            }
        }

        private static void TransformLUlong(byte[] inputBlock, byte i, ulong result)
        {
            ulong maskForUlong = 0b1111111100000000000000000000000000000000000000000000000000000000;
            byte shiftValue = 56;
            for (byte j = i; maskForUlong > 0; ++j, maskForUlong >>= minBloсkSize, shiftValue -= minBloсkSize)
            {
                inputBlock[j] = (byte)((result & maskForUlong) >> shiftValue);
            }
        }

        private ulong TransformLInt(byte[] inputBlock, byte i, ulong result)
        {
            for (byte j = 0, indexOfByte = i; j < blockSize; ++indexOfByte, j += minBloсkSize)
            {
                byte maskForByte = 0b10000000;
                for (byte k = 0; maskForByte > 0; ++k, maskForByte >>= 1)
                {
                    if ((inputBlock[indexOfByte] & maskForByte) == maskForByte)
                    {
                        result ^= MatrixA[j + k];
                    }
                }
            }

            return result;
        }

        private void TransformE(ref byte[] inputBlock, ref byte[] key)
        {
            for (byte i = 0; i < 12; ++i)
            {
                TransformSPL(ref inputBlock, key);
                TransformSPL(ref key, TransformationBlocks[i]);
            }
            XOR(ref inputBlock, key);
        }

        private void TransformG(ref byte[] hash, ref byte[] inputBlock, byte[] n)
        {
            byte[] copyOfH = (byte[])hash.Clone();
            XOR(ref hash, inputBlock);
            TransformSPL(ref copyOfH, n);
            TransformE(ref inputBlock, ref copyOfH);
            XOR(ref hash, inputBlock);
        }

        private void TransformG(ref byte[] hash, ref byte[] inputBlock)
        {
            byte[] hashCopy = (byte[])hash.Clone();
            XOR(ref hash, inputBlock);
            TransformS(ref hashCopy);
            TransformP(ref hashCopy);
            TransformL(ref hashCopy);
            TransformE(ref inputBlock, ref hashCopy);
            XOR(ref hash, inputBlock);
        }

        private void ExtendArraysByte(ref byte[] arrayA, byte[] arrayB)
        {
            byte copyByte;
            bool hasOneForNextByte = false;
            for (int i = arrayA.Length - 1, j = arrayB.Length - 1; j >= 0; i--, j--)
            {
                copyByte = arrayA[i];
                if (hasOneForNextByte)
                {
                    arrayA[i]++;
                }
                arrayA[i] += arrayB[j];
                hasOneForNextByte = arrayA[i] < copyByte;
            }
        }

        private void TransformSPL(ref byte[] inputBlock, byte[] roundKey)
        {
            XOR(ref inputBlock, roundKey);
            TransformS(ref inputBlock);
            TransformP(ref inputBlock);
            TransformL(ref inputBlock);
        }


    }
}
