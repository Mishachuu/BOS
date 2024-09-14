using System;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("Netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
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

    [DllImport("Netapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int NetSessionDel(
        string servername,
        string UncClientName,
        string username);

    [DllImport("Netapi32.dll", SetLastError = true)]
    public static extern int NetApiBufferFree(IntPtr Buffer);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct SESSION_INFO_10
    {
        public string sesi10_cname;       // Имя компьютера клиента
        public string sesi10_username;    // Имя пользователя
        public uint sesi10_time;          // Время работы сессии
        public uint sesi10_idle_time;     // Время бездействия сессии
    }

    static void Main()
    {
        IntPtr bufPtr = IntPtr.Zero;
        int entriesRead = 0, totalEntries = 0, resumeHandle = 0;
        int result = NetSessionEnum(null, null, null, 10, ref bufPtr, uint.MaxValue, ref entriesRead, ref totalEntries, ref resumeHandle);

        if (result == 0 && entriesRead > 0)
        {
            Console.WriteLine($"{entriesRead} сессий найдено");
            IntPtr currentPtr = bufPtr;

            // Перебираем каждую сессию
            for (int i = 0; i < entriesRead; i++)
            {
                // Извлекаем информацию о сессии
                SESSION_INFO_10 sessionInfo = Marshal.PtrToStructure<SESSION_INFO_10>(currentPtr);

                // Выводим информацию о сессии: имя компьютера и имя пользователя
                Console.WriteLine($"Компьютер: {sessionInfo.sesi10_cname}, Пользователь: {sessionInfo.sesi10_username}, " +
                                  $"Время работы: {sessionInfo.sesi10_time}, Время простоя: {sessionInfo.sesi10_idle_time}");

                // Переход к следующей структуре
                currentPtr = IntPtr.Add(currentPtr, Marshal.SizeOf(typeof(SESSION_INFO_10)));
            }

            // Освобождаем память
            NetApiBufferFree(bufPtr);
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
                Console.WriteLine($"Ошибка при завершении сессии: {Marshal.GetLastWin32Error()}");
            }
        }
    }
}
