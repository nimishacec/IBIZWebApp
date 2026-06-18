namespace IBIZWebApp.Services;
public interface IToastService
{
    event Func<string, string, Task> OnShow;

    Task ShowSuccessAsync(string title, string message = "");
    Task ShowErrorAsync(string title, string message = "");
    Task ShowWarningAsync(string title, string message = "");
    Task ShowInfoAsync(string title, string message = "");
}
