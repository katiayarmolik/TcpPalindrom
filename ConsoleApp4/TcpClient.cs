using System.Net.Sockets;
using System.Text;

// Клиент для отправки сообщений на сервер
TcpClient client = new TcpClient();

// Присоединяемся к серверу
client.Connect("127.0.0.1", 5005);

// Просим пользователя ввести сообщение
Console.WriteLine("Input message: ");
string requestMessage = Console.ReadLine();

// Формируем байты для отправки и отправляем на сервер
byte[] request = Encoding.UTF8.GetBytes(requestMessage, 0, requestMessage.Length);
client.GetStream().Write(request, 0, request.Length);

client.GetStream().Close();
client.Close();

