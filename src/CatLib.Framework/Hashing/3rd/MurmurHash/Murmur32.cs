// Copyright 2012 Darren Kopp
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Security.Cryptography;

namespace CatLib._3rd.Murmur
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Murmur32 : HashAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        protected const uint C1 = 0xcc9e2d51;

        /// <summary>
        /// 
        /// </summary>
        protected const uint C2 = 0x1b873593;

        private readonly uint _Seed;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        protected Murmur32(uint seed)
        {
            _Seed = seed;
            Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public override int HashSize { get { return 32; } }

        /// <summary>
        /// 
        /// </summary>
        public uint Seed { get { return _Seed; } }

        /// <summary>
        /// 
        /// </summary>
        protected uint H1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected int Length { get; set; }

        private void Reset()
        {
            H1 = Seed;
            Length = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override byte[] HashFinal()
        {
            H1 = (H1 ^ (uint)Length).FMix();

            return BitConverter.GetBytes(H1);
        }
    }
}
