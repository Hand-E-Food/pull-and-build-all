using CredentialManagement;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;

namespace PullAndBuildAll
{
    public class GitService
    {

        /// <summary>
        /// Pulls the specified repository.
        /// </summary>
        /// <param name="repositoryDirectory">The repository's directory.</param>
        /// <returns>The result of the operation.</returns>
        public string PullRepository(string repositoryDirectory)
        {
            using (Repository repository = new Repository(repositoryDirectory))
            {
                string userName = repository.Config.Get<string>("user.name").Value;
                string userEmail = repository.Config.Get<string>("user.email").Value;
                Signature merger = new Signature(userName, userEmail, DateTimeOffset.Now);

                PullOptions options = new PullOptions {
                    FetchOptions = new FetchOptions {
                        CredentialsProvider = new CredentialsHandler(GetCredentials)
                    }
                };

                MergeResult mergeResult = Commands.Pull(repository, merger, options);

                var localBranch = repository.Head.FriendlyName;
                var originBranch = repository.Head.RemoteName;
                switch (mergeResult.Status)
                {
                    case MergeStatus.UpToDate:
                        return $"{localBranch} was up to date.";
                    case MergeStatus.FastForward:
                        return $"{localBranch} was fast-forwarded to {originBranch}.";
                    case MergeStatus.NonFastForward:
                        return $"{localBranch} was merged with {originBranch}.";
                    case MergeStatus.Conflicts:
                        throw new MergeConflictException($"{localBranch} has conflicts with {originBranch}.");
                    default:
                        throw new InvalidCastException();
                }
            }
        }

        private Credentials GetCredentials(string url, string usernameFromUrl, SupportedCredentialTypes types)
        {
            var uri = new Uri(url);
            var target = $"git:{uri.Scheme}://{uri.Host}";
            var credential = new Credential { Target = target };
            if (!credential.Load())
                throw new ArgumentException($"No credential was found for \"{target}\"");

            return new UsernamePasswordCredentials { Username = credential.Username, Password = credential.Password };
        }
    }
}
