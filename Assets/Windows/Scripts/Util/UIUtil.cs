using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static System.Net.Mime.MediaTypeNames;
using static UnityEngine.RectTransform;
public enum DragDirection
{
	None,
	Left,
	Top,
	Right,
	Bottom,
	LeftTop,
	LeftBottom,
	RightTop,
	RightBottom,
}


public static class UIUtil
{

	#region  RectTransform相关方法 

	/// <summary>
	/// 设置RectTransform的宽高
	/// </summary>
	/// <param name="rectTransform"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	public static void SetRectTransformSizeWithCurrentAnchors(this RectTransform rectTransform, float width, float height)
	{
		if (rectTransform != null)
		{
			rectTransform.SetSizeWithCurrentAnchors(Axis.Horizontal, width);
			rectTransform.SetSizeWithCurrentAnchors(Axis.Vertical, height);
		}
	}



	/// <summary>
	/// 设置RectTransform的宽高位置
	/// </summary>
	/// <param name="rectTransform"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	public static void SetRectTransformSizeWithCurrentAnchors(this RectTransform rectTransform, Vector2 size, Vector3 pos)
	{
		if (rectTransform != null)
		{
			rectTransform.SetSizeWithCurrentAnchors(Axis.Horizontal, size.x);
			rectTransform.SetSizeWithCurrentAnchors(Axis.Vertical, size.y);

			
			rectTransform.position = pos;
			
		}
	}



	



	/// <summary>
	/// 获取RectTransform 四个角在屏幕上的坐标
	/// </summary>
	/// <param name="rectTransform"></param>
	/// <param name="uiCanvas">渲染UI的Canvas</param>
	/// <returns></returns>
	public static Vector3 GetRectTransformToScreenPosition(this RectTransform rectTransform, Canvas uiCanvas)
	{
		
		return uiCanvas.worldCamera.WorldToScreenPoint(rectTransform.position);
	}

	/// <summary>
	/// 获取RectTransform 四个角在屏幕上的坐标
	/// </summary>
	/// <param name="rectTransform"></param>
	/// <param name="uiCanvas">渲染UI的Canvas</param>
	/// <returns></returns>
	public static Vector3[] GetRectTransformToScreenPositionFromCorners(this RectTransform rectTransform, Canvas uiCanvas)
	{
		Vector3[] fourCornersArray = new Vector3[4];
		rectTransform.GetWorldCorners(fourCornersArray);
		if (uiCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
		{
			for (int i = 0; i < fourCornersArray.Length; i++)
			{
				fourCornersArray[i] = uiCanvas.worldCamera.WorldToScreenPoint(fourCornersArray[i]);
			}
		}
		return fourCornersArray;
	}
	/// <summary>
	/// 判断屏幕上的点是否在RectTransform里面
	/// </summary>
	/// <param name="rectTransform"></param>
	/// <param name="screenPoint"></param>
	/// <param name="uiCanvas">渲染UI的Canvas</param>
	/// <returns></returns>

	public static bool RectangleContainsScreenPoint(this RectTransform rectTransform, Vector2 screenPoint, Canvas uiCanvas = null)
	{
		if (uiCanvas == null)
		{
			

			return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint);
		}
		if (uiCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
		{
			return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint);
		}
		else
		{
			

			return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint, uiCanvas.worldCamera);
		}
	}


	/// <summary>
	/// 获取屏幕上的点在 Canvas 上的世界坐标
	/// </summary>
	/// <param name="screenPoint"></param>
	/// <param name="uiCanvas"></param>
	/// <returns></returns>
	public static Vector3 GetScreenPointToWorldPointInRectangle(Vector2 screenPoint, Canvas uiCanvas = null)
	{
		Vector3 worldPos;
		if (uiCanvas == null)
		{
			RectTransformUtility.ScreenPointToWorldPointInRectangle(uiCanvas.GetComponent<RectTransform>(), screenPoint, null, out worldPos);
			return worldPos;

		}
		if (uiCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
		{
			RectTransformUtility.ScreenPointToWorldPointInRectangle(uiCanvas.GetComponent<RectTransform>(), screenPoint, uiCanvas.worldCamera, out worldPos);


			
			return worldPos;
		}
		else
		{
			RectTransformUtility.ScreenPointToWorldPointInRectangle(uiCanvas.GetComponent<RectTransform>(), screenPoint, null, out worldPos);
			return worldPos;
		}
		
	}
	/// <summary>
	/// 获取屏幕点点在 RectTransform 上的本地坐标
	/// </summary>
	/// <param name="uiCanvas"></param>
	/// <returns></returns>

	public static Vector2 GetScreenPointToLocalPointInRectangle(this RectTransform rectTransform, Vector3 screenPoint, Canvas uiCanvas)
	{
		Vector2 worldPos;
		if (uiCanvas == null)
		{
			return Vector3.one;

		}
		if (uiCanvas.renderMode != RenderMode.ScreenSpaceOverlay)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, uiCanvas.worldCamera, out worldPos);
			return worldPos;
		}
		else
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, null, out worldPos);
			return worldPos;
		}
	}


	/// <summary>
	/// 强制刷新UI
	/// </summary>
	/// <param name="rectTransform"></param>
	public static IEnumerator ForceRebuildLayoutImmediate(this RectTransform rectTransform)
	{
		yield return new WaitForEndOfFrame();
		UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);


	}



	#endregion


}

