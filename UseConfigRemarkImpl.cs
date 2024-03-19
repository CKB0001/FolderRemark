using IniParser;
using IniParser.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace FolderRemark
{
    internal class UseIniParserRemarkImpl : IFolderRemark
    {
        public Encoding ansiEncoding { get ; set ; }
        public string filePath { get; set; } = "";
        public string Path { get; set; } = "";
        public string Remark { get; set; } = "";

        public UseIniParserRemarkImpl()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.ansiEncoding = Encoding.GetEncoding("GB2312");// GBK 编码，对应 Windows-936
        }

        public UseIniParserRemarkImpl(string path, string remark)
        {
            this.Path = path;
            this.Remark = remark;
            this.filePath = @$"{path}\desktop.ini";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.ansiEncoding = Encoding.GetEncoding("GB2312");// GBK 编码，对应 Windows-936
        }


        public bool AddRemark()
        {
            bool flag = false;

            flag = FileStreamChange(File.Exists(filePath));

            return flag;
        }

        public bool AddRemark(string path, string remark)
        {
            this.Path = path;
            this.Remark = remark;
            this.filePath = @$"{path}\desktop.ini";
            return this.AddRemark();
        }

        public bool FileStreamChange(bool exists)
        {
            bool flag = false;
            var parser = new FileIniDataParser();
            if (exists) //文件存在
            {                
                IniData data = parser.ReadFile(filePath, ansiEncoding);
                data[".ShellClassInfo"]["InfoTip"] = Remark;
                parser.WriteFile(filePath, data,ansiEncoding);
                flag = true;
            }
            else //文件不存在
            {
                using (File.Create(filePath)) { }  //创建文件
                IniData data = new IniData();
                data.Sections.AddSection(".ShellClassInfo");
                data[".ShellClassInfo"].AddKey("InfoTip", Remark);
                parser.WriteFile(filePath, data, ansiEncoding);
                flag = true ;
            }
            return flag;
        }

        public bool FileStreamChange(string path, string remark, bool exists)
        {
            this.Path = path;
            this.Remark = remark;
            this.filePath = @$"{path}\desktop.ini";
            return this.FileStreamChange(exists);
        }

        public string ReadFile()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(filePath, ansiEncoding);
            StringBuilder stringBuilder = new StringBuilder();
            foreach(var section in data.Sections)
            {
                stringBuilder.AppendLine($"[{section.SectionName}]");
                foreach(var item in section.Keys)
                {
                    stringBuilder.AppendLine($"{item.KeyName}={item.Value}");
                }
            }
            return stringBuilder.ToString();
        }

        public string ReadFile(string path, string remark)
        {
            this.Path = path;
            this.Remark = remark;
            this.filePath = @$"{path}\desktop.ini";
            return this.ReadFile();
        }


        
    }
}
