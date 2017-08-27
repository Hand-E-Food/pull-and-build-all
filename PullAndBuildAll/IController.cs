using System.Collections.Generic;
using System.Threading.Tasks;

namespace PullAndBuildAll
{
    internal interface IController : IHash
    {
        TaskControl Control { get; }

        TaskControl[] DependencyTaskControls { get; }

        int Dependents { get; set; }

        void ScheduleTask();
    }
}