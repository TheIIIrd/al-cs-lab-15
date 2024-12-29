/*
Задание 1
Реализуйте аналог класса FileSystemWatcher без обращения к компонентам ОС,
то есть с простой проверкой состояния директории по таймеру.

Используйте паттерн «Наблюдатель»
*/

using System.Timers;

public interface IObserver
{
    void Update(string message);
}

public class Subject
{
    private List<IObserver> observers = new List<IObserver>();

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify(string message)
    {
        foreach (var observer in observers)
        {
            observer.Update(message);
        }
    }
}

public class DirectoryWatcher : Subject
{
    private string directoryPath;
    private System.Timers.Timer timer; // Явно указываем, что это System.Timers.Timer
    private DateTime lastCheckTime;

    public DirectoryWatcher(string path, double interval)
    {
        directoryPath = path;
        lastCheckTime = DateTime.Now;

        timer = new System.Timers.Timer(interval); // Явно указываем, что это System.Timers.Timer
        timer.Elapsed += CheckDirectory;
        timer.AutoReset = true;
        timer.Enabled = true;
    }

    private void CheckDirectory(object sender, ElapsedEventArgs e)
    {
        var lastWriteTime = Directory.GetLastWriteTime(directoryPath);
        if (lastWriteTime > lastCheckTime)
        {
            lastCheckTime = lastWriteTime;
            Notify($"Directory {directoryPath} has been modified.");
        }
    }
}

public class DirectoryObserver : IObserver
{
    public void Update(string message)
    {
        Console.WriteLine(message);
    }
}

class Program
{
    static void Main(string[] args)
    {
        // string path = @"/Path/To/Your/Directory";
        string path = @"/home/theiiird/BitLab/CreationLab/CodeStore/CsLab/Lab15/TestDirectory";
        double interval = 5000; // Интервал проверки в миллисекундах

        DirectoryWatcher watcher = new DirectoryWatcher(path, interval);
        DirectoryObserver observer = new DirectoryObserver();

        watcher.Attach(observer);

        Console.WriteLine("Press [Enter] to exit...");
        Console.ReadLine();
    }
}
