using System.Reflection.Metadata.Ecma335;
using Microsoft.Win32.SafeHandles;

namespace Reading_csv;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}

public struct AccessRecord
{
    public string DisplayName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string WorkEmail { get; set; }
    public bool CloudLifecycleState { get; set; }
    public string IdentityID { get; set; }
    public bool IsManager { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }
    public string Uid { get; set; }
    public string AccessType { get; set; }
    public string AccessSourceName { get; set; }
    public string AccessDisplayName { get; set; }
    public string AccessDescription { get; set; }
}

public class CsvReader
{
    public static List<AccessRecord> Read(string filePath)
    {
        List<AccessRecord> records = new List<AccessRecord>();
        using (StreamReader s = new StreamReader(filePath))
        {
            string headerline = s.ReadLine();

            while (!s.EndOfStream)
            {
                string line = s.ReadLine();
                string[] values = line.Split(",");
                AccessRecord record = new AccessRecord
                {
                    DisplayName = values[0],
                    FirstName = values[1],
                    LastName = values[2],
                    WorkEmail = values[3],
                    IdentityID = values[5],
                    Department = values[7],
                    JobTitle = values[8],
                    Uid = values[9],
                    AccessType = values[10],
                    AccessSourceName = values[11],
                    AccessDisplayName = values[12],
                    AccessDescription = values[13],

                };
                records.Add(record);
            }
        }
        return records;
    }
}