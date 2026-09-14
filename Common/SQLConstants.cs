namespace student_log_api.Common
{
    public class SQLConstants
    {
        //Auth
        public static string GET_USER_BY_EMAIL = "Get_UserByEmail";
        public static string CHECK_VALID_USER = "Get_CheckValidUser";
        //Account
        public static string GET_ACCOUNTS = "Get_AccountsList";
        public static string GET_ACCOUNT_DETAILS = "Get_AccountDetails";
        public static string GET_SCHOOLS_BY_ACCOUNTID = "Get_SchoolsByAccountID";
        public static string GET_USERS_BY_ACCOUNTID = "Get_UsersByAccountID";
        public static string UPSERT_CLASSES_JSON = "Upsert_TNS_Classes_JSON";
        public static string UPSERT_SCHOOL = "Upsert_Schools";
        public static string UPSERT_USER = "Upsert_Users";

        // Students
        public static string GET_STUDENTS_LIST = "Get_StudentsList";
        public static string GET_CLASSES_LIST = "Get_ClassesList";
        public static string UPSERT_STUDENTS_JSON = "Upsert_Students_JSON";

        // Tickets
        public static string INSERT_NEW_TICKET = "Insert_New_Ticket";
        public static string GET_TICKET_LIST = "Get_TicketsList";
        public static string GET_TICKET_DETAILS = "Get_TicketDetailsById";
        public static string ASSIGN_TICKETS_TO_USER = "Upsert_Assign_TicketsToUser";

        // Subjects
        public static string GET_SUBJECTS = "Get_Subjects";
        public static string GET_SUBJECT_BY_ID = "Get_SubjectByID";
        public static string UPSERT_SUBJECT = "Upsert_Subject";
        public static string DEACTIVATE_SUBJECT = "Deactivate_Subject";

        // Daily work
        public static string ASSIGN_DAILY_WORK = "Assign_DailyWork";
        public static string GET_TEACHER_DAILY_WORK = "Get_TeacherDailyWork";
        public static string GET_STUDENT_DAILY_WORK = "Get_StudentDailyWork";
        public static string GET_DAILY_WORK_DETAILS = "Get_DailyWorkDetails";

        // Student gate attendance
        public static string RECORD_STUDENT_GATE_SWIPE = "Record_StudentGateSwipe";
        public static string RECORD_MANUAL_STUDENT_GATE_SWIPE = "Record_ManualStudentGateSwipe";
        public static string GET_STUDENT_GATE_DASHBOARD = "Get_StudentGateDashboard";
        public static string GET_STUDENT_GATE_LIVE_EVENTS = "Get_StudentGateLiveEvents";
        public static string GET_STUDENT_GATE_HISTORY = "Get_StudentGateHistory";
    }
}
