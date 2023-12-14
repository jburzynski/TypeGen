using System;
using System.Collections.Generic;
using System.Linq;
using TypeGen.Core.Extensions;

namespace TypeGen.Cli.Ui.Validation;

internal class ValidationResult
{
    public bool IsSuccess { get; }
    public IReadOnlyCollection<string> Messages { get; }

    public ValidationResult(IEnumerable<string> messages)
    {
        var messagesList = messages.ToList();
        
        if (messagesList.IsNullOrEmpty())
        {
            IsSuccess = true;
            Messages = Array.Empty<string>();
        }
        else
        {
            IsSuccess = false;
            Messages = messagesList.ToList();
        }
    }

    public void Match(Action onSuccess, Action<IReadOnlyCollection<string>> onFailure)
    {
        if (IsSuccess)
            onSuccess();
        else
            onFailure(Messages);
    }
}