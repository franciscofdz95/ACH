using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


/// <summary>
/// Summary description for ListHandler
/// </summary>
public static class ListHandler
{
    public static ListItem GetListItem(ListControl lst, string strValue)
    {
        ListItem item = null;

        for (int i = 0; i < lst.Items.Count; i++)
        {
            item = lst.Items[i];
            if (item.Value.ToUpper() == strValue.ToUpper())
            {
                break;
            }
        }

        return item;
    }

    public static void ListFindItem(ListControl lst, string strValue)
    {
        lst.SelectedIndex = -1;

        for (int i = 0; i < lst.Items.Count; i++)
        {
            ListItem item = lst.Items[i];
            if (item.Value.ToUpper() == strValue.ToUpper())
            {
                lst.SelectedIndex = i;
                break;
            }
        }
    }

    public static string GetListSelectedItemValues(ListControl lst)
    {
        string list = string.Empty;

        foreach (ListItem item in lst.Items)
        {
            if (item.Selected && item.Value != "-1")
                list += item.Value + ",";
        }

        if (list != string.Empty)
            list = list.Substring(0, list.Length - 1);

        return list;
    }

    public static void SetListSelectedItemValues(ListControl lst, string SelectedList)
    {
        string[] items = SelectedList.Split(new char[] { ',' });

        lst.ClearSelection();
        foreach (string item in items)
        {
            foreach (ListItem i in lst.Items)
            {
                if (i.Value.ToUpper() == item.ToUpper())
                    i.Selected = true;
            }
        }

    }

    public static void FillListFromEnum<T>(ListControl myList)
    {
        myList.Items.Clear();

        Array itemValues = System.Enum.GetValues(typeof(T));
        Array itemNames = System.Enum.GetNames(typeof(T));

        for (int i = 0; i <= itemNames.Length - 1; i++)
        {
            ListItem item = new ListItem((itemNames as string[])[i], Convert.ToInt32((itemValues as int[])[i]).ToString());
            myList.Items.Add(item);
        }
    }

}
