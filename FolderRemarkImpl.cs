using FolderRemark;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderRemark 
{

    internal class FolderRemarkImpl : IFolderRemark
    {
        public string Path { get; set; } = "";
        public string Remark { get; set; } = "";
        public string filePath { get; set; } = "";

        public Encoding ansiEncoding { get; set; }

        public FolderRemarkImpl()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.ansiEncoding = Encoding.GetEncoding("GB2312");// GBK 编码，对应 Windows-936
        }

        public FolderRemarkImpl(string path, string remark)
        {
            this.Path = path;
            this.Remark = remark;
            this.filePath = @$"{path}\desktop.ini";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            this.ansiEncoding = Encoding.GetEncoding("GB2312");// GBK 编码，对应 Windows-936
        }



        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public string ReadFile(String path, String remark)
        {
            this.Path = path;
            this.Remark = remark;
            this.filePath = @$"{path}\desktop.ini";
            return this.ReadFile();
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        public string ReadFile()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                stringBuilder.Append("路径为空或者路径有问题");
                return stringBuilder.ToString();
            }

            string line = "";
            FileStream f = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            StreamReader reader = new StreamReader(f, ansiEncoding);

#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            while ((line = reader.ReadLine()) != null)
            {
                stringBuilder.Append(line + "\n");

            }
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。


            reader.Close();
            f.Close();


            return stringBuilder.ToString();

        }

        /// <summary>
        /// 添加备注
        /// </summary>
        /// <param name="path"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddRemark(string path, string remark)
        {
            this.Path = path;
            this.Remark = remark;
            this.filePath = @$"{path}\desktop.ini";
            return this.AddRemark();
        }

        /// <summary>
        /// 添加备注
        /// </summary>
        /// <returns></returns>
        public bool AddRemark()
        {
            bool flag = false;

            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            if (File.Exists(filePath)) //文件存在
            {
                /*
                 * 出现文件无法读取的问题
                var lines ;
                using(lines = File.ReadAllLines(filePath)){}

                var infoTipLineIndex = lines.Select((line, index) => new { Line = line, Index = index })
                                     .FirstOrDefault(x => x.Line.StartsWith("InfoTip="))?.Index;


                if (infoTipLineIndex == null) //源文件中不存在"InfoTip="
                {
                    var shellClassLineIndex = lines.Select((line, index) => new { Line = line, Index = index })
                                     .FirstOrDefault(x => x.Line.StartsWith("[.ShellClassInfo]"))?.Index;
                    if (shellClassLineIndex == null) //源文件中不存在"ShellClassInfo="
                    {
                        lines[lines.Length + 1] = "[.ShellClassInfo]";
                        lines[lines.Length + 2] = "InfoTip=" + Remark;
                    }
                    else //在源文件中ShellClassInfo 的下一行插入 InfoTip
                    {
                        lines[shellClassLineIndex.Value + 1] = "InfoTip=" + Remark;
                    }
                }
                else
                {
                    lines[infoTipLineIndex.Value] = "InfoTip=" + Remark;

                    // 将修改后的内容写回文件  
                    using(File.WriteAllLines(filePath, lines, ansiEncoding)){}

                }
                 flag = true;
                */


                flag = FileStreamChange(true);

            }
            else //文件不存在
            {

                flag = FileStreamChange(false);
            }


            return flag;
        }


        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="remark"></param>
        /// <param name="exists">文件存在为true</param>
        /// <returns></returns>
        public bool FileStreamChange(string path, string remark, bool exists)
        {
            this.Path = path;
            this.Remark = remark;
            this.filePath = @$"{path}\desktop.ini";
            return this.FileStreamChange(exists);
        }

        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="exists">文件存在为true</param>
        /// <returns></returns>
        public bool FileStreamChange(bool exists)
        {
            bool flag = false;
            FileStream f;
            StreamWriter streamWriter;
            if (exists)
            {
                List<string> dataList = this.GetAndChangeOriginData();

                f = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                streamWriter = new StreamWriter(f, ansiEncoding);

                foreach (string str in dataList)
                {
                    streamWriter.WriteLine(str);
                }


                flag = true;
            }
            else
            {
                f = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite); ;
                streamWriter = new StreamWriter(f, ansiEncoding);


                streamWriter.WriteLine("[.ShellClassInfo]");
                streamWriter.WriteLine($"InfoTip={Remark}");
                flag = true;
            }

            streamWriter.Close();
            f.Close();

            return flag;
        }


        /// <summary>
        /// 拿到源文件中的字符
        /// </summary>
        /// <param name="path"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public List<String> GetAndChangeOriginData(string path, string remark)
        {
            this.Path = path;
            this.Remark = remark;
            this.filePath = @$"{path}\desktop.ini";
            return GetAndChangeOriginData();
        }

        /// <summary>
        /// 拿到源文件中的字符
        /// </summary>
        /// <returns></returns>
        public List<String> GetAndChangeOriginData()
        {
            FileStream f = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader reader = new StreamReader(f, ansiEncoding);
            List<String> dataList = new List<String>();
            int isInfoTip = 0; //判读源文件中是否有备注的开头
            String line = "";

#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("[.ShellClassInfo]"))
                {
                    isInfoTip = 1; //有开头
                }

                if (line.Contains("InfoTip="))  //源文件中有备注
                {
                    dataList.Add($"InfoTip={Remark}");
                    isInfoTip = 2;
                }
                else
                {
                    dataList.Add(line);
                }

            }
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            if (isInfoTip == 0) //源文件中没有备注信息的开头
            {
                dataList.Add("[.ShellClassInfo]");
                dataList.Add($"InfoTip={Remark}");
            }
            if (isInfoTip == 1)//源文件中有开头但没备注信息
            {
                for (int i = 0; i < dataList.LongCount(); i++)
                {
                    if (dataList[i].Contains("[.ShellClassInfo]"))
                    {
                        dataList.Insert(i, $"InfoTip={Remark}");
                        break;
                    }
                }
            }
            reader.Close();
            f.Close();
            return dataList;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Path = {this.Path} remark = {Remark} filePath = {filePath} \n");

            StreamReader reader = new StreamReader(filePath, ansiEncoding);

            String line = "";
            int num = 0;
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            while ((line = reader.ReadLine()) != null)
            {
                sb.Append($"[{num}]  {line} \n");
                num++;
            }
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
            reader.Close();

            return sb.ToString();
        }
    }
}
