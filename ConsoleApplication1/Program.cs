using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication3
{
    public static class Program
    {
        private static void Main()
        {
            ShowTags("https://www.yandex.ru/", "a");
            Console.ReadKey();
        }

        private static async void ShowTags(string url, string tag) //ищем теги <a></a>
        {
            // Загружем страницу 
            string data = await GetHtmlPageText(url);

            if (data != null)
            {
                // \<{0}.*?\> - открывающий тег
                // \<\/{0}\> - закрывающий тег
                // (?<data>.+?) - содержимое тега, записываем в группу tegData
                string tegText = string.Format(@"\<{0}.*?\>(?<data>.+?)\<\/{0}\>", tag.Trim());

                Regex reg = new Regex(tegText, RegexOptions.ExplicitCapture);
                MatchCollection matchColl = reg.Matches(data);

                foreach (Match matche in matchColl)
                {
                    Console.WriteLine(matche.Value);
                    Console.WriteLine("Title:");
                    Console.WriteLine(matche.Groups["data"].Value);
                    Console.WriteLine("______________ следуюая ссылка =>");
                }
            }
            else
            {
                Console.WriteLine("Возникла какая-то ошибка. Ваш запрос страницы: " + url);
            }
        }

        public static async Task<string> GetHtmlPageText(string url)
        {        
                var client = new WebClient();
                Stream data = await client.OpenReadTaskAsync(url);

                StreamReader reader = new StreamReader(data);

                return reader.ReadToEnd();
        }
    }
}