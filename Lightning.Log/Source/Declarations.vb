Namespace Lightning
    Public Partial Class Log
        Private Const Warning As String  = "[!] "
        Private Const Critical As String = "[#] "
        Private Const Debug As String    = "[?] "
        Private Const Info As String     = "[.] "
        
        Private Const CallerStart As String = "["
        Private Const CallerSplit1 As String = " - '"
        Private Const CallerSplit2 As String = "' | Line: "
        Private Const CallerEnd As String   = "]: "
        
        Private Const LineNumberFormat As String = "D7"
    End Class
End Namespace
