namespace TypeGen.Cli.Ui;

internal class ActionResult
{
    public bool IsSuccess { get; private set; }

    private ActionResult()
    {
    }

    public static ActionResult Success() => new() { IsSuccess = true };
    public static ActionResult Failure() => new() { IsSuccess = false };
}