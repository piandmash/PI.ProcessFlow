using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PI.ProcessFlow.Interfaces;
using PI.ProcessFlow.Models;

namespace PI.ProcessFlow.Tests.Helpers
{

    public class Initiator : IStep
    {
        private string _StepType = "Initiator";
        public string StepType
        {
            get { return _StepType;  }
        }

        public string Id { get; set; } = "0";

        public string Name { get; set; } = "Initiator";

        public string Description { get; set; } = "Initiator";

        public bool Processed { get; set; } = false;

        public int ProcessCount { get; set; } = 0;

        public IStep NextStep { get; set; }

        public List<string> Errors { get; set; }

        public bool ProcessError { get; set; }

        public string ProcssingItemData { get; set; }
        public List<object> DetailedErrors { get; set; } = new List<object>();
    }
}
