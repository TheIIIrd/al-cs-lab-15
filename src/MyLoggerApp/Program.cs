/*
Задание 2
Реализуйте класс MyLogger с возможностью записи в текстовый файл построчно,
в JSON – файл с сохранением разметки JSON (при желании так же в БД).

В процессе реализации используйте паттерн «Репозиторий».
*/

using Newtonsoft.Json;

// Интерфейс для логирования
public interface ILogger
{
    void Log(string message);
}

// Интерфейс репозитория для логов
public interface ILogRepository
{
    void SaveLog(string logEntry);
}

// Реализация репозитория для текстового файла
public class TextLogRepository : ILogRepository
{
    private string filePath;

    public TextLogRepository(string path)
    {
        filePath = path;
    }

    public void SaveLog(string logEntry)
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(logEntry);
        }
    }
}

// Реализация репозитория для JSON файла
public class JsonLogRepository : ILogRepository
{
    private string filePath;

    public JsonLogRepository(string path)
    {
        filePath = path;
    }

    public void SaveLog(string logEntry)
    {
        var logEntryObject = new { Message = logEntry, Timestamp = DateTime.Now };
        string json = JsonConvert.SerializeObject(logEntryObject, Formatting.Indented);

        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine(json);
        }
    }
}

// Класс MyLogger, использующий репозиторий
public class MyLogger : ILogger
{
    private List<ILogRepository> logRepositories = new List<ILogRepository>();

    public void AddRepository(ILogRepository repository)
    {
        logRepositories.Add(repository);
    }

    public void Log(string message)
    {
        foreach (var repository in logRepositories)
        {
            repository.SaveLog(message);
        }
    }
}

// Основной класс программы
class Program
{
    static void Main(string[] args)
    {
        MyLogger logger = new MyLogger();

        // Укажите пути к файлам для логирования
        string textLogPath = @"/home/theiiird/BitLab/CreationLab/CodeStore/CsLab/Lab15/TestDirectory/Log.txt";
        string jsonLogPath = @"/home/theiiird/BitLab/CreationLab/CodeStore/CsLab/Lab15/TestDirectory/Log.json";

        // Добавляем репозитории
        logger.AddRepository(new TextLogRepository(textLogPath));
        logger.AddRepository(new JsonLogRepository(jsonLogPath));

        // Записываем логи
        logger.Log("This is a log message.");
        logger.Log("Another log message.");

        Console.WriteLine("Logs have been written.");
    }
}
