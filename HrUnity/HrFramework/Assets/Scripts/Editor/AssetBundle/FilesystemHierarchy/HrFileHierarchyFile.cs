using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hr.Editor.Hierarchy
{
    public class HrFileHierarchyFile
    {

        private HrFileItem m_sourceAsset;


        public int Depth
        {
            get
            {
                return m_sourceAsset.Depth;
            }
        }

        /// <summary>
        /// 在层级的底基层
        /// </summary>
        public int Row
        {
            get;
            set;
        }

        public string Name
        {
            get
            {
                return m_sourceAsset.Name;
            }
        }

        public Texture Icon
        {
            get
            {
                return m_sourceAsset.Icon;
            }
        }

        public bool Selected
        {
            get;
            set;
        }

        public HrFileItem FileItem
        {
            get
            {
                return m_sourceAsset;
            }
        }

        public HrFileHierarchyFile(HrFileItem file)
        {
            m_sourceAsset = file;
            file.FileHierarchyFile = this;
            
        }
    }
}
