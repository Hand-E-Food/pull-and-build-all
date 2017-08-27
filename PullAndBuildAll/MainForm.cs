using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PullAndBuildAll
{
    public partial class MainForm : Form
    {
        private Configuration _configuration;
        private GitService _gitService;
        private NuGetService _nugetService;
        private BuildService _buildService;
        private Dictionary<RepositoryHash, PullController > _pullControllers  = new Dictionary<RepositoryHash, PullController >();
        private Dictionary<RepositoryHash, NuGetController> _nugetControllers = new Dictionary<RepositoryHash, NuGetController>();
        private Dictionary<PlatformHash  , BuildController> _buildControllers = new Dictionary<PlatformHash  , BuildController>();

        private IEnumerable<IController> AllControllers
        {
            get
            {
                return _pullControllers.Values
                    .Concat<IController>(_nugetControllers.Values)
                    .Concat(_buildControllers.Values);
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var msBuildPath = ConfigurationManager.AppSettings["MsBuildPath"];
            var msBuildTimeOut = TimeSpan.Parse(ConfigurationManager.AppSettings["MsBuildTimeOut"]);

            _configuration = LoadConfigration();
            _gitService = new GitService();
            _nugetService = new NuGetService();
            _buildService = new BuildService(msBuildPath, msBuildTimeOut);
            CreateControllers();
            LinkDependencies();
            CountDependents();
            CreateControls();
        }

        private static Configuration LoadConfigration()
        {
            var json = File.ReadAllText("repos.json");
            return JsonConvert.DeserializeObject<Configuration>(json);
        }

        private void CreateControllers()
        {
            foreach (var repository in _configuration.Repositories)
            {
                var repositoryDirectory = repository.Directory ?? Path.Combine(_configuration.RootDirectory, repository.Name);

                var pullController = new PullController(_gitService, repositoryDirectory, repository.Name, repository.Dependencies);
                _pullControllers.Add(pullController, pullController);

                if (repository.Platforms.Length > 0)
                {
                    var nugetController = new NuGetController(_nugetService, repositoryDirectory, repository.Name, repository.Dependencies);
                    _nugetControllers.Add(nugetController, nugetController);

                    foreach (var platform in repository.Platforms)
                    {
                        var buildController = new BuildController(_buildService, repositoryDirectory, repository.Name, platform, repository.Dependencies);
                        _buildControllers.Add(buildController, buildController);
                    }
                }
            }
        }

        private void LinkDependencies()
        {
            foreach (var pullController in _pullControllers.Values)
            {
                pullController.DependencyTaskControls = pullController.DependencyHashes
                    .Select(hash => _pullControllers[hash].Control)
                    .ToArray();
            }

            foreach (var nugetController in _nugetControllers.Values)
            {
                nugetController.DependencyTaskControls = new[] { _pullControllers[nugetController].Control };
            }

            foreach (var buildController in _buildControllers.Values)
            {
                var dependencyControls = new List<TaskControl> { _nugetControllers[new RepositoryHash(buildController.Name)].Control };
                foreach (var hash in buildController.DependencyHashes)
                {
                    if (_buildControllers.TryGetValue(hash, out BuildController controller))
                        dependencyControls.Add(controller.Control);
                    else
                        dependencyControls.Add(_pullControllers[new RepositoryHash(hash.Name)].Control);
                }

                buildController.DependencyTaskControls = dependencyControls.ToArray();
            }
        }

        private void CountDependents()
        {
            var allControllers = AllControllers.ToList();

            foreach (var controller in allControllers)
            {
                var dependencies =
                    Flatten(controller, x => x.DependencyTaskControls
                        .Select(dependency => allControllers
                            .First(parent => parent.Control == dependency)
                        )
                    );

                foreach (var dependency in dependencies)
                    dependency.Dependents++;
            }
        }

        private void CreateControls()
        {
            var rowNumber = 1;
            var pullControllers = _pullControllers.Values
                .OrderBy(pullController => pullController.Name)
                .ToList();

            outputPanel.SuspendLayout();
            foreach (var pullController in pullControllers)
            {
                var buildControllers = _buildControllers.Values
                    .Where(buildController => buildController.Name == pullController.Name)
                    .OrderBy(buildController => buildController.Platform)
                    .ToList();

                var nameLabel = new Label {
                    Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                    AutoSize = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(3),
                    Text = pullController.Name,
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                outputPanel.Controls.Add(nameLabel, 0, rowNumber);
                outputPanel.Controls.Add(pullController.Control, 1, rowNumber);
                if (buildControllers.Count > 0)
                {
                    outputPanel.SetRowSpan(nameLabel, buildControllers.Count);
                    outputPanel.SetRowSpan(pullController.Control, buildControllers.Count);

                    var nugetController = _nugetControllers[pullController];
                    outputPanel.Controls.Add(nugetController.Control, 2, rowNumber);
                    outputPanel.SetRowSpan(nugetController.Control, buildControllers.Count);

                    foreach (var buildController in buildControllers)
                    {
                        var platformLabel = new Label {
                            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                            AutoSize = true,
                            BorderStyle = BorderStyle.FixedSingle,
                            Margin = new Padding(3),
                            Text = buildController.Platform,
                            TextAlign = ContentAlignment.MiddleCenter,
                        };
                        outputPanel.Controls.Add(platformLabel, 3, rowNumber);
                        outputPanel.Controls.Add(buildController.Control, 4, rowNumber);
                        rowNumber++;
                    }
                }
                else
                {
                    rowNumber++;
                }
            }

            var rowHeight = outputPanel.ColumnStyles[1].Width;
            while (outputPanel.RowStyles.Count < rowNumber)
                outputPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));
            outputPanel.RowCount = rowNumber;
            outputPanel.ResumeLayout(true);

            ClientSize = new Size(
                outputPanel.Size.Width + outputPanel.Margin.Horizontal,
                outputPanel.Size.Height + outputPanel.Margin.Vertical);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            var allControllers = AllControllers
                .OrderByDescending(controller => controller.Dependents)
                .ThenBy(controller => controller.HashString)
                .ToList();

            while (true)
            {
                var nextController = allControllers
                    .FirstOrDefault(controller =>
                        controller.Control.Task == null
                        && controller.DependencyTaskControls.All(control => control.Task != null)
                    );

                if (nextController == null)
                    break;

                nextController.ScheduleTask();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var allControls = AllControllers
                .Select(controller => controller.Control)
                .ToList();

            foreach (var control in allControls)
                control.Cancel();

            Task[] allTasks = allControls
                .Select(control => control.Task)
                .ToArray();

            if (e.CloseReason == CloseReason.UserClosing && !allTasks.All(task => task.IsCompleted))
            {
                e.Cancel = true;
                Task.Factory.ContinueWhenAll(allTasks, tasks => Invoke((Action)Close));
            }
        }

        private IEnumerable<T> Flatten<T>(T parent, Func<T, IEnumerable<T>> selector)
        {
            var results = new List<T> { parent };
            var descendants = selector(parent)
                .SelectMany(child => Flatten(child, selector));
            results.AddRange(descendants);
            return results;
        }
    }
}
