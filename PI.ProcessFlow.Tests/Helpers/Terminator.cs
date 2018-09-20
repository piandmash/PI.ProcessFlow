using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PI.ProcessFlow.Interfaces;
using PI.ProcessFlow.Models;

namespace PI.ProcessFlow.Tests.Helpers
{

    public class Terminator : IStep
    {
        private string _StepType = "Terminator";
        public string StepType
        {
            get { return _StepType;  }
        }

        public string Id { get; set; } = "0";

        public string Name { get; set; } = "Terminator";

        public string Description { get; set; } = "Terminator";

        public bool Processed { get; set; } = false;

        public int ProcessCount { get; set; } = 0;

        public List<string> Errors { get; set; }

        public bool ProcessError { get; set; }

        public IStep NextStep
        {
            get { return null; }
            set { var nothing = value; }
        }

        public string ProcssingItemData { get; set; }
        public List<object> DetailedErrors { get; set; } = new List<object>();
    }
}
