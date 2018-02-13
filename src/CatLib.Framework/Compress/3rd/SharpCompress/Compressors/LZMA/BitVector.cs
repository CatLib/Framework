﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CatLib._3rd.SharpCompress.Compressors.LZMA
{
    internal class BitVector
    {
        private readonly uint[] mBits;

        public BitVector(int length)
        {
            Length = length;
            mBits = new uint[(length + 31) >> 5];
        }

        public BitVector(int length, bool initValue)
        {
            Length = length;
            mBits = new uint[(length + 31) >> 5];

            if (initValue)
            {
                for (int i = 0; i < mBits.Length; i++)
                {
                    mBits[i] = ~0u;
                }
            }
        }

        public BitVector(List<bool> bits)
            : this(bits.Count)
        {
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                {
                    SetBit(i);
                }
            }
        }

        public bool[] ToArray()
        {
            bool[] bits = new bool[Length];
            for (int i = 0; i < bits.Length; i++)
            {
                bits[i] = this[i];
            }
            return bits;
        }

        public int Length { get; set; }

        public bool this[int index]
        {
            get
            {
                if (index < 0 || index >= Length)
                {
                    throw new ArgumentOutOfRangeException("index");
                }

                return (mBits[index >> 5] & (1u << (index & 31))) != 0;
            }
        }

        public void SetBit(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            mBits[index >> 5] |= 1u << (index & 31);
        }

        internal bool GetAndSet(int index)
        {
            if (index < 0 || index >= Length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            uint bits = mBits[index >> 5];
            uint mask = 1u << (index & 31);
            mBits[index >> 5] |= mask;
            return (bits & mask) != 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Length);
            for (int i = 0; i < Length; i++)
            {
                sb.Append(this[i] ? 'x' : '.');
            }
            return sb.ToString();
        }
    }
}