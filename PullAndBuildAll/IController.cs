using System.Threading.Tasks;

namespace PullAndBuildAll
{
    internal interface IController
    {
        TaskControl Control { get; }

        TaskControl[] DependencyControls { get; }

        int Dependents { get; set; }

        IHash Hash { get; }

        void ScheduleTask(Task[] dependencyTasks = null);
    }
}