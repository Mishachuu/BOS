using System;
using System.Runtime.InteropServices;

class Program
{
    // Функция для получения списка сессий
    [DllImport("Netapi32.dll", SetLastError = true)]
    public static extern int NetSessionEnum(
        string servername,
        string UncClientName,
        string username,
        int level,
        ref IntPtr bufptr,
        uint prefmaxlen,
        ref int entriesread,
        ref int totalentries,
        ref int resume_handle);

    // Функция для завершения сессии
    [DllImport("Netapi32.dll", SetLastError = true)]
    public static extern int NetSessionDel(
        string servername,
        string UncClientName,
        string username);

    static void Main()
    {
        // Получаем список всех активных сессий
        IntPtr bufPtr = IntPtr.Zero;
        int entriesRead = 0, totalEntries = 0, resumeHandle = 0;
        int result = NetSessionEnum(null, null, null, 10, ref bufPtr, uint.MaxValue, ref entriesRead, ref totalEntries, ref resumeHandle);

        if (result == 0 && entriesRead > 0)
        {
            // Код для обработки списка сессий
            Console.WriteLine($"{entriesRead} сессий найдено");
            // Здесь вывод информации о каждой сессии
        }
        else
        {
            Console.WriteLine("Ошибка при получении списка сессий");
        }

        // Пример завершения сессии по имени клиента и пользователя
        string clientName = "ClientName";
        string userName = "UserName";
        result = NetSessionDel(null, clientName, userName);

        if (result == 0)
        {
            Console.WriteLine($"Сессия для {userName} завершена.");
        }
        else
        {
            Console.WriteLine("Ошибка при завершении сессии.");
        }
    }
}
