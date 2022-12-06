using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Interface
{
    public interface ISubjectPerformance
    {
        SubjectPerformanceViewModel GetMenuList();
       /* SubjectPerformanceViewModel ShowStudentsClassWiseSubjectWise(string sortColumn, string sortColumnDir, string Search, string examType, string studentClass, string subject, string examType1, string studentClass1, int CurrentSessionID);*/
        SubjectPerformanceViewModel GetSessionExamDetail(int subjectID, int classID, int examTypeID, int CurrentSessionID);
        int InsertUpdateStudentMarks(StudentExamPerformance studentExamPerformance);
        bool InsertUpdateSessionExam(SubjectPerformanceViewModel subjectPerformance);
        SubjectPerformanceViewModel  ShowStudentsClassWiseSubjectWise(string sortColumn, string sortColumnDir, string searchValue, string examType, string studentClass, string subject, int currentSessionID);
        List<StudentPerformanceViewModel> ShowAllStudentPerformance(string ExamType, string studentClass, string subject, string sortColumn, string sortColumnDir, string searchValue, int currentSessionID);
        bool SetDefaultMarksZeroBySesseionExamID(int examSessionID, int createdBy);
    }
}
