using PullAndBuildAll.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PullAndBuildAll
{
    [System.Diagnostics.DebuggerDisplay(nameof(TaskControl) + " {Name}")]
    public partial class TaskControl : UserControl
    {

        /// <summary>
        /// The <see cref="Image"/> to display for a particular <see cref="TaskStatus"/>.
        /// </summary>
        private static readonly Dictionary<TaskStatus, Image> TaskStatusImages = new Dictionary<TaskStatus, Image>
        {
            { TaskStatus.Created        , Resources.WaitingImage   },
            { TaskStatus.Running        , Resources.RunningImage   },
            { TaskStatus.RanToCompletion, Resources.CompletedImage },
            { TaskStatus.Faulted        , Resources.FaultedImage   },
            { TaskStatus.Canceled       , Resources.CanceledImage  },
        };

        private CancellationTokenSource _cancellationSource = new CancellationTokenSource();

        /// <summary>
        /// Gets this task's <see cref="CancellationToken"/>.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CancellationToken CancellationToken => _cancellationSource.Token;

        /// <summary>
        /// The exception causing this task to fault.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Exception Exception { get; private set; }

        /// <summary>
        /// The logged information.
        /// </summary>
        public string Log { get; set; } = "Waiting to start...";

        /// <summary>
        /// Gets or sets this task's status.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TaskStatus Status
        {
            get => _Status;
            private set
            {
                _Status = value;
                SetPictureBoxImage(TaskStatusImages[value]);
            }
        }
        private TaskStatus _Status = TaskStatus.Created;

        /// <summary>
        /// The <see cref="Task"/> executing the code.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Task Task { get; private set; }

        /// <summary>
        /// The task factory that schedules the task.
        /// </summary>
        public TaskFactory TaskFactory { get; set; }

        public TaskControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Cancels this task.
        /// </summary>
        public CancellationToken Cancel()
        {
            _cancellationSource.Cancel();
            return CancellationToken;
        }

        /// <summary>
        /// Sets the <see cref="Task"/> upon which to report.
        /// </summary>
        /// <param name="task">The task upon which to report.</param>
        public void ContinueWhenAll(Task[] tasks, Action<Task[]> continuationAction)
        {
            if (Task != null && !Task.IsCompleted)
                throw new InvalidOperationException("The previous task is not completed.");

            Log = "Waiting to start...";
            Status = TaskStatus.Created;
            Task = tasks.Length == 0
                ? TaskFactory.StartNew(() => WrapTask(tasks, continuationAction))
                : TaskFactory.ContinueWhenAll(tasks, t => WrapTask(t, continuationAction));
        }

        /// <summary>
        /// Sets the picture box's image in a thread-safe manner.
        /// </summary>
        /// <param name="image">The image to set.</param>
        private void SetPictureBoxImage(Image image)
        {
            if (InvokeRequired)
                Invoke((Action<Image>)SetPictureBoxImage, new[] { image });

            lock (image)
            {
                pictureBox.Image = image;
            }
        }

        /// <summary>
        /// Executes the <paramref name="continuationAction"/> while displying the correct status.
        /// </summary>
        /// <param name="tasks">The tasks that this tasks continues after.</param>
        /// <param name="continuationAction">The action to execute after the <paramref name="tasks"/>.</param>
        private void WrapTask(Task[] tasks, Action<Task[]> continuationAction)
        {
            try
            {
                Log = "Executing...";
                Status = TaskStatus.Running;
                continuationAction(tasks);
                Status = TaskStatus.RanToCompletion;
            }
            catch (OperationCanceledException ex)
            {
                Status = TaskStatus.Canceled;
                Exception = ex;
                Log = "A dependency failed to complete.";
                throw;
            }
            catch (Exception ex)
            {
                Status = TaskStatus.Faulted;
                Exception = ex;
                Log = ex.ToString();
                throw;
            }
        }

        /// <summary>
        /// Displays the <see cref="Log"/>.
        /// </summary>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            var logForm = new LogForm {
                Log = Log,
            };
            logForm.Show();
        }
    }
}
