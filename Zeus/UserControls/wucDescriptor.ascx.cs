using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;
using System.Data;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucDescriptor : System.Web.UI.UserControl
    {

        public static int OptimalMaxDescriptorLength = 50;

        public GridView grdDesc
        {
            get { return grdDescriptor; }
        }


        public DropDownList ddpPageSize
        {
            get { return cboPageSize; }
        }

        //public delegate void ButtonClickHandler(object sender, EventArgs e);
        //public event ButtonClickHandler ButtonClick;

        //public MerchantDescriptorTypeID DescriptorTypeID
        //{
        //    get { if (ViewState["DescriptorTypeID"] != null) return (MerchantDescriptorTypeID)ViewState["DescriptorTypeID"]; else return MerchantDescriptorTypeID.General; }
        //    set { ViewState["DescriptorTypeID"] = value; }
        //}

        public bool ShowValidators
        {
            get { if (ViewState["ShowValidators"] != null) return (bool)ViewState["ShowValidators"]; else return false; }
            set { ViewState["ShowValidators"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorDes.Text = "";
            //if (ShowValidators)
            //{
            WebDialogWindow3.Header.CaptionText = "Descriptor";
            btnAdd.Text = "Add Descriptor";
            CreditDescriptor.Visible = false;
            //}
            //else
            //{
            //    WebDialogWindow3.Header.CaptionText = "High Risk Descriptor";
            //    btnAdd.Text = "Add";
            //}
            if (!IsPostBack)
            {
                LoadValidators();
                LookupTableHandler.LoadDescriptorTypes(DescriptorTypeID, false);
                //pnlRecords.Visible = ShowValidators;

                cboPageSize.SelectedIndex = 0;
                grdDescriptor.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
                grdDescriptor.PageIndex = 0;

                // show/hide the information panel
                pnlMeritusIrvineOnly.Visible = (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.Office== CommonUtility.Util.Offices.Irvine && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus) ? true : false;  //pxp-3736
                pnlMeritus.Visible = (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.Office != CommonUtility.Util.Offices.Irvine && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus) ? true : false;
                pnlOptimal.Visible = (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal) ? true : false;
                this.btnValidate.Visible = (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus) ? true : false;
                // show hide the 2nd line
                phOptimal.Visible = (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal) ? true : false;

                // change the descriptor description if optimal
                if (UserSessions.CurrentMerchantApp != null && UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal)
                {
                    litDescr.Text = "Descriptor Line 1";
                    ValDescriptor.MaxLength = OptimalMaxDescriptorLength;
                    ValDescriptorLine2.MaxLength = OptimalMaxDescriptorLength;

                    // disable the client side checking if optimal.
                    Save.OnClientClick = null;
                }

            }

            grdDescriptor.Columns[0].Visible = ShowValidators;
            pnlAdd.Visible = ShowValidators;
        }


        public void LoadValidators()
        {
            DataMerchantApp data = new DataMerchantApp();
            DataSet ds = new DataSet();

            if (UserSessions.CurrentMerchantApp != null)
            {
                ds = data.GetMerchantDescriptors(UserSessions.CurrentMerchantApp.ID);
                grdDescriptor.DataSource = ds;
                grdDescriptor.DataBind();

                lblRecordCount.Text = string.Format("Total Records Found: {0}", ds.Tables[0].Rows.Count.ToString());
            }

        }

        protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdDescriptor.PageIndex = 0;
            grdDescriptor.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
            this.LoadValidators();
        }
        /// <summary>
        /// Event added for PXP-13483 by koshlendra 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DescriptorTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine && DescriptorTypeID.SelectedValue.Trim() == "2")
                Session["ValidaionCount"] = null;
                
           
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            WebDialogWindow3.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ValDescriptor.Text = "";
            ValDescriptorLine2.Text = "";
            ListHandler.ListFindItem(DescriptorTypeID, "-1");
            WebDialogWindow3.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
        }

        public bool IsValidForm()
        {
            // start it off as good.
            bool ret = true;

            // clear the error
            lblErrorDes.Text = "";

            // every descriptor needs a type!!
            if (DescriptorTypeID.SelectedValue.Trim() == "-1")
            {
                ListHandler.ListFindItem(DescriptorTypeID, "-1");
                lblErrorDes.ForeColor = System.Drawing.Color.Red;
                lblErrorDes.Text = "Select a descriptor type.\n";
                ret = false;
            }

            // regardless of brand, when adding, descriptor must not be empty.
            if (ValDescriptor.Text.Trim() == string.Empty)
            {
                lblErrorDes.ForeColor = System.Drawing.Color.Red;
                lblErrorDes.Text = "Descriptor cannot be blank.\n";
                ret = false;
            }

            // run through correct descriptor logic/rules.
            if (ret)
            {
                if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus)
                {
                    // remove the format restriction for Irvine office only. Please refer to PXP-3736. 
                    if (UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
                    {
                       //PXP-13243 code change done for HR Descriptor Validation at 17 Characters by koshlendra on 03/09/2020  start
                        if (ValDescriptor.Text != string.Empty )
                        {
                            if (DescriptorTypeID.SelectedValue.Trim() == "2" && !CommonUtility.Validation.IsAlphaNumericSpaceSpecialChars(ValDescriptor.Text, 22))
                            {
                                lblErrorDes.ForeColor = System.Drawing.Color.Red;
                                lblErrorDes.Text = "Enter a valid descriptor.\n";
                                //Code changes done for larry observations by koshlendra for PXP-13335 start
                                grdExistingDescriptors.EmptyDataText = "";
                                grdExistingDescriptors.DataBind();
                                //Code changes done for larry observations by koshlendra for PXP-13335 end
                                ret = false;
                            }
                            if ( ValDescriptor.Text.Length > 22)
                            {
                                lblErrorDes.ForeColor = System.Drawing.Color.Red;
                                lblErrorDes.Text = "Enter a valid descriptor.\n";
                                //Code changes done for larry observations by koshlendra for PXP-13335 start
                                grdExistingDescriptors.EmptyDataText = "";
                                grdExistingDescriptors.DataBind();
                                //Code changes done for larry observations by koshlendra for PXP-13335 end
                                ret = false;
                            }
                                                       
                        }
                        else
                        {
                            lblErrorDes.ForeColor = System.Drawing.Color.Red;
                                lblErrorDes.Text = "Enter a valid descriptor.\n";
                                //Code changes done for larry observations by koshlendra for PXP-13335 start
                                grdExistingDescriptors.EmptyDataText = "";
                                grdExistingDescriptors.DataBind();
                                //Code changes done for larry observations by koshlendra for PXP-13335 end
                                ret = false;
                        }
                        //PXP-13243 code change done for HR Descriptor Validation at 17 Characters by koshlendra on 03/09/2020  End
                    }

                    // for meritus merchants, we must run it through our meritus descriptor logic
                    else if (ValDescriptor.Text == string.Empty || !CommonUtility.Validation.IsValidMerchantDescriptor(ValDescriptor.Text, 21))
                    {
                        lblErrorDes.ForeColor = System.Drawing.Color.Red;
                        lblErrorDes.Text = "Enter a valid descriptor.\n";
                        //Code changes done for larry observations by koshlendra for PXP-13335 start
                        grdExistingDescriptors.EmptyDataText="";
                        grdExistingDescriptors.DataBind();
                        //Code changes done for larry observations by koshlendra for PXP-13335 end
                        ret = false;
                    }
                }
                else if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal)
                {
                    if (ValDescriptor.Text.Length > OptimalMaxDescriptorLength || ValDescriptorLine2.Text.Length > OptimalMaxDescriptorLength)
                    {
                        lblErrorDes.ForeColor = System.Drawing.Color.Red;
                        lblErrorDes.Text = "Enter a valid descriptor.\n";
                        //Code changes done for larry observations by koshlendra for PXP-13335 start
                        grdExistingDescriptors.EmptyDataText = "";
                        grdExistingDescriptors.DataBind();
                        //Code changes done for larry observations by koshlendra for PXP-13335 end
                        ret = false;
                    }
                }
            }

            // check for duplicates
            if (ret)
            {
                if (this.HasDuplicates())
                {
                    ValDescriptor.Text = string.Empty;
                    ListHandler.ListFindItem(DescriptorTypeID, "-1");
                    lblErrorDes.ForeColor = System.Drawing.Color.Red;
                    lblErrorDes.Text = "Descriptor already exists";
                    //Code changes done for larry observations by koshlendra for PXP-13335 start
                    grdExistingDescriptors.EmptyDataText = "";
                    grdExistingDescriptors.DataBind();
                    //Code changes done for larry observations by koshlendra for PXP-13335 end
                    ret = false;
                }
            }

            return ret;
        }

        public bool HasDuplicates()
        {
            bool hasDuplicates = false;
            DataSet ds = DataMerchantApp.GetInstance().GetMerchantDescriptors(UserSessions.CurrentMerchantApp.ID);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus)
                    {
                        if (dr["Descriptor"].ToString().Trim() == ValDescriptor.Text.Trim())
                        {
                            hasDuplicates = true;
                            break;
                        }
                    }
                    else if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal)
                    {
                        if (dr["Descriptor"].ToString().Trim() == ValDescriptor.Text.Trim() && dr["Descriptor2"].ToString().Trim() == ValDescriptorLine2.Text.Trim())
                        {
                            hasDuplicates = true;
                            break;
                        }
                    }
                }
            }

            return hasDuplicates;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //changes done for PXP-13483 by koshlendra start
            if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine && DescriptorTypeID.SelectedValue.Trim() == "2" && (Convert.ToInt32(Session["ValidaionCount"]) == 0 || Session["ValidaionCount"] == null))
            {

                lblErrorDes.Text = "Validate descirptor before saving.\n";
                grdExistingDescriptors.DataSource = null;
                grdExistingDescriptors.EmptyDataText = "";
                grdExistingDescriptors.DataBind();
                return;
            }
            else
            {
                //changes done for PXP-13483 by koshlendra end          
                if (this.IsValidForm())
                {
                    // save new merchant descriptor
                    MerchantDescriptor objDes = new MerchantDescriptor();

                    objDes.MerchantID = UserSessions.CurrentMerchantApp.ID;
                    objDes.Descriptor = ValDescriptor.Text.Trim();

                    if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal)
                    {
                        objDes.Descriptor2 = ValDescriptorLine2.Text.Trim();
                    }

                    objDes.UserName = UserSessions.CurrentUser.UserName;
                    objDes.MerchantDescriptorTypeID = int.Parse(DescriptorTypeID.SelectedValue);
                    DataAccess.DataMerchantAppDao.InsertMerchantDescriptor(objDes);

                    LoadValidators();
                    CreditDescriptor.Text = ValDescriptor.Text;

                    ValDescriptor.Text = string.Empty;
                    ValDescriptorLine2.Text = string.Empty;
                    if (DescriptorTypeID.SelectedValue.Trim() != "2")
                        ListHandler.ListFindItem(DescriptorTypeID, "-1");
                    lblErrorDes.ForeColor = System.Drawing.Color.Green;
                    lblErrorDes.Text = "Descriptor is added.";
                    grdExistingDescriptors.EmptyDataText = string.Empty;
                    this.grdExistingDescriptors.DataSource = null;
                    grdExistingDescriptors.DataBind();
                    //PXP-13243 code change done for HR Descriptor Validation at 17 Characters by koshlendra on 03/09/2020  start
                    if (DescriptorTypeID.SelectedValue.Trim() == "2")
                        WebDialogWindow3.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
                    //PXP-13243 code change done for HR Descriptor Validation at 17 Characters by koshlendra on 03/09/2020  end   
                    //changes done for PXP-13483 by koshlendra start
                    if (Session["ValidaionCount"] != null && Convert.ToInt32(Session["ValidaionCount"]) != 0)
                      Session["ValidaionCount"] = null;
                }
            }
           
        }

        protected void grdDescriptor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int DescriptorID = DataLayer.Field2IntSafe(grdDescriptor.Rows[e.RowIndex].Cells[7].Text);
             bool perform = DataAccess.DataMerchantAppDao.DeleteMerchantDescriptor(DescriptorID, UserSessions.CurrentMerchantApp.ID);

            if (perform)
                LoadValidators();
        }

        int GetColumnIndexByName(GridViewRow row, string columnName)
        {
            int columnIndex = 0;
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.ContainingField is BoundField)
                    if (((BoundField)cell.ContainingField).DataField.Equals(columnName))
                        break;
                columnIndex++; // keep adding 1 while we don't have the correct name
            }
            return columnIndex > 0 ? columnIndex : -1;
        }


        protected void grdDescriptor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                ImageButton btnDelete = (ImageButton)e.Row.Cells[0].Controls[0];
                e.Row.ToolTip = "Delete";

                btnDelete.Attributes.Add("onClick",
                    "var ch=confirm('Are you sure, you want to delete this descriptor?');if(ch==false) return false;");

                btnDelete.CommandName = "Delete";

                int typeID = DataLayer.Field2IntSafe(DataBinder.Eval(e.Row.DataItem, "MerchantDescriptorTypeID"));

                e.Row.Cells[3].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[3].Text);

                // default this to hidden
                Label descriptor2 = (Label)(e.Row.FindControl("Label2"));
                descriptor2.Visible = false;

                // only show if optimal, and if has value
                if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Optimal)
                {
                    if (!string.IsNullOrWhiteSpace(descriptor2.Text))
                    {
                        descriptor2.Visible = true;
                        descriptor2.Style.Add("display", "block");
                    }
                }
            }
        }

        protected void grdDescriptor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDescriptor.PageIndex = e.NewPageIndex;
            LoadValidators();
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            //Code changes done for larry observations by koshlendra for PXP-13335 start
            grdExistingDescriptors.DataSource = null;
            grdExistingDescriptors.EmptyDataText = "No Matching Descriptors found.";
            grdExistingDescriptors.DataBind();
            //Code changes done for larry observations by koshlendra for PXP-13335 end
            if (IsValidForm())
            {
                if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus)
                {
                    //PXP-4299 code change done by koshlendra for adding officeID criteria on 02/27/2018 start       
                    //PXP-13243 code change done for HR Descriptor Validation at 17 Characters by koshlendra on 03/09/2020  start
                    DataTable dt = DataAccess.DataMerchantAppDao.GetExistingMatchingDescriptors(this.ValDescriptor.Text, (int)UserSessions.CurrentMerchantApp.Office, Convert.ToInt32(DescriptorTypeID.SelectedValue.Trim()));
                    //PXP-13243 code change done for HR Descriptor Validation at 17 Characters by koshlendra on 03/09/2020 start
                    //PXP-4299 code change done by koshlendra for adding officeID criteria on 02/27/2018 end            
                    grdExistingDescriptors.DataSource = dt;
                    grdExistingDescriptors.DataBind();
                }
            }
            //changes done for PXP-13483 by koshlendra start
            if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine && DescriptorTypeID.SelectedValue.Trim() == "2")
            {
                if (Session["ValidaionCount"] == null || Convert.ToInt32(Session["ValidaionCount"]) == 0)
                    Session["ValidaionCount"] = 1;
                else
                    Session["ValidaionCount"] = Convert.ToInt32(Session["ValidaionCount"]) + 1;
            }
            //changes done for PXP-13483 by koshlendra end          
        }

        /// <summary>
        /// Event added for PXP-13483 by koshlendra          
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ValDescriptor_TextChanged(object sender, EventArgs e)
        {
            if (UserSessions.CurrentMerchantApp.Brand == MerchantBrand.Meritus && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine && DescriptorTypeID.SelectedValue.Trim() == "2")
            Session["ValidaionCount"] = null;
        }

    }
}