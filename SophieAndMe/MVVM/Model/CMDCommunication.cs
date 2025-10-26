using System.Windows.Forms;

namespace SophieAndMe.MVVM.Model;

public class CMDCommunication
{
    public static void run_cmd(object command)
    {
        System.Diagnostics.ProcessStartInfo procstatinfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c" + command);
        procstatinfo.UseShellExecute = false;
        procstatinfo.CreateNoWindow = true;
        procstatinfo.RedirectStandardOutput = true;
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo = procstatinfo;
        proc.Start();
        //MessageBox.Show("Executed");
        Cursor.Current = Cursors.WaitCursor;
        proc.WaitForExit();
    }
}