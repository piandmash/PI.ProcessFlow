using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PI.ProcessFlow.Interfaces;
using PI.ProcessFlow.Models;

namespace PI.ProcessFlow.Tests.Helpers
{
    public interface IData
    {
        int A { get; set; }
        int B { get; set; }
        double Result { get; set; }
    }

    public class TestData : IData
    {
        public int A { get; set; }
        public int B { get; set; }
        public double Result { get; set; }
    }

    public static class Constants
    {        
        public static string Tree = "{\"shapeType\":\"Entry\",\"id\":0,\"description\":\"End\",\"log\":0,\"processed\":false,\"processCount\":0,\"nextShape\":{\"$type\":\"UnitTest.Helpers.DecisionMultiplier, UnitTest\",\"shapeType\":\"Decision\",\"id\":1,\"description\":\"DecisionMultiplier\",\"log\":0,\"processed\":false,\"processCount\":0,\"nextShape\":null,\"processName\":null,\"decideName\":null,\"paths\":[{\"value\":25,\"selected\":false,\"nextShapeId\":0,\"nextShape\":{\"$type\":\"UnitTest.Helpers.OperationMultiplier, UnitTest\",\"shapeType\":\"Operation\",\"id\":2,\"description\":\"OperationMultiplier\",\"log\":0,\"processed\":false,\"processCount\":0,\"nextShape\":{\"$type\":\"PI.DecisionTree.Shapes.Terminator, DecisionTree\",\"shapeType\":\"Terminator\",\"id\":0,\"description\":\"End\",\"log\":0,\"processed\":false,\"processCount\":0,\"nextShape\":null},\"processName\":null}},{\"value\":75,\"selected\":false,\"nextShapeId\":0,\"nextShape\":{\"$type\":\"UnitTest.Helpers.OperationAdder, UnitTest\",\"shapeType\":\"Operation\",\"id\":3,\"description\":\"OperationAdder\",\"log\":0,\"processed\":false,\"processCount\":0,\"nextShape\":{\"$type\":\"PI.DecisionTree.Shapes.Terminator, DecisionTree\",\"shapeType\":\"Terminator\",\"id\":0,\"description\":\"End\",\"log\":0,\"processed\":false,\"processCount\":0,\"nextShape\":null},\"processName\":null}}]}}";

    }

    public class OperationAdder : Operation
    {
        public OperationAdder()
        {
            Description = "OperationAdder";
        }

        public override async Task ProcessAsync(object data)
        {
            if (data is IData)
            {
                IData d = (IData)data;
                d.Result = d.A + d.B;
            }
            return;
        }
    }

    public class OperationMultiplier : Operation
    {
        public OperationMultiplier()
        {
            Description = "OperationMultiplier";
        }

        public override async Task ProcessAsync(object data)
        {
            if (data is IData)
            {
                IData d = (IData)data;
                d.Result = d.A * d.B;
            }
            return;
        }
    }

    public class DecisionMultiplier : Decision
    {
        public DecisionMultiplier()
        {
            Description = "DecisionMultiplier";
        }

        public override async Task ProcessAsync(object data)
        {
            if (data is IData)
            {
                IData d = (IData)data;
                d.Result = d.A * d.B;
            }
            return;
        }

        public override async Task<DecisionPath> DecideAsync(object data)
        {
            if (data is IData)
            {
                IData d = (IData)data;
                foreach (DecisionPath path in this.Paths)
                {
                    if (path.Value.GetType() == typeof(int) && d.Result < (int)path.Value)
                    {
                        return path;
                    }
                }
                if (this.Paths.Count > 0) return this.Paths.Last();
            }
            return null;
        }
    }


    public class OperationForcedError : Operation
    {
        public OperationForcedError()
        {
            Description = "OperationForcedError";
        }

        public override async Task ProcessAsync(object data)
        {
            Errors = new List<string>();
            Errors.Add("Test Error");
            ProcessError = true;
            return;
        }
    }
}
