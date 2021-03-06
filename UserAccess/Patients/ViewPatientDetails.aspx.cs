﻿using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserAccess_Patients_ViewPatientDetails : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        // redirect if query string not found
        if (Request.QueryString["ID"] == null)
        {
            RedirectToViewAllPatients();
        }
        else
        {
            long temp;
            // redirect if cannot parse
            if (long.TryParse(Request.QueryString["ID"], out temp))
            {
                // redirect if patient not found
                if (new DataClassesDataContext().Patients.Where(p => p.ID == long.Parse(Request.QueryString["ID"])).Count() == 0)
                {
                    RedirectToViewAllPatients();
                }
                else
                {
                    // OK
                }
            }
            else
            {
                RedirectToViewAllPatients();
            }
        }

        RedirectSuccessAlert.SetAlert("Patient inserted successfully!",
            RedirectSuccessConstants.RedirectSuccessAddPatient);
    }

    protected void RedirectToViewAllPatients()
    {
        // set the session variable
        Session[RedirectConstants.RedirectPatientDetailsSessionName] = "yes";

        Response.Redirect("/UserAccess/Patients/ViewAllPatients.aspx");
    }

    protected void PatientDetailFormView_ItemDeleted(object sender, FormViewDeletedEventArgs e)
    {
        if (e.Exception == null)
        {
            Session[RedirectSuccessConstants.RedirectSuccessDeletePatient] = "yes";
            Response.Redirect("/UserAccess/Patients/ViewAllPatients.aspx");
        }
        else
        {
            e.ExceptionHandled = ResultAlert.SetResultAlertReturn("error", e.Exception);
        }
    }
    protected void PatientDetailFormView_ItemDeleting(object sender, FormViewDeleteEventArgs e)
    {
        System.Threading.Thread.Sleep(1000);

        // delete all the dependencies first
        PatientOperations.DeleteDependencies(long.Parse(e.Keys["ID"].ToString()));
    }

    protected void ClearForm()
    {
        ((TextBox)PatientDetailFormView.FindControl("NameTextBox")).Text = "";
        ((TextBox)PatientDetailFormView.FindControl("AddressTextBox")).Text = "";
        ((DropDownList)PatientDetailFormView.FindControl("GenderDropdownList")).SelectedIndex = 0;
        var dateOfBirthDatePicker = (TemplateControls_DatePicker)
            PatientDetailFormView.FindControl("DateOfBirthDatePicker");
        dateOfBirthDatePicker.SelectedDate = DateTime.Now.Ticks;
    }

    protected void ClearButton_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    protected void PatientDetailFormView_ItemUpdated(object sender, FormViewUpdatedEventArgs e)
    {
        System.Threading.Thread.Sleep(1000);
        if (e.Exception == null)
        {
            ResultAlert.SetResultAlert("Patient updated successfully!",
                TemplateControls_ResultAlert.AlertTypeSuccess);
        }
        else
        {
            ResultAlert.SetResultAlert(e.Exception.Message,
                TemplateControls_ResultAlert.AlertTypeError);
            e.ExceptionHandled = true;
        }
    }
    protected void PatientDetailFormView_ModeChanged(object sender, EventArgs e)
    {
        
    }
    protected void PatientDetailDataSource_Updating(object sender, LinqDataSourceUpdateEventArgs e)
    {
        // get the new object
        var newObject = (Patient)e.NewObject;

        // get the controls from the view
        var dateOfBirthDatePicker = (TemplateControls_DatePicker)
            PatientDetailFormView.FindControl("DateOfBirthDatePicker");

        // set the date for the new object
        newObject.DateOfBirth = dateOfBirthDatePicker.SelectedDate;
        
    }
    protected void PatientDetailDataSource_DataBinding(object sender, EventArgs e)
    {
        
    }
    protected void PatientDetailDataSource_Selected(object sender, LinqDataSourceStatusEventArgs e)
    {
        
    }
}