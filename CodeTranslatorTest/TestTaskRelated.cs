using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CodeTranslator.Utility;

namespace CodeTranslatorTest
{
    internal class TestTaskRelated
    {
        private int counter;
        private CancellationTokenSource source;
        private CancellationToken token;

        [SetUp]
        public void Setup()
        {
            counter = 0;
            source = new CancellationTokenSource();
            token = source.Token;
        }

        [Test]
        public void TestCounterEquals10000()
        {
            var list = new List<Task>();
            for(int i = 0; i < 100; i++)
            {
                for(int k = 0; k < 100; k++)
                {
                    // spawn tasks for increasing the counter by 1
                    list.Add(Task.Run(() => { }));
                }
            }

            Console.WriteLine(
                Performance.GetTimeElapsed(() =>
                {
                    while (list.Count > 0)
                    {
                        Thread.Sleep(500);

                        //standard linq - 0.515s average
                        var newList = list.SkipWhile((task) => task.IsCompleted).ToList();
                        counter += list.Count - newList.Count;
                        list = newList;

                        /*// normal implementation - 0.56s average
                        for(int i = 0; i < list.Count; i++)
                            if (list[i].IsCompleted)
                            {
                                list.RemoveAt(i);
                                i--;
                                counter++;
                            }
                        */
                    }
                }).TotalSeconds
            );

            Assert.AreEqual(10000, counter);
        }

        [Test]
        public void TestHandlingCancellationToken()
        {
            Assert.Throws<TaskCanceledException>(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    Task.Run(async () => await Task.Delay(10), token);
                    if (i == 400) source.Cancel();
                }
            });
        }
    }
}
