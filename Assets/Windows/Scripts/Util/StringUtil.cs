using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StringUtil
{
	/// <summary>
	/// 转换成钱的通用符表示
	/// </summary>
	/// <param name="money">金额</param>
	/// <param name="accuracy">精准度C C1 C2..........</param>
	/// exapmle
	/// input  :ConvertToMoney(100.345,C3)
	/// output :￥100.345
	/// <returns></returns>
	public static string ConvertToMoneyFormat(float money, string accuracy)
	{

		return string.Format("{0:" + accuracy + "}", money);
	}
	/// <summary>  
	/// 将数据填充固定位数
	/// <param name="accuracy">精准度D D1 D2..........</param>
	/// exapmle
	/// input  :ConvertToMoney(10,D3)
	/// output :010
	/// </summary>
	/// <returns></returns>
	public static string ConvertToDeFormat(int data, string accuracy)
	{
		return string.Format("{0:" + accuracy + "}", data);
	}
	/// <summary>
	/// 将数据用逗号隔开 并且指定显示的小数位数
	/// </summary>
	/// <param name="data"></param>
	/// <param name="accuracy"> N  N1  N2 .......</param>
	/// example
	/// input: ConvertToNFormat(123456.45f,"n3")
	/// output: 123,456.500
	/// <returns></returns>
	public static string ConvertToNFormat(float data, string accuracy)
	{
		return string.Format("{0:" + accuracy + "}", data);
	}
	/// <summary>
	/// 将数据转换成百分号表示的形式
	/// </summary>
	/// <param name="data"></param>
	/// <param name="accuracy"> 精确到小数点后面几位 P  P1  P2 .......</param>
	/// example
	/// input: ConvertToPercentage(0.45f,"P3")
	/// output: 45.000%
	/// <returns></returns>
	public static string ConvertToPercentage(float data, string accuracy)
	{
		return string.Format("{0:" + accuracy + "}", data);
	}

	public static string StringSplicing(string separator, params string[] content)
	{
		if (content.Length > 1)
		{
			StringBuilder stringBuilder = new StringBuilder(content[0]);
			for (int i = 1; i < content.Length; i++)
			{
				stringBuilder.Append(separator);
				stringBuilder.Append(content[i]);
			}
			return stringBuilder.ToString();
		}
		return null;
	}

	public static string[] StringSplit(char separator, string content)
	{
		if (content != null)
		{
			return content.Split(separator);
		}
		return null;
	}
}
