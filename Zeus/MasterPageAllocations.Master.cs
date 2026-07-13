using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


    public partial class MasterPageAllocations : frmBaseMaster
    {
        public enum eMasterSideMenu : int
        {
            NotSet,
            AgentAllocations,
            AgentAllocationsDetail,
        }
        public void SetStatusBarText(string msg)
        {
            wucTopMenu1.StatusBarText = msg;
        }
        protected void Page_Load(eMasterSideMenu eM)
        {
            switch (eM)
            {
                case eMasterSideMenu.AgentAllocations:
                    wucTopMenu1.StatusBarText = "Agent Allocations";
                    lnkAgentAllocation.CssClass = "active";
                    break;
                case eMasterSideMenu.AgentAllocationsDetail:
                    break;
                default:
                    break;
            }
        }
        public int ErrorCount()
        {
            return this.WucMessage1.ErrorCount();
        }

        public void AddMessageError(string msg)
        {
            this.WucMessage1.AddMessageError(msg);
        }

        public void AddMessageStatus(string msg)
        {
            this.WucMessage1.AddMessageStatus(msg);
        }

        public void AddMessageSuccess(string msg)
        {
            this.WucMessage1.AddMessageSuccess(msg);
        }
    }
