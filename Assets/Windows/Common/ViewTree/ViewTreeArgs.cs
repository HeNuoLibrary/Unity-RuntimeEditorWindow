using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTreeArgs 
{
  

    /// <summary>
    /// item��ʼ����ʱ����Ҫ���û�ʵ�ָ���ȥ����
    /// </summary>
    public class InitDataArgs : EventArgs
    {

        public IEnumerable ItemData
        {
            get;
            set;
        }

        public InitDataArgs()
        {

        }
    }
    /// <summary>
    /// item��ʼ����ʱ����Ҫ���û�ʵ�ָ���ȥ����
    /// </summary>
    public class ItemBindDataArgs : EventArgs
    {
        /// <summary>
        /// item�󶨵�������
        /// </summary>
        public object Item
        {
            get;
            private set;
        }
        //�Ƿ�������
        public bool canExpand
        {
            get;
            set;
        }
        //��ʾ������
        public string showContent
        {
            get;
            set;
        }
        public ItemBindDataArgs(object item)
        {
            Item = item;
        }

    }
    /// <summary>
    /// ��Itemչ����ʱ����Ҫ�û����ж���չ��������
    /// </summary>
    public class ItemExpandingArgs : EventArgs
    {
        /// <summary>
        /// item�󶨵�������
        /// </summary>
        public object Item
        {
            get;
            private set;
        }

        /// <summary>
        /// ʹ�ô�����ָ���������
        /// </summary>
        public IEnumerable Children
        {
            get;
            set;
        }

        public ItemExpandingArgs(object item)
        {
            Item = item;
        }
    }
    /// <summary>
    /// �ṹ�����ı�  ��ʱ����Ҫ����ȥ��������¼�
    /// </summary>
    public class StructuralChangeArgs : EventArgs
    {
        /// <summary>
        /// �û�ȥ�ж��Ƿ���Ҫ���Ƿ���Ҫ�����������
        /// </summary>
        public bool isHandle
        {
            get;
            set;
        }
        /// <summary>
        /// ��ǰͣ���ķ���
        /// </summary>
        public DropSibling dropSibling
        {
            get;
            private set;
        }
        /// <summary>
        /// ��ǰ�϶���Ŀ��
        /// </summary>

        public TreeItemBase dragItem
        {
            get;
            private set;
        }
        /// <summary>
        /// ��ǰͣ����Ŀ��
        /// </summary>
        public TreeItemBase droupItem
        {
            get;
            private set;
        }

        public StructuralChangeArgs(DropSibling dropSibling, TreeItemBase dragItem, TreeItemBase droupItem)
        {
            this.dropSibling = dropSibling;
            this.dragItem = dragItem;
            this.droupItem = droupItem;

        }
    }

}
