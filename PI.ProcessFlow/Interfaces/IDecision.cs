using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PI.ProcessFlow.Models;

namespace PI.ProcessFlow.Interfaces
{

    public interface IDecision
    {
        /// <summary>
        /// Async decision task called by the flow manager
        /// </summary>
        /// <param name="data">The object data to process</param>
        /// <returns>The matching decision path</returns>
        Task<DecisionPath> DecideAsync(object data);
        
        /// <summary>
        /// The name of the process for administration
        /// </summary>
        string DecideName { get; set; }

        /// <summary>
        /// A list of Decision Paths to help target the next step
        /// </summary>
        List<DecisionPath> Paths { get; set; }
    }
}
