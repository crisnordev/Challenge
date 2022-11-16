namespace courseappchallenge.ViewModels;

public class ErrorResultViewModel : ErrorViewModel
{
    public ErrorResultViewModel(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public ErrorResultViewModel(IList<string> errors)
    {
        Errors = errors;
    }

    public ErrorResultViewModel(string errorMessage, IList<string> errors)
    {
        ErrorMessage = errorMessage;
        Errors = errors;
    }

    public ErrorResultViewModel(string errorMessage, string error)
    {
        ErrorMessage = errorMessage;
        Errors.Add(error);
    }

    public string? ErrorMessage { get; set; } = string.Empty;

    public IList<string>? Errors { get; set; } = new List<string>();
}