using IniParser;
using IniParser.Model;
using System.Text;

namespace FolderRemark
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            
            String? remark = "";
            String? path = "";
            bool flag = false;
            bool isEmptyArgs = args.Length > 0 && !string.IsNullOrEmpty(args[0]);

            Console.WriteLine("----------------- 文 件 夹 备 注 -----------------");
            Console.WriteLine();
            if (isEmptyArgs) //鼠标右键传输文件夹的值
            {
                Console.WriteLine($"文件夹路径：{args[0]}");
                path = args[0];

                Console.Write("请输入备注：");
                remark = Console.ReadLine();
                while (string.IsNullOrEmpty(remark))
                {
                    Console.Write("备注为空,请重新输入：");
                    remark = Console.ReadLine();
                }

                flag = Run(path, remark);

                if (flag)
                {
                    Console.WriteLine("--【文件夹备注添加成功,请稍等一会并刷新】--");
                    Console.WriteLine("--【按任意键退出程序】--");
                }
                else
                {
                    Console.WriteLine("文件夹添加备注失败");
                    Console.WriteLine("按任意键退出程序");
                }
                Console.Read();
            }

            while (!isEmptyArgs) //控制台没有传值
            {

                Console.WriteLine("-----------------Ctrl + c 退出系统-------------------");

                Console.Write("请输入文件夹地址：");
                path = Console.ReadLine();
                while (!isPath(path))
                {
                    Console.Write("文件夹地址有问题,请重新输入：");
                    path = Console.ReadLine();
                }
                path = path.Replace("\"", "");


                Console.Write("请输入备注：");
                remark = Console.ReadLine();
                while (string.IsNullOrEmpty(remark))
                {
                    Console.Write("备注为空,请重新输入：");                    
                    remark = Console.ReadLine();
                }

                flag = Run(path, remark);

                if (flag)
                {
                    Console.WriteLine("--【文件夹备注添加成功,请稍等一会并刷新】--");
                }
                else
                {
                    Console.WriteLine("文件夹添加备注失败");
                }
            }
            

        }

        

        public static bool Run(string path , string remark)
        {
            

            try
            {
                bool flag = false;
                IFolderRemark folderRemark = new UseIniParserRemarkImpl(path, remark);
                if (File.Exists(folderRemark.filePath))
                {
                    CMDCommand.RunCMDCommand(folderRemark, true); //文件取消权限                       
                }
                flag = folderRemark.AddRemark();
                flag = CMDCommand.RunCMDCommand(folderRemark, false); //文件添加权限
                                                                      //
                return flag;
            }
            catch (Exception ex)
            {
                Console.WriteLine("----------Error:--------");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("----------Error:--------");
                return false;
            }          
        }


        /// <summary>
        /// 判断路径是否正确
        /// </summary>
        /// <param name="path"></param>
        /// <returns>有问题为false</returns>
        public static bool isPath(String path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false; //文件夹地址为空
            }else
            {
                path = path.Replace("\"", "");
                if (!Directory.Exists(path)) //文件夹不存在
                {
                    return false;
                }
            }

            return true;
        }
       


    }
}
