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
            string name;
            CommandType commandType;

            name = "10-05-18_21-04-46_speed_measure";
            commandType = CommandType.speed;
            ProcessRawData(name, commandType);

            name = "10-05-18_21-04-46_rpm_measure";
            commandType = CommandType.rpm;
            ProcessRawData(name, commandType);
        }

        /// <summary>
        /// It takes the nema of raw data files (in desktop folder) and save all the various
        /// versions in other txt files. The most useful are the converted one.
        /// </summary>
        private static void ProcessRawData(string name, CommandType commandType)
        {
            OBDRawDataFormatter obdFormatter = new OBDRawDataFormatter();

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
