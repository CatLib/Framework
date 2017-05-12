/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */
using System.Collections.Generic;

namespace CatLib.Demo.MsgPack
{
    public class MsgPackTest1Data
    {
        public int age;
        public string name;
        public bool sex;

        public MsgPackTest1Data()
        {
            age = 1;
            name = "test";
            sex = false;
        }
    }

    public class MsgPackTest2Data
    {
        public List<string> testList;
        public MsgPackTest2Data()
        {
            testList = new List<string>();
            testList.Add("1");
            testList.Add("2");
            testList.Add("3");
            testList.Add("4");
            testList.Add("5");
        }
    }

    public class MsgPackTest3Data
    {
        public List<MsgPackTest1Data> testList;

        public MsgPackTest3Data()
        {
            testList = new List<MsgPackTest1Data>();
            MsgPackTest1Data data = new MsgPackTest1Data();
            data.age = 10;
            data.name = "CatLib";
            data.sex = true;

            testList.Add(data);
        }
    }
}

