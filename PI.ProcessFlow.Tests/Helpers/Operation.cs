using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PI.ProcessFlow.Interfaces;
using PI.ProcessFlow.Models;

namespace PI.ProcessFlow.Tests.Helpers
{

    public class Operation : IStep, IProcess
    {
        private string _StepType = "Operation";
        public string StepType
        {
            get { return _StepType; }
        }

        public string Id { get; set; }

        public string Name { get; set; } = "Operation";

        public string Description { get; set; }
        
        public bool Processed { get; set; }

        public int ProcessCount { get; set; }

        public IStep NextStep { get; set; }

        public List<string> Errors { get; set; }
        
        public bool ProcessError { get; set; }

        public Operation()
        {
            Id = "0";
            Description = "Operation";
            Processed = false;
            ProcessCount = 0;
        }

        public virtual async Task ProcessAsync(object data)
        {
            await Task.Delay(10);
            return;
        }

        public string ProcessName { get; set; }

        public string ProcssingItemData { get; set; }
        public List<object> DetailedErrors { get; set; } = new List<object>();
    }
}
