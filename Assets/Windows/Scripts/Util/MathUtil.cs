using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class MathUtil
{

	/// <summary>
	/// 计算二阶贝塞尔曲线的点 公式 （1-t）*(1-t)*p0+2t*(1-t)*p1+t*t*p2   t属于[0,1]
	/// </summary>
	/// <param name="t">  0<t<1 </param>
	/// <param name="p0">起点</param>
	/// <param name="p1">中点</param>
	/// <param name="p2">结束点</param>
	/// <returns></returns>
	internal static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float u = 1 - t;
		float tt = t * t;
		float uu = u * u;
		return uu * p0+ 2 * u * t * p1+ tt * p2;
	}
	/// <summary>
	/// 获取一个向量
	/// </summary>
	/// <param name="data">"(1,1,1)"</param>
	/// <returns>(1,1,1)</returns>
	internal static Vector3 GetVector3(string data)
	{
		 data = data.Substring(1, data.Length-2);
		string[] vec = StringUtil.StringSplit(',',data);
		return new Vector3(float.Parse(vec[0]), float.Parse(vec[1]), float.Parse(vec[2]));
	}

	internal static Vector2 GetVector2(string data)
	{
		data = data.Substring(1, data.Length - 2);
		string[] vec = StringUtil.StringSplit(',', data);
		return new Vector3(float.Parse(vec[0]), float.Parse(vec[1]));
	}
	/// <summary>
	/// 获取一个四元数
	/// </summary>
	/// <param name="data">"(1,1,1,1)"</param>
	/// <returns>(1,1,1,1)</returns>

	internal static Quaternion GetQuaterion(string data) {
		data = data.Substring(1, data.Length - 2);
		string[] vec = StringUtil.StringSplit(',', data);
		return new Quaternion(float.Parse(vec[0]), float.Parse(vec[1]), float.Parse(vec[2]), float.Parse(vec[3]));

	}

	/// <summary>
	/// 获取鼠标在屏幕上移动的方向上的垂直向量
	/// </summary>
	/// <param name="lastMousePosition">鼠标上一次所在屏幕上的位置</param>
	/// <param name="curretMousePosition">鼠标当前在屏幕上的位置</param>
	/// <returns></returns>
	internal static Vector3 GetMouseMoveDirectionCross(Vector3 lastMousePosition,Vector3 curretMousePosition) {
		Vector3 newVec = curretMousePosition - lastMousePosition;
		Vector3 crossVec = Vector3.Cross(newVec.normalized, Vector3.forward).normalized;
		return crossVec;
	}

	/// <summary>
	/// 绕模型中心点旋转角度后的坐标,旋转矩阵为
	/// 【 Cos(a)  Sin(a)】
	/// 【-Sin(a)  Cos(a)】
	/// </summary>
	/// <param name="degree"></param>
	/// <param name="point"></param>
	/// <returns></returns>
	internal static Vector2   Rotate2D(int degree, Vector2 point) {
		float angle = degree / 360.0f * Mathf.PI;
		point.x = point.x * Mathf.Cos(angle) - point.y * Mathf.Sin(angle);
		point.y = point.x * Mathf.Sin(angle) + point.y * Mathf.Cos(angle);
		return point;
	}

	/// <summary>
	/// 计算直线与平面的交点
	/// </summary>
	/// <param name="point1"></param>
	/// <param name="point2"></param>
	/// <param name="point3"></param>
	/// <returns></returns>
	public static Vector3 PlaneEquation(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3,Vector3 lPoint,Vector3 lPoint2) {
		Vector3 dir12 = (vertex2 - vertex1).normalized;
		Vector3 dir13 = (vertex3 - vertex1).normalized;
		Vector3 normal = Vector3.Cross(dir12, dir13);
		return GetIntersectWithLineAndPlane(lPoint, lPoint2- lPoint, normal, vertex1);
	}

	/// <summary>
	/// 计算直线与平面的交点
	/// </summary>
	/// <param name="lPoint">直线上某一点</param>
	/// <param name="lDirect">直线的方向</param>
	/// <param name="planeNormal">垂直于平面的的向量</param>
	/// <param name="planePoint">平面上的任意一点</param>
	/// <returns></returns>
	internal static Vector3 GetIntersectWithLineAndPlane(Vector3 lPoint, Vector3 lDirect, Vector3 planeNormal, Vector3 planePoint)
	{
		float d = Vector3.Dot(planePoint - lPoint, planeNormal) / Vector3.Dot(lDirect.normalized, planeNormal);

		return d * lDirect.normalized + lPoint;
	}
	/// <summary>
	/// 判断一个点是否在三角形之内，三个点顺时针传递
	/// </summary>
	/// <param name="A"></param>
	/// <param name="B"></param>
	/// <param name="C"></param>
	/// <param name="P">需要判断的点</param>
	/// <returns></returns>
	internal static bool PointInTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
	{
		Vector3 v0 = C - A;
		Vector3 v1 = B - A;
		Vector3 v2 = P - A;

		float dot00 = Vector3.Dot(v0,v0);
		float dot01 = Vector3.Dot(v0, v1);
		float dot02 = Vector3.Dot(v0, v2);
		float dot11 = Vector3.Dot(v1, v1);
		float dot12 = Vector3.Dot(v1, v2);

		float inverDeno = 1 / (dot00 * dot11 - dot01 * dot01);

		float u = (dot11 * dot02 - dot01 * dot12) * inverDeno;
		if (u < 0 || u > 1)  
		{
			return false;
		}

		float v = (dot00 * dot12 - dot01 * dot02) * inverDeno;
		if (v < 0 || v > 1) 
		{
			return false;
		}
		return u + v <= 1;
	}
}

