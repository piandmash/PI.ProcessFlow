using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PI.ProcessFlow.Interfaces;

namespace PI.ProcessFlow.Models
{
    /// <summary>
    /// Class containing all the relevant data to determine which path the decision tree should take next.
    /// </summary>
    public class DecisionPath
    {
        /// <summary>
        /// The value to match the decision on
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Flag to advise if the path has been selected
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// A reference to the next step id to allow for jumping throughout the tree
        /// </summary>
        public string NextStepId { get; set; }

        /// <summary>
        /// The next step in the process flow, if set to null then the processor will use the NextStepId to find the next step
        /// </summary>
        public IStep NextStep { get; set; }
        
    }
}
