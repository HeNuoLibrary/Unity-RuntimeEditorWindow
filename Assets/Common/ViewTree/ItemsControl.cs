using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ViewTreeArgs;

public class ItemsControl : MonoBehaviour
{

	public MaskItem maskItem;
	/// <summary>
	/// 当前鼠标悬停的物体
	/// </summary>
	private TreeItemBase droupItemBase;
	private TreeItemBase dragItemBase;


	private TreeItemBase selectedItemBase;



	private void Start()
	{
		
		InitDataBind();
	}

	private void OnEnable()
	{
		TreeItemBase.OnExpand += OnExpand;
		TreeItemBase.BeginDrag += OnBeginDrag;
		TreeItemBase.Drag += OnDrag;
		TreeItemBase.EndDrag += OnEndDrag;

		TreeItemBase.PointerEnter += OnItemPointerEnter;
		TreeItemBase.PointerExit += OnPointerExit;
		TreeItemBase.PointerDown += OnPointerDown;
		TreeItemBase.UnSelected += OnUnSelected;
		TreeItemBase.Selected += OnSelected;
	}


	private void OnDisable()
	{
		TreeItemBase.OnExpand -= OnExpand;
		TreeItemBase.BeginDrag -= OnBeginDrag;
		TreeItemBase.Drag -= OnDrag;
		TreeItemBase.EndDrag -= OnEndDrag;

		TreeItemBase.PointerEnter -= OnItemPointerEnter;
		TreeItemBase.PointerExit -= OnPointerExit;
		TreeItemBase.PointerDown -= OnPointerDown;
		TreeItemBase.UnSelected -= OnUnSelected;
		TreeItemBase.Selected -= OnSelected;
	}
	private void OnDestroy()
	{
		Debug.LogError("被销毁");
	}
	private void OnPointerDown(TreeItemBase sender, PointerEventData eventData)
	{
		sender.IsSelected = true;
		if (selectedItemBase != null)
		{
			selectedItemBase.IsSelected = false;
		}
		selectedItemBase = sender;
	}
	private void OnUnSelected(object sender, EventArgs eventData)
	{

	}
	private void OnSelected(object sender, EventArgs eventData)
	{

	}
	/// <summary>
	/// 鼠标进入
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="eventData"></param>
	private void OnItemPointerEnter(TreeItemBase sender, PointerEventData eventData)
	{
		if (!CanHandleEvent(sender))
		{
			return;
		}
		droupItemBase = sender;
		if (dragItemBase!=null&& dragItemBase!= sender) {
			maskItem.SetDropItemBase(droupItemBase);
		}
		
	}
	/// <summary>
	/// 鼠标移出
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="eventData"></param>
	private void OnPointerExit(TreeItemBase sender, PointerEventData eventData)
	{
		if (!CanHandleEvent(sender))
		{
			return;
		}
		droupItemBase = null;
		maskItem.SetDropItemBase(droupItemBase);
	}
	/// <summary>
	/// 开始拖拽
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="eventData"></param>
	private void OnBeginDrag(TreeItemBase sender, PointerEventData eventData)
	{
		selectedItemBase = sender;
		selectedItemBase.IsSelected = true;
		dragItemBase = sender;
	}
	/// <summary>
	/// 拖拽中
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="eventData"></param>
	private void OnDrag(TreeItemBase sender, PointerEventData eventData)
	{
		maskItem.SetPoistion(eventData.position);

	}
	/// <summary>
	/// 拖拽结束
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="eventData"></param>
	private void OnEndDrag(TreeItemBase sender, PointerEventData eventData)
	{
		dragItemBase = null;

		if (maskItem.dropSibling == DropSibling.None || droupItemBase == null || sender == droupItemBase)
		{
			///没有在合适的拖动位置或者放的位置和拖动的位置是同一个
			maskItem.SetDropItemBase(null);
			return;
		}
		else
		{
			if (sender.IsContain(droupItemBase)) {
				maskItem.SetDropItemBase(null);
				//Debug.LogError("不能拖动到子物体上");
				return;
			}
		}
	   ///将拖动情况发送给子类 去处理这种情况
		StructuralChangeArgs structuralChangeArgs = new StructuralChangeArgs(maskItem.dropSibling, sender, droupItemBase);
		OnStructuralChangeArgs(structuralChangeArgs);
		///如果子类处理这个事情
		if (structuralChangeArgs.isHandle)
		{
			if (droupItemBase != null)
			{
				switch (maskItem.dropSibling)
				{
					case DropSibling.None:
						Debug.LogError("没有停靠");
						break;
					case DropSibling.Child:
						InsertSubTree(droupItemBase, sender);

						break;
					case DropSibling.PrevSibling:
						InsertPrevSibling(droupItemBase, sender);

						break;
					case DropSibling.NextSibling:

						if (droupItemBase.IsExpand)
						{
							Debug.LogError("不作为兄弟，应该作为第一个子节点");
							InsertPrevSibling(droupItemBase.childs[0], sender);
						}
						else
						{
							InsertNextSibling(droupItemBase, sender);
						}
						break;
					default:
						break;
				}
			}
		}
		maskItem.SetDropItemBase(null);

	}

	/// <summary>
	/// 获取当前节点下面的所有节点，包括子节点，子子节点
	/// </summary>
	/// <param name="treeItemBase"></param>
	/// <param name="childs"></param>
	private void GetAllSubNode(TreeItemBase treeItemBase, ref List<TreeItemBase> childs)
	{
		childs.Add(treeItemBase);
		if (treeItemBase.Childs.Count > 0)
		{
			foreach (var item in treeItemBase.Childs)
			{
				GetAllSubNode(item, ref childs);
			}
		}

	}
	
	private void InsertNextSibling(TreeItemBase parent, TreeItemBase subtree)
	{
		//从父节点上移除
		if (subtree.Parent != null)
		{
			subtree.Parent.RemoveChild(subtree);
		}

		List<TreeItemBase> subTreeList = new List<TreeItemBase>();
		GetAllSubNode(subtree, ref subTreeList);

	

		foreach (var item in subTreeList)
		{
			item.transform.SetAsLastSibling();
		}

		TreeItemBase lastTree = parent.FindLastNode(parent);

		int index = lastTree.transform.GetSiblingIndex()+1;

		if (parent.Parent != null)
		{
			Debug.LogError((parent.Parent.ItemObj as GameObject).name + "插入位置==" + parent.Parent.IndexOf(parent));
			parent.Parent.AddChild(parent.Parent.IndexOf(parent), subtree);

		}
		
		//Debug.LogError("index==" + index);

		
			for (int i =0; i< subTreeList.Count; i++)
			{
				subTreeList[i].transform.SetSiblingIndex(index+i);
			}

		//从父节点上移除




		if (parent.Parent != null)
		{
			UpdateChildRectOffset(parent.Parent);

		}
		else
		{
			subtree.Parent = null;

			RectOffset rectOffset = new RectOffset(30, 0, 0, 0);
			subtree.GetComponentInChildren<HorizontalLayoutGroup>().padding = rectOffset;
			UpdateChildRectOffset(subtree);
		}

	}
	private void InsertPrevSibling(TreeItemBase parent, TreeItemBase subtree)
	{
		//从父节点上移除
		if (subtree.Parent != null)
		{
			subtree.Parent.RemoveChild(subtree);
		}

		List<TreeItemBase> subTreeList = new List<TreeItemBase>();
		GetAllSubNode(subtree, ref subTreeList);

		foreach (var item in subTreeList)
		{
			item.transform.SetAsLastSibling();
		}

		int index = parent.transform.GetSiblingIndex();
		if (parent.Parent != null)
		{
			//Debug.LogError((parent.Parent.ItemObj as GameObject).name + "插入位置==" + parent.Parent.IndexOf(parent));
			parent.Parent.AddChild(parent.Parent.IndexOf(parent), subtree);

		}
	

		//Debug.LogError("index==" + index);

		if (index <= 0)
		{

			for (int i = subTreeList.Count - 1; i >= 0; i--)
			{
				subTreeList[i].transform.SetAsFirstSibling();
			}

		}
		else
		{
			for (int i = subTreeList.Count - 1; i >= 0; i--)
			{
				//Debug.LogError("修改物体"+(subTreeList[i].ItemObj as GameObject).name);
				subTreeList[i].transform.SetSiblingIndex(index);
			}


		}


		if (parent.Parent != null)
		{
			UpdateChildRectOffset(parent.Parent);

		}
		else {
			subtree.Parent = null;
			RectOffset rectOffset = new RectOffset(30, 0, 0, 0);
			subtree.GetComponentInChildren<HorizontalLayoutGroup>().padding = rectOffset;
			UpdateChildRectOffset(subtree);
		}

	}
	/// <summary>
	/// 添加一个子树
	/// </summary>
	private void InsertSubTree(TreeItemBase parent, TreeItemBase subtree)
	{
		//从父节点上移除
		if (subtree.Parent != null)
		{
			subtree.Parent.RemoveChild(subtree);
		}



		if (!parent.IsInitData)
		{
			//如果没有初始化先初始化数据
			ItemExpanding(parent);
			parent.IsInitData = true;
		}
		parent.CanExpand = true;
		parent.IsExpand = true;

		List<TreeItemBase> subTreeList = new List<TreeItemBase>();
		GetAllSubNode(subtree, ref subTreeList);

		foreach (var item in subTreeList)
		{
			item.transform.SetAsLastSibling();
		}

		//找到子树的最后一个节点

		TreeItemBase lastTree = parent.FindLastNode(parent);

		int index = lastTree.transform.GetSiblingIndex();

		Debug.LogError("最后一个孩子的名字" + (lastTree.ItemObj as GameObject).name + "   " + index);

		foreach (TreeItemBase item in subTreeList)
		{
			item.transform.SetSiblingIndex(++index);
		}

		parent.AddChild(parent.Childs.Count, subtree);

		UpdateChildRectOffset(parent);
	}
	/// <summary>
	/// 当拖动结构改变的时候，让子类去自行处理这种情况，并且将是否处理的字段返回
	/// </summary>
	/// <param name="structuralChangeArgs"></param>
	protected virtual void OnStructuralChangeArgs(StructuralChangeArgs structuralChangeArgs)
	{

	}
	private void OnExpand(TreeItemBase item, PointerEventData eventData)
	{
		if (!item.CanExpand)
		{
			return;
		}
		if (!item.IsInitData)
		{
			ItemExpanding(item);
		}

		if (!item.IsExpand)
		{
			ExpandChildTree(item);

		}
		else
		{
			// 直接则叠起来
			FoldTreeItem(item);
		}

	}

	private void ItemExpanding(TreeItemBase item)
	{
		if (!item.IsInitData && item.CanExpand)
		{
			item.IsInitData = true;
			ItemExpandingArgs args = new ItemExpandingArgs(item.ItemObj);

			///没有子项的时候让用户自行添加数据
			OnExpand(args);
			IEnumerable children = args.Children;
			///点击的时候主要是为了填充子节点  让用户自行去添加子节点

			if (children != null)
			{
				//设置状态为展开状态
				foreach (object childItem in children)
				{

					TreeItem childTreeItem = (TreeItem)InstantiateTreeItemBase();
					//设置父节点

					childTreeItem.ItemObj = childItem;
					//childTreeItem.Parent = item;
					//设置子节点
					item.AddChild(item.Childs.Count, childTreeItem);

					DataBindItem(this, childTreeItem);
				}

			}

		}
		else
		{
			Debug.LogError("已经绑定过相关数据" + item.CanExpand + "  " + item.IsInitData);
		}

	}
	protected virtual void OnExpand(ItemExpandingArgs args)
	{


	}

	protected bool CanHandleEvent(object sender)
	{
		TreeItemBase treeItemBase = sender as TreeItemBase;
		if (!treeItemBase)
		{
			return false;
		}
		return treeItemBase.transform.IsChildOf(transform);
	}

	
	/// <summary>
	/// 初始化数据
	/// </summary>
	private void InitDataBind()
	{
		InitDataArgs datas = new InitDataArgs();
		InitData(datas);
		GameObject[] gameobjects = datas.ItemData.OfType<GameObject>().ToArray();
		for (int i = 0; i < gameobjects.Length; i++)
		{
			TreeItemBase treeItemBase = InstantiateTreeItemBase();
			treeItemBase.ItemObj = gameobjects[i];
			DataBindItem(this, treeItemBase);
		}

	}
	/// <summary>
	/// 给子类开的接口，子类自行添加
	/// </summary>
	/// <param name="itemBindDataArgs"></param>

	protected virtual void InitData(InitDataArgs itemBindDataArgs)
	{

	}

	protected virtual TreeItemBase InstantiateTreeItemBase()
	{
		GameObject treeItemObj = Resources.Load<GameObject>("ViewTree/treeItem");
		TreeItemBase treeItemBase = Instantiate(treeItemObj, transform).GetComponent<TreeItemBase>();
		return treeItemBase;
	}
	/// <summary>
	/// 绑定数据
	/// </summary>
	/// <param name="item"></param>
	/// <param name="treeItemBase"></param>
	private void DataBindItem(object item, TreeItemBase treeItemBase)
	{
		ItemBindDataArgs args = new ItemBindDataArgs(treeItemBase.ItemObj);
		DataBindItem(args);
		treeItemBase.CanExpand = args.canExpand;
		treeItemBase.InitData(args.showContent);
	}


	protected virtual void DataBindItem(ItemBindDataArgs args)
	{

	}

	/// <summary>
	/// 更新子节点位置缩进
	/// </summary>
	/// <param name="treeItemBase"></param>
	public void UpdateChildRectOffset(TreeItemBase treeItemBase) {
		for (int i = 0; i < treeItemBase.Childs.Count; i++)
		{
			treeItemBase.UpdateChildRectOffset(treeItemBase.Childs[i]);
			UpdateChildRectOffset(treeItemBase.Childs[i]);


		}
	}

	/// <summary>
	/// 展开子树
	/// </summary>
	/// <param name="treeItemBase"></param>
	public void ExpandChildTree(TreeItemBase treeItemBase)
	{
		
		treeItemBase.IsExpand = true;
		

		for (int i = 0; i < treeItemBase.Childs.Count; i++)
		{
			treeItemBase.UpdateChildRectOffset(treeItemBase.Childs[i]);
			if (treeItemBase.Childs[i].IsExpand)
			{
				ExpandChildTree(treeItemBase.Childs[i]);

			}

		}


	}


	public void FoldTreeItem(TreeItemBase treeItemBase)
	{
		treeItemBase.IsExpand = false;
		for (int i = 0; i < treeItemBase.Childs.Count; i++)
		{
			if (treeItemBase.Childs[i].CanExpand)
			{
				FoldChiidTreeItem(treeItemBase.Childs[i]);
			}
			else
			{
				treeItemBase.Childs[i].gameObject.SetActive(false);
			}
		}

		///找到最后一个兄弟节点

	}

	private void FoldChiidTreeItem(TreeItemBase child)
	{
		child.gameObject.SetActive(false);

		if (child.CanExpand)
		{
			for (int i = 0; i < child.Childs.Count; i++)
			{
				FoldChiidTreeItem(child.Childs[i]);

			}

		}

	}



}
