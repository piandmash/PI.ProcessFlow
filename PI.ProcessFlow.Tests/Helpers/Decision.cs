using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PI.ProcessFlow.Interfaces;
using PI.ProcessFlow.Models;

namespace PI.ProcessFlow.Tests.Helpers
{

    public class Decision : IStep, IProcess, IDecision
    {
        private string _StepType = "Decision";
        public string StepType
        {
            get { return _StepType;  }
        }

        public string Id { get; set; }


        public string Name { get; set; } = "Decision";

        public string Description { get; set; }

        public bool Processed { get; set; }

        public int ProcessCount { get; set; }

        public IStep NextStep { get; set; }

        public List<string> Errors { get; set; }

        public bool ProcessError { get; set; }

        public Decision()
        {
            Id = "0";
            Description = "Decision";
            Processed = false;
            ProcessCount = 0;
        }

        public virtual async Task ProcessAsync(object data)
        {
            await Task.Delay(10);
            return;
        }

        public string ProcessName { get; set; }

        public virtual async Task<DecisionPath> DecideAsync(object data)
        {
            await Task.Delay(10);
            return null;
        }

        public string DecideName { get; set; }
        
        public List<DecisionPath> Paths { get; set; }

        public string ProcssingItemData { get; set; }

        public List<object> DetailedErrors { get; set; } = new List<object>();
    }
}
