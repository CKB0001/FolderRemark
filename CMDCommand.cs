using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderRemark
{
    internal class CMDCommand
    {
        /// <summary>
        /// 运行CMD命令
        /// </summary>
        /// <param name="cmdCommand"></param>
        /// <returns></returns>
        public static string RunCmdCommand(String cmdCommand)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe")
            {
                // 设置使用ShellExecute为false，以便我们可以重定向标准输出  
                UseShellExecute = false,
                // 设置创建窗口为false，因为我们不希望显示CMD窗口  
                CreateNoWindow = true,
                // 设置窗口样式为隐藏  
                WindowStyle = ProcessWindowStyle.Hidden,
                // 重定向标准输出，以便我们可以读取它  
                RedirectStandardOutput = true,
                // 设置CMD命令  
                Arguments = "/C " + cmdCommand
            };
            string output = "";
            // 创建一个新的Process对象  
            using (Process process = new Process())
            {
                // 将ProcessStartInfo对象分配给Process对象  
                process.StartInfo = startInfo;

                // 启动进程  
                process.Start();

                // 读取CMD命令的输出  
                output = process.StandardOutput.ReadToEnd();

                // 等待进程结束  
                process.WaitForExit();

            }

            return output;
        }


        /// <summary>
        /// 是否取消文件权限
        /// </summary>
        /// <param name="isCancel">取消为True</param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static bool RunCMDCommand(IFolderRemark folder, bool isCancel)
        {
        

            try
            {
                string cmdCommnad1 = "";
                string cmdCommnad2 = "";
                if (!isCancel)
                {
                    cmdCommnad1 = $"attrib \"{folder.filePath}\" +s +h";
                    cmdCommnad2 = $"attrib \"{folder.Path}\" +s";

                }
                else
                {
                    cmdCommnad1 = $"attrib -s -h \"{folder.filePath}\" ";
                    cmdCommnad2 = $"attrib -s \"{folder.Path}\"";
                }
                CMDCommand.RunCmdCommand(cmdCommnad1);
                CMDCommand.RunCmdCommand(cmdCommnad2);
                Console.WriteLine("修改中....");
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }           
        }
    }
    
}
