namespace IBIZWebApp.Services;

public class ToastService : IToastService
{
    public event Func<string, string, Task>? OnShow;

    public async Task ShowSuccessAsync(string title, string message = "")
    {
        if (OnShow != null)
            await OnShow.Invoke(title, message);
    }

    public async Task ShowErrorAsync(string title, string message = "")
    {
        if (OnShow != null)
            await OnShow.Invoke(title, message);
    }

    public async Task ShowWarningAsync(string title, string message = "")
    {
        if (OnShow != null)
            await OnShow.Invoke(title, message);
    }

    public async Task ShowInfoAsync(string title, string message = "")
    {
        if (OnShow != null)
            await OnShow.Invoke(title, message);
    }
}
