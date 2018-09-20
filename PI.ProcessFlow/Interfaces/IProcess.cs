using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI.ProcessFlow.Interfaces
{

    public interface IProcess
    {
        /// <summary>
        /// Async processing task called by the flow manager
        /// </summary>
        /// <param name="data">The object data to process</param>
        /// <returns></returns>
        Task ProcessAsync(object data);

    }
    
}
