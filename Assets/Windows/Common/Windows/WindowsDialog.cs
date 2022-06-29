using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Ambilight
{
	sealed class ComdlgDll
	{
		[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Unicode)]
		public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
		[DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Unicode)]
		public static extern bool GetSaveFileName([In, Out] OpenFileName ofn);
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public class OpenFileName
	{
		private int structSize = 0;
		private IntPtr dlgOwner = IntPtr.Zero;
		private IntPtr instance = IntPtr.Zero;
		private string filter = null;
		private string customFilter = null;
		private int maxCustFilter = 0;
		private int filterIndex = 0;
		public string filePath { get; set; }
		private int maxFile = 0;
		public string fileName { get; set; }
		private int maxFileTitle = 0;
		public string initialDir { get; set; }
		public string title { get; set; }
		private int flags = 0;
		private short fileOffset = 0;
		private short fileExtension = 0;
		private string defExt = null;
		private IntPtr custData = IntPtr.Zero;
		private IntPtr hook = IntPtr.Zero;
		private string templateName = null;
		private IntPtr reservedPtr = IntPtr.Zero;
		private int reservedInt = 0;
		private int flagsEx = 0;
		public OpenFileName(params string[] ext)
		{
			structSize = Marshal.SizeOf(this);
			defExt = ext[0];
			string n = null;
			string e = null;
			foreach (string _e in ext)
			{
				if (_e == ".*")
				{
					n += "All Files";
					e += "*.*;";
				}
				else
				{
					string _n = _e + ";";
					n += _n;
					e += "*" + _n;
				}
			}
			n = n.Substring(0, n.Length - 1);
			filter = n + "\0" + e + "\0";
			filePath = new string(new char[256]);
			maxFile = filePath.Length;
			fileName = new string(new char[64]);
			maxFileTitle = fileName.Length; flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;
			initialDir = Application.dataPath;
		}
	}

	public sealed class WindowsDialog
	{
		/// <summary>
		/// 打开文件对话框
		/// </summary>
		/// <param name="action"></param>
		/// <param name="suffix"></param>
		public static void OpenFile(Action<OpenFileName> action, params string[] suffix)
		{
			OpenFileName openFileName = new OpenFileName(suffix);
			if (ComdlgDll.GetOpenFileName(openFileName))
			{
				action?.Invoke(openFileName);
			}
		}

		/// <summary>
		/// 打开文件选择文件
		/// </summary>
		/// <param name="fileType"></param>
		public static void SaveFile(Action<OpenFileName> action, params string[] suffix)
		{
			OpenFileName openFileName = new OpenFileName(suffix);
			if (ComdlgDll.GetSaveFileName(openFileName))
			{
				action?.Invoke(openFileName);
			}
		}
	}
}