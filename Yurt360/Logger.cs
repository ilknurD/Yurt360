using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Net;

public class Logger
{
    private string connectionString = "server=DESKTOP-57F0A7E\\SQLEXPRESS;database=Yurt360;integrated security=True";

    public void Log(string logLevel, string message)
    {
        string ipAddress = GetIpAddress();
        string macAddress = GetMacAddress();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "INSERT INTO Logs (LogLevel, Message, LogDate, IpAddress, MacAddress) VALUES (@LogLevel, @Message, @LogDate, @IpAddress, @MacAddress)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LogLevel", logLevel);
                command.Parameters.AddWithValue("@Message", message);
                command.Parameters.AddWithValue("@LogDate", DateTime.Now);
                command.Parameters.AddWithValue("@IpAddress", ipAddress);
                command.Parameters.AddWithValue("@MacAddress", macAddress);

                command.ExecuteNonQuery();
            }
        }
    }

    public static string GetIpAddress()
    {
        string hostName = Dns.GetHostName();
        string ipAddress = Dns.GetHostEntry(hostName)
                              .AddressList
                              .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                              ?.ToString();

        return ipAddress ?? "IP adresi bulunamadı";
    }

    public static string GetMacAddress()
    {
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                                                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                                                .FirstOrDefault();

        if (networkInterfaces != null)
        {
            return string.Join(":", networkInterfaces.GetPhysicalAddress().GetAddressBytes().Select(b => b.ToString("X2")));
        }

        return "MAC adresi bulunamadı";
    }
}

