using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PI.ProcessFlow.Interfaces;
using PI.ProcessFlow.Models;

namespace PI.ProcessFlow
{
    /// <summary>
    /// Manages the execution of a Process Flow
    /// </summary>
    public class FlowManager
    {
        
        //store the Processed steps
        private List<IStep> _ProcessedSteps = new List<IStep>();
        
        /// <summary>
        /// A list of all the steps run in order
        /// </summary>
        public List<IStep> ProcessedSteps { 
            get { return _ProcessedSteps; }
        }

        private int _ProcessedCounter = 0;
        /// <summary>
        /// Provides a count of the process steps completed
        /// </summary>
        public int ProcessedCounter
        {
            get { return _ProcessedCounter; }
        }

        //store the data
        private object _Data = null;
        
        //store the current step
        private IStep _CurrentStep = null;

        //store the root step
        private IStep _RootStep = null;

        //get or set the KeepProcessedSteps
        private bool _KeepProcessedSteps = false;
        /// <summary>
        /// Flag to set if the process steps are kept
        /// </summary>
        public bool KeepProcessedSteps
        {
            get { return _KeepProcessedSteps; }
            set { _KeepProcessedSteps = value; }
        }
        
        /// <summary>
        /// Information of the item being processed
        /// </summary>
        public string ProcssingItemData { get; set; }

        /// <summary>
        /// The Current Step running
        /// </summary>
        public IStep CurrentStep
        {
            get { return _CurrentStep; }
        }

        /// <summary>
        /// Flag to say if the processing has an error
        /// </summary>
        public bool ProcessError { get; set; } = false;

        /// <summary>
        /// Stores a list of errors
        /// </summary>
        public List<string> Errors { get; set; } = new List<string>();

        /// <summary>
        /// Stores a list of detailed errors
        /// </summary>
        public List<object> DetailedErrors { get; set; } = new List<object>();

        /// <summary>
        /// Executes the process flow from the step sent
        /// This method will continue calling itself as it traverses the flow from the initial step sent
        /// </summary>
        /// <param name="step">The step to execute</param>
        /// <param name="data">The data to execute against</param>
        /// <returns>True if the flow is compelted, False if any step returns a ProcessError</returns>
        public async Task<bool> Execute<T>(IStep step, T data)
        {
            _Data = data;
            //reset Processed steps
            _ProcessedSteps = new List<IStep>();
            //reset the counter
            _ProcessedCounter = 0;
            //if the step is empty then just do the callback
            if (step == null)
            {
                return true;
            }
            //set the root step
            _RootStep = step;
            //call the first step to Process
            bool processError = await ProcessWrapper(step);
            //return
            return processError;
        }

        private void ProcessedStep(IStep step)
        {
            _ProcessedCounter += 1;
            step.Processed = true;
            step.ProcessCount = _ProcessedCounter;
            //might have to drop this?
            if (_KeepProcessedSteps) _ProcessedSteps.Add(step);
            return;
        }

        private async Task<bool> ProcessWrapper(IStep step)
        {
            //add the step to be Processed
            _CurrentStep = step;
            //set the ProcssingItemData
            if (_CurrentStep != null) _CurrentStep.ProcssingItemData = this.ProcssingItemData;
            //expected next step
            IStep NextStep = step.NextStep;
            //check for IProcess
            if (step is IProcess){
                //call the Process method
                await ((IProcess)step).ProcessAsync(_Data);
            }
            //check for IDecision and no process error discovered
            if(!step.ProcessError && step is IDecision){
                //call the decide method that sets the chosenPath
                DecisionPath chosenPath = await ((IDecision)step).DecideAsync(_Data);
                if(chosenPath != null){
                    //set the NextStep if required
                    if(chosenPath.NextStep != null){
                        NextStep = chosenPath.NextStep;
                    }
                    else if(!String.IsNullOrEmpty(chosenPath.NextStepId))
                    {
                        //discover next step by id
                        NextStep = FindStepById(_RootStep, chosenPath.NextStepId);
                    }
                }
            }
            //call the Processed step method        
            ProcessedStep(step);

            //add processing step errors
            if(step.Errors != null) foreach(string s in step.Errors) Errors.Add(s);
            //add processing step detailed errors
            if (step.DetailedErrors != null) foreach (object e in step.DetailedErrors) DetailedErrors.Add(e);

            //stop processing as process error set
            if (step.ProcessError)
            {
                ProcessError = true;
                return false;
            }

            //call the next step
            if(NextStep != null) {
                return await ProcessWrapper(NextStep);
            }
            //no more steps return true
            return true;
        }

        private IStep FindStepById(IStep step, string id)
        {
            if (step.Id == id) return step;
            if (step.NextStep != null)
            {
                return FindStepById(step, id);
            }
            return null;
        }
    }
}
