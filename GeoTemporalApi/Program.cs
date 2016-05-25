using System;
using Nancy.Hosting.Self;

namespace GeoTemporalApi
{
    class Program
    {
        static void Main()
        {
            const string uri = "http://localhost:3579";
            using (var host = GetHost(uri))
            {
                host.Start();
                Console.WriteLine("Your application is running on " + uri);
                Console.WriteLine("Press any [Enter] to close the host.");
                Console.ReadLine();
            }
        }

        private static NancyHost GetHost(string uri)
        {
            return
                new NancyHost(
                    new HostConfiguration { UrlReservations = new UrlReservations { CreateAutomatically = true } },
                    new Uri(uri));
        }
    }
}
