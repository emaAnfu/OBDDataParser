using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OBDDataParser
{
    public class OBDRawDataFormatter
    {
        private static string DIGITS_LETTERS_PATTERN = "([0-9A-F])+";
        private static string MY_WORDS_PATTERN = "(Start measure)|(Finish measure)|(STOPPED)";
        private static string EIGHT_HEX_PATTERN = "[0-9A-F]{8}";
        private static string SIX_HEX_PATTERN = "[0-9A-F]{6}";

        public OBDRawDataFormatter()
        {

        }

        public string ReadAndFormatRawData(string filePath)
        {
            string textFormatted = "";

            using (StreamReader st = new StreamReader(filePath))
            {
                string allText = st.ReadToEnd();

                StringBuilder sb = new StringBuilder();

                foreach (char c in allText)
                {
                    string temp = String.Format("{0}", c);
                    if (Regex.IsMatch(temp, DIGITS_LETTERS_PATTERN))
                    {
                        sb.Append(c);
                    }
                    if (temp == ">")
                    {
                        sb.AppendLine();
                    }
                    if (temp == "\n")
                    {
                        sb.AppendLine();
                    }
                }

                textFormatted = sb.ToString();
            }

            return textFormatted;
        }

        public void SaveData(string filePath, string text)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(text);
            }
        }

        public string ReadAndCleanRawData(string filePath, CommandType commandType, string replyMode = "41")
        {
            string dataCleaned = "";

            using (StreamReader st = new StreamReader(filePath))
            {
                string line;

                StringBuilder sb = new StringBuilder();

                while ((line = st.ReadLine()) != null)
                {
                    if (commandType == CommandType.rpm)
                    {
                        if (Regex.IsMatch(line, EIGHT_HEX_PATTERN))
                        {
                            int start = line.IndexOf(replyMode);
                            sb.AppendLine(line.Substring(start, 8));
                        }
                    }
                    else if (commandType == CommandType.speed)
                    {
                        if (Regex.IsMatch(line, SIX_HEX_PATTERN))
                        {
                            int start = line.IndexOf(replyMode);
                            sb.AppendLine(line.Substring(start, 6));
                        }
                    }
                }

                dataCleaned = sb.ToString();

                return dataCleaned;
            }
        }

        public string ConvertRawData(string filePath, CommandType commandType)
        {
            string dataConverted = "";
            List<int> buffer = new List<int>();

            using (StreamReader st = new StreamReader(filePath))
            {
                string line;

                StringBuilder sb = new StringBuilder();

                while ((line = st.ReadLine()) != null)
                {
                    buffer.Clear();
                    int begin = 0;
                    int end = 2;
                    while (end <= line.Length)
                    {
                        buffer.Add(Convert.ToInt32(line.Substring(begin, 2), 16));
                        begin = end;
                        end += 2;
                    }
                    if (commandType == CommandType.rpm)
                    {
                        int rpm = (buffer[2] * 256 + buffer[3]) / 4;
                        sb.AppendLine(rpm.ToString());
                    }
                    else if (commandType == CommandType.speed)
                    {
                        int speed = buffer[2];
                        sb.AppendLine(speed.ToString());
                    }
                }

                dataConverted = sb.ToString();

                return dataConverted;
            }
        }
    }
}
