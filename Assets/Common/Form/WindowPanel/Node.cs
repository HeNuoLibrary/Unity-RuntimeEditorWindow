using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Newtonsoft.Json;

public enum NodeType
{
	Node,
	LeftNode,
	RightNode,
	MidleNode,
}
public class NodeData {
	public List<NodeData> nodeDatas=new List<NodeData>();

	public string nodeName;
	public string path;
	public DragDirection direction;

	public float width;
	public float height;

	public string localPosition=Vector3.one.ToString();
	public string localRotation = Vector4.one.ToString();
	public string localScale = Vector3.one.ToString();

	public string anchorMax=Vector2.one.ToString();
	public string anchorMin=Vector2.one.ToString();

	public string offsetMax=Vector2.one.ToString();
	public string offsetMin=Vector2.one.ToString();

	public string pivot = Vector2.one.ToString();

	public bool isFloatWindow;
	public int siblingIndex;
	public bool isPanel;

	public string[] lables;
}

public class Node : MonoBehaviour
{
	public DragDirection direction { get; private set; }//面板在兄弟面板中的关系
	public Node parentNode
	{
		get
		{
			return transform.parent.GetComponent<Node>();
		}
		
	}//父节点

	public RectTransform rectTransform
	{
		get
		{
			return GetComponent<RectTransform>();
		}
	}

	public bool isFloatWindow;//标记当前窗口是否是一个浮动窗口
	public bool isPanel;//标记当前的节点是个panel

	public Node brotherNode {
		get {

			if (parentNode!=null) {
				for (int i = 0; i < parentNode.rectTransform.childCount; i++)
				{

					Node node = parentNode.rectTransform.GetChild(i).GetComponent<Node>();
					if (node!=null&&node.direction!=direction) {

						
						return node;
					}

				}
			
			}
			return null;
		}
	}

    private void Start()
    {
		this.name = gameObject.GetInstanceID().ToString();
    }
    public NodeData ToNodeData() {
		NodeData nodeData = new NodeData();
		string path = GetAbsolutePath(transform);
		nodeData.nodeName = this.name;
		path = path.Substring(0, path.LastIndexOf('/'));
		nodeData.path= path;
		nodeData.direction=direction;

		nodeData.width=rectTransform.rect.width;
		nodeData.height= rectTransform.rect.height;

		nodeData.localPosition = rectTransform.localPosition.ToString();
		nodeData.localRotation = transform.localRotation.ToString();
		nodeData.localScale = transform.localScale.ToString();

		nodeData.anchorMax = rectTransform.anchorMax.ToString();
		nodeData.anchorMin = rectTransform.anchorMin.ToString();

		nodeData.offsetMax = rectTransform.offsetMax.ToString();

		nodeData.offsetMin = rectTransform.offsetMin.ToString();

		nodeData.pivot = rectTransform.pivot.ToString();

		nodeData.isFloatWindow = isFloatWindow;
		nodeData.siblingIndex = rectTransform.GetSiblingIndex();
		nodeData.isPanel = isPanel;
		if (isPanel) {
			nodeData .lables=GetComponent<WindowPanel>().panelTitle.lableRect.GetLables();
			
		}
		return nodeData;
	}

	public  void  SetData(NodeData nodeData) {
	
		rectTransform.SetParent( GameObject.Find(nodeData.path).transform);

		rectTransform.SetRectTransformSizeWithCurrentAnchors(nodeData.width,nodeData.height);
		rectTransform.localPosition = MathUtil.GetVector3(nodeData.localPosition);
		rectTransform.localRotation = MathUtil.GetQuaterion(nodeData.localRotation);
		rectTransform.localScale = MathUtil.GetVector3(nodeData.localScale);

		rectTransform.anchorMax = MathUtil.GetVector2(nodeData.anchorMax);
		rectTransform.anchorMin = MathUtil.GetVector2(nodeData.anchorMin);
		rectTransform.offsetMax = MathUtil.GetVector2(nodeData.offsetMax);
		rectTransform.offsetMin = MathUtil.GetVector2(nodeData.offsetMin);
		rectTransform.pivot = MathUtil.GetVector2(nodeData.pivot);

		this.isFloatWindow = nodeData.isFloatWindow;
		this.isPanel = nodeData.isPanel;
		
		direction = nodeData.direction;
	}

	public void UpdateRectransform()
	{
		transform.localScale = Vector3.one;
		Vector2 anchorMax = Vector2.one;
		Vector2 anchorMin = Vector2.zero;

		float scaleValue = 0;
		switch (direction)
		{
			case DragDirection.None:
				break;
			case DragDirection.Left:

				scaleValue = rectTransform.rect.width / parentNode.rectTransform.rect.width;

				anchorMax.x = scaleValue;
				break;
			case DragDirection.Right:
				scaleValue = rectTransform.rect.width / parentNode.rectTransform.rect.width;
				anchorMin.x = 1 - scaleValue;
				break;
			case DragDirection.Top:
				scaleValue = rectTransform.rect.height / parentNode.rectTransform.rect.height;
				anchorMin.y = 1 - scaleValue;
				break;
			case DragDirection.Bottom:
				scaleValue = rectTransform.rect.height / parentNode.rectTransform.rect.height;
				anchorMax.y = scaleValue;
				break;
			default:
				break;
		}



		rectTransform.anchorMax = anchorMax;
		rectTransform.anchorMin = anchorMin;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.offsetMin = Vector2.zero;
	}

	public void SetDirection(DragDirection direction)
	{

		this.direction = direction;
		this.name = direction.ToString();
		UpdateRectransform();
	}

	/// <summary>
	/// 找到拖动相关的两个节点
	/// </summary>
	/// <param name="dragDirection"></param>
	/// <param name="node"></param>
	/// <param name="leftOrTopNode"></param>
	/// <param name="rightOrBottomNode"></param>
	/// <returns></returns>
	public bool FindRelationNode(DragDirection dragDirection, Node node, out Node leftOrTopNode, out Node rightOrBottomNode)
	{
		leftOrTopNode = null;
		rightOrBottomNode = null;

		if (node == null)
		{
			Debug.LogError("node==null");
			return false;
		}

		//Debug.LogError(node.name+"  节点方向=" + node.direction+"   拖动方向="+ dragDirection);

		switch (node.direction)
		{
			case DragDirection.None:
				return false;

			case DragDirection.Left:

				//如果当前节点是在左边 拖动的方向在右边  ，那这个时候就是兄弟节点拖动

				if (dragDirection == DragDirection.Right)
				{
					leftOrTopNode = node;
					rightOrBottomNode = node.brotherNode;
					return true;

				}
				else
				{
					return FindRelationNode(dragDirection, node.parentNode, out leftOrTopNode, out rightOrBottomNode);
				}


			case DragDirection.Right:
				//如果当前节点是在右边   拖动的方向在左边  ，那这个时候就是兄弟节点拖动
				if (dragDirection == DragDirection.Left)
				{
					rightOrBottomNode = node;
					leftOrTopNode = node.brotherNode;
					return true;

				}
				else
				{
					return FindRelationNode(dragDirection, node.parentNode, out leftOrTopNode, out rightOrBottomNode);
				}

			case DragDirection.Top:
				//如果当前节点是在上面   拖动的方向在下面  ，那这个时候就是兄弟节点拖动
				if (dragDirection == DragDirection.Bottom)
				{
					leftOrTopNode = node;
					rightOrBottomNode = node.brotherNode;
					return true;

				}
				else
				{
					return FindRelationNode(dragDirection, node.parentNode, out leftOrTopNode, out rightOrBottomNode);
				}

			case DragDirection.Bottom:
				//如果当前节点是在下面边   拖动的方向在上边  ，那这个时候就是兄弟节点拖动
				if (dragDirection == DragDirection.Top)
				{
					rightOrBottomNode = node;
					leftOrTopNode = node.brotherNode;
					return true;

				}
				else
				{
					return FindRelationNode(dragDirection, node.parentNode, out leftOrTopNode, out rightOrBottomNode);
				}

			default:
				return false;


		}

	}



	public void FullParentNode()
	{
		Rect rect = new Rect();
		rect.position = parentNode.rectTransform.position;
		rect.size = parentNode.rectTransform.rect.size;
		UIUtil.SetRectTransformSizeWithCurrentAnchors(rectTransform, rect.size, rect.position);

		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			Transform node = transform.GetChild(i);
			node.SetParent(parentNode.rectTransform);
			node.SetAsFirstSibling();
		}

		DestroyImmediate(this.gameObject);
	}

	/// <summary>
	/// 获取Transform的绝对路径
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static string GetAbsolutePath(Transform obj)
	{
		if (obj.transform.parent == null)
		{
			return obj.name;
		}
		else
		{
			return GetAbsolutePath(obj.transform.parent) + "/" + obj.name;
		}
	}
}
