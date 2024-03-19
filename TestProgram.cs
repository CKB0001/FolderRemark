using IniParser.Model;
using IniParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderRemark
{
    internal class TestProgram
    {
        static void Main1(string[] args)
        {
            string path = @"D:\temp\test";
            //string path = @"D:\CloudMusic";
            string remark = "测试";

            FolderRemarkImpl folder = new FolderRemarkImpl(path, remark);

            bool flag = folder.AddRemark(path, remark);

            string cmdCommnad1 = $@"attrib {folder.filePath} +s +h";
            string cmdCommnad2 = $@"attrib {folder.Path} +s";

            CMDCommand.RunCmdCommand(cmdCommnad1);
            CMDCommand.RunCmdCommand(cmdCommnad2);

            Console.WriteLine($"flag {flag}");

            Console.WriteLine(folder.ToString());
            // Console.WriteLine(folder.ReadFile());
        }

        public static void Main3(String[] args)
        {
            //Test();
            string path = @"D:\temp\test";
            //string path = @"D:\CloudMusic";
            string remark = "测asdf as试";
            CMDCommand.RunCMDCommand(new UseIniParserRemarkImpl(path,remark) ,true);
            IFolderRemark folder = new UseIniParserRemarkImpl(path, remark);
            //Console.WriteLine(folder.ReadFile());
            folder.AddRemark();
            CMDCommand.RunCMDCommand(new UseIniParserRemarkImpl(path, remark), false);
            Console.WriteLine(folder.ReadFile());

        }
       
        public static void Test()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding ansiEncoding = Encoding.GetEncoding("GB2312");// GBK 编码，对应 Windows-936

            var parser = new FileIniDataParser();
            string filePath = @"D:\temp\test\desktop.ini";
            // 这将加载INI文件，读取失败中包含的数据，并解析该数据
            IniData data = parser.ReadFile(filePath, ansiEncoding);
            //通过所有的段迭代
            foreach (SectionData section in data.Sections)
            {
                Console.WriteLine("[" + section.SectionName + "---]");

                //遍历当前节中的所有键以打印值
                foreach (KeyData key in section.Keys)
                    Console.WriteLine(key.KeyName + " = ===" + key.Value);
            }

            data[".ShellClassInfo"]["InfoTip"] = "使用Congfig ini测试";


            parser.WriteFile(filePath, data);

        }
    }
}
