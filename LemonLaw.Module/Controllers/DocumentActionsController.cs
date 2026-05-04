using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using LemonLaw.Core.Entities;
using System.Diagnostics;

namespace LemonLaw.Module.Controllers;

/// <summary>
/// Controller for document-related actions like downloading files.
/// </summary>
public class DocumentActionsController : ObjectViewController<ListView, ApplicationDocument>
{
    private SimpleAction downloadDocumentAction;

    public DocumentActionsController()
    {
        downloadDocumentAction = new SimpleAction(this, "DownloadDocument", DevExpress.Persistent.Base.PredefinedCategory.RecordEdit)
        {
            Caption = "Download",
            ImageName = "Action_Export_ToFile",
            SelectionDependencyType = SelectionDependencyType.RequireSingleObject,
            ToolTip = "Download the selected document"
        };

        downloadDocumentAction.Execute += DownloadDocumentAction_Execute;
    }

    private void DownloadDocumentAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        var document = e.CurrentObject as ApplicationDocument;
        if (document == null)
            return;

        try
        {
            if (!File.Exists(document.StoragePath))
            {
                throw new FileNotFoundException($"Document file not found: {document.FileName}");
            }

            // Open the file with the default application
            var processStartInfo = new ProcessStartInfo
            {
                FileName = document.StoragePath,
                UseShellExecute = true
            };
            Process.Start(processStartInfo);
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException($"Failed to open document: {ex.Message}");
        }
    }

    protected override void OnActivated()
    {
        base.OnActivated();
        // Enable the action only when viewing documents
        downloadDocumentAction.Active["HasDocuments"] = View?.CollectionSource?.Collection != null;
    }
}
