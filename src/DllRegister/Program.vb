Public Class Program

    <STAThread()>
    Shared Sub Main(ByVal args As String())
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        Application.Run(New ConsoleDialog(args))
    End Sub

End Class