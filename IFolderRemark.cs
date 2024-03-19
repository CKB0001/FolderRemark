using System.Text;

namespace FolderRemark
{
    internal interface IFolderRemark
    {
        Encoding ansiEncoding { get; set; } //编码
        string filePath { get; set; } //文件夹路径+文件名
        string Path { get; set; } //文件夹路径
        string Remark { get; set; } //备注
        /// <summary>
        /// 添加备注
        /// </summary>
        /// <returns></returns>
        bool AddRemark();
        /// <summary>
        /// 添加备注
        /// </summary>
        /// <returns></returns>
        bool AddRemark(string path, string remark);

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns></returns>
        string ReadFile();
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        string ReadFile(string path, string remark);


        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="exists">文件存在为true</param>
        /// <returns></returns>
        bool FileStreamChange(bool exists);
        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="remark"></param>
        /// <param name="exists">文件存在为true</param>
        /// <returns></returns>
        bool FileStreamChange(string path, string remark, bool exists);

    }
}