using System.Net;
using System.Net.Sockets;
using System.Text;

// Запускаем прослушку по любому адресу компьютера по порту 5005
TcpListener listener = new TcpListener(IPAddress.Any, 5005);
listener.Start();

Console.WriteLine("TCP server started...");

// Бесконечный цикл ожидания входящих запросов и обработки их асинхронно
while (true)
{
    TcpClient client = await listener.AcceptTcpClientAsync();
    Guid clientId = Guid.NewGuid();

    // Асинхронная обработка подключения клиента
    _ = HandleClientAsync(client, clientId);
}

// Метод для обработки каждого клиента
async Task HandleClientAsync(TcpClient client, Guid clientId)
{
    Console.WriteLine($"Client {clientId} connected!");

    using NetworkStream stream = client.GetStream();
    byte[] buffer = new byte[1024];
    int bytesRead;

    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
    {
        // Получаем входящее сообщение
        string requestMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
        Console.WriteLine($"Request from {clientId}: {requestMessage}");

        // Проверка на палиндромность
        bool isPalindrome = IsPalindrome(requestMessage);
        string responseMessage = isPalindrome ? $"YES {requestMessage}" : $"NO {requestMessage}";

        // Подготовка и отправка ответа клиенту
        byte[] response = Encoding.UTF8.GetBytes(responseMessage);
        await stream.WriteAsync(response, 0, response.Length);
    }

    // Завершение обработки клиента
    Console.WriteLine($"Client {clientId} disconnected.");
}

// Метод проверки, является ли строка палиндромом
bool IsPalindrome(string text)
{
    int length = text.Length;
    for (int i = 0; i < length / 2; i++)
    {
        if (text[i] != text[length - i - 1])
            return false;
    }
    return true;
}
