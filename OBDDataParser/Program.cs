using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBDDataParser
{
    class Program
    {
        static void Main(string[] args)
        {
            OBDRawDataFormatter obdFormatter = new OBDRawDataFormatter();

            string name = "10-05-18_14-10-39_speed_measure";
            CommandType commandType = CommandType.speed;

            string path = "C:\\Users\\emanu\\Desktop\\" + name + ".txt";
            string formattedRawData = obdFormatter.ReadAndFormatRawData(path);
            Console.WriteLine(formattedRawData);

            //Console.ReadLine();

            path = "C:\\Users\\emanu\\Desktop\\" + name + "_formatted.txt";
            obdFormatter.SaveData(path, formattedRawData);

            string cleanedRawData = obdFormatter.ReadAndCleanRawData("C:\\Users\\emanu\\Desktop\\" + name + "_formatted.txt", commandType);
            Console.WriteLine(cleanedRawData);

            //Console.ReadLine();

            path = "C:\\Users\\emanu\\Desktop\\" + name + "_cleaned.txt";
            obdFormatter.SaveData(path, cleanedRawData);

            string convertedData = obdFormatter.ConvertRawData(path, commandType);
            Console.WriteLine(convertedData);

            path = "C:\\Users\\emanu\\Desktop\\" + name + "_converted.txt";
            obdFormatter.SaveData(path, convertedData);
        }
    }
}
