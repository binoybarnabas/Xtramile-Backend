namespace XtramileBackend.Utils
{
    public interface IMailService
    {
        Task SendToEmployeeOnSubmit(int requestId);
        Task SendToManagersOnSubmit(int requestId);
        Task SendToTravelAdminTeamOnSubmit(int requestId);
        Task SendToEmployeeOnManagerApproval(int requestId);
        Task SendToTravelAdminTeamOnManagerApproval(int requestId);
        Task SendToEmployeeOnManagerDenial(int requestId);
        Task SendToTravelAdminTeamOnManagerDenial(int requestId);
        Task SendToReportingManagerOnOptionSent(int requestId);
        Task SendToTrvaelAdminTeamOnOptionSelection(int requestId);
        Task SendToEmployeeOnTravelAdminApproval(int requestId);
    }
}
