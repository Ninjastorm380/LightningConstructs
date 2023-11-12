Imports System.Text

Namespace Lightning
    Public Partial Class Log
        Public Shared Sub Print(ByVal Type As Type, Byval FileName As String, ByVal LineNumber As UInt32, ByVal LogType As LogType, ByVal Message As String)
            If CInt(LogType) <= CInt(LogLevel)
                Dim TypeString As String = Type.FullName
            
                Dim LogTypeString As String = String.Empty : Select Case LogType
                    Case LogType.Critical : LogTypeString = Critical
                    Case LogType.Warning  : LogTypeString = Warning
                    Case LogType.Info     : LogTypeString = Info
                    Case LogType.Debug    : LogTypeString = Debug
                End Select
            
                With New StringBuilder
                    .Append(LogTypeString)
                    .Append(CallerStart)
                    .Append(TypeString)
                    .Append(CallerSplit1)
                    .Append(FileName)
                    .Append(CallerSplit2)
                    .Append(LineNumber.ToString(LineNumberFormat))
                    .Append(CallerEnd)
                    .Append(Message)
                    Console.WriteLine(.ToString)
                End With
            End If
        End Sub
        
        Public Shared Sub PrintAsync(ByVal Type As Type, Byval FileName As String, ByVal LineNumber As UInt32, ByVal LogType As LogType, ByVal Message As String)
            LambdaThread(Of Type, String, UInt32, LogType, String).Start(AddressOf Print, Type, FileName, LineNumber, LogType, Message)
        End Sub
    End Class
End Namespace