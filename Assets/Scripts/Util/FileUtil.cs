
using System.Collections.Generic;

using System.IO;
using System.Net;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
/// <summary>
/// 功能介绍
/// 文件操作部分
/// 1.LocalFileIsExists(string filePath) 判断本地文件是否存在
/// 2.NetWorkFileIsExists(string url)判断网络文件是否存在
/// 3.GetFileSize(string filePath)获取文件大小
/// 4.GetFileNameWithoutExtension(string filePath) 获取文件名字 不带扩展名的
/// 5.GetFileName(string filePath)获取文件名字 带有扩展名
/// 6.GetFileParentPath(string filePath)  获取文件的父目录  
/// 7.CreateFile(string filePath) 创建一个文件
/// 8.DeleteFile(string filePath) 删除一个文件
/// 9.SaveFile(string fileContent,string filePath)  保存文件  保存内容是字符串
/// 10.SaveFile(byte[] fileContent, string filePath)  保存一个文件  数据是字节数组
/// 11.ReadFileToString(string filePath) 读取一个文本文件
/// 12.ReadFileToByte(string filePath) 读取一个字节文件
/// 文件夹操作部分
/// 1.CreateFolder(string folderPath)  创建一个文件夹
/// 2.GetFolderSize(string folderPath)  获取文件夹的大小
/// 3.GetFolderFilePath(string folderPath)  获取文件夹下面的文件完整路径，不包括子文件夹的文件
/// 4.GetFolderFileNameWithoutExtension(string folderPath)获取文件夹下面的文件名字，注意名字不带扩展名，不包括子文件夹的文件
/// 5.GetFolderFileName(string folderPath)获取文件夹下面的文件名字，注意名字带扩展名，不包括子文件夹的文件
/// 6.GetFolderAndSubFolderFilePath(string folderPath)获取文件夹下面的文件完整路径，包括子文件夹的文件
/// 7.GetFolderAndSubFolderAllFileNameWithoutExtension(string folderPath)获取文件夹下面的文件名字，注意名字不带扩展名，包括子文件夹的文件
/// 8.GetFolderAndSubFolderAllFileName(string folderPath)获取文件夹下面的文件名字，注意名字带扩展名，包括子文件夹的文件
/// 9.DeleteFolder(string folderPath) 删除文件夹以及子文件夹里面的文件
/// 10.DeleteFolderFile(string folderPath)  删除文件夹里面的文件  注意不包括子文件夹
/// 11.
/// 12.
/// 13.
/// 14.
/// 15.
/// </summary>


public static class FileUtil
{
	#region  文件操作
	/// <summary>
	/// 判断本地文件是否存在
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	public static bool LocalFileIsExists(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return false;
		}
		return File.Exists(filePath);
	}

	/// <summary>
	/// 判断网络文件是否存在
	/// </summary>
	/// <param name="url"></param>
	/// <returns></returns>
	public static bool NetWorkFileIsExists(string url)
	{
		if (string.IsNullOrEmpty(url))
		{
			return false;
		}
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new System.Uri(url));
			httpWebRequest.Method = "HEAD";
			httpWebRequest.Timeout = 1000;
			return ((HttpWebResponse)httpWebRequest.GetResponse()).StatusCode == HttpStatusCode.OK;
		}
		catch
		{
			return false;
		}
	}

	/// <summary>
	/// 获取文件大小
	/// </summary>
	/// <param name="filePath">文件路径</param>
	/// <returns></returns>
	public static long GetFileSize(string filePath)
	{
		if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
		{
			return -1;
		}
		return new FileInfo(filePath).Length;
	}
	/// <summary>
	/// 获取文件名字  
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns>带扩展名的文件名字</returns>
	public static string GetFileNameWithoutExtension(string filePath)
	{
		if (File.Exists(filePath))
		{
			return Path.GetFileNameWithoutExtension(filePath);
		}
		return "";

	}
	/// <summary>
	/// 获取文件名字  
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns>没有扩展名的文件名字</returns>
	public static string GetFileName(string filePath)
	{

		if (File.Exists(filePath))
		{
			return Path.GetFileName(filePath);

		}
		return null;

	}

	/// <summary>
	/// 获取文件的父目录
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	public static string GetFileParentPath(string filePath)
	{
		if (File.Exists(filePath))
		{
			return Path.GetDirectoryName(filePath);

		}
		return null;
	}


	/// <summary>
	/// 创建一个文件
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	public static bool CreateFile(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return false;
		}
		try
		{
			if (!File.Exists(filePath))
			{
				File.Create(filePath).Close();

			}
			return true;
		}
		catch
		{
			return false;
		}
	}
	/// <summary>
	/// 刪除一个文件
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns></returns>
	public static bool DeleteFile(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return false;
		}
		try
		{
			if (!File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			return true;
		}
		catch
		{
			return false;
		}
	}
	/// <summary>
	/// 保存文件字符串的形式
	/// </summary>
	/// <param name="fileContent"></param>
	/// <param name="filePath"></param>

	public static void SaveFile(string fileContent, string filePath)
	{
		
		File.WriteAllText(filePath, fileContent);
	}


	/// <summary>
	/// 保存文件 字节形式
	/// </summary>
	/// <param name="fileContent"></param>
	/// <param name="filePath"></param>
	public static void SaveFile(byte[] fileContent, string filePath)
	{
		FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate);
		fileStream.Write(fileContent, 0, fileContent.Length);
	}

	/// <summary>
	/// 读取一个文件  
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns>返回文件文本</returns>
	public static string ReadFileToString(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return "不存在" + filePath;
		}
		return File.ReadAllText(filePath);
	}


	/// <summary>
	/// 将文件赋值到其他文件
	/// </summary>
	/// <param name="sourceFilePath"></param>
	/// <param name="targetFilePath"></param>
	public static void CopyFileToOtherFile(string sourceFilePath, string targetFilePath)
	{
		FileInfo fileInfo = new FileInfo(sourceFilePath);
		fileInfo.CopyTo(targetFilePath, true);
	}

	/// <summary>
	/// 读取一个文件
	/// </summary>
	/// <param name="filePath"></param>
	/// <returns>   返回文件字节数据</returns>
	public static byte[] ReadFileToByte(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return null;
		}
		return File.ReadAllBytes(filePath);
	}

	

	private static IEnumerator IE_ReadFileFromStreamAsset(string filePath, UnityAction<string> onComplete)
	{
		UnityWebRequest unityWebRequest = new UnityWebRequest(filePath);
		yield return unityWebRequest.SendWebRequest();
		if (string.IsNullOrEmpty(unityWebRequest.error)) { 
			onComplete?.Invoke(unityWebRequest.downloadHandler.text);
		}
	}
	#endregion

	#region 文件夹操作
	/// <summary>
	/// 创建一个文件夹
	/// </summary>
	/// <param name="folderPath"></param>

	public static void CreateFolder(string folderPath)
	{
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
	}
	/// <summary>
	/// 获取文件夹大小
	/// </summary>
	/// <param name="folderPath">文件夹路径</param>
	/// <returns></returns>
	public static long GetFolderSize(string folderPath)
	{
		long fileSize = 0;
		if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
		{
			return 0;
		}
		string[] files = Directory.GetFiles(folderPath);
		foreach (var filePath in files)
		{
			fileSize += new FileInfo(filePath).Length;
		}

		string[] subFolder = Directory.GetDirectories(folderPath);
		foreach (var folder in subFolder)
		{
			fileSize += GetFolderSize(folder);
		}
		return fileSize;
	}


	/// <summary>
	/// 获取文件夹下面的所有文件完整路径
	/// 不包含子文件夹里面的文件
	/// </summary>
	/// <param name="folderPath"></param>
	/// <returns>返回完整的文件路径数组</returns>
	public static string[] GetFolderFilePath(string folderPath)
	{
		if (string.IsNullOrEmpty(folderPath))
		{
			return null;
		}
		if (!Directory.Exists(folderPath))
		{
			return null;
		}
		return Directory.GetFiles(folderPath);
	}

	/// <summary>
	/// 获取文件夹下面的所有文件名字
	/// 不包含子文件夹里面的文件
	/// </summary>
	/// <param name="folderPath"></param>
	/// <returns>返回文件名数组
	/// 文件名不带扩展名
	/// </returns>
	public static string[] GetFolderFileNameWithoutExtension(string folderPath)
	{
		if (string.IsNullOrEmpty(folderPath))
		{
			return null;
		}
		if (!Directory.Exists(folderPath))
		{
			return null;
		}
		string[] files = Directory.GetFiles(folderPath);
		if (files == null || files.Length < 1)
		{
			return null;

		}
		string[] fileName = new string[files.Length];

		for (int i = 0; i < files.Length; i++)
		{
			fileName[i] = Path.GetFileNameWithoutExtension(files[i]);
		}

		return fileName;
	}
	/// <summary>
	/// 获取文件夹下面的所有文件名字  注意这个名字是带扩展名的
	/// 不包含子文件夹里面的文件
	/// </summary>
	/// <param name="folderPath"></param>
	/// <returns>返回文件名数组   带扩展名</returns>
	public static string[] GetFolderFileName(string folderPath)
	{
		if (string.IsNullOrEmpty(folderPath))
		{
			return null;
		}
		if (!Directory.Exists(folderPath))
		{
			return null;
		}
		string[] files = Directory.GetFiles(folderPath);
		if (files == null || files.Length < 1)
		{
			return null;

		}
		string[] fileName = new string[files.Length];

		for (int i = 0; i < files.Length; i++)
		{
			fileName[i] = Path.GetFileName(files[i]);
		}

		return fileName;
	}

	/// <summary>
	/// 获取文件夹下面的文件
	/// 包含子文件夹里面的文件
	/// </summary>
	/// <param name="folderPath"></param>
	/// <returns>返回完整的文件路径数组</returns>
	public static string[] GetFolderAndSubFolderFilePath(string folderPath)
	{
		if (string.IsNullOrEmpty(folderPath))
		{
			return null;

		}
		if (!Directory.Exists(folderPath))
		{
			return null;
		}
		List<string> files = new List<string>();
		files.AddRange(Directory.GetFiles(folderPath));
		string[] folders = Directory.GetDirectories(folderPath);
		foreach (var folder in folders)
		{
			string[] file = GetFolderAndSubFolderFilePath(folder);
			if (file != null)
			{
				files.AddRange(file);

			}
		}
		return files.ToArray();
	}

	/// <summary>
	/// 获取文件夹下面的文件
	/// 包含子文件夹里面的文件
	/// </summary>
	/// <param name="folderPath"></param>
	/// <returns>返回文件名数组
	/// 不带扩展名</returns>
	public static string[] GetFolderAndSubFolderAllFileNameWithoutExtension(string folderPath)
	{
		if (string.IsNullOrEmpty(folderPath))
		{
			return null;

		}
		if (!Directory.Exists(folderPath))
		{
			return null;
		}
		List<string> files = new List<string>();
		files.AddRange(Directory.GetFiles(folderPath));
		string[] folders = Directory.GetDirectories(folderPath);
		foreach (var folder in folders)
		{
			string[] file = GetFolderAndSubFolderFilePath(folder);
			if (file != null)
			{
				files.AddRange(file);
			}
		}
		for (int i = 0; i < files.Count; i++)
		{
			files[i] = Path.GetFileNameWithoutExtension(files[i]);
		}
		return files.ToArray();
	}
	/// <summary>
	/// 获取文件夹下面的文件
	/// 包含子文件夹里面的文件
	/// </summary>
	/// <param name="folderPath"></param>
	/// <returns>返回文件名数组  带扩展名</returns>
	public static string[] GetFolderAndSubFolderAllFileName(string folderPath)
	{
		if (string.IsNullOrEmpty(folderPath))
		{
			return null;

		}
		if (!Directory.Exists(folderPath))
		{
			return null;
		}
		List<string> files = new List<string>();
		files.AddRange(Directory.GetFiles(folderPath));
		string[] folders = Directory.GetDirectories(folderPath);
		foreach (var folder in folders)
		{
			string[] file = GetFolderAndSubFolderFilePath(folder);
			if (file != null)
			{
				files.AddRange(file);
			}
		}
		for (int i = 0; i < files.Count; i++)
		{
			files[i] = Path.GetFileName(files[i]);
		}
		return files.ToArray();
	}
	/// <summary>
	/// 获取folderPath路径下面的子文件夹
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string[] GetSubFolder(string folderPath)
	{
		if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
		{
			return null;
		}
		return Directory.GetDirectories(folderPath);
	}

	/// <summary>
	/// 删除文件夹 包括里面的文件和子文件
	/// </summary>
	/// <param name="folderPath"></param>
	public static void DeleteFolder(string folderPath)
	{
		if (!Directory.Exists(folderPath))
		{
			return;
		}
		Directory.Delete(folderPath, true);
	}

	/// <summary>
	/// 删除文件夹 下面的文件
	/// </summary>
	/// <param name="folderPath"></param>
	public static void DeleteFolderFile(string folderPath)
	{
		if (!Directory.Exists(folderPath))
		{
			return;
		}
		string[] files = Directory.GetFiles(folderPath);
		if (files != null && files.Length > 0)
		{
			foreach (var item in files)
			{
				File.Delete(item);
			}
		}
	}
	#endregion
}

