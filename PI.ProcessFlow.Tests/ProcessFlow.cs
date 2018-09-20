using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using PI.ProcessFlow;
using PI.ProcessFlow.Models;
using PI.ProcessFlow.Interfaces;
using PI.ProcessFlow.Tests.Helpers;

namespace PI.ProcessFlow.Tests
{
    [TestClass]
    public class ProcessFlow
    {
        [TestMethod]
        public void OperationAdderTest()
        {
            Task.Run(async () =>
            {
                //create tree
                Initiator root = new Initiator();
                root.NextStep = new OperationAdder()
                {
                    NextStep = new Terminator()
                };

                //create data
                TestData data = new TestData()
                {
                    A = 5,
                    B = 10
                };

                FlowManager man = new FlowManager();
                bool result = await man.Execute(root, data);

                Assert.IsTrue(result);
                Assert.AreEqual(15, data.Result);
                Assert.AreEqual(3, man.ProcessedCounter);

            }).GetAwaiter().GetResult();
        }


        [TestMethod]
        public void OperationMultiplierTest()
        {
            Task.Run(async () =>
            {
                //create tree
                Initiator root = new Initiator();
                root.NextStep = new OperationMultiplier()
                {
                    NextStep = new Terminator()
                };

                //create data
                TestData data = new TestData()
                {
                    A = 5,
                    B = 10
                };

                FlowManager man = new FlowManager();
                bool result = await man.Execute(root, data);

                Assert.IsTrue(result);
                Assert.AreEqual(50, data.Result);
                Assert.AreEqual(3, man.ProcessedCounter);

            }).GetAwaiter().GetResult();
        }


        [TestMethod]
        public void DecisionMultiplierTest()
        {
            Task.Run(async () =>
            {
                //create tree
                Initiator root = new Initiator();
                DecisionMultiplier child = new DecisionMultiplier()
                {
                    Paths = new List<DecisionPath>(),
                };
                child.Paths.Add(new DecisionPath()
                {
                    Value = 25,
                    NextStep = new OperationMultiplier()
                    {
                        NextStep = new Terminator()
                    }
                });
                child.Paths.Add(new DecisionPath()
                {
                    Value = 75,
                    NextStep = new OperationAdder()
                    {
                        NextStep = new Terminator()
                    }
                });
                root.NextStep = child;


                //create data
                TestData data = new TestData()
                {
                    A = 5,
                    B = 2
                };

                FlowManager man = new FlowManager();
                bool result = await man.Execute(root, data);

                //should go through to id 2 and therefore result = A * B
                Assert.IsTrue(result);
                Assert.AreEqual(10, data.Result);
                Assert.AreEqual(4, man.ProcessedCounter);

            }).GetAwaiter().GetResult();
        }


        [TestMethod]
        public void DecisionMultiplierTest2()
        {
            Task.Run(async () =>
            {
                //create tree
                Initiator root = new Initiator();
                DecisionMultiplier child = new DecisionMultiplier()
                {
                    Paths = new List<DecisionPath>(),
                };
                child.Paths.Add(new DecisionPath()
                {
                    Value = 25,
                    NextStep = new OperationMultiplier()
                    {
                        NextStep = new Terminator()
                    }
                });
                child.Paths.Add(new DecisionPath()
                {
                    Value = 75,
                    NextStep = new OperationAdder()
                    {
                        NextStep = new Terminator()
                    }
                });
                root.NextStep = child;


                //create data
                TestData data = new TestData()
                {
                    A = 5,
                    B = 10
                };

                FlowManager man = new FlowManager();
                bool result = await man.Execute(root, data);

                //should go through to id 3 and therefore result = A + B
                Assert.IsTrue(result);
                Assert.AreEqual(15, data.Result);
                Assert.AreEqual(4, man.ProcessedCounter);

            }).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void WriteJsonToFile()
        {
            Task.Run(async () =>
            {

                //create tree
                Initiator root = new Initiator();
                DecisionMultiplier child = new DecisionMultiplier()
                {
                    Paths = new List<DecisionPath>(),
                };
                child.Paths.Add(new DecisionPath()
                {
                    Value = 25,
                    NextStep = new OperationMultiplier()
                    {
                        NextStep = new Terminator()
                    }
                });
                child.Paths.Add(new DecisionPath()
                {
                    Value = 75,
                    NextStep = new OperationAdder()
                    {
                        NextStep = new Terminator()
                    }
                });
                root.NextStep = child;

                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(root, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented,
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto
                });
                
                File.WriteAllText(@"..\..\example.json", json);

                //create data
                TestData data = new TestData()
                {
                    A = 5,
                    B = 10
                };

                FlowManager man = new FlowManager();
                bool result = await man.Execute(root, data);

                //should go through to id 3 and therefore result = A + B
                Assert.IsTrue(result);
                Assert.AreEqual(15, data.Result);
                Assert.AreEqual(4, man.ProcessedCounter);

            }).GetAwaiter().GetResult();
        }


        [TestMethod]
        public void TestForError()
        {
            Task.Run(async () =>
            {
                //create tree
                Initiator root = new Initiator();
                root.NextStep = new OperationForcedError()
                {
                    NextStep = new Terminator()
                };

                //create data
                TestData data = new TestData()
                {
                    A = 5,
                    B = 10
                };

                FlowManager man = new FlowManager();
                bool result = await man.Execute(root, data);

                Assert.IsFalse(result);
                Assert.AreEqual(man.CurrentStep.Errors.Count, 1);
            }).GetAwaiter().GetResult();
        }
    }
}
