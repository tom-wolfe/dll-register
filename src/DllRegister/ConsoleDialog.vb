Public Class ConsoleDialog
#Region "  Fields  "

    Private _args As String()

#End Region

#Region "  Constructors  "

    Public Sub New(ByVal args As String())
        InitializeComponent()
        _args = args
    End Sub

#End Region

#Region "  Event Handlers  "

    Private Sub ConsoleDialog_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ClearScreen()
        Dim paths As String()
        If (_args.Length > 0) Then
            paths = _args
        Else
            Using ofd = New OpenFileDialog()
                ofd.Multiselect = True
                ofd.Title = "Select a DLL to Register"
                ofd.Filter = "Binary Files|*.dll;*.exe;*.ocx|All Files|*.*"

                If (ofd.ShowDialog(Me) <> DialogResult.OK) Then Return
                paths = ofd.FileNames
            End Using
        End If

        Dim registerer = New DllRegisterer(Me)
        For Each path In paths
            registerer.Register(path)
            WriteLine(String.Empty)
        Next
    End Sub

    Private Sub txtConsole_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtConsole.KeyDown
        If (e.KeyCode = Keys.Enter) Then Application.Exit()
    End Sub

#End Region

#Region "  Methods  "
    Public Sub ClearScreen()
        txtConsole.Text = String.Empty
    End Sub

    Public Sub Write(ByVal text As String)
        txtConsole.Text += text
    End Sub

    Public Sub WriteLine(ByVal text As String)
        Write(text + Environment.NewLine)
    End Sub

#End Region
End Class