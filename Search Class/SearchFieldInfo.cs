using System;
using System.Collections.Generic;
using System.Text;

namespace AchSystem
{
    public enum Search_Field_Type
    { 
        String,
        Date,
        Number,
        Int,
        Bool
    }

    class SearchFieldInfo
    {
        private string m_ParamterName = string.Empty;
        private Search_Field_Type m_FieldType;

        public SearchFieldInfo(string strParamName, Search_Field_Type pftType)
        {
            m_ParamterName = strParamName;
            m_FieldType = pftType;
        }

        public SearchFieldInfo(string strParamName, Search_Field_Type pftType, bool isKeyColumn)
        {
            m_ParamterName = strParamName;
            m_FieldType = pftType;
        }

        public string ParamterName
        {
            get { return m_ParamterName; }
            set { m_ParamterName = value; }
        }

        public Search_Field_Type FieldType
        {
            get { return m_FieldType; }
            set { m_FieldType = value; }
        }

  
    }
}
