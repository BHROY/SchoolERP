using SchoolManagement.Models;
using System.Collections.Generic;
using System.Data;

namespace SchoolManagement.Interface
{
    public interface ISubjectPerformance
    {
        SubjectPerformanceViewModel GetMenuList();
        /* SubjectPerformanceViewModel ShowStudentsClassWiseSubjectWise(string sortColumn, string sortColumnDir, string Search, string examType, string studentClass, string subject, string examType1, string studentClass1, int CurrentSessionID);*/
        SubjectPerformanceViewModel GetSessionExamDetail(int subjectID, int classID, int examTypeID, int CurrentSessionID);
        int InsertUpdateStudentMarks(StudentExamPerformance studentExamPerformance);
        bool InsertUpdateSessionExam(SubjectPerformanceViewModel subjectPerformance);
        SubjectPerformanceViewModel ShowStudentsClassWiseSubjectWise(string sortColumn, string sortColumnDir, string searchValue, string examType, string studentClass, string subject, int currentSessionID);
        List<StudentPerformanceViewModel> ShowAllStudentPerformance(string ExamType, string studentClass, string subject, string sortColumn, string sortColumnDir, string searchValue, int currentSessionID);
        bool SetDefaultMarksZeroBySesseionExamID(int examSessionID, int createdBy);
        DataSet GetStudentMarksDataForGraph(int RegistrationID, int currentSessionID);
        List<StudentViewModel> ShowStudentsClassWiseRollnumber(string sortColumn, string sortColumnDir, string searchValue, string studentClass, int currentSessionID);
        int InsertUpdateStudentRollNumber(StudentSessionInfo studentSessionInfo);
        bool SetDefaultAlphabeticallyRollNumber(int classID, int v);
        List<DropDown> ShowClassWiseAddedSubjects(string studentClass, int currentSessionID);
        bool CheckIfExamHeldForClass(string studentClass, int currentSessionID);
        bool InsertUpdateClassToSubject(List<ClassToSubject> classToSubjectsList);
        List<DropDown> GetClassBasedSubjects(int classID, int CurrentSessionID);
        DataSet GetMarksAnalysisForAdminForGraph(int examTypeID, int classID, int subjectID, int currentSessionID);
        List<StudentPerformanceViewModel> GetGradeWiseStudentList(int examTypeID, int classID, int subjectID, string grade, int currentSessionID);
    }
}
