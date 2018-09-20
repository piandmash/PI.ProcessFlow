using System;
using System.Collections.Generic;
using System.Text;

namespace PI.ProcessFlow.Interfaces
{
    public interface IStep
    {
        /// <summary>
        /// The type name for the step
        /// </summary>
        string StepType { get; }

        /// <summary>
        /// The id of the step
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// The name of the step for administration
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Descirption of the step for administration purposes
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Information of the item being processed
        /// </summary>
        string ProcssingItemData { get; set; }

        /// <summary>
        /// Contains the next step in the process flow
        /// </summary>
        IStep NextStep { get; set; }

        /// <summary>
        /// Flag to set if the process has been completed
        /// </summary>
        bool Processed { get; set; }

        /// <summary>
        /// Counts the number of time the process has been run
        /// </summary>
        int ProcessCount { get; set; }

        /// <summary>
        /// Stores a list of errors
        /// </summary>
        List<string> Errors { get; set; }

        /// <summary>
        /// Stores a list of detailed errors
        /// </summary>
        List<object> DetailedErrors { get; set; }

        /// <summary>
        /// Stores a list of errors
        /// </summary>
        bool ProcessError { get; set; }

    }
}
