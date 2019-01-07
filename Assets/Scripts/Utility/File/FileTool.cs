using System.IO;

public class FileTool {

    /// <summary>
    /// 创建目录
    /// </summary>
    /// <param name="filePath">需要创建的目录路径</param>
    public static void CreateDirectory (string filePath) {
        if (!string.IsNullOrEmpty (filePath)) {
            string dirName = Path.GetDirectoryName (filePath);
            if (!Directory.Exists (dirName)) {
                Directory.CreateDirectory (dirName);
            }
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="bytes">文件内容</param>
    public static void CreatFile (string filePath, byte[] bytes) {
        FileInfo file = new FileInfo (filePath);
        Stream stream = file.Create ();

        stream.Write (bytes, 0, bytes.Length);

        stream.Close ();
        stream.Dispose ();
    }

    public static string ReadFile (string PathName, int linenumber) {
        string[] strs = File.ReadAllLines (PathName); //读取txt文本的内容，返回sring数组的元素是每行内容
        if (linenumber == 0) {
        return "";
        } else {
        return strs[linenumber - 1]; //返回第linenumber行内容
        }
    }

}