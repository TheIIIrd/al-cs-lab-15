/*
Задание 3
Реализуйте класс SingleRandomizer, возвращающий следующее число ГСЧ, используя паттерн «Одиночка».

Предусмотрите возможность работы с классом из разных потоков.
*/

public class SingleRandomizer
{
    private static readonly Lazy<SingleRandomizer> instance = new Lazy<SingleRandomizer>(() => new SingleRandomizer());
    private static readonly Random random = new Random();
    private static readonly object lockObject = new object();

    // Приватный конструктор для предотвращения создания экземпляров
    private SingleRandomizer() { }

    // Публичный метод для получения экземпляра
    public static SingleRandomizer Instance => instance.Value;

    // Метод для получения следующего случайного числа
    public int GetNextRandomNumber(int min, int max)
    {
        lock (lockObject)
        {
            return random.Next(min, max);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Создаем несколько задач, которые будут использовать SingleRandomizer
        Task[] tasks = new Task[10];

        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                int randomNumber = SingleRandomizer.Instance.GetNextRandomNumber(1, 100);
                Console.WriteLine($"Random Number: {randomNumber}");
            });
        }

        // Ожидаем завершения всех задач
        Task.WaitAll(tasks);

        Console.WriteLine("All random numbers have been generated.");
    }
}
