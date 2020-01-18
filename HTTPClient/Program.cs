using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HTTPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string vybor;
            bool isTrue = false;
            while (!isTrue)
            {
                Console.WriteLine("1. Авторизация");
                Console.WriteLine("2. Регистрация");
                vybor = Console.ReadLine();
                switch (vybor)
                {
                    case "1":
                        Console.Clear();
                        Auth();
                        break;
                    case "2":
                        Console.Clear();
                        SignUp();
                        break;
                    default:
                        break;
                }
            }
        }
        public static async Task Auth()
        {
            Console.WriteLine("Введите логин:");
            string login = Console.ReadLine();
            Console.WriteLine("Введите пароль:");
            string password = Console.ReadLine();
            Console.Clear();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://localhost/users/auth/");
            webRequest.Method = "POST";
            var user = new User()
            {
                Login = login,
                Password = password
            };
            var query = new Query()
            {
                QueryType = "AUTH"
            };
            var queryData = JsonSerializer.Serialize(query);
            var userData = JsonSerializer.Serialize(user);
            var queryUser = $"{queryData}*{userData}";
            var bytes = Encoding.UTF8.GetBytes(queryUser);
            using (var stream = webRequest.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }
            var response = await webRequest.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    Console.WriteLine(streamReader.ReadToEnd());
                }
            }
            response.Close();
        }
        public static async Task SignUp()
        {
            string login;
            string password;
            Console.WriteLine("Введите логин:");
            login = Console.ReadLine();
            Console.WriteLine("Введите пароль:");
            password = Console.ReadLine();
            Console.Clear();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://localhost/users/singup/");
            webRequest.Method = "POST";
            var user = new User()
            {
                Login = login,
                Password = password
            };
            var query = new Query()
            {
                QueryType = "SIGNUP"
            };
            var operationData = JsonSerializer.Serialize(query);
            var userData = JsonSerializer.Serialize(user);
            var operationAndUser = $"{operationData}*{userData}";
            var dataBytes = Encoding.UTF8.GetBytes(operationAndUser);
            using (var stream = webRequest.GetRequestStream())
            {
                stream.Write(dataBytes, 0, dataBytes.Length);
            }
            var response = await webRequest.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    Console.WriteLine(streamReader.ReadToEnd());
                }
            }
            response.Close();
        }
    }
}
