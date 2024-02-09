Imports System.ComponentModel

Namespace Lightning
    Public Partial Class LambdaThread
        Private ReadOnly Worker As BackgroundWorker
        Private ReadOnly Workload As ThreadStart
        
        Private BaseRunning As Boolean
    End Class
    
    Public Partial Class LambdaThread(Of T1)
        Private ReadOnly Worker As BackgroundWorker
        Private ReadOnly Workload As ThreadStart
        
        Private P1 As T1
        
        Private BaseRunning As Boolean
    End Class
    
    Public Partial Class LambdaThread(Of T1, T2)
        Private ReadOnly Worker As BackgroundWorker
        Private ReadOnly Workload As ThreadStart
        
        Private P1 As T1
        Private P2 As T2
        
        Private BaseRunning As Boolean
    End Class
    
    Public Partial Class LambdaThread(Of T1, T2, T3)
        Private ReadOnly Worker As BackgroundWorker
        Private ReadOnly Workload As ThreadStart
        
        Private P1 As T1
        Private P2 As T2
        Private P3 As T3
        
        Private BaseRunning As Boolean
    End Class
    
    Public Partial Class LambdaThread(Of T1, T2, T3, T4)
        Private ReadOnly Worker As BackgroundWorker
        Private ReadOnly Workload As ThreadStart
        
        Private P1 As T1
        Private P2 As T2
        Private P3 As T3
        Private P4 As T4
        
        Private BaseRunning As Boolean
    End Class
    
    Public Partial Class LambdaThread(Of T1, T2, T3, T4, T5)
        Private ReadOnly Worker As BackgroundWorker
        Private ReadOnly Workload As ThreadStart
        
        Private P1 As T1
        Private P2 As T2
        Private P3 As T3
        Private P4 As T4
        Private P5 As T5
        
        Private BaseRunning As Boolean
    End Class
    
    Public Partial Class LambdaThread(Of T1, T2, T3, T4, T5, T6)
        Private ReadOnly Worker As BackgroundWorker
        Private ReadOnly Workload As ThreadStart
        
        Private P1 As T1
        Private P2 As T2
        Private P3 As T3
        Private P4 As T4
        Private P5 As T5
        Private P6 As T6
        
        Private BaseRunning As Boolean
    End Class
End Namespace
