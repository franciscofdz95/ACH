using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Collections.Generic;
using PaymentXP.Facade;
using System.Linq;
using System.Text;


public partial class MerchantCategories : System.Web.UI.UserControl
{
    public LinkButton btnAdd
    {
        get { return lnkAdd; }
    }

    public Label labelName
    {
        get { return lblName; }
    }

    public Panel pnlGrid
    {
        get { return pnlGrd; }
    }

    public Panel pnl
    {
        get { return Panel1; }
    }

    public Panel pnlCat
    {
        get { return pnlCategories; }
    }

    public string MerchantAppUID
    {
        get { if (ViewState["MerchantAppUID"] != null) return ViewState["MerchantAppUID"].ToString(); else return string.Empty; }
        set { ViewState["MerchantAppUID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LookupTableHandler.LoadMerchantCategories(Category, false);

            //PXP-8237:Sanidhya start
            lblError.Text = String.Empty;
            if (UserSessions.CurrentMerchantApp != null)
            {
                //PXP-10236:Sanidhya
                if ((UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                {
                    LookupTableHandler.LoadCRMList(CRMList);
                    CRMList.Items.Add(new ListItem("Other", "0"));
                }
            }

            //PXP-8237:Sanidhya end
        }

        lnkAdd.Enabled = (MerchantAppUID != string.Empty);

        //PXP-6495 Rohit Thakur disable Add button If User is SS 
        if (UserSessions.CurrentMerchantApp != null)
        {
            string m_statusUID = UserSessions.CurrentMerchantApp.StatusUID;
            string m_statusName = UserSessions.CurrentMerchantApp.StatusName;
            if (MerchantAppUID != string.Empty && UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_SALES_SUPPORT)
            {
                if (m_statusName.StartsWith("SS") || (m_statusName.StartsWith("CU") && !m_statusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED)))
                {
                    lnkAdd.Enabled = true;
                }
                else
                {
                    lnkAdd.Enabled = false;
                }
            }

            //Add Code for PXP-8129 by Anuj
            bool nutraTrail = UserSessions.CurrentMerchantApp.IsNutraMerchant;
            if (nutraTrail && m_statusUID.ToUpper().Equals(Constants.QUEUESTATUS_MS_ACTIVE))
            {
                if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_CREDIT_UNDERWRITING.ToUpper() || UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_APPLICATION_BOARDING.ToUpper())
                {
                    lnkAdd.Visible = true;
                }
                else
                {
                    lnkAdd.Visible = false;
                }
            }

        }

        //PXP-6254 RThakur
        if (UserSessions.CurrentMerchantApp != null)
        {
              //Remove hard coded indexing value from grid by koshlendra start
            for (int colIndex = 0; colIndex < grdBusiness.Columns.Count; colIndex++)
            {
                if (grdBusiness.Columns[colIndex].HeaderText.ToString() == "Source")
                {
            if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST) ||
                UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB) ||
                UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS))
            {
                        grdBusiness.Columns[colIndex].Visible = true;
            }
            else
            {
                        grdBusiness.Columns[colIndex].Visible = false;
            }
        }
                
    }
            //Remove hard coded indexing value from grid by koshlendra end 

        }
    }

    protected void grdBusiness_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;
        string status = string.Empty;
        string Merchantuid = string.Empty;
        string assignto = string.Empty;
        GridViewRow grdRow = null;

        // basically, we just want to have a whitelist of valid commands.
        Dictionary<string, string> diValidCommands = new Dictionary<string, string>();

        diValidCommands.Add("EditMerchant", "ImageButton");
        diValidCommands.Add("UpdateMerchant", "ImageButton");
        diValidCommands.Add("CancelMerchant", "ImageButton");
        diValidCommands.Add("DeleteMerchant", "ImageButton");

        if (diValidCommands.ContainsKey(e.CommandName))
        {
            if (diValidCommands[e.CommandName] == "ImageButton")
            {
                ImageButton btn = (ImageButton)e.CommandSource;
                grdRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            }
            else
                return;
            DataMerchantServices objdataservice = new DataMerchantServices();
            Hashtable prms = new Hashtable();

            switch (e.CommandName)
            {
                case "UpdateMerchant":

                    prms = new Hashtable();
                    prms.Add("@RelationShipRecordID", Convert.ToInt32(grdBusiness.DataKeys[grdRow.RowIndex].Values["RelationShipRecordID"]));// Added for PXP-8431 by koshlendra                  
                    prms.Add("@MerchantAppUID", MerchantAppUID);
                    prms.Add("@MerchantServiceUID", grdBusiness.DataKeys[grdRow.RowIndex].Values["ServiceID"].ToString().ToUpper());
                    prms.Add("@Checked", true);
                    prms.Add("@OfficeID", (int)(UserSessions.CurrentMerchantApp.Office));
                    //PXP-8237 Sanidhya:Start
                    //PXP-10236:Sanidhya
                    if (grdRow.Cells[1].Text.ToUpper() == "CRM" && UserSessions.CurrentMerchantApp != null &&
                        (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))//cell Id updated for PXP-8431 by koshlendra
                    {
                        string CrmName = ((TextBox)grdBusiness.Rows[grdRow.RowIndex].FindControl("txtCrmName")).Text.Trim();
                        if (!FormDataCheck_UpdateBusinessRelationships(CrmName, grdBusiness.Rows[grdRow.RowIndex].Cells[3].Text))
                        {
                            return;
                        }
                        else
                        {
                            //START: Notify Compliance team when new CRM Added by Ali Khan for PXP-8804
                           MPSEmailTemplate emailTemplate = ComplianceFacade.CheckNotifyNewCRM(CrmName, UserSessions.CurrentMerchantApp.BusinessLegalName, UserSessions.CurrentMerchantApp.BusinessDBAName,
                                UserSessions.CurrentMerchantApp.ID, UserSessions.CurrentMerchantApp.StatusName, UserSessions.CurrentMerchantApp.AgentDBA);
                            if (emailTemplate != null)
                                FormHandler.SendEmail(emailTemplate.Subject, "", emailTemplate.Content, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, ConfigurationManager.AppSettings["NewCRMEmailTO"], ConfigurationManager.AppSettings["NewCRMEmailCC"], "", new Hashtable(), UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentMerchantApp.AgentUID);
                            //START: Notify Compliance team when new CRM Added by Ali Khan for PXP-8804
                            }
                        prms.Add("@Description", (CrmName));
                    }
                    else
                    {
                        prms.Add("@Description", ((TextBox)grdBusiness.Rows[grdRow.RowIndex].FindControl("txtContactName")).Text);
                    }
                    //PXP-8237 Sanidhya:Start
                    prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);

                    //Code added by amit for PXP-6205
                    if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST) || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS))
                    {
                        prms.Add("@isWoodForestApp", true);
                        prms.Add("@MerchantAppSevicesUID", grdBusiness.DataKeys[grdRow.RowIndex].Values["MerchantAppSevicesUID"].ToString());
                        prms.Add("@Source", "Zeus"); //PXP-6254 RThakur

                    }
                    else
                        prms.Add("@isWoodForestApp", false);
                    //PXP-10236:Sanidhya:Start
                    if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS))
                    {
                        prms.Add("@isBBVACompassApp", true);
                        prms.Add("@MerchantAppSevicesUID", grdBusiness.DataKeys[grdRow.RowIndex].Values["MerchantAppSevicesUID"].ToString());
                        prms.Add("@Source", "Zeus");
                    }
                    else
                        prms.Add("@isBBVACompassApp", false);
                    //PXP-10236:Sanidhya:End
                    objdataservice.UpdateMerchantServicesDetails(prms);
                    grdBusiness.EditIndex = -1;

                    BindCategories();
                    break;

                case "EditMerchant":
                    grdBusiness.EditIndex = grdRow.RowIndex;
                    BindCategories();
                    //PXP-8237 Sanidhya:Start
                    //PXP-10236:Sanidhya
                    if (grdRow.Cells[1].Text.ToUpper() == "CRM" && UserSessions.CurrentMerchantApp != null && 
                        (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))//cell Id updated for PXP-8431 by koshlendra
                    {
                        TextBox crmName = (TextBox)grdBusiness.Rows[grdRow.RowIndex].FindControl("txtCrmName");
                        TextBox textField = (TextBox)grdBusiness.Rows[grdRow.RowIndex].FindControl("txtContactName");
                        crmName.Visible = true;
                        textField.Visible = false;
                    }
                    else
                    {
                        TextBox crmName = (TextBox)grdBusiness.Rows[grdRow.RowIndex].FindControl("txtCrmName");
                        TextBox textField = (TextBox)grdBusiness.Rows[grdRow.RowIndex].FindControl("txtContactName");
                        crmName.Visible = false;
                        textField.Visible = true;

                    }
                    //PXP-8237 Sanidhya:End
                    break;

                case "CancelMerchant":

                    grdBusiness.EditIndex = -1;
                    BindCategories();
                    break;

                case "DeleteMerchant":

                    prms = new Hashtable();
                    string desc = string.Empty;

                    if (grdBusiness.EditIndex > -1)
                        desc = ((TextBox)grdBusiness.Rows[grdRow.RowIndex].FindControl("txtContactName")).Text;
                    else
                        desc = ((Literal)grdBusiness.Rows[grdRow.RowIndex].FindControl("litContactName")).Text;
                    prms.Add("@RelationShipRecordID", Convert.ToInt32(grdBusiness.DataKeys[grdRow.RowIndex].Values["RelationShipRecordID"]));// Added for PXP-8431 by koshlendra 
                    prms.Add("@MerchantAppUID", MerchantAppUID);
                    prms.Add("@MerchantServiceUID", grdBusiness.DataKeys[grdRow.RowIndex].Values["ServiceID"].ToString());
                    prms.Add("@Checked", false);
                    prms.Add("@Description", desc);
                    prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);

                    //Code added by amit for PXP-6205
                    if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST) || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)
                        || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS))
                    {
                        prms.Add("@isWoodForestApp", true);
                        prms.Add("@MerchantAppSevicesUID", grdBusiness.DataKeys[grdRow.RowIndex].Values["MerchantAppSevicesUID"].ToString());
                        prms.Add("@Source", "Zeus");//PXP-6254 RThakur
                    }
                    else
                        prms.Add("@isWoodForestApp", false);

                    //PXP-10236:Sanidhya:Start
                    if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS))
                    {
                        prms.Add("@isBBVACompassApp", true);
                        prms.Add("@MerchantAppSevicesUID", grdBusiness.DataKeys[grdRow.RowIndex].Values["MerchantAppSevicesUID"].ToString());
                        prms.Add("@Source", "Zeus");
                    }
                    else
                        prms.Add("@isBBVACompassApp", false);
                    //PXP-10236:Sanidhya:End

                    objdataservice.UpdateMerchantServicesDetails(prms);
                    //PXP-8409:Sanidhya
                    if (grdRow.Cells[1].Text.ToUpper() == "CRM") {
                        UserSessions.CurrentMerchantApp.CRMCount -=1;
                    }
                    grdBusiness.EditIndex = -1;

                    BindCategories();
                    break;
            }
        }
    }

    protected void grdBusiness_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        UserFacade userFacade = new UserFacade();
        var userRoles = userFacade.GetUser(UserSessions.CurrentUser.UID).UserRoles.Where(u => u.Value.Enabled == true);  // Dynamic list of enabled user roles;
        string m_statusName = UserSessions.CurrentMerchantApp.StatusName;
        string m_statusUID = UserSessions.CurrentMerchantApp.StatusUID;

        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                break;

            case DataControlRowType.DataRow:


                e.Row.Cells[3].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[3].Text);
                //Niranjan:PXP-9121
                if (e.Row.Cells[5].Text == "True")
                {
                    e.Row.Cells[5].Text = "Yes";
                }
                else if (e.Row.Cells[5].Text == "False")
                {
                    e.Row.Cells[5].Text = "No";
                }
                else
                {
                    e.Row.Cells[5].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[5].Text);
                }
                if (grdBusiness.EditIndex != e.Row.RowIndex || grdBusiness.EditIndex == -1)
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                }

                //PXP-6495
                if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS)
                {
                    if (grdBusiness.EditIndex != e.Row.RowIndex || grdBusiness.EditIndex == -1)
                    {
                        ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                        ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                        ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: none;");
                        //SS team should be able to update before CU_Approved status
                        if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_SALES_SUPPORT)
                        {
                            if (m_statusName.StartsWith("SS") || (m_statusName.StartsWith("CU") && !m_statusUID.ToUpper().Equals(Constants.QUEUESTATUS_CU_APPROVED))
                                )
                            {
                                ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");
                            }
                            else
                            {
                                ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                            }
                        }
                        else
                        {
                            ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");
                        }
                    }
                    else
                    {
                        ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: inline;");
                        ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: inline;");
                        //Only SS team has access to delete before CU_Received status
                        if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_SALES_SUPPORT)
                        {
                            if (!IsMerchantStatusHistory_CU() && m_statusName.StartsWith("SS"))
                            {
                                ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: inline;");
                            }
                            else
                            {
                                ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: none;");
                            }
                        }
                        else
                        {
                            ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: inline;");
                        }
                        ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                    }
                }


                //Add Code for PXP-8129 by Anuj
                bool nutraTrail = UserSessions.CurrentMerchantApp.IsNutraMerchant;
                if (nutraTrail && m_statusUID.ToUpper().Equals(Constants.QUEUESTATUS_MS_ACTIVE))
                {
                    if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_CREDIT_UNDERWRITING.ToUpper() || UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_APPLICATION_BOARDING.ToUpper())
                    {
                        if (grdBusiness.EditIndex != e.Row.RowIndex || grdBusiness.EditIndex == -1)
                        {
                            ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                            ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                            ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");
                            if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST
                                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS)
                            {
                                ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: none;");
                            }
                            else
                            {

                                ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: inline;");
                            }
                        }
                        else
                        {
                            ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: inline;");
                            ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: inline;");
                            ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                            if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
                                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS)
                            {
                                if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == Constants.ROLE_SALES_SUPPORT)
                                {
                                    if (!IsMerchantStatusHistory_CU() && m_statusName.StartsWith("SS"))
                                    {
                                        ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: inline;");
                                    }
                                    else
                                    {
                                        ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: none;");
                                    }
                                }
                                else
                                {
                                    ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: inline;");
                                }
                            }
                            else
                            {
                                ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: none;");
                            }
                        }
                    }
                    else
                    {
                        ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                        ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                        ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                        ((ImageButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: none;");
                        lnkAdd.Visible = false;
                    }
                }

               


                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                CheckBox chk = (CheckBox)e.Row.FindControl("chkChecked");
                chk.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Checked"));

                TextBox txt = (TextBox)e.Row.FindControl("text");
                txt.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();

                break;
            default:
                break;
        }
    }
    protected string GetSubmitPostBack()
    {
        return Page.ClientScript.GetPostBackEventReference(btnSaveB, string.Empty);
    }

    protected void btnSaveB_Click(object sender, EventArgs e)
    {
        //PXP-8429 Sanidhya
        if (!this.FormDataCheck())
        {
            WebDialogWindow4.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }
        else
        {
            Hashtable prms;
            DataMerchantServices MerchantServices = new DataMerchantServices();

            prms = new Hashtable();
            //Code added by amit for PXP-6205
            if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS))
            {
                prms.Add("@isWoodForestApp", true);
                prms.Add("@MerchantAppSevicesUID", Guid.NewGuid().ToString());
                prms.Add("@Source", "Zeus"); //PXP-6254 RThakur
            }
            else
                prms.Add("@isWoodForestApp", false);
            //End Code added by amit for PXP-6205

            //PXP-10236:Sanidhya:Start
            if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS))
            {
                prms.Add("@isBBVACompassApp", true);
                prms.Add("@MerchantAppSevicesUID", Guid.NewGuid().ToString());
                prms.Add("@Source", "Zeus"); 
            }
            else
                prms.Add("@isBBVACompassApp", false);
            //PXP-10236:Sanidhya:End

            prms.Add("@MerchantServiceUID", Category.SelectedValue);
            prms.Add("@MerchantAppUID", DataLayer.UID2Field(MerchantAppUID));
            //PXP-8237 Sanidhya:Start
            //PXP-10236:Sanidhya
            if (Category.SelectedItem.Text.ToUpper() == "CRM")
            {
                if (UserSessions.CurrentMerchantApp != null && 
                    (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
                    || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
                {
                    if (CRMList.Visible && CRMList.SelectedValue != "-1")
                    {
                        Notes.Text = CRMList.SelectedItem.Text;
                        prms.Add("@CRMID", CRMList.SelectedValue); //DM-2403
                    }
                    // PXP-8618 Rohit Thakur : start
                    else if (CRMList.Visible && CRMList.SelectedValue == "1")
                    {
                        //START: Notify Compliance team when new CRM Added by Ali Khan for PXP-8804
                        MPSEmailTemplate emailTemplate = ComplianceFacade.CheckNotifyNewCRM(Notes.Text, UserSessions.CurrentMerchantApp.BusinessLegalName, UserSessions.CurrentMerchantApp.BusinessDBAName,UserSessions.CurrentMerchantApp.ID, UserSessions.CurrentMerchantApp.StatusName, UserSessions.CurrentMerchantApp.AgentDBA);
                        if (emailTemplate != null)
                            FormHandler.SendEmail(emailTemplate.Subject, "", emailTemplate.Content, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, ConfigurationManager.AppSettings["NewCRMEmailTO"], ConfigurationManager.AppSettings["NewCRMEmailCC"], "", new Hashtable(), UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentMerchantApp.AgentUID);
                        //END: Notify Compliance team when new CRM Added by Ali Khan for PXP-8804
                    }
                    // PXP-8618 Rohit Thakur : end
                    ListHandler.ListFindItem(CRMList, "-1");
                    CRMListRow.Visible = false;
                    NotesSection.Visible = true;
                    OtherCRMLabel.Visible = false;
                    CRMLabel.Visible = true;
                    //PXP-8638:Sanidhya
                    TppRow.Visible = false;
                    AcceptTransRow.Visible = false;
                }
            //PXP-8237 Sanidhya:End
            prms.Add("@Description", Notes.Text.Trim());
            prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
            prms.Add("@Checked", true);
            MerchantServices.UpdateMerchantServicesDetails(prms);
                //PXP-8409:Sanidhya
                UserSessions.CurrentMerchantApp.CRMCount +=1;
            }
            else {
            prms.Add("@Description", Notes.Text.Trim());
            prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
            prms.Add("@Checked", true);
            MerchantServices.UpdateMerchantServicesDetails(prms);
            }


            ListHandler.ListFindItem(Category, "-1");
            Notes.Text = "";
            WebDialogWindow4.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
            FormServices();
        }
    }

    protected void btnCancelB_Click(object sender, EventArgs e)
    {
        //PXP-8429,8237:Sanidhya
        lblError.Text = String.Empty;
        ListHandler.ListFindItem(Category, "-1");
        if (CRMListRow.Visible)
        {
            ListHandler.ListFindItem(CRMList, "-1");
            CRMListRow.Visible = false;
            NotesSection.Visible = true;
            OtherCRMLabel.Visible = false;
            CRMLabel.Visible = true;
            //PXP-8638:Sanidhya
            TppRow.Visible = false;
            AcceptTransRow.Visible = false;
        }
        Notes.Text = "";
        WebDialogWindow4.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        //PXP-8429:Sanidhya
        lblError.Text = String.Empty;
        Notes.Text = string.Empty;
        ListHandler.ListFindItem(Category, "-1");
        WebDialogWindow4.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    public void BindCategories()
    {
        if (MerchantAppUID != string.Empty)
        {
            DataMerchantServices objServices = new DataMerchantServices();
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", MerchantAppUID);
            prms.Add("@CategoryID", 6);
            prms.Add("@Checked", true);

            DataSet ds = objServices.GetMerchantServicesDetails(prms);

            grdBusiness.DataSource = ds;
            grdBusiness.DataBind();

        }
    }

    public void UpdateServices()
    {
        Hashtable prms;
        DataMerchantServices objServices = new DataMerchantServices();

        foreach (GridViewRow grdRow in grd1.Rows)
        {
            prms = new Hashtable();
            prms.Add("@MerchantServiceUID", DataLayer.UID2Field(grd1.DataKeys[grdRow.RowIndex].Values["MerchantServiceID"].ToString()));
            prms.Add("@MerchantAppUID", DataLayer.UID2Field(MerchantAppUID));
            prms.Add("@Checked", ((CheckBox)grdRow.Cells[0].FindControl("chkChecked")).Checked);
            prms.Add("@Description", ((TextBox)grdRow.Cells[2].FindControl("text")).Text);
            prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
            prms.Add("@Source", "Zeus"); //PXP-6254 RThakur

            objServices.UpdateMerchantServicesDetails(prms);
        }

        foreach (GridViewRow grdRow in grd2.Rows)
        {
            prms = new Hashtable();
            prms.Add("@MerchantServiceUID", DataLayer.UID2Field(grd2.DataKeys[grdRow.RowIndex].Values["MerchantServiceID"].ToString()));
            prms.Add("@MerchantAppUID", DataLayer.UID2Field(MerchantAppUID));
            prms.Add("@Checked", ((CheckBox)grdRow.Cells[0].FindControl("chkChecked")).Checked);
            prms.Add("@Description", ((TextBox)grdRow.Cells[2].FindControl("text")).Text);
            prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
            prms.Add("@Source", "Zeus"); //PXP-6254 RThakur

            objServices.UpdateMerchantServicesDetails(prms);
        }
    }

    public void FormServices()
    {
        grd1.DataBind();
        grd2.DataBind();

        BindCategories();
    }

    protected void odsMerchants1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", DataLayer.UID2Field(MerchantAppUID));
        prms.Add("@CategoryID", 7);
        e.InputParameters[0] = prms;
    }

    protected void odsMerchants2_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", DataLayer.UID2Field(MerchantAppUID));
        prms.Add("@CategoryID", 8);
        e.InputParameters[0] = prms;
    }

    /// <summary>PXP-6495
    /// Check if application has ever been set to CU_Received. Check the history of the application.
    /// </summary>
    /// <returns></returns>
    private bool IsMerchantStatusHistory_CU()
    {
        bool retVal = false;

        DataMerchantApp dataMerchantApp = new DataMerchantApp();
        Hashtable hs = new Hashtable();

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
        prms.Add("@StatusUID", Constants.QUEUESTATUS_CU_RECEIVED);

        DataSet ds = dataMerchantApp.GetMerchantStatusHistory(prms);

        if (ds.Tables[0].Rows.Count > 0)
            retVal = true;

        return retVal;
    }
    /// <summary>
    /// PXP-8237 by Sanidhya
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void onCategorylistChanged(object sender, EventArgs e)
    {
        DropDownList ddlList = (DropDownList)sender;
        string selectedItem = ddlList.SelectedItem.Text;
        if (selectedItem.ToUpper() == "CRM")
        {
            //PXP-10236:Sanidhya
            if (UserSessions.CurrentMerchantApp != null && 
                (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
            {
                CRMListRow.Visible = true;
                NotesSection.Visible = false;
            }

        }
        else
        {
            CRMListRow.Visible = false;
            NotesSection.Visible = true;
            OtherCRMLabel.Visible = false;
            CRMLabel.Visible = true;
            //PXP-8638:Sanidhya
            TppRow.Visible = false;
            AcceptTransRow.Visible = false;
        }

    }
    /// <summary>
    /// PXP-8237 by Sanidhya
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void onCRMListChanged(object sender, EventArgs e)
    {
        DropDownList ddlList = (DropDownList)sender;
        string selectedItem = ddlList.SelectedItem.Value;
        //PXP-8638:Sanidhya
        if (selectedItem != "0" && selectedItem != "-1") {
            TppRow.Visible = true;
            AcceptTransRow.Visible = true;
            Hashtable prms = new Hashtable();
            DataApp _dataApp = new DataApp();
            var _crmData = _dataApp.GetCRMList(prms).FirstOrDefault(x => x.Type.ToUpper() == Constant.CRM_Type && x.CRMID.ToString() == selectedItem);
            tpp_flaglbl.Text = _crmData.TPPCertifiedFlag;
            //Niranjan: PXP-9121
            if (_crmData.AcceptTransactions.ToString() == "True")
            {
                accept_translbl.Text = "Yes";
            }
            else if (_crmData.AcceptTransactions.ToString() == "False")
            {
                accept_translbl.Text = "No";
            }
            else
            { accept_translbl.Text = _crmData.AcceptTransactions.ToString(); }
            NotesSection.Visible = false;
        }
        else if (selectedItem == "1")
        {
            //PXP-10236:Sanidhya
            NotesSection.Visible = true;
            if (UserSessions.CurrentMerchantApp != null && 
                (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
                 || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
            {
                OtherCRMLabel.Visible = true;
                CRMLabel.Visible = false;
            }
            TppRow.Visible = false;
            AcceptTransRow.Visible = false;
        }
        else {
            TppRow.Visible = false;
            AcceptTransRow.Visible = false;
        }

    }
    /// <summary>
    /// PXP-8237 Sanidhya
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void WebDialogWindow1_StateChanged(object sender, Infragistics.Web.UI.LayoutControls.DialogWindowStateChangedEventArgs e)
    {
        if (e.NewState == Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden)
        {
            ListHandler.ListFindItem(CRMList, "-1");
            CRMListRow.Visible = false;
            NotesSection.Visible = true;
            OtherCRMLabel.Visible = false;
            CRMLabel.Visible = true;
            //PXP-8638:Sanidhya
            TppRow.Visible = false;
            AcceptTransRow.Visible = false;
        }
    }
    /// <summary>
    /// PXP-8429:Sanidhya
    /// </summary>
    /// <returns></returns>

    public bool FormDataCheck()
    {
        string message = string.Empty;
        if (Category.SelectedValue == "-1")
        {
            message += "Please Select Category." + Environment.NewLine;
        }
        //PXP-10236:Sanidhya
        if (UserSessions.CurrentMerchantApp != null && 
            (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
        {

            if (CRMList.Visible && CRMList.SelectedValue == "1")
            {
                if (string.IsNullOrEmpty(Notes.Text))
                {
                    message += "Please Enter Other CRM/Notes." + Environment.NewLine;
                }
                //PXP-8618 RThakur check if duplicate CRM present in the Ref_CRM and MerchantAppSevices table
                DataCompliance dataComp = DataAccess.DataComplianceDao;
                Hashtable prms_IsExistsTPP = new Hashtable();
                prms_IsExistsTPP.Add("@TPPName", Notes.Text.Trim());
                prms_IsExistsTPP.Add("@Type", "CRM");
                DataMerchantApp dataApp = DataAccess.DataMerchantAppDao;
                Hashtable prms_dataApp = new Hashtable();
                prms_dataApp.Add("@Description", Notes.Text.Trim());
                prms_dataApp.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
                //PXP-9153 by sanidhya
                if ((UserSessions.CurrentMerchantApp.CRMCount >= 1) && UserSessions.CurrentMerchantApp.IsNutraMerchant && !(UserSessions.CurrentMerchantApp.MerchantAppUID.ToUpper() == Constants.BANK_ACH_ONLY.ToUpper() && UserSessions.CurrentMerchantApp.AchID > 0))
                {
                    message += "Merchant should have only one CRM record.";
                }
                if (dataComp.IsExistsTTPName(prms_IsExistsTPP))
                {
                    message += "CRM name : " + Notes.Text + " already exists in the dropdown.";
                }
                if (dataApp.IsExistsMerchantAppServiceName(prms_dataApp))
                {
                    message += "CRM name : " + Notes.Text + " already exists.";
                }
            }//PXP-8072 by Sanidhya
            else if (CRMList.Visible && CRMList.SelectedValue != "-1")
            {
                DataMerchantApp dataApp = DataAccess.DataMerchantAppDao;
                Hashtable prms_dataApp = new Hashtable();
                prms_dataApp.Add("@CRMID", CRMList.SelectedValue);
                prms_dataApp.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
                //PXP-9153 by sanidhya
             if ((UserSessions.CurrentMerchantApp.CRMCount >= 1) && UserSessions.CurrentMerchantApp.IsNutraMerchant && !(UserSessions.CurrentMerchantApp.MerchantAppUID.ToUpper() == Constants.BANK_ACH_ONLY.ToUpper() && UserSessions.CurrentMerchantApp.AchID > 0))
                {
                message += "Merchant should have only one CRM record.";
                }
                if (dataApp.IsExistsMerchantAppServiceName(prms_dataApp))
                {
                    message += "CRM name : " + CRMList.SelectedItem.Text + " already exists.";
                }
            }
            else if (CRMList.Visible && CRMList.SelectedValue == "-1")
            {
                message += "Please Select At Least One CRM." + Environment.NewLine;
            }
            else if (!CRMList.Visible && Category.SelectedValue != "-1" && string.IsNullOrEmpty(Notes.Text))
            {
                message += "Please Enter Notes." + Environment.NewLine;
            }

        }
        else {
            if (Category.SelectedValue != "-1" && string.IsNullOrEmpty(Notes.Text))
            {
                message += "Please Enter Notes." + Environment.NewLine;
            }
        }

        if (message == string.Empty)
        {
            return true;
        }
        else
        {
            lblError.Text = message.Replace(Environment.NewLine, "<br/>"); ;
            return false;
        }
    }

    //PXP-8618 RThakur
    public bool FormDataCheck_UpdateBusinessRelationships(string crm_details, string crmVendorUID)
    {
        string message = string.Empty;
        //PXP-10236:Sanidhya
        if (UserSessions.CurrentMerchantApp != null && 
            (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS)
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)) && UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Irvine))
        {
            if (string.IsNullOrEmpty(HttpUtility.HtmlDecode(crmVendorUID).Trim()))
            {
                if (string.IsNullOrEmpty(crm_details))
                {
                    message += "Please Enter Other CRM/Notes." + Environment.NewLine;
                }
            }
            DataMerchantApp dataApp = DataAccess.DataMerchantAppDao;
            Hashtable prms_dataApp = new Hashtable();
            prms_dataApp.Add("@Description", crm_details);
            prms_dataApp.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
            if (dataApp.IsExistsMerchantAppServiceName(prms_dataApp))
            {
                message += "CRM name : " + crm_details + " already exists.";
            }
        }
        if (message == string.Empty)
        {
            return true;
        }
        else
        {
            ScriptManager.RegisterStartupScript(pnlBusiness, pnlBusiness.GetType(), "Notification", "alert(" + '"' + message + '"' + ");", true);
            return false;
        }
    }

}
