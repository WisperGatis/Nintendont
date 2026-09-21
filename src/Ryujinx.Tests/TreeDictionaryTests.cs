using NUnit.Framework;
using Ryujinx.Common.Collections;
using System;
using System.Collections.Generic;

namespace Ryujinx.Tests.Collections
{
    class TreeDictionaryTests
    {
        [Test]
        public void EnsureAddIntegrity()
        {
            TreeDictionary<int, int> dictionary = new();

            Assert.That(0, Is.EqualTo(dictionary.Count));

            dictionary.Add(2, 7);
            dictionary.Add(1, 4);
            dictionary.Add(10, 2);
            dictionary.Add(4, 1);
            dictionary.Add(3, 2);
            dictionary.Add(11, 2);
            dictionary.Add(5, 2);

            Assert.That(7, Is.EqualTo(dictionary.Count));

            List<KeyValuePair<int, int>> list = dictionary.AsLevelOrderList();

            /*
             *  Tree Should Look as Follows After Rotations
             *  
             *        2
             *    1        4
             *           3    10
             *              5    11
             *  
             */

            Assert.That(dictionary.Count, Is.EqualTo(list.Count));
            Assert.That(2, Is.EqualTo(list[0].Key));
            Assert.That(1, Is.EqualTo(list[1].Key));
            Assert.That(4, Is.EqualTo(list[2].Key));
            Assert.That(3, Is.EqualTo(list[3].Key));
            Assert.That(10, Is.EqualTo(list[4].Key));
            Assert.That(5, Is.EqualTo(list[5].Key));
            Assert.That(11, Is.EqualTo(list[6].Key));
        }

        [Test]
        public void EnsureRemoveIntegrity()
        {
            TreeDictionary<int, int> dictionary = new();

            Assert.That(0, Is.EqualTo(dictionary.Count));

            dictionary.Add(2, 7);
            dictionary.Add(1, 4);
            dictionary.Add(10, 2);
            dictionary.Add(4, 1);
            dictionary.Add(3, 2);
            dictionary.Add(11, 2);
            dictionary.Add(5, 2);
            dictionary.Add(7, 2);
            dictionary.Add(9, 2);
            dictionary.Add(8, 2);
            dictionary.Add(13, 2);
            dictionary.Add(24, 2);
            dictionary.Add(6, 2);
            Assert.That(13, Is.EqualTo(dictionary.Count));

            List<KeyValuePair<int, int>> list = dictionary.AsLevelOrderList();

            /*
             *  Tree Should Look as Follows After Rotations
             *  
             *              4
             *      2               10
             *  1      3       7         13
             *              5      9  11    24
             *                6  8 
             */

            foreach (KeyValuePair<int, int> node in list)
            {
                Console.WriteLine($"{node.Key} -> {node.Value}");
            }
            Assert.That(dictionary.Count, Is.EqualTo(list.Count));
            Assert.That(4, Is.EqualTo(list[0].Key));
            Assert.That(2, Is.EqualTo(list[1].Key));
            Assert.That(10, Is.EqualTo(list[2].Key));
            Assert.That(1, Is.EqualTo(list[3].Key));
            Assert.That(3, Is.EqualTo(list[4].Key));
            Assert.That(7, Is.EqualTo(list[5].Key));
            Assert.That(13, Is.EqualTo(list[6].Key));
            Assert.That(5, Is.EqualTo(list[7].Key));
            Assert.That(9, Is.EqualTo(list[8].Key));
            Assert.That(11, Is.EqualTo(list[9].Key));
            Assert.That(24, Is.EqualTo(list[10].Key));
            Assert.That(6, Is.EqualTo(list[11].Key));
            Assert.That(8, Is.EqualTo(list[12].Key));

            list.Clear();

            dictionary.Remove(7);

            /*
             *  Tree Should Look as Follows After Removal
             *  
             *              4
             *      2               10
             *  1      3       6         13
             *              5      9  11    24
             *                  8 
             */

            list = dictionary.AsLevelOrderList();
            foreach (KeyValuePair<int, int> node in list)
            {
                Console.WriteLine($"{node.Key} -> {node.Value}");
            }
            Assert.That(4, Is.EqualTo(list[0].Key));
            Assert.That(2, Is.EqualTo(list[1].Key));
            Assert.That(10, Is.EqualTo(list[2].Key));
            Assert.That(1, Is.EqualTo(list[3].Key));
            Assert.That(3, Is.EqualTo(list[4].Key));
            Assert.That(6, Is.EqualTo(list[5].Key));
            Assert.That(13, Is.EqualTo(list[6].Key));
            Assert.That(5, Is.EqualTo(list[7].Key));
            Assert.That(9, Is.EqualTo(list[8].Key));
            Assert.That(11, Is.EqualTo(list[9].Key));
            Assert.That(24, Is.EqualTo(list[10].Key));
            Assert.That(8, Is.EqualTo(list[11].Key));

            list.Clear();

            dictionary.Remove(10);

            list = dictionary.AsLevelOrderList();
            /*
             *  Tree Should Look as Follows After Removal
             *  
             *              4
             *      2               9
             *  1      3       6         13
             *              5      8  11    24
             *                   
             */
            foreach (KeyValuePair<int, int> node in list)
            {
                Console.WriteLine($"{node.Key} -> {node.Value}");
            }
            Assert.That(4, Is.EqualTo(list[0].Key));
            Assert.That(2, Is.EqualTo(list[1].Key));
            Assert.That(9, Is.EqualTo(list[2].Key));
            Assert.That(1, Is.EqualTo(list[3].Key));
            Assert.That(3, Is.EqualTo(list[4].Key));
            Assert.That(6, Is.EqualTo(list[5].Key));
            Assert.That(13, Is.EqualTo(list[6].Key));
            Assert.That(5, Is.EqualTo(list[7].Key));
            Assert.That(8, Is.EqualTo(list[8].Key));
            Assert.That(11, Is.EqualTo(list[9].Key));
            Assert.That(24, Is.EqualTo(list[10].Key));
        }

        [Test]
        public void EnsureOverwriteIntegrity()
        {
            TreeDictionary<int, int> dictionary = new();

            Assert.That(0, Is.EqualTo(dictionary.Count));

            dictionary.Add(2, 7);
            dictionary.Add(1, 4);
            dictionary.Add(10, 2);
            dictionary.Add(4, 1);
            dictionary.Add(3, 2);
            dictionary.Add(11, 2);
            dictionary.Add(5, 2);
            dictionary.Add(7, 2);
            dictionary.Add(9, 2);
            dictionary.Add(8, 2);
            dictionary.Add(13, 2);
            dictionary.Add(24, 2);
            dictionary.Add(6, 2);
            Assert.That(13, Is.EqualTo(dictionary.Count));

            List<KeyValuePair<int, int>> list = dictionary.AsLevelOrderList();

            foreach (KeyValuePair<int, int> node in list)
            {
                Console.WriteLine($"{node.Key} -> {node.Value}");
            }

            /*
             *  Tree Should Look as Follows After Rotations
             *  
             *              4
             *      2               10
             *  1      3       7         13
             *              5      9  11    24
             *                6  8 
             */

            Assert.That(dictionary.Count, Is.EqualTo(list.Count));
            Assert.That(4, Is.EqualTo(list[0].Key));
            Assert.That(2, Is.EqualTo(list[1].Key));
            Assert.That(10, Is.EqualTo(list[2].Key));
            Assert.That(1, Is.EqualTo(list[3].Key));
            Assert.That(3, Is.EqualTo(list[4].Key));
            Assert.That(7, Is.EqualTo(list[5].Key));
            Assert.That(13, Is.EqualTo(list[6].Key));
            Assert.That(5, Is.EqualTo(list[7].Key));
            Assert.That(9, Is.EqualTo(list[8].Key));
            Assert.That(11, Is.EqualTo(list[9].Key));
            Assert.That(24, Is.EqualTo(list[10].Key));
            Assert.That(6, Is.EqualTo(list[11].Key));
            Assert.That(8, Is.EqualTo(list[12].Key));

            Assert.That(2, Is.EqualTo(list[4].Value));

            dictionary.Add(3, 4);

            list = dictionary.AsLevelOrderList();

            Assert.That(4, Is.EqualTo(list[4].Value));


            // Assure that none of the nodes locations have been modified.
            Assert.That(4, Is.EqualTo(list[0].Key));
            Assert.That(2, Is.EqualTo(list[1].Key));
            Assert.That(10, Is.EqualTo(list[2].Key));
            Assert.That(1, Is.EqualTo(list[3].Key));
            Assert.That(3, Is.EqualTo(list[4].Key));
            Assert.That(7, Is.EqualTo(list[5].Key));
            Assert.That(13, Is.EqualTo(list[6].Key));
            Assert.That(5, Is.EqualTo(list[7].Key));
            Assert.That(9, Is.EqualTo(list[8].Key));
            Assert.That(11, Is.EqualTo(list[9].Key));
            Assert.That(24, Is.EqualTo(list[10].Key));
            Assert.That(6, Is.EqualTo(list[11].Key));
            Assert.That(8, Is.EqualTo(list[12].Key));
        }
    }
}
