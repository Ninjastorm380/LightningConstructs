Namespace Lightning
    Public Partial Class LambdaThread
        Public Property Name As String
    
        Public ReadOnly Property Running As Boolean
            Get 
                Return BaseRunning
            End Get
        End Property
    End Class

    Public Partial Class LambdaThread(Of T1)
        Public Property Name As String
        
        Public ReadOnly Property Running As Boolean
            Get 
                Return BaseRunning
            End Get
        End Property
    End Class

    Public Partial Class LambdaThread(Of T1, T2)
        Public Property Name As String
        
        Public ReadOnly Property Running As Boolean
            Get 
                Return BaseRunning
            End Get
        End Property
    End Class

    Public Partial Class LambdaThread(Of T1, T2, T3)
        Public Property Name As String
        
        Public ReadOnly Property Running As Boolean
            Get 
                Return BaseRunning
            End Get
        End Property
    End Class

    Public Partial Class LambdaThread(Of T1, T2, T3, T4)
        Public Property Name As String
        
        Public ReadOnly Property Running As Boolean
            Get 
                Return BaseRunning
            End Get
        End Property
    End Class

    Public Partial Class LambdaThread(Of T1, T2, T3, T4, T5)
        Public Property Name As String
        
        Public ReadOnly Property Running As Boolean
            Get 
                Return BaseRunning
            End Get
        End Property
    End Class

    Public Partial Class LambdaThread(Of T1, T2, T3, T4, T5, T6)
        Public Property Name As String
        
        Public ReadOnly Property Running As Boolean
            Get 
                Return BaseRunning
            End Get
        End Property
    End Class
End Namespace