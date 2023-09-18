using Nuke.Common;
using Nuke.Common.Tooling;

class Build : NukeBuild
{
    [PathExecutable(name: "docker-compose")]
    public readonly Tool DockerCompose;

    [PathExecutable(name: "samlocal")]
    public readonly Tool SamLocal;

    public static int Main () => Execute<Build>(x => x.SamLocalBuild);

    [Parameter()]
    public string SlackWebHook;

    public bool IsRunning()
    {
        var output = DockerCompose("ps --status running --quiet");
        return output.Count > 0;
    }

    Target StartEnv => _ => _
    .OnlyWhenDynamic(() => !IsRunning())
    .Executes(() =>
    {
        DockerCompose("up -d");
    });

    Target StopEnv => _ => _
        .Executes(() =>
        {
            DockerCompose("down");
        });

    Target SamLocalBuild => _ => _
        .Executes(() =>
        {
            SamLocal("build");
        });

    Target SamLocalDeploy => _ => _
        .Requires(() => SlackWebHook)
        .DependsOn(SamLocalBuild)
        .DependsOn(StartEnv)
        .Executes(() =>
        {
            SamLocal($"deploy --no-confirm-changeset --disable-rollback --resolve-s3 --s3-prefix sns2slack --stack-name sns2slack --region us-east-1  --capabilities CAPABILITY_IAM --parameter-overrides SlackWebHook={SlackWebHook}");
        });
}
