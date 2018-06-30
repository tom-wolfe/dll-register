Imports System.Reflection

Public Class DllRegisterer
#Region "  Fields  "

    Private _console As ConsoleDialog

#End Region

#Region "  Constructors  "

    Public Sub New(ByVal console As ConsoleDialog)
        _console = console
    End Sub

#End Region

#Region "  Methods  "

    Public Sub Register(ByVal path As String)
        Try
            Dim comparePath = path.ToLowerInvariant()
            If (comparePath.EndsWith(".dll") OrElse comparePath.EndsWith(".ocx")) Then
                RegisterDotNetDll(Assembly.ReflectionOnlyLoadFrom(path))
            ElseIf (comparePath.EndsWith(".exe")) Then
                Execute(path, "/regserver")
            End If
        Catch ex As Exception
            Try
                RegisterVB6Dll(path)
            Catch ex2 As Exception
                WriteLine("File could not be registered: " & path)
            End Try
        End Try
        WriteLine("File registered successfully: " & path)
    End Sub

    Private Sub RegisterVB6Dll(ByVal path As String)
        Execute("regsvr32.exe", String.Format("/s {0}{1}{0}", """", path))
    End Sub

    Private Sub RegisterDotNetDll(ByVal assembly As Assembly)
        Execute(GetRegAsmPath(assembly.ImageRuntimeVersion), String.Format("/tlb /codebase {0}{1}{0}", """", assembly.CodeBase))
    End Sub

    Private Function GetRegAsmPath(version As String) As String
        Return String.Format("C:\Windows\Microsoft.NET\Framework\{0}\RegAsm.exe", version)
    End Function

    Private Sub Execute(ByVal exe As String, ByVal params As String)
        Using process As New Process()
            With process.StartInfo
                .FileName = exe
                .Arguments = params
                .UseShellExecute = False
                .CreateNoWindow = True
                .RedirectStandardOutput = True
            End With

            process.Start()
            Dim reader = process.StandardOutput
            Dim readLine = reader.ReadLine()
            While (Not String.IsNullOrEmpty(readLine))
                WriteLine(readLine)
                readLine = reader.ReadLine()
            End While
            process.WaitForExit()
            process.Close()
        End Using
    End Sub

    Private Sub WriteLine(ByVal text As String)
        _console.WriteLine(text)
    End Sub

#End Region
End Class