using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Win32.SafeHandles;

namespace Reading_csv;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "Francis Tuttle Identities_Basic.csv";
        List<AccessRecord> records = CsvReader.Read(filePath);
        Console.WriteLine("total records: " + records.Count);

        var p1 = from record in records
                 where !record.CloudLifecycleState
                 select record;
        Console.WriteLine("inactive records: " + p1.Count());

        var p2 = (from record in records
                  where !record.CloudLifecycleState
                  select record.DisplayName)
                 .Distinct()
                 .OrderBy(name => name);

        foreach (var name in p2)
        {
            Console.WriteLine(name);
        }
        var p3 = from record in p1
                 group record by record.DisplayName;

        foreach (var group in p3)
        {
            Console.WriteLine(group.Key);

            foreach (var record in group)
            {
                if (!string.IsNullOrEmpty(record.AccessSourceName) && !string.IsNullOrEmpty(record.AccessDisplayName))
                { Console.WriteLine(" " + record.AccessSourceName + "--" + record.AccessDisplayName); }

            }
        }
        var p4 = (from record in records
                  select record.Department)
        .Distinct()
        .OrderBy(name => name);
        foreach (var dept in p4)
        {
            if (!string.IsNullOrEmpty(dept))
            {
                Console.WriteLine(dept);
            }
        }
        var p5 = from record in records
                 group record by record.Department into deptgroup
                 select new
                 {
                     Department = deptgroup.Key,
                    DeptCount = deptgroup
                     .Select(r => r.DisplayName)
                     .Distinct()
                     .Count()
        };
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
                bool cloud = values[4] == "active";
                bool manager = values[6] == "TRUE";
                AccessRecord record = new AccessRecord
                {
                    DisplayName = values[0],
                    FirstName = values[1],
                    LastName = values[2],
                    WorkEmail = values[3],
                    CloudLifecycleState = cloud,
                    IdentityID = values[5],
                    IsManager = manager,
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