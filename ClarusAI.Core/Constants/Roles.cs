namespace ClarusAI.Core.Constants;

public static class Roles
{
    public const string Administrator = "Administrator";
    public const string Radiologist = "Radiologist";
    public const string Clinician = "Clinician";
    public const string Technician = "Technician";
    public const string Viewer = "Viewer";
}

public static class Permissions
{
    public const string ViewPatients = "ViewPatients";
    public const string EditPatients = "EditPatients";
    public const string DeletePatients = "DeletePatients";
    public const string ViewMedicalImages = "ViewMedicalImages";
    public const string UploadMedicalImages = "UploadMedicalImages";
    public const string DeleteMedicalImages = "DeleteMedicalImages";
    public const string RequestAnalysis = "RequestAnalysis";
    public const string ViewAnalysisResults = "ViewAnalysisResults";
    public const string ManageUsers = "ManageUsers";
    public const string ViewAuditLogs = "ViewAuditLogs";
}