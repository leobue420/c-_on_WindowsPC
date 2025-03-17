using ReactiveUI;
using System;
using System.Diagnostics; // For Debug output
using System.Reactive.Linq;
using System.Windows.Input; // For ReactiveCommand threading
using System.Reactive.Concurrency; // For RxApp.MainThreadScheduler

namespace MyAvaloniaApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private int _counter;

    public int Counter
    {
        get => _counter;
        set => this.RaiseAndSetIfChanged(ref _counter, value);
    }  

    public ICommand IncrementCommand { get; }

    public MainWindowViewModel()
    {
        IncrementCommand = ReactiveCommand.Create(() =>
        {
            try
            {
                Counter++;
                Debug.WriteLine($"Counter incremented to: {Counter}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception during increment: {ex.Message}\n{ex.StackTrace}");
                throw; // Re-throw to see if it’s caught elsewhere
            }
        }, outputScheduler: RxApp.MainThreadScheduler); // Ensure UI thread
    }
}