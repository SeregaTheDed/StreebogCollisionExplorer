﻿namespace StreebogCollisionExplorer.Streebog
{
    internal partial class StreebogAlgorithm
    {
        //Не знаю насколько правильно выносить константы в частичном классе,
        //но идея показалось интересной, решил попробовать
        //Взято отсюда https://habr.com/ru/articles/188152/

        protected const byte blockSize = 64;
        protected const byte minBloсkSize = 8;

        protected readonly byte[] Pi = {
            0xFC, 0xEE, 0xDD, 0x11, 0xCF, 0x6E, 0x31, 0x16, 0xFB, 0xC4, 0xFA, 0xDA, 0x23, 0xC5, 0x04, 0x4D,
            0xE9, 0x77, 0xF0, 0xDB, 0x93, 0x2E, 0x99, 0xBA, 0x17, 0x36, 0xF1, 0xBB, 0x14, 0xCD, 0x5F, 0xC1,
            0xF9, 0x18, 0x65, 0x5A, 0xE2, 0x5C, 0xEF, 0x21, 0x81, 0x1C, 0x3C, 0x42, 0x8B, 0x01, 0x8E, 0x4F,
            0x05, 0x84, 0x02, 0xAE, 0xE3, 0x6A, 0x8F, 0xA0, 0x06, 0x0B, 0xED, 0x98, 0x7F, 0xD4, 0xD3, 0x1F,
            0xEB, 0x34, 0x2C, 0x51, 0xEA, 0xC8, 0x48, 0xAB, 0xF2, 0x2A, 0x68, 0xA2, 0xFD, 0x3A, 0xCE, 0xCC,
            0xB5, 0x70, 0x0E, 0x56, 0x08, 0x0C, 0x76, 0x12, 0xBF, 0x72, 0x13, 0x47, 0x9C, 0xB7, 0x5D, 0x87,
            0x15, 0xA1, 0x96, 0x29, 0x10, 0x7B, 0x9A, 0xC7, 0xF3, 0x91, 0x78, 0x6F, 0x9D, 0x9E, 0xB2, 0xB1,
            0x32, 0x75, 0x19, 0x3D, 0xFF, 0x35, 0x8A, 0x7E, 0x6D, 0x54, 0xC6, 0x80, 0xC3, 0xBD, 0x0D, 0x57,
            0xDF, 0xF5, 0x24, 0xA9, 0x3E, 0xA8, 0x43, 0xC9, 0xD7, 0x79, 0xD6, 0xF6, 0x7C, 0x22, 0xB9, 0x03,
            0xE0, 0x0F, 0xEC, 0xDE, 0x7A, 0x94, 0xB0, 0xBC, 0xDC, 0xE8, 0x28, 0x50, 0x4E, 0x33, 0x0A, 0x4A,
            0xA7, 0x97, 0x60, 0x73, 0x1E, 0x00, 0x62, 0x44, 0x1A, 0xB8, 0x38, 0x82, 0x64, 0x9F, 0x26, 0x41,
            0xAD, 0x45, 0x46, 0x92, 0x27, 0x5E, 0x55, 0x2F, 0x8C, 0xA3, 0xA5, 0x7D, 0x69, 0xD5, 0x95, 0x3B,
            0x07, 0x58, 0xB3, 0x40, 0x86, 0xAC, 0x1D, 0xF7, 0x30, 0x37, 0x6B, 0xE4, 0x88, 0xD9, 0xE7, 0x89,
            0xE1, 0x1B, 0x83, 0x49, 0x4C, 0x3F, 0xF8, 0xFE, 0x8D, 0x53, 0xAA, 0x90, 0xCA, 0xD8, 0x85, 0x61,
            0x20, 0x71, 0x67, 0xA4, 0x2D, 0x2B, 0x09, 0x5B, 0xCB, 0x9B, 0x25, 0xD0, 0xBE, 0xE5, 0x6C, 0x52,
            0x59, 0xA6, 0x74, 0xD2, 0xE6, 0xF4, 0xB4, 0xC0, 0xD1, 0x66, 0xAF, 0xC2, 0x39, 0x4B, 0x63, 0xB6
        };

        protected readonly (byte, byte)[] Tau ={
            (01, 08), (02, 16), (03, 24), (04, 32), (05, 40), (06, 48), (07, 56),
            (10, 17), (11, 25), (12, 33), (13, 41), (14, 49), (15, 57),
            (19, 26), (20, 34), (21, 42), (22, 50), (23, 58),
            (28, 35), (29, 43), (30, 51), (31, 59),
            (37, 44), (38, 52), (39, 60),
            (46, 53), (47, 61),
            (55, 62)
        };

        protected readonly ulong[] MatrixA = {
            0x8E20FAA72BA0B470, 0x47107DDD9B505A38, 0xAD08B0E0C3282D1C, 0xD8045870EF14980E,
            0x6C022C38F90A4C07, 0x3601161CF205268D, 0x1B8E0B0E798C13C8, 0x83478B07B2468764,
            0xA011D380818E8F40, 0x5086E740CE47C920, 0x2843FD2067ADEA10, 0x14AFF010BDD87508,
            0x0AD97808D06CB404, 0x05E23C0468365A02, 0x8C711E02341B2D01, 0x46B60F011A83988E,
            0x90DAB52A387AE76F, 0x486DD4151C3DFDB9, 0x24B86A840E90F0D2, 0x125C354207487869,
            0x092E94218D243CBA, 0x8A174A9EC8121E5D, 0x4585254F64090FA0, 0xACCC9CA9328A8950,
            0x9D4DF05D5F661451, 0xC0A878A0A1330AA6, 0x60543C50DE970553, 0x302A1E286FC58CA7,
            0x18150F14B9EC46DD, 0x0C84890AD27623E0, 0x0642CA05693B9F70, 0x0321658CBA93C138,
            0x86275DF09CE8AAA8, 0x439DA0784E745554, 0xAFC0503C273AA42A, 0xD960281E9D1D5215,
            0xE230140FC0802984, 0x71180A8960409A42, 0xB60C05CA30204D21, 0x5B068C651810A89E,
            0x456C34887A3805B9, 0xAC361A443D1C8CD2, 0x561B0D22900E4669, 0x2B838811480723BA,
            0x9BCF4486248D9F5D, 0xC3E9224312C8C1A0, 0xEFFA11AF0964EE50, 0xF97D86D98A327728,
            0xE4FA2054A80B329C, 0x727D102A548B194E, 0x39B008152ACB8227, 0x9258048415EB419D,
            0x492C024284FBAEC0, 0xAA16012142F35760, 0x550B8E9E21F7A530, 0xA48B474F9EF5DC18,
            0x70A6A56E2440598E, 0x3853DC371220A247, 0x1CA76E95091051AD, 0x0EDD37C48A08A6D8,
            0x07E095624504536C, 0x8D70C431AC02A736, 0xC83862965601DD1B, 0x641C314B2B8EE083
        };

        protected readonly byte[][] TransformationBlocks = {
            new byte[blockSize] {
                0xB1,0x08,0x5B,0xDA,0x1E,0xCA,0xDA,0xE9,0xEB,0xCB,0x2F,0x81,0xC0,0x65,0x7C,0x1F,
                0x2F,0x6A,0x76,0x43,0x2E,0x45,0xD0,0x16,0x71,0x4E,0xB8,0x8D,0x75,0x85,0xC4,0xFC,
                0x4B,0x7C,0xE0,0x91,0x92,0x67,0x69,0x01,0xA2,0x42,0x2A,0x08,0xA4,0x60,0xD3,0x15,
                0x05,0x76,0x74,0x36,0xCC,0x74,0x4D,0x23,0xDD,0x80,0x65,0x59,0xF2,0xA6,0x45,0x07
            },
            new byte[blockSize] {
                0x6F,0xA3,0xB5,0x8A,0xA9,0x9D,0x2F,0x1A,0x4F,0xE3,0x9D,0x46,0x0F,0x70,0xB5,0xD7,
                0xF3,0xFE,0xEA,0x72,0x0A,0x23,0x2B,0x98,0x61,0xD5,0x5E,0x0F,0x16,0xB5,0x01,0x31,
                0x9A,0xB5,0x17,0x6B,0x12,0xD6,0x99,0x58,0x5C,0xB5,0x61,0xC2,0xDB,0x0A,0xA7,0xCA,
                0x55,0xDD,0xA2,0x1B,0xD7,0xCB,0xCD,0x56,0xE6,0x79,0x04,0x70,0x21,0xB1,0x9B,0xB7
            },
            new byte[blockSize] {
                0xF5,0x74,0xDC,0xAC,0x2B,0xCE,0x2F,0xC7,0x0A,0x39,0xFC,0x28,0x6A,0x3D,0x84,0x35,
                0x06,0xF1,0x5E,0x5F,0x52,0x9C,0x1F,0x8B,0xF2,0xEA,0x75,0x14,0xB1,0x29,0x7B,0x7B,
                0xD3,0xE2,0x0F,0xE4,0x90,0x35,0x9E,0xB1,0xC1,0xC9,0x3A,0x37,0x60,0x62,0xDB,0x09,
                0xC2,0xB6,0xF4,0x43,0x86,0x7A,0xDB,0x31,0x99,0x1E,0x96,0xF5,0x0A,0xBA,0x0A,0xB2
            },
            new byte[blockSize] {
                0xEF,0x1F,0xDF,0xB3,0xE8,0x15,0x66,0xD2,0xF9,0x48,0xE1,0xA0,0x5D,0x71,0xE4,0xDD,
                0x48,0x8E,0x85,0x7E,0x33,0x5C,0x3C,0x7D,0x9D,0x72,0x1C,0xAD,0x68,0x5E,0x35,0x3F,
                0xA9,0xD7,0x2C,0x82,0xED,0x03,0xD6,0x75,0xD8,0xB7,0x13,0x33,0x93,0x52,0x03,0xBE,
                0x34,0x53,0xEA,0xA1,0x93,0xE8,0x37,0xF1,0x22,0x0C,0xBE,0xBC,0x84,0xE3,0xD1,0x2E
            },
            new byte[blockSize] {
                0x4B,0xEA,0x6B,0xAC,0xAD,0x47,0x47,0x99,0x9A,0x3F,0x41,0x0C,0x6C,0xA9,0x23,0x63,
                0x7F,0x15,0x1C,0x1F,0x16,0x86,0x10,0x4A,0x35,0x9E,0x35,0xD7,0x80,0x0F,0xFF,0xBD,
                0xBF,0xCD,0x17,0x47,0x25,0x3A,0xF5,0xA3,0xDF,0xFF,0x00,0xB7,0x23,0x27,0x1A,0x16,
                0x7A,0x56,0xA2,0x7E,0xA9,0xEA,0x63,0xF5,0x60,0x17,0x58,0xFD,0x7C,0x6C,0xFE,0x57
            },
            new byte[blockSize] {
                0xAE,0x4F,0xAE,0xAE,0x1D,0x3A,0xD3,0xD9,0x6F,0xA4,0xC3,0x3B,0x7A,0x30,0x39,0xC0,
                0x2D,0x66,0xC4,0xF9,0x51,0x42,0xA4,0x6C,0x18,0x7F,0x9A,0xB4,0x9A,0xF0,0x8E,0xC6,
                0xCF,0xFA,0xA6,0xB7,0x1C,0x9A,0xB7,0xB4,0x0A,0xF2,0x1F,0x66,0xC2,0xBE,0xC6,0xB6,
                0xBF,0x71,0xC5,0x72,0x36,0x90,0x4F,0x35,0xFA,0x68,0x40,0x7A,0x46,0x64,0x7D,0x6E
            },
            new byte[blockSize] {
                0xF4,0xC7,0x0E,0x16,0xEE,0xAA,0xC5,0xEC,0x51,0xAC,0x86,0xFE,0xBF,0x24,0x09,0x54,
                0x39,0x9E,0xC6,0xC7,0xE6,0xBF,0x87,0xC9,0xD3,0x47,0x3E,0x33,0x19,0x7A,0x93,0xC9,
                0x09,0x92,0xAB,0xC5,0x2D,0x82,0x2C,0x37,0x06,0x47,0x69,0x83,0x28,0x4A,0x05,0x04,
                0x35,0x17,0x45,0x4C,0xA2,0x3C,0x4A,0xF3,0x88,0x86,0x56,0x4D,0x3A,0x14,0xD4,0x93
            },
            new byte[blockSize] {
                0x9B,0x1F,0x5B,0x42,0x4D,0x93,0xC9,0xA7,0x03,0xE7,0xAA,0x02,0x0C,0x6E,0x41,0x41,
                0x4E,0xB7,0xF8,0x71,0x9C,0x36,0xDE,0x1E,0x89,0xB4,0x44,0x3B,0x4D,0xDB,0xC4,0x9A,
                0xF4,0x89,0x2B,0xCB,0x92,0x9B,0x06,0x90,0x69,0xD1,0x8D,0x2B,0xD1,0xA5,0xC4,0x2F,
                0x36,0xAC,0xC2,0x35,0x59,0x51,0xA8,0xD9,0xA4,0x7F,0x0D,0xD4,0xBF,0x02,0xE7,0x1E
            },
            new byte[blockSize] {
                0x37,0x8F,0x5A,0x54,0x16,0x31,0x22,0x9B,0x94,0x4C,0x9A,0xD8,0xEC,0x16,0x5F,0xDE,
                0x3A,0x7D,0x3A,0x1B,0x25,0x89,0x42,0x24,0x3C,0xD9,0x55,0xB7,0xE0,0x0D,0x09,0x84,
                0x80,0x0A,0x44,0x0B,0xDB,0xB2,0xCE,0xB1,0x7B,0x2B,0x8A,0x9A,0xA6,0x07,0x9C,0x54,
                0x0E,0x38,0xDC,0x92,0xCB,0x1F,0x2A,0x60,0x72,0x61,0x44,0x51,0x83,0x23,0x5A,0xDB
            },
            new byte[blockSize] {
                0xAB,0xBE,0xDE,0xA6,0x80,0x05,0x6F,0x52,0x38,0x2A,0xE5,0x48,0xB2,0xE4,0xF3,0xF3,
                0x89,0x41,0xE7,0x1C,0xFF,0x8A,0x78,0xDB,0x1F,0xFF,0xE1,0x8A,0x1B,0x33,0x61,0x03,
                0x9F,0xE7,0x67,0x02,0xAF,0x69,0x33,0x4B,0x7A,0x1E,0x6C,0x30,0x3B,0x76,0x52,0xF4,
                0x36,0x98,0xFA,0xD1,0x15,0x3B,0xB6,0xC3,0x74,0xB4,0xC7,0xFB,0x98,0x45,0x9C,0xED
            },
            new byte[blockSize] {
                0x7B,0xCD,0x9E,0xD0,0xEF,0xC8,0x89,0xFB,0x30,0x02,0xC6,0xCD,0x63,0x5A,0xFE,0x94,
                0xD8,0xFA,0x6B,0xBB,0xEB,0xAB,0x07,0x61,0x20,0x01,0x80,0x21,0x14,0x84,0x66,0x79,
                0x8A,0x1D,0x71,0xEF,0xEA,0x48,0xB9,0xCA,0xEF,0xBA,0xCD,0x1D,0x7D,0x47,0x6E,0x98,
                0xDE,0xA2,0x59,0x4A,0xC0,0x6F,0xD8,0x5D,0x6B,0xCA,0xA4,0xCD,0x81,0xF3,0x2D,0x1B
            },
            new byte[blockSize] {
                0x37,0x8E,0xE7,0x67,0xF1,0x16,0x31,0xBA,0xD2,0x13,0x80,0xB0,0x04,0x49,0xB1,0x7A,
                0xCD,0xA4,0x3C,0x32,0xBC,0xDF,0x1D,0x77,0xF8,0x20,0x12,0xD4,0x30,0x21,0x9F,0x9B,
                0x5D,0x80,0xEF,0x9D,0x18,0x91,0xCC,0x86,0xE7,0x1D,0xA4,0xAA,0x88,0xE1,0x28,0x52,
                0xFA,0xF4,0x17,0xD5,0xD9,0xB2,0x1B,0x99,0x48,0xBC,0x92,0x4A,0xF1,0x1B,0xD7,0x20
            }
        };
    }
}
