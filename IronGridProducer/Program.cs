using System.Text.Json;
using IronGridProducer.IronGridServices;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

string bootstrapServices = config["Kafka:BootstrapServices"]!;

string PerimeterSensorTopic = config["Kafka:Topics:PerimeterSensor"]!;
string UavTopic = config["Kafka:Topics:UAV"]!;

string ReportsPath = "Data/field_reports.json";

string ReportsJsonContent = File.ReadAllText(ReportsPath);
static List<string> GetListOfData(string content)
{
    List<string> outputList = new List<string>();
    using (JsonDocument doc = JsonDocument.Parse(content))
    {
        foreach (JsonElement element in doc.RootElement.EnumerateArray())
        {
            outputList.Add(element.GetRawText());
        }
    }
    return outputList;
}

List<string> ReportsList = GetListOfData(ReportsJsonContent);

var producer = new KafkaProducerServices(bootstrapServices);

Console.WriteLine($"loaded {ReportsList.Count} rows from {ReportsPath}");

int LentghOfData = ReportsList.Count();
int UavCounter= 0;
int PerimeterSensorCouner = 0;

for(int i = 0; i<LentghOfData; i++)
{
    if (ReportsList[i].Contains("UAV"))
    {
        await producer.SendAsync(UavTopic, ReportsList[i]);
        UavCounter += 1;
        Console.WriteLine($"send data number {UavCounter} to {UavTopic} topic");
    }
    else
    {
        PerimeterSensorCouner += 1;
        await producer.SendAsync(PerimeterSensorTopic, ReportsList[i]);
        Console.WriteLine($"send data number {PerimeterSensorCouner} to {PerimeterSensorTopic} topic");
    }
}
Console.WriteLine("loded all data successffuly");
