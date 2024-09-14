using System;
using System.Runtime.InteropServices;

class Program
{
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

    [DllImport("Netapi32.dll", SetLastError = true)]
    public static extern int NetSessionDel(
        string servername,
        string UncClientName,
        string username);

    static void Main()
    {
        IntPtr bufPtr = IntPtr.Zero;
        int entriesRead = 0, totalEntries = 0, resumeHandle = 0;
        int result = NetSessionEnum(null, null, null, 10, ref bufPtr, uint.MaxValue, ref entriesRead, ref totalEntries, ref resumeHandle);

        if (result == 0 && entriesRead > 0)
        {
            Console.WriteLine($"{entriesRead} сессий найдено");
        }
        else
        {
            Console.WriteLine("Сессии не найдены");
        }
        if (entriesRead > 0)
        {
            Console.WriteLine("Введите имя компьютера для завершения сессии:");
            string clientName = Console.ReadLine();
            Console.WriteLine("Введите имя пользователя для завершения сессии:");
            string userName = Console.ReadLine();

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
}
