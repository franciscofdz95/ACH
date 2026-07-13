using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AchRecurring
{
    public class SortYearMonthFileAppender : log4net.Appender.RollingFileAppender
    {
        protected override void OpenFile(string fileName, bool append)
        {
            //Inject folder [yyyyMMdd] before the file name
            string baseDirectory = Path.GetDirectoryName(fileName);
            string fileNameOnly = Path.GetFileName(fileName);
            string newDirectory = Path.Combine(baseDirectory, DateTime.Now.ToString("yyyyMM"));
            string newFileName = Path.Combine(newDirectory, fileNameOnly);

            base.OpenFile(newFileName, append);
        }
    }
}
