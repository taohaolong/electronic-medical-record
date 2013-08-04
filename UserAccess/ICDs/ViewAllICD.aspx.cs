﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserAccess_ICDs_ViewAllICD : System.Web.UI.Page
{
    protected readonly string AllICDChapterValue = "-1";

    protected void Page_Load(object sender, EventArgs e)
    {
        // bind the icd chapter dropdownlist
        if (!IsPostBack)
        {
            BindICDChapterDropdownList();
        }

        // filter criteria for the dropdownlist
        if (FindICDChapterDropdownList.SelectedValue == AllICDChapterValue)
        {
            AllICDDataSource.Where = "Code.Contains(@CodePart) && Name.Contains(@NamePart)";
        }
        else
        {
            AllICDDataSource.Where = "Code.Contains(@CodePart) && Name.Contains(@NamePart) && ICDChapterID == @ICDChapter";
        }

        AllICDGridView.DataBind();
    }

    protected void BindICDChapterDropdownList()
    {
        // add one item to the top of the dropdownlist
        FindICDChapterDropdownList.Items.Add(new ListItem("Al ICD Chapters", AllICDChapterValue));

        // select all ICD chapters from database
        var chapters = from ic in new DataClassesDataContext().ICDChapters
                          select ic;
        foreach (var chapter in chapters)
        {
            FindICDChapterDropdownList.Items.Add(new ListItem(chapter.Name,chapter.ID.ToString()));
        }

        FindICDChapterDropdownList.SelectedIndex = 0;
    }

    protected void AllICDGridView_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        // display the result alert
        e.ExceptionHandled = ResultAlert.SetResultAlertReturn("ICD deleted successfully", e.Exception);
    }

    protected void AllICDGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        System.Threading.Thread.Sleep(1000);

        // delete all its dependencies first
        ICDOperations.DeleteDependencies(long.Parse(e.Keys["ID"].ToString()));
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetICDCodeCompletionList(string prefixText, int count, string contextKey)
    {
        // select the icd codes that contain the string prefix text
        var icdCodes = from i in new DataClassesDataContext().ICDs
                       where i.Code.Contains(prefixText)
                       select i.Code;
        return icdCodes.ToArray();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetICDNameCompletionList(string prefixText, int count, string contextKey)
    {
        // select the icd names that contain the string prefix text
        var icdNames = from i in new DataClassesDataContext().ICDs
                       where i.Name.Contains(prefixText)
                       select i.Name;
        return icdNames.ToArray();
    }

    protected void FindICDButton_Click(object sender, EventArgs e)
    {
        AllICDGridView.DataBind();
    }

    protected void CancelFindButton_Click(object sender, EventArgs e)
    {
        // remove all search criteria
        //FindICDCodeTextBox.Text = "";
        //FindICDNameTextBox.Text = "";
        //FindICDChapterDropdownList.SelectedIndex = 0;
        //AllICDGridView.DataBind();
        Response.Redirect(Request.RawUrl);
    }

    protected void FindICDChapterDropdownList_DataBound(object sender, EventArgs e)
    {
        //// add one new item on top of the dropdown list
        //ListItem item = new ListItem("All ICD Chapters", AllICDChapterValue);
        //FindICDChapterDropdownList.Items.Insert(0, item);

        //if (FindICDChapterDropdownList.SelectedValue == AllICDChapterValue)
        //{
        //    AllICDDataSource.Where = "Code.Contains(@CodePart) && Name.Contains(@NamePart)";
        //}
        //else
        //{
        //    AllICDDataSource.Where = "Code.Contains(@CodePart) && Name.Contains(@NamePart) && ICDChapterID == @ICDChapter";
        //}
    }
}