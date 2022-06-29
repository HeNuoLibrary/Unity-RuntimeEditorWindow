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
	/// ��ǰ�����ͣ������
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
		Debug.LogError("������");
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
	/// ������
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
	/// ����Ƴ�
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
	/// ��ʼ��ק
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
	/// ��ק��
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="eventData"></param>
	private void OnDrag(TreeItemBase sender, PointerEventData eventData)
	{
		maskItem.SetPoistion(eventData.position);

	}
	/// <summary>
	/// ��ק����
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="eventData"></param>
	private void OnEndDrag(TreeItemBase sender, PointerEventData eventData)
	{
		dragItemBase = null;

		if (maskItem.dropSibling == DropSibling.None || droupItemBase == null || sender == droupItemBase)
		{
			///û���ں��ʵ��϶�λ�û��߷ŵ�λ�ú��϶���λ����ͬһ��
			maskItem.SetDropItemBase(null);
			return;
		}
		else
		{
			if (sender.IsContain(droupItemBase)) {
				maskItem.SetDropItemBase(null);
				//Debug.LogError("�����϶�����������");
				return;
			}
		}
	   ///���϶�������͸����� ȥ�����������
		StructuralChangeArgs structuralChangeArgs = new StructuralChangeArgs(maskItem.dropSibling, sender, droupItemBase);
		OnStructuralChangeArgs(structuralChangeArgs);
		///������ദ���������
		if (structuralChangeArgs.isHandle)
		{
			if (droupItemBase != null)
			{
				switch (maskItem.dropSibling)
				{
					case DropSibling.None:
						Debug.LogError("û��ͣ��");
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
							Debug.LogError("����Ϊ�ֵܣ�Ӧ����Ϊ��һ���ӽڵ�");
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
	/// ��ȡ��ǰ�ڵ���������нڵ㣬�����ӽڵ㣬���ӽڵ�
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
		//�Ӹ��ڵ����Ƴ�
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
			Debug.LogError((parent.Parent.ItemObj as GameObject).name + "����λ��==" + parent.Parent.IndexOf(parent));
			parent.Parent.AddChild(parent.Parent.IndexOf(parent), subtree);

		}
		
		//Debug.LogError("index==" + index);

		
			for (int i =0; i< subTreeList.Count; i++)
			{
				subTreeList[i].transform.SetSiblingIndex(index+i);
			}

		//�Ӹ��ڵ����Ƴ�




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
		//�Ӹ��ڵ����Ƴ�
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
			//Debug.LogError((parent.Parent.ItemObj as GameObject).name + "����λ��==" + parent.Parent.IndexOf(parent));
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
				//Debug.LogError("�޸�����"+(subTreeList[i].ItemObj as GameObject).name);
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
	/// ���һ������
	/// </summary>
	private void InsertSubTree(TreeItemBase parent, TreeItemBase subtree)
	{
		//�Ӹ��ڵ����Ƴ�
		if (subtree.Parent != null)
		{
			subtree.Parent.RemoveChild(subtree);
		}



		if (!parent.IsInitData)
		{
			//���û�г�ʼ���ȳ�ʼ������
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

		//�ҵ����������һ���ڵ�

		TreeItemBase lastTree = parent.FindLastNode(parent);

		int index = lastTree.transform.GetSiblingIndex();

		Debug.LogError("���һ�����ӵ�����" + (lastTree.ItemObj as GameObject).name + "   " + index);

		foreach (TreeItemBase item in subTreeList)
		{
			item.transform.SetSiblingIndex(++index);
		}

		parent.AddChild(parent.Childs.Count, subtree);

		UpdateChildRectOffset(parent);
	}
	/// <summary>
	/// ���϶��ṹ�ı��ʱ��������ȥ���д���������������ҽ��Ƿ�����ֶη���
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
			// ֱ���������
			FoldTreeItem(item);
		}

	}

	private void ItemExpanding(TreeItemBase item)
	{
		if (!item.IsInitData && item.CanExpand)
		{
			item.IsInitData = true;
			ItemExpandingArgs args = new ItemExpandingArgs(item.ItemObj);

			///û�������ʱ�����û������������
			OnExpand(args);
			IEnumerable children = args.Children;
			///�����ʱ����Ҫ��Ϊ������ӽڵ�  ���û�����ȥ����ӽڵ�

			if (children != null)
			{
				//����״̬Ϊչ��״̬
				foreach (object childItem in children)
				{

					TreeItem childTreeItem = (TreeItem)InstantiateTreeItemBase();
					//���ø��ڵ�

					childTreeItem.ItemObj = childItem;
					//childTreeItem.Parent = item;
					//�����ӽڵ�
					item.AddChild(item.Childs.Count, childTreeItem);

					DataBindItem(this, childTreeItem);
				}

			}

		}
		else
		{
			Debug.LogError("�Ѿ��󶨹��������" + item.CanExpand + "  " + item.IsInitData);
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
	/// ��ʼ������
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
	/// �����࿪�Ľӿڣ������������
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
	/// ������
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
	/// �����ӽڵ�λ������
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
	/// չ������
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

		///�ҵ����һ���ֵܽڵ�

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
